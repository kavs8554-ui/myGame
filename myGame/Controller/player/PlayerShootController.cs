using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using myGame.Model;
using myGame.Model.map;

namespace myGame.Controller.Player
{
    public class PlayerShootController
    {
        private MouseState _previousMouseState;

        public void Update(GameModel model, GameTime gameTime)
        {
            if (model.CurrentLevel == null || model.Player == null) return;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Обновляем таймер перезарядки
            if (model.Player.ShootTimer > 0)
                model.Player.ShootTimer -= dt;

            var mouseState = Mouse.GetState();
            // Стрельба по левой кнопке мыши или по пробелу
            bool shoot = (mouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) ||
                         Keyboard.GetState().IsKeyDown(Keys.Space);
            if (shoot && model.Player.ShootTimer <= 0)
            {
                // Направление от игрока к курсору мыши
                Vector2 direction = new Vector2(mouseState.X, mouseState.Y) - model.Player.Position;
                if (direction.Length() > 0.01f)
                    direction.Normalize();
                else
                    direction = Vector2.UnitX;

                var bullet = new BulletModel
                {
                    Position = model.Player.Position,
                    Direction = direction,
                    Speed = 500f,
                    IsActive = true,
                    Owner = null,
                    IsPlayerBullet = true
                };
                model.CurrentLevel.Bullets.Add(bullet);
                model.Player.ShootTimer = model.Player.ShootCooldown;
            }
            _previousMouseState = mouseState;
        }
    }
}
