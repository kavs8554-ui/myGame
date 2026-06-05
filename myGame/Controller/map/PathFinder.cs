using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace myGame.Controller.map
{
    public static class PathFinder
    {
        public static List<Vector2> FindPath(Vector2 start, Vector2 goal, bool[,] walkable, int cellSize)
        {
            int width = walkable.GetLength(1);
            int height = walkable.GetLength(0);

            Point startCell = WorldToCell(start, cellSize);
            Point goalCell = WorldToCell(goal, cellSize);

            if (!IsValid(startCell, width, height) || !IsValid(goalCell, width, height))
                return new List<Vector2>();

            if (!walkable[startCell.Y, startCell.X] || !walkable[goalCell.Y, goalCell.X])
                return new List<Vector2>();

            var cameFrom = new Dictionary<Point, Point>();
            var costSoFar = new Dictionary<Point, float>();
            var frontier = new PriorityQueue<Point>();

            frontier.Enqueue(startCell, 0);
            costSoFar[startCell] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                if (current.Equals(goalCell))
                    break;

                foreach (var neighbor in GetNeighbors(current, width, height))
                {
                    if (!walkable[neighbor.Y, neighbor.X]) continue;

                    float newCost = costSoFar[current] + 1;
                    if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                    {
                        costSoFar[neighbor] = newCost;
                        float priority = newCost + Heuristic(neighbor, goalCell);
                        frontier.Enqueue(neighbor, priority);
                        cameFrom[neighbor] = current;
                    }
                }
            }

            if (!cameFrom.ContainsKey(goalCell))
                return new List<Vector2>();

            var path = new List<Vector2>();
            var cur = goalCell;
            while (!cur.Equals(startCell))
            {
                path.Add(CellToWorld(cur, cellSize));
                cur = cameFrom[cur];
            }
            path.Reverse();
            return path;
        }

        private static float Heuristic(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        private static IEnumerable<Point> GetNeighbors(Point p, int width, int height)
        {
            if (p.X > 0) yield return new Point(p.X - 1, p.Y);
            if (p.X < width - 1) yield return new Point(p.X + 1, p.Y);
            if (p.Y > 0) yield return new Point(p.X, p.Y - 1);
            if (p.Y < height - 1) yield return new Point(p.X, p.Y + 1);
        }

        private static bool IsValid(Point p, int width, int height) =>
            p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height;

        private static Point WorldToCell(Vector2 world, int cellSize) =>
            new Point((int)(world.X / cellSize), (int)(world.Y / cellSize));

        private static Vector2 CellToWorld(Point cell, int cellSize) =>
            new Vector2(cell.X * cellSize + cellSize / 2, cell.Y * cellSize + cellSize / 2);

        private class PriorityQueue<T>
        {
            private List<(T item, float priority)> elements = new List<(T, float)>();
            public int Count => elements.Count;
            public void Enqueue(T item, float priority)
            {
                elements.Add((item, priority));
                int ci = elements.Count - 1;
                while (ci > 0)
                {
                    int pi = (ci - 1) / 2;
                    if (elements[ci].priority >= elements[pi].priority) break;
                    var tmp = elements[ci];
                    elements[ci] = elements[pi];
                    elements[pi] = tmp;
                    ci = pi;
                }
            }
            public T Dequeue()
            {
                int li = elements.Count - 1;
                var frontItem = elements[0];
                elements[0] = elements[li];
                elements.RemoveAt(li);

                li--;
                int pi = 0;
                while (true)
                {
                    int ci = pi * 2 + 1;
                    if (ci > li) break;
                    int rc = ci + 1;
                    if (rc <= li && elements[rc].priority < elements[ci].priority) ci = rc;
                    if (elements[pi].priority <= elements[ci].priority) break;
                    var tmp = elements[pi];
                    elements[pi] = elements[ci];
                    elements[ci] = tmp;
                    pi = ci;
                }
                return frontItem.item;
            }
        }
    }
}