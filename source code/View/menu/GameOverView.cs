using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace myGame.View.menu
{
    public class GameOverView
    {
        public void Draw(
            SpriteBatch spriteBatch,
            SpriteFont font,
            int width,
            int height)
        {
            string text = "GAME OVER";

            Vector2 size = font.MeasureString(text);

            float scale = 4.0f;

            spriteBatch.DrawString(
                font,
                text,
                new Vector2(
                    (width - size.X * scale) / 2,
                    (height - size.Y * scale) / 2),
                Color.Cyan,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f);
        }
    }
}