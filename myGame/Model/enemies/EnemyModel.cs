using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace myGame.Model.enemies
{
    public abstract class EnemyModel
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; } = 12f;
        public int Health { get; set; } = 1;
        public bool IsAlive { get; set; } = true;
        public List<Vector2> PatrolPoints { get; set; } = new List<Vector2>();
    }
}
