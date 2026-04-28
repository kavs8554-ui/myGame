using Microsoft.Xna.Framework;
using myGame.Model;
using myGame.Model.enemies;
using myGame.Model.map;

namespace myGame.Controller.enemies
{
    public class AIController
    {
        private float _speed = 80f;

        public void Update(GameModel model, GameTime gameTime)
        {
            if (model.CurrentLevel == null) return;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < model.CurrentLevel.Bullets.Count; i++)
            {
                var bullet = model.CurrentLevel.Bullets[i];
                if (!bullet.IsActive) continue;
                bullet.Position += bullet.Direction * bullet.Speed * dt;
                
                if (bullet.Position.X < -100 || bullet.Position.X > model.CurrentLevel.Width + 100 ||
                    bullet.Position.Y < -100 || bullet.Position.Y > model.CurrentLevel.Height + 100)
                {
                    bullet.IsActive = false;
                }
            }
            model.CurrentLevel.Bullets.RemoveAll(b => !b.IsActive);

            
            foreach (var enemy in model.CurrentLevel.Enemies)
            {
                if (!enemy.IsAlive) continue;

                Vector2 target = enemy.GetCurrentTarget();
                Vector2 direction = target - enemy.Position;
                if (direction.Length() < 5f)
                {
                    enemy.MoveToNextTarget();
                    target = enemy.GetCurrentTarget();
                    direction = target - enemy.Position;
                }
                if (direction != Vector2.Zero)
                    direction.Normalize();
                enemy.Position += direction * _speed * dt;

                if (enemy is ShooterEnemyModel shooter)
                {
                    float distanceToPlayer = Vector2.Distance(shooter.Position, model.Player.Position);
                    bool lineOfSight = IsLineOfSight(shooter.Position, model.Player.Position, model.CurrentLevel);
                    shooter.SeesPlayer = distanceToPlayer <= shooter.VisionRange && lineOfSight;

                    if (shooter.SeesPlayer && shooter.ShootTimer <= 0)
                    {
                        
                        Vector2 dirToPlayer = model.Player.Position - shooter.Position;
                        dirToPlayer.Normalize();
                        var bullet = new BulletModel
                        {
                            Position = shooter.Position,
                            Direction = dirToPlayer,
                            Speed = shooter.BulletSpeed,
                            Owner = shooter
                        };
                        model.CurrentLevel.Bullets.Add(bullet);
                        shooter.ShootTimer = shooter.ShootCooldown;
                    }
                    else if (shooter.ShootTimer > 0)
                    {
                        shooter.ShootTimer -= dt;
                    }
                }
            }
        }

        private bool IsLineOfSight(Vector2 from, Vector2 to, LevelModel level)
        {
            
            return true;
        }
    }
}
