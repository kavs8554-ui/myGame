using Microsoft.Xna.Framework;
using myGame.Controller.enemies;
using myGame.Controller.map;
using myGame.Controller.Map;
using myGame.Controller.player;
using myGame.Controller.Player;
using myGame.Model;
using myGame.Model.player;

namespace myGame.Controller
{
    public class GameController
    {
        private PlayerMovementController _movement;
        private CollisionController _collision;
        private AIController _ai;
        private TricksterRebindController _tricksterRebind;
        private ProceduralGenerator _generator;
        private PlayerShootController _playerShoot;   // Добавлено

        public GameController()
        {
            _movement = new PlayerMovementController();
            _collision = new CollisionController();
            _ai = new AIController();
            _tricksterRebind = new TricksterRebindController();
            _generator = new ProceduralGenerator();
            _playerShoot = new PlayerShootController();   // Добавлено
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

            // 1. Движение игрока
            _movement.Update(model.Player, gameTime);
            // 2. Коллизии со стенами
            _collision.CollideWithWalls(model);

            // 3. ИИ врагов (движение, стрельба врагов)
            _ai.Update(model, gameTime);

            // 4. Стрельба игрока (добавлено)
            _playerShoot.Update(model, gameTime);

            // 5. Коллизии пуль и врагов (включая пули игрока)
            _collision.CollideWithBulletsAndEnemies(model);

            // 6. Обновление состояния Трикстера (уязвимость, сброс клавиш)
            _tricksterRebind.Update(model);

            // 7. Проверка смерти игрока
            if (!model.Player.IsAlive)
            {
                model.CurrentMode = GameMode.Menu;
                model.CurrentLevel = null;
            }
        }
    }
}