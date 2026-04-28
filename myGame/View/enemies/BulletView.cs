using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using myGame.Model.map;

namespace myGame.View.enemies
{
    public class BulletView
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

        public void Draw(SpriteBatch spriteBatch, BulletModel bullet)
        {
            if (!bullet.IsActive) return;
            Texture2D tex = GetPixelTexture(spriteBatch.GraphicsDevice);
            spriteBatch.Draw(tex, new Rectangle((int)bullet.Position.X - 3, (int)bullet.Position.Y - 3, 6, 6), Color.Yellow);
        }
    }
}
