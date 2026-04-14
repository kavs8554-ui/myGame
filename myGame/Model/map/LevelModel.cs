using Microsoft.Xna.Framework;
using myGame.Model.enemies;
using System.Collections.Generic;
using System.IO;

namespace myGame.Model.map
{
    public class LevelModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public TileModel[,] Tiles { get; set; }
        public Vector2 PlayerStart { get; set; }
        public List<EnemyModel> Enemies { get; set; } = new List<EnemyModel>();
        public List<BulletModel> Bullets { get; set; } = new List<BulletModel>();
        public int EnemiesAliveCount { get; set; }
    }
}
