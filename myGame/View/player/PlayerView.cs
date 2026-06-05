using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using myGame.Model.player;

namespace myGame.player
{
    public class PlayerView
    {
        public void Draw(SpriteBatch spriteBatch, PlayerModel player)
        {
            Texture2D pixel = GetPixelTexture(spriteBatch.GraphicsDevice);
            Rectangle rect = new Rectangle(
                (int)(player.Position.X - player.Radius),
                (int)(player.Position.Y - player.Radius),
                (int)(player.Radius * 2),
                (int)(player.Radius * 2));
            spriteBatch.Draw(pixel, rect, Color.Cyan);
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
