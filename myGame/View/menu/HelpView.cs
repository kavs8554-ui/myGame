using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace myGame.View.menu
{
    public class HelpView
    {
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, int width, int height)
        {
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

            spriteBatch.DrawString(font, helpText, new Vector2(100, 80), Color.White);
        }
    }
}