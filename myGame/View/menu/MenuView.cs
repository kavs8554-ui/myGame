using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace myGame.View.menu
{
    public class MenuView
    {
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, int width, int height)
        {
            string title = "TRICKSTER'S LABYRINTH";
            Vector2 titleSize = font.MeasureString(title);
            spriteBatch.DrawString(font, title,
                new Vector2((width - titleSize.X) / 2, 180),
                Color.Gold, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 0);

            string[] items = { "1 - Start Game", "2 - Help", "3 - Exit" };
            for (int i = 0; i < items.Length; i++)
            {
                Vector2 size = font.MeasureString(items[i]);
                spriteBatch.DrawString(font, items[i],
                    new Vector2((width - size.X) / 2, 340 + i * 50),
                    Color.LightGreen, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
            }
        }
    }
}
