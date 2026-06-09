using Microsoft.Xna.Framework;
using System.Collections.Generic;
using myGame.Controller.enemies;

namespace myGame.Model.enemies
{
    public class EnemyModel
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; } = 12f;
        public int Health { get; set; } = 1;
        public bool IsAlive { get; set; } = true;
        public List<Vector2> PatrolPoints { get; set; } = new List<Vector2>();

        public EnemyState State { get; set; } = EnemyState.Patrol;
        public List<Vector2> Path { get; set; }          
        public float PathRecalcTimer { get; set; }       

        private int _currentTargetIndex = 0;

        public Vector2 GetCurrentTarget()
        {
            if (PatrolPoints.Count == 0) return Position;
            return PatrolPoints[_currentTargetIndex];
        }

        public void MoveToNextTarget()
        {
            if (PatrolPoints.Count == 0) return;
            _currentTargetIndex = (_currentTargetIndex + 1) % PatrolPoints.Count;
        }
    }
}
