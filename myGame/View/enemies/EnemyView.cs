using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using myGame.Model.enemies;

namespace myGame.View.enemies
{
    public class EnemyView
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

        public void Draw(SpriteBatch spriteBatch, EnemyModel enemy)
        {
            if (!enemy.IsAlive) return;
            Texture2D tex = GetPixelTexture(spriteBatch.GraphicsDevice);
            Rectangle rect = new Rectangle(
                (int)(enemy.Position.X - enemy.Radius),
                (int)(enemy.Position.Y - enemy.Radius),
                (int)(enemy.Radius * 2),
                (int)(enemy.Radius * 2));
            spriteBatch.Draw(tex, rect, Color.Red);
        }
    }
}
