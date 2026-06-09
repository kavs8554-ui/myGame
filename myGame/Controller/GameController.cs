using Microsoft.Xna.Framework;
using myGame.Controller.enemies;
using myGame.Controller.map;
using myGame.Controller.player;
using myGame.Controller.Player;
using myGame.Model;
using myGame.Model.player;
using System.Linq;

namespace myGame.Controller
{
    public class GameController
    {
        private PlayerMovementController _movement;
        private CollisionController _collision;
        private AIController _ai;
        private TricksterRebindController _tricksterRebind;
        private ProceduralGenerator _generator;
        private PlayerShootController _playerShoot;  

        public GameController()
        {
            _movement = new PlayerMovementController();
            _collision = new CollisionController();
            _ai = new AIController();
            _tricksterRebind = new TricksterRebindController();
            _generator = new ProceduralGenerator();
            _playerShoot = new PlayerShootController();   
        }

        public void Update(GameModel model, GameTime gameTime)
        {
            if (model.CurrentMode != GameMode.Game) return;

            if (model.CurrentLevel == null)
            {
                model.CurrentLevel = _generator.GenerateLevel(model.ViewportWidth, model.ViewportHeight);
                model.Player = new PlayerModel
                {
                    Position = model.CurrentLevel.PlayerStart,
                    Radius = 16f,
                    IsAlive = true,
                    Velocity = Vector2.Zero,
                    Health = 3,
                    MaxHealth = 3
                };
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _movement.Update(model.Player, gameTime);

            _collision.CollideWithWalls(model);

            _playerShoot.Update(model, gameTime);

            _ai.Update(model, gameTime);

            _collision.CollideWithBulletsAndEnemies(model);

            _tricksterRebind.Update(model);

            if (model.Player.InvincibilityTimer > 0)
                model.Player.InvincibilityTimer -= dt;

            if (model.Player.Health <= 0 || !model.Player.IsAlive)
            {
                model.Player.IsAlive = false;
                model.CurrentMode = GameMode.GameOver;
                model.CurrentLevel = null;
                model.Player = null;
                return;
            }

            if (model.CurrentLevel.Enemies.Count > 0 && model.CurrentLevel.Enemies.All(e => !e.IsAlive))
            {
                model.CurrentMode = GameMode.Victory;
            }
        }
    }
}