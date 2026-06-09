using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace myGame.View.menu
{
    public class MenuView
    {
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, int width, int height)
        {
            string title = "TRICKSTER'S LABYRINTH";
            string[] items = { "1 - Start Game", "2 - Help", "3 - Exit" };

            Vector2 titleSize = font.MeasureString(title) * 2.0f;
            float itemSpacing = 50;
            float itemScale = 1.2f;
            float totalHeight = titleSize.Y + 40 + items.Length * (font.MeasureString("A").Y * itemScale) + (items.Length - 1) * itemSpacing;
            float startY = (height - totalHeight) / 2;

            spriteBatch.DrawString(font, title,
                new Vector2((width - titleSize.X) / 2, startY),
                Color.Cyan, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 0);

            float itemStartY = startY + titleSize.Y + 40;
            for (int i = 0; i < items.Length; i++)
            {
                Vector2 size = font.MeasureString(items[i]) * itemScale;
                float y = itemStartY + i * (size.Y + itemSpacing);
                spriteBatch.DrawString(font, items[i],
                    new Vector2((width - size.X) / 2, y),
                    Color.LightGray, 0, Vector2.Zero, itemScale, SpriteEffects.None, 0);
            }
        }
    }
}