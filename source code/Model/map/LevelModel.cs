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
        public List<Rectangle> Walls { get; set; } = new List<Rectangle>();
        public List<EnemyModel> Enemies { get; set; } = new List<EnemyModel>();
        public List<BulletModel> Bullets { get; set; } = new List<BulletModel>();
        public Vector2 PlayerStart { get; set; }

        public bool[,] WalkableGrid { get; set; }
        public int CellSize { get; set; } = 20;
        public List<Vector2> AllWalkablePositions { get; set; } = new List<Vector2>();
        public int GridCellSize { get; set; } = 80;
        public Dictionary<Point, List<Rectangle>> WallGrid { get; set; } = new Dictionary<Point, List<Rectangle>>();
    }
}
