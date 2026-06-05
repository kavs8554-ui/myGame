using Microsoft.Xna.Framework;
using myGame.Model.enemies;
using myGame.Model.map;
using System;
using System.Collections.Generic;

namespace myGame.Controller.map
{
    public class ProceduralGenerator
    {
        private Random _rng = new Random();
        private const int minRoomSize = 6;    
        private const int minNodeSize = 10;   
        private const int padding = 2;        
        private const int cellSize = 30;     
        private const int corridorWidth = 4;  

        public LevelModel GenerateLevel(int screenWidth, int screenHeight)
        {
            int widthCells = screenWidth / cellSize;
            int heightCells = screenHeight / cellSize;

            int[,] grid = new int[heightCells, widthCells];
            for (int y = 0; y < heightCells; y++)
                for (int x = 0; x < widthCells; x++)
                    grid[y, x] = 1;

            BSPNode root = new BSPNode(0, 0, widthCells, heightCells);
            SplitNode(root);
            CreateRooms(root, grid);
            ConnectChildren(root, grid);

            var level = new LevelModel
            {
                Width = screenWidth,
                Height = screenHeight,
                Walls = new List<Rectangle>(),
                Enemies = new List<EnemyModel>(),
                Bullets = new List<BulletModel>()
            };

            level.WalkableGrid = new bool[heightCells, widthCells];
            for (int y = 0; y < heightCells; y++)
                for (int x = 0; x < widthCells; x++)
                    level.WalkableGrid[y, x] = (grid[y, x] == 0);
            level.CellSize = cellSize;

            level.AllWalkablePositions.Clear();
            for (int y = 0; y < heightCells; y++)
                for (int x = 0; x < widthCells; x++)
                    if (level.WalkableGrid[y, x])
                    {
                        float centerX = x * cellSize + cellSize / 2f;
                        float centerY = y * cellSize + cellSize / 2f;
                        level.AllWalkablePositions.Add(new Vector2(centerX, centerY));
                    }

            level.Walls.Add(new Rectangle(0, 0, screenWidth, cellSize));
            level.Walls.Add(new Rectangle(0, screenHeight - cellSize, screenWidth, cellSize));
            level.Walls.Add(new Rectangle(0, 0, cellSize, screenHeight));
            level.Walls.Add(new Rectangle(screenWidth - cellSize, 0, cellSize, screenHeight));

            for (int y = 0; y < heightCells; y++)
                for (int x = 0; x < widthCells; x++)
                    if (grid[y, x] == 1)
                    {
                        int px = x * cellSize;
                        int py = y * cellSize;
                        level.Walls.Add(new Rectangle(px, py, cellSize, cellSize));
                    }

            BuildWallGrid(level);

            List<Rectangle> rooms = new List<Rectangle>();
            CollectRooms(root, rooms);

            if (rooms.Count == 0)
            {
                level.PlayerStart = new Vector2(screenWidth / 2, screenHeight / 2);
                return level;
            }

            var startRoom = rooms[0];
            level.PlayerStart = new Vector2(
                (startRoom.X + startRoom.Width / 2) * cellSize,
                (startRoom.Y + startRoom.Height / 2) * cellSize
            );

            if (rooms.Count > 1)
            {
                int shooterCount = Math.Min(rooms.Count - 1, rooms.Count / 2);
                for (int i = 0; i < shooterCount; i++)
                {
                    var room = rooms[_rng.Next(1, rooms.Count)];
                    int cx = room.X + room.Width / 2;
                    int cy = room.Y + room.Height / 2;
                    Vector2 pos = new Vector2(cx * cellSize, cy * cellSize);
                    var shooter = new ShooterEnemyModel
                    {
                        Position = pos,
                        Radius = 16f,
                        ShootCooldown = 1.2f,
                        VisionRange = 200f,
                        BulletSpeed = 300f
                    };
                    level.Enemies.Add(shooter);
                }

                var tricksterRoom = rooms[rooms.Count - 1];
                int tx = tricksterRoom.X + tricksterRoom.Width / 2;
                int ty = tricksterRoom.Y + tricksterRoom.Height / 2;
                Vector2 tricksterPos = new Vector2(tx * cellSize, ty * cellSize);
                var trickster = new TricksterEnemyModel
                {
                    Position = tricksterPos,
                    Radius = 16f,
                    IsVulnerable = false,
                    HasTriggeredSwap = false,
                    ShootCooldown = 2.0f,
                    ShootTimer = 1.0f,
                    VisionRange = 250f,
                    BulletSpeed = 180f
                };
                level.Enemies.Add(trickster);
            }

            return level;
        }

