using Microsoft.Xna.Framework;
using myGame.Controller.enemies;
using myGame.Controller.map;
using myGame.Controller.Map;
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
        private TricksterRebindController _TricksterRebind;
        private ProceduralGenerator _generator;

        public GameController()
        {
            _movement = new PlayerMovementController();
            _collision = new CollisionController();
            _ai = new AIController();
            _TricksterRebind = new TricksterRebindController();
            _generator = new ProceduralGenerator();
        }

        public void Update(GameModel model, GameTime gameTime)
        {
            if (model.CurrentMode != GameMode.Game) return;

            if (model.CurrentLevel == null)
            {
                model.CurrentLevel = _generator.GenerateLevel(800, 600);
                model.Player = new PlayerModel
                {
                    Position = model.CurrentLevel.PlayerStart,
                    Radius = 16f,
                    IsAlive = true,
                    Velocity = Vector2.Zero
                };
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _movement.Update(model.Player, gameTime);
            _collision.CollideWithWalls(model);

            _ai.Update(model, gameTime);

            _collision.CollideWithBulletsAndEnemies(model);
            _TricksterRebind.Update(model);

            if (!model.Player.IsAlive)
            {
                model.CurrentMode = GameMode.Menu;
                model.CurrentLevel = null;
            }
        }
    }
}