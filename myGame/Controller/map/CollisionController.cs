using Microsoft.Xna.Framework;
using myGame.Controller.enemies;
using myGame.Model;
using myGame.Model.enemies;
using myGame.Model.player;
using System;

namespace myGame.Controller.map
{
    public class CollisionController
    {
        private TricksterRebindController _tricksterRebind = new TricksterRebindController();

        public void CollideWithWalls(GameModel model)
        {
            if (model.CurrentLevel == null || model.Player == null) return;
            PlayerModel player = model.Player;
            foreach (var wall in model.CurrentLevel.Walls)
            {
                Vector2 closest = new Vector2(
                    MathHelper.Clamp(player.Position.X, wall.Left, wall.Right),
                    MathHelper.Clamp(player.Position.Y, wall.Top, wall.Bottom)
                );
                Vector2 delta = player.Position - closest;
                float distSq = delta.LengthSquared();
                float rad = player.Radius;
                if (distSq < rad * rad)
                {
                    float dist = (float)Math.Sqrt(distSq);
                    Vector2 push = delta / dist * (rad - dist);
                    player.Position += push;
                    Vector2 newVel = player.Velocity;
                    if (push.X != 0) newVel.X *= -0.5f;
                    if (push.Y != 0) newVel.Y *= -0.5f;
                    player.Velocity = newVel;
                }
            }
        }

        public void CollideWithBulletsAndEnemies(GameModel model)
        {
            if (model.CurrentLevel == null || model.Player == null) return;
            PlayerModel player = model.Player;

            // 1. Столкновения с врагами (касание)
            foreach (var enemy in model.CurrentLevel.Enemies)
            {
                if (!enemy.IsAlive) continue;
                float dx = player.Position.X - enemy.Position.X;
                float dy = player.Position.Y - enemy.Position.Y;
                float distSq = dx * dx + dy * dy;
                float radSum = player.Radius + enemy.Radius;
                if (distSq < radSum * radSum)
                {
                    if (enemy is TricksterEnemyModel trickster)
                    {
                        if (!trickster.IsVulnerable)
                        {
                            _tricksterRebind.OnTricksterHit(trickster, model);
                            // Выталкивание из неуязвимого Трикстера
                            float dist = (float)Math.Sqrt(distSq);
                            if (dist > 0.001f)
                            {
                                Vector2 pushDir = new Vector2(dx, dy) / dist;
                                player.Position = enemy.Position + pushDir * radSum;
                                player.Velocity += pushDir * 150f;
                            }
                            else
                            {
                                player.Position = enemy.Position + Vector2.UnitX * radSum;
                                player.Velocity += Vector2.UnitX * 150f;
                            }
                        }
                        else
                        {
                            // Трикстер уязвим — убиваем его, игрок не получает урона
                            enemy.IsAlive = false;
                        }
                    }
                    else // Обычный враг (ShooterEnemy)
                    {
                        if (!player.IsInvincible)
                        {
                            player.Health--;
                            player.InvincibilityTimer = player.InvincibilityDuration;

                            // Отбрасывание игрока от врага
                            float dist = (float)Math.Sqrt(distSq);
                            if (dist > 0.001f)
                            {
                                Vector2 pushDir = new Vector2(dx, dy) / dist;
                                player.Position = enemy.Position + pushDir * radSum;
                                player.Velocity += pushDir * 200f;
                            }
                            else
                            {
                                player.Position = enemy.Position + Vector2.UnitX * radSum;
                                player.Velocity += Vector2.UnitX * 200f;
                            }
                        }
                    }
                }
            }

            // 2. Столкновения вражеских пуль с игроком
            foreach (var bullet in model.CurrentLevel.Bullets)
            {
                if (!bullet.IsActive || bullet.IsPlayerBullet) continue;
                float dx = player.Position.X - bullet.Position.X;
                float dy = player.Position.Y - bullet.Position.Y;
                float distSq = dx * dx + dy * dy;
                float radSum = player.Radius + 4f;
                if (distSq < radSum * radSum)
                {
                    bullet.IsActive = false;

                    if (bullet.IsTricksterBullet)
                    {
                        // Вызываем эффект подмены управления
                        if (bullet.Owner is TricksterEnemyModel trickster)
                            _tricksterRebind.OnTricksterHit(trickster, model);

                        // Небольшое отталкивание (только физическое, без урона)
                        Vector2 pushDir = new Vector2(dx, dy);
                        if (pushDir != Vector2.Zero) pushDir.Normalize();
                        player.Velocity += pushDir * 50f;   // слабый толчок
                    }
                    else
                    {
                        // Обычная вражеская пуля – урон
                        if (!player.IsInvincible)
                        {
                            player.Health--;
                            player.InvincibilityTimer = player.InvincibilityDuration;

                            Vector2 pushDir = new Vector2(dx, dy);
                            if (pushDir != Vector2.Zero) pushDir.Normalize();
                            player.Velocity += pushDir * 100f;
                        }
                        if (player.Health <= 0)
                        {
                            player.IsAlive = false;
                            return;
                        }
                    }
                }
            }

            // 3. Столкновения пуль игрока с врагами
            foreach (var bullet in model.CurrentLevel.Bullets)
            {
                if (!bullet.IsActive || !bullet.IsPlayerBullet) continue;
                bool hit = false;
                foreach (var enemy in model.CurrentLevel.Enemies)
                {
                    if (!enemy.IsAlive) continue;
                    float dx = enemy.Position.X - bullet.Position.X;
                    float dy = enemy.Position.Y - bullet.Position.Y;
                    float distSq = dx * dx + dy * dy;
                    float radSum = enemy.Radius + 4f;
                    if (distSq < radSum * radSum)
                    {
                        if (enemy is TricksterEnemyModel trickster)
                        {
                            if (!trickster.IsVulnerable)
                            {
                                _tricksterRebind.OnTricksterHit(trickster, model);
                                bullet.IsActive = false;
                                hit = true;
                                break;
                            }
                            else
                            {
                                enemy.IsAlive = false;
                                bullet.IsActive = false;
                                hit = true;
                                break;
                            }
                        }
                        else
                        {
                            enemy.IsAlive = false;
                            bullet.IsActive = false;
                            hit = true;
                            break;
                        }
                    }
                }
                if (hit) continue;
            }
        }
    }
}
