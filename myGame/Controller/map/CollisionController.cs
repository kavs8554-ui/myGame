using Microsoft.Xna.Framework;
using myGame.Controller.enemies;
using myGame.Model;
using myGame.Model.enemies;
using myGame.Model.map;
using myGame.Model.player;
using System;

namespace myGame.Controller.map
{
    public class CollisionController
    {
        private TricksterRebindController _chaosRebind = new TricksterRebindController();

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

            foreach (var enemy in model.CurrentLevel.Enemies)
            {
                if (!enemy.IsAlive) continue;

                float dx = player.Position.X - enemy.Position.X;
                float dy = player.Position.Y - enemy.Position.Y;
                float distSq = dx * dx + dy * dy;
                float radSum = player.Radius + enemy.Radius;

                if (distSq < radSum * radSum)
                {
                    if (enemy is TricksterEnemyModel chaos)
                    {
                        if (!chaos.IsVulnerable)
                        {
                            _chaosRebind.OnChaosHit(chaos, model);
                            return; 
                        }
                        else
                        {
                            enemy.IsAlive = false;
                            return; 
                        }
                    }
                    else
                    {
                        player.IsAlive = false;
                        return;
                    }
                }
            }

            foreach (var bullet in model.CurrentLevel.Bullets)
            {
                if (!bullet.IsActive) continue;

                float dx = player.Position.X - bullet.Position.X;
                float dy = player.Position.Y - bullet.Position.Y;
                float distSq = dx * dx + dy * dy;
                float radSum = player.Radius + 4f; 

                if (distSq < radSum * radSum)
                {                
                    player.IsAlive = false;
                    bullet.IsActive = false;
                    return;
                }
            }
        }

        public void Update(GameModel model)
        {
            CollideWithWalls(model);
            CollideWithBulletsAndEnemies(model);
        }
    }
}
