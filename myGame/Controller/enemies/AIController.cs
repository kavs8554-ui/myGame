using Microsoft.Xna.Framework;
using myGame.Controller.Map;
using myGame.Model;
using myGame.Model.enemies;
using myGame.Model.map;
using myGame.Model.player;
using System;

namespace myGame.Controller.enemies
{
    public enum EnemyState { Patrol, Chase, Attack }

    public class AIController
    {
        private float _patrolSpeed = 60f;
        private float _chaseSpeedMultiplier = 1.5f;
        private float _pathRecalcInterval = 0.5f;
        private Random _random = new Random();

        public void Update(GameModel model, GameTime gameTime)
        {
            if (model.CurrentLevel == null || model.Player == null) return;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateBullets(model, dt);

            foreach (var enemy in model.CurrentLevel.Enemies)
            {
                if (!enemy.IsAlive) continue;
                UpdateEnemyState(enemy, model, dt);
            }
        }

        private void UpdateBullets(GameModel model, float dt)
        {
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
        }

        private void UpdateEnemyState(EnemyModel enemy, GameModel model, float dt)
        {
            bool seesPlayer = CanSeePlayer(enemy, model.Player, model.CurrentLevel, out float distanceToPlayer);
            float visionRange = (enemy is ShooterEnemyModel shooter) ? shooter.VisionRange : 200f;
            float attackRange = 40f;
            float loseRange = visionRange * 1.5f;
            float attackExitRange = attackRange * 1.5f;

            switch (enemy.State)
            {
                case EnemyState.Patrol:
                    PatrolUpdate(enemy, model.CurrentLevel, dt);
                    if (seesPlayer && distanceToPlayer < visionRange)
                        enemy.State = EnemyState.Chase;
                    break;

                case EnemyState.Chase:
                    ChaseUpdate(enemy, model, dt); // теперь передаём model, а не только player и level
                    if (!seesPlayer && distanceToPlayer > loseRange)
                        enemy.State = EnemyState.Patrol;
                    else if (distanceToPlayer < attackRange)
                        enemy.State = EnemyState.Attack;
                    break;

                case EnemyState.Attack:
                    AttackUpdate(enemy, model, dt);
                    if (distanceToPlayer > attackExitRange)
                        enemy.State = EnemyState.Chase;
                    else if (!seesPlayer && distanceToPlayer > loseRange)
                        enemy.State = EnemyState.Patrol;
                    break;
            }
        }

        private void PatrolUpdate(EnemyModel enemy, LevelModel level, float dt)
        {
            // Случайное блуждание по всей карте
            if (enemy.Path == null || enemy.Path.Count == 0)
            {
                if (level.AllWalkablePositions == null || level.AllWalkablePositions.Count == 0)
                    return;
                Vector2 randomTarget = level.AllWalkablePositions[_random.Next(level.AllWalkablePositions.Count)];
                enemy.Path = PathFinder.FindPath(enemy.Position, randomTarget, level.WalkableGrid, level.CellSize);
                if (enemy.Path == null || enemy.Path.Count == 0)
                    return;
            }

            Vector2 targetWp = enemy.Path[0];
            Vector2 dir = targetWp - enemy.Position;
            if (dir.Length() < 5f)
            {
                enemy.Path.RemoveAt(0);
                return;
            }
            dir.Normalize();
            enemy.Position += dir * _patrolSpeed * dt;
        }

        private void ChaseUpdate(EnemyModel enemy, GameModel model, float dt)
        {
            LevelModel level = model.CurrentLevel;
            PlayerModel player = model.Player;

            // Пересчёт пути к игроку
            if (enemy.Path == null || enemy.PathRecalcTimer <= 0)
            {
                RecalculatePath(enemy, player.Position, level);
                enemy.PathRecalcTimer = _pathRecalcInterval;
            }
            else
            {
                enemy.PathRecalcTimer -= dt;
            }

            // Движение по пути
            if (enemy.Path != null && enemy.Path.Count > 0)
            {
                Vector2 target = enemy.Path[0];
                Vector2 direction = target - enemy.Position;
                if (direction.Length() < 5f)
                {
                    enemy.Path.RemoveAt(0);
                    if (enemy.Path.Count == 0)
                        return;
                    target = enemy.Path[0];
                    direction = target - enemy.Position;
                }
                if (direction != Vector2.Zero)
                    direction.Normalize();
                float chaseSpeed = _patrolSpeed * _chaseSpeedMultiplier;
                enemy.Position += direction * chaseSpeed * dt;
            }

            // Стрельба во время преследования (для стреляющих врагов)
            TryShoot(enemy, model, dt);
        }

        private void AttackUpdate(EnemyModel enemy, GameModel model, float dt)
        {
            // Для стреляющих врагов – стрельба (ближняя дистанция)
            TryShoot(enemy, model, dt);
            // Для обычных врагов (не стреляющих) здесь могла бы быть ближняя атака, но она обрабатывается в CollisionController
        }

        private void TryShoot(EnemyModel enemy, GameModel model, float dt)
        {
            if (enemy is ShooterEnemyModel shooter)
            {
                if (shooter.ShootTimer <= 0)
                {
                    Vector2 dirToPlayer = model.Player.Position - shooter.Position;
                    if (dirToPlayer.Length() > 0.01f)
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
                else
                {
                    shooter.ShootTimer -= dt;
                }
            }
        }

        private void RecalculatePath(EnemyModel enemy, Vector2 targetPos, LevelModel level)
        {
            if (level.WalkableGrid == null) return;
            enemy.Path = PathFinder.FindPath(enemy.Position, targetPos, level.WalkableGrid, level.CellSize);
        }

        private bool CanSeePlayer(EnemyModel enemy, PlayerModel player, LevelModel level, out float distance)
        {
            distance = Vector2.Distance(enemy.Position, player.Position);
            float visionRange = (enemy is ShooterEnemyModel shooter) ? shooter.VisionRange : 200f;
            if (distance > visionRange) return false;
            return HasLineOfSight(enemy.Position, player.Position, level);
        }

        private bool HasLineOfSight(Vector2 from, Vector2 to, LevelModel level)
        {
            if (level.WalkableGrid == null) return true;

            int cellSize = level.CellSize;
            float dx = to.X - from.X;
            float dy = to.Y - from.Y;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);
            if (dist < 0.01f) return true;

            float step = cellSize / 2f;
            int steps = (int)(dist / step) + 1;
            for (int i = 0; i <= steps; i++)
            {
                float t = i / (float)steps;
                float x = from.X + dx * t;
                float y = from.Y + dy * t;
                int cx = (int)(x / cellSize);
                int cy = (int)(y / cellSize);
                if (cx >= 0 && cx < level.WalkableGrid.GetLength(1) &&
                    cy >= 0 && cy < level.WalkableGrid.GetLength(0))
                {
                    if (!level.WalkableGrid[cy, cx])
                        return false;
                }
            }
            return true;
        }
    }
}