        private void SplitNode(BSPNode node)
        {
            if (node.Width < minNodeSize * 2 && node.Height < minNodeSize * 2)
                return;

            bool splitHorizontally;
            if (node.Width > node.Height)
                splitHorizontally = false;
            else if (node.Height > node.Width)
                splitHorizontally = true;
            else
                splitHorizontally = _rng.Next(2) == 0;

            if (splitHorizontally)
            {
                int splitPos = _rng.Next(minNodeSize, node.Height - minNodeSize);
                node.Left = new BSPNode(node.X, node.Y, node.Width, splitPos);
                node.Right = new BSPNode(node.X, node.Y + splitPos, node.Width, node.Height - splitPos);
            }
            else
            {
                int splitPos = _rng.Next(minNodeSize, node.Width - minNodeSize);
                node.Left = new BSPNode(node.X, node.Y, splitPos, node.Height);
                node.Right = new BSPNode(node.X + splitPos, node.Y, node.Width - splitPos, node.Height);
            }

            SplitNode(node.Left);
            SplitNode(node.Right);
        }

        private void CreateRooms(BSPNode node, int[,] grid)
        {
            if (node.Left == null && node.Right == null)
            {
                int maxW = node.Width - padding * 2;
                int maxH = node.Height - padding * 2;
                if (maxW < minRoomSize || maxH < minRoomSize)
                    return;

                int roomW = _rng.Next(minRoomSize, maxW + 1);
                int roomH = _rng.Next(minRoomSize, maxH + 1);
                int roomX = node.X + _rng.Next(padding, node.Width - roomW - padding + 1);
                int roomY = node.Y + _rng.Next(padding, node.Height - roomH - padding + 1);

                node.Room = new Rectangle(roomX, roomY, roomW, roomH);
                for (int y = roomY; y < roomY + roomH; y++)
                    for (int x = roomX; x < roomX + roomW; x++)
                        if (y >= 0 && y < grid.GetLength(0) && x >= 0 && x < grid.GetLength(1))
                            grid[y, x] = 0;
            }
            else
            {
                if (node.Left != null) CreateRooms(node.Left, grid);
                if (node.Right != null) CreateRooms(node.Right, grid);
            }
        }

        private void ConnectChildren(BSPNode node, int[,] grid)
        {
            if (node.Left != null && node.Right != null)
            {
                ConnectChildren(node.Left, grid);
                ConnectChildren(node.Right, grid);

                Point? leftCenter = GetRoomCenter(node.Left);
                Point? rightCenter = GetRoomCenter(node.Right);
                if (leftCenter.HasValue && rightCenter.HasValue)
                {
                    CarveCorridor(grid, leftCenter.Value, rightCenter.Value);
                }
            }
        }

        private Point? GetRoomCenter(BSPNode node)
        {
            if (node.Room.HasValue)
            {
                var r = node.Room.Value;
                return new Point(r.X + r.Width / 2, r.Y + r.Height / 2);
            }
            if (node.Left != null)
            {
                var left = GetRoomCenter(node.Left);
                if (left.HasValue) return left;
            }
            if (node.Right != null)
            {
                var right = GetRoomCenter(node.Right);
                if (right.HasValue) return right;
            }
            return null;
        }

        private void CarveCorridor(int[,] grid, Point from, Point to)
        {
            int x1 = from.X;
            int y1 = from.Y;
            int x2 = to.X;
            int y2 = to.Y;

            int startX = Math.Min(x1, x2);
            int endX = Math.Max(x1, x2);
            for (int y = y1 - corridorWidth / 2; y <= y1 + corridorWidth / 2; y++)
                for (int x = startX; x <= endX; x++)
                    if (y >= 0 && y < grid.GetLength(0) && x >= 0 && x < grid.GetLength(1))
                        grid[y, x] = 0;

            int startY = Math.Min(y1, y2);
            int endY = Math.Max(y1, y2);
            for (int y = startY; y <= endY; y++)
                for (int x = x2 - corridorWidth / 2; x <= x2 + corridorWidth / 2; x++)
                    if (y >= 0 && y < grid.GetLength(0) && x >= 0 && x < grid.GetLength(1))
                        grid[y, x] = 0;
        }

        private void CollectRooms(BSPNode node, List<Rectangle> rooms)
        {
            if (node.Room.HasValue)
                rooms.Add(node.Room.Value);
            if (node.Left != null) CollectRooms(node.Left, rooms);
            if (node.Right != null) CollectRooms(node.Right, rooms);
        }

        private class BSPNode
        {
            public int X, Y, Width, Height;
            public BSPNode Left, Right;
            public Rectangle? Room;

            public BSPNode(int x, int y, int w, int h)
            {
                X = x; Y = y; Width = w; Height = h;
            }
        }

        private void BuildWallGrid(LevelModel level)
        {
            level.WallGrid.Clear();
            int cellSize = level.GridCellSize;

            foreach (var wall in level.Walls)
            {
                int startX = wall.Left / cellSize;
                int startY = wall.Top / cellSize;
                int endX = wall.Right / cellSize;
                int endY = wall.Bottom / cellSize;

                for (int y = startY; y <= endY; y++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        var key = new Point(x, y);
                        if (!level.WallGrid.ContainsKey(key))
                            level.WallGrid[key] = new List<Rectangle>();
                        level.WallGrid[key].Add(wall);
                    }
                }
            }
        }
    }
}