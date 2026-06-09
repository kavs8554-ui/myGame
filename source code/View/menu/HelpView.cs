using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace myGame.View.menu
{
    public class HelpView
    {
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, int width, int height)
        {
            string title = "HOW TO PLAY";
            string helpText =
                "Movement: WASD or Arrow Keys\n" +
                "Shoot: Left Mouse Button or Space\n" +
                "Menu: Esc (in game) or 1/2/3 in menus\n" +
                "Exit: Press 3 in Main Menu\n\n" +
                "Enemies:\n" +
                "- Red patrol and chase you, firing yellow bullets.\n" +
                "- The purple Trickster fires slow purple bullets.\n" +
                "  They invert your movement keys for 5 seconds!\n" +
                "  He becomes vulnerable only when he's the last enemy.\n\n" +
                "Clear the level to win. Press Esc to return to menu.";

            float titleScale = 1.5f;
            Vector2 titleSize = font.MeasureString(title) * titleScale;
            Vector2 textSize = font.MeasureString(helpText);
            float gap = 30;
            float totalHeight = titleSize.Y + gap + textSize.Y;
            float startY = (height - totalHeight) / 2;

            spriteBatch.DrawString(font, title,
                new Vector2((width - titleSize.X) / 2, startY),
                Color.Cyan, 0, Vector2.Zero, titleScale, SpriteEffects.None, 0);

            float textX = (width - textSize.X) / 2;
            float textY = startY + titleSize.Y + gap;
            spriteBatch.DrawString(font, helpText, new Vector2(textX, textY), Color.White);
        }
    }
}