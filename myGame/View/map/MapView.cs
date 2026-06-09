using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using myGame.Model.map;

namespace myGame.View.map
{
    public class MapView
    {
        private Texture2D _pixel;

        private Texture2D GetPixelTexture(GraphicsDevice gd)
        {
            if (_pixel == null)
            {
                _pixel = new Texture2D(gd, 1, 1);
                _pixel.SetData(new[] { Color.White });
            }

            return _pixel;
        }

        public void Draw(
            SpriteBatch spriteBatch,
            LevelModel level,
            Vector2 playerPosition)
        {
            if (level == null)
                return;

            Texture2D pixel = GetPixelTexture(spriteBatch.GraphicsDevice);

            spriteBatch.Draw(
                pixel,
                new Rectangle(0, 0, level.Width, level.Height),
                new Color(15, 25, 70));

            int visionCells = 8;

            int playerCellX =
                (int)(playerPosition.X / level.CellSize);

            int playerCellY =
                (int)(playerPosition.Y / level.CellSize);

            foreach (var wall in level.Walls)
            {
                int wallCellX = wall.Center.X / level.CellSize;
                int wallCellY = wall.Center.Y / level.CellSize;

                int dx = wallCellX - playerCellX;
                int dy = wallCellY - playerCellY;

                if (dx * dx + dy * dy <= visionCells * visionCells)
                {
                    spriteBatch.Draw(
                        pixel,
                        wall,
                        new Color(70, 90, 130));
                }
            }

            for (int y = 0; y < level.Height; y += level.CellSize)
            {
                for (int x = 0; x < level.Width; x += level.CellSize)
                {
                    int cellX = x / level.CellSize;
                    int cellY = y / level.CellSize;

                    int dx = cellX - playerCellX;
                    int dy = cellY - playerCellY;

                    if (dx * dx + dy * dy > visionCells * visionCells)
                    {
                        spriteBatch.Draw(
                            pixel,
                            new Rectangle(
                                x,
                                y,
                                level.CellSize,
                                level.CellSize),
                            Color.Black);
                    }
                }
            }
        }
    }
}