using Microsoft.Xna.Framework;
using myGame.Model.enemies;

namespace myGame.Model.map
{
    public class BulletModel
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; } = 300f;
        public bool IsActive { get; set; } = true;
        public EnemyModel Owner { get; set; }
    }
}
