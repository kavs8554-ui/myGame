using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using myGame.Model.map;

namespace myGame.View.map
{
    public class MapView
    {
        public void Draw(SpriteBatch spriteBatch, LevelModel level)
        {
            if (level == null) return;
            foreach (var wall in level.Walls)
            {
                Texture2D pixel = GetPixelTexture(spriteBatch.GraphicsDevice);
                spriteBatch.Draw(pixel, wall, Color.Gray);
            }
        }

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
    }
}
