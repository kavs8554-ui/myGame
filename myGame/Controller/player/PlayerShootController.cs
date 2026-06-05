using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using myGame.Model;
using myGame.Model.map;
using System;

namespace myGame.Controller.Player
{
    public class PlayerShootController
    {
        private MouseState _previousMouseState;

        public void Update(GameModel model, GameTime gameTime)
        {
            if (model.CurrentLevel == null || model.Player == null) return;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (model.Player.ShootTimer > 0)
                model.Player.ShootTimer -= dt;

            var mouseState = Mouse.GetState();
            bool shoot = (mouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) ||
                         Keyboard.GetState().IsKeyDown(Keys.Space);

            if (shoot && model.Player.ShootTimer <= 0)
            {
                Vector2 mouseWorldPos = new Vector2(mouseState.X, mouseState.Y);
                Vector2 direction = mouseWorldPos - model.Player.Position;
                if (direction.Length() < 0.01f) direction = Vector2.UnitX;
                else direction.Normalize();

                // Проверяем прямую видимость от игрока до позиции мыши
                if (HasLineOfSight(model.Player.Position, mouseWorldPos, model.CurrentLevel))
                {
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
            }
            _previousMouseState = mouseState;
        }

        private bool HasLineOfSight(Vector2 from, Vector2 to, LevelModel level)
        {
            // Проверяем все стены на пересечение с отрезком [from, to]
            foreach (var wall in level.Walls)
            {
                // Пропускаем стены, в которых находится игрок или точка мыши (чтобы не блокировать выстрел изнутри)
                if (wall.Contains(new Point((int)from.X, (int)from.Y)) ||
                    wall.Contains(new Point((int)to.X, (int)to.Y)))
                    continue;

                if (RayIntersectsAABB(from, to, wall))
                    return false; // есть препятствие на пути
            }
            return true; // прямая видимость свободна
        }

        /// <summary>
        /// Точный тест пересечения отрезка [p1, p2] с выровненным по осям прямоугольником.
        /// Возвращает true, если отрезок пересекает прямоугольник (включая касание).
        /// </summary>
        private bool RayIntersectsAABB(Vector2 p1, Vector2 p2, Rectangle rect)
        {
            // Для каждой оси вычисляем t-интервал, когда отрезок находится внутри проекции прямоугольника
            float tMin = 0.0f;
            float tMax = 1.0f;

            // Ось X
            float dx = p2.X - p1.X;
            if (Math.Abs(dx) < 0.00001f)
            {
                // Вертикальный луч: проверяем, лежит ли X внутри прямоугольника
                if (p1.X < rect.Left || p1.X > rect.Right)
                    return false;
            }
            else
            {
                float t1 = (rect.Left - p1.X) / dx;
                float t2 = (rect.Right - p1.X) / dx;
                if (t1 > t2) { float tmp = t1; t1 = t2; t2 = tmp; }
                tMin = Math.Max(tMin, t1);
                tMax = Math.Min(tMax, t2);
                if (tMin > tMax) return false;
            }

            // Ось Y
            float dy = p2.Y - p1.Y;
            if (Math.Abs(dy) < 0.00001f)
            {
                // Горизонтальный луч: проверяем, лежит ли Y внутри прямоугольника
                if (p1.Y < rect.Top || p1.Y > rect.Bottom)
                    return false;
            }
            else
            {
                float t1 = (rect.Top - p1.Y) / dy;
                float t2 = (rect.Bottom - p1.Y) / dy;
                if (t1 > t2) { float tmp = t1; t1 = t2; t2 = tmp; }
                tMin = Math.Max(tMin, t1);
                tMax = Math.Min(tMax, t2);
                if (tMin > tMax) return false;
            }

            // Если общий интервал существует и пересекает [0,1] — есть пересечение
            return (tMin <= 1.0f && tMax >= 0.0f);
        }
    }
}