using Microsoft.Xna.Framework;
using myGame.Controller.enemies;
using myGame.Controller.map;
using myGame.Controller.player;
using myGame.Model;
using myGame.Model.enemies;
using myGame.Model.map;
using myGame.Model.player;

namespace myGame.Controller
{
    public class GameController
    {
        private PlayerMovementController _movement;
        private CollisionController _collision;
        private AIController _ai;

        public GameController()
        {
            _movement = new PlayerMovementController();
            _collision = new CollisionController();
            _ai = new AIController();
        }

        public void Update(GameModel model, GameTime gameTime)
        {
            if (model.CurrentMode != GameMode.Game) return;

            if (model.CurrentLevel == null)
            {
                CreateTestLevel(model);
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _movement.Update(model.Player, gameTime); 
            _collision.CollideWithWalls(model);       

            _ai.Update(model, gameTime);

            _collision.CollideWithBulletsAndEnemies(model);

            if (!model.Player.IsAlive)
            {
                model.CurrentMode = GameMode.Menu;
                model.CurrentLevel = null;
            }
        }

        private void CreateTestLevel(GameModel model)
        {
            var level = new LevelModel();

            var shooter = new ShooterEnemyModel
            {
                Position = new Vector2(600, 150),
                Radius = 12f,
                ShootCooldown = 1.2f,
                VisionRange = 250f,
                BulletSpeed = 400f
            };
            shooter.PatrolPoints.Add(new Vector2(600, 150));
            shooter.PatrolPoints.Add(new Vector2(600, 350));
            level.Enemies.Add(shooter);

            var enemy = new EnemyModel
            {
                Position = new Vector2(500, 200),
                Radius = 12f
            };
            enemy.PatrolPoints.Add(new Vector2(500, 200));
            enemy.PatrolPoints.Add(new Vector2(500, 400));
            enemy.PatrolPoints.Add(new Vector2(200, 400));
            level.Enemies.Add(enemy);

            level.Walls.Add(new Rectangle(0, 0, 800, 20));
            level.Walls.Add(new Rectangle(0, 580, 800, 20));
            level.Walls.Add(new Rectangle(0, 0, 20, 600));
            level.Walls.Add(new Rectangle(780, 0, 20, 600));
            level.Walls.Add(new Rectangle(300, 200, 100, 20));
            level.PlayerStart = new Vector2(400, 300);

            model.CurrentLevel = level;
            model.Player = new PlayerModel
            {
                Position = level.PlayerStart,
                Radius = 16f,
                IsAlive = true,
                Velocity = Vector2.Zero
            };
        }
    }
}
