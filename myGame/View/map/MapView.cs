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

        public void Draw(SpriteBatch spriteBatch, LevelModel level)
        {
            if (level == null) return;
            Texture2D pixel = GetPixelTexture(spriteBatch.GraphicsDevice);

            // Пол – очень тёмный синий
            spriteBatch.Draw(pixel, new Rectangle(0, 0, level.Width, level.Height), new Color(15, 20, 35));

            // Стены – приглушённый сине-серый
            foreach (var wall in level.Walls)
            {
                spriteBatch.Draw(pixel, wall, new Color(50, 60, 80));
            }
        }
    }
}