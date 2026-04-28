using Microsoft.Xna.Framework;
using myGame.Model.enemies;
using System.Collections.Generic;
using System.IO;

namespace myGame.Model.map
{
    public class LevelModel
    {
        public List<Rectangle> Walls { get; set; } = new List<Rectangle>();
        public Vector2 PlayerStart { get; set; } = new Vector2(100, 100);
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 600;
        public List<EnemyModel> Enemies { get; set; } = new List<EnemyModel>();
        public List<BulletModel> Bullets { get; set; } = new List<BulletModel>();
    }
}
