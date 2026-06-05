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

            // Если уровень ещё не создан, генерируем новый
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

            // 1. Движение игрока
            _movement.Update(model.Player, gameTime);

            // 2. Коллизии игрока со стенами
            _collision.CollideWithWalls(model);

            // 3. Стрельба игрока (добавляет пулю в список)
            _playerShoot.Update(model, gameTime);

            // 4. ИИ врагов: обновление состояний (включая стрельбу врагов) и затем движение ВСЕХ пуль
            _ai.Update(model, gameTime);

            // 5. Коллизии пуль с врагами и игроком, контакт с врагами
            _collision.CollideWithBulletsAndEnemies(model);

            // 6. Обновление состояния Трикстера (уязвимость, сброс управления)
            _tricksterRebind.Update(model);

            // 7. Обновление таймера неуязвимости игрока
            if (model.Player.InvincibilityTimer > 0)
                model.Player.InvincibilityTimer -= dt;

            // 8. Проверка смерти игрока
            if (model.Player.Health <= 0 || !model.Player.IsAlive)
            {
                model.Player.IsAlive = false;
                model.CurrentMode = GameMode.Menu;
                model.CurrentLevel = null;
                model.Player = null;
                return;
            }

            // 9. Проверка победы: все враги мертвы
            if (model.CurrentLevel.Enemies.Count > 0 && model.CurrentLevel.Enemies.All(e => !e.IsAlive))
            {
                model.CurrentMode = GameMode.Victory;
                // Уровень остаётся для отрисовки, но игра останавливается
            }
        }
    }
}