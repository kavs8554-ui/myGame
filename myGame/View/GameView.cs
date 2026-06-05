using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using myGame.Model;

namespace myGame.View
{
    public class GameView
    {
        public Color GetBackgroundColor(GameModel model)
        {
            return model.CurrentMode switch
            {
                GameMode.Menu => new Color(30, 30, 30),    // тёмный фон для меню
                GameMode.Game => Color.Black,              // игра
                GameMode.Help => Color.DarkSlateGray,      // помощь
                _ => Color.Black
            };
        }

        // Основной Draw для режимов Menu и Help
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, GameModel model)
        {
            int w = spriteBatch.GraphicsDevice.Viewport.Width;
            int h = spriteBatch.GraphicsDevice.Viewport.Height;
            switch (model.CurrentMode)
            {
                case GameMode.Menu:
                    DrawMenu(spriteBatch, font, w, h);
                    break;
                case GameMode.Help:
                    DrawHelp(spriteBatch, font, w, h);
                    break;
            }
        }

        // Игровой HUD (теперь принимает размеры)
        public void DrawHUD(SpriteBatch spriteBatch, SpriteFont font, GameModel model)
        {
            if (font == null) return;
            int w = spriteBatch.GraphicsDevice.Viewport.Width;
            int h = spriteBatch.GraphicsDevice.Viewport.Height;
            if (model.CurrentLevel == null || model.Player == null) return;

            string enemyCount = $"Enemies: {model.CurrentLevel.Enemies.FindAll(e => e.IsAlive).Count}";
            spriteBatch.DrawString(font, enemyCount, new Vector2(20, 20), Color.White);

            string healthText = $"HP: {model.Player.Health}/{model.Player.MaxHealth}";
            Color healthColor = model.Player.Health > 1 ? Color.White : Color.Red;
            spriteBatch.DrawString(font, healthText, new Vector2(20, 40), healthColor);

            if (model.Player.ControlsSwapped)
            {
                string swapText = $"SWAPPED! {model.Player.SwapTimer:F1}s";
                spriteBatch.DrawString(font, swapText, new Vector2(20, 65), Color.OrangeRed);
            }

            // Подсказка управления (снизу по центру)
            string controls = "WASD/Arrows: Move | Click/Space: Shoot | Esc: Menu";
            Vector2 measure = font.MeasureString(controls);
            spriteBatch.DrawString(font, controls, new Vector2((w - measure.X) / 2, h - 25), Color.LightGray);

            // Экран победы
            if (model.CurrentMode == GameMode.Victory)
            {
                string victoryText = "VICTORY!";
                Vector2 victorySize = font.MeasureString(victoryText);
                spriteBatch.DrawString(font, victoryText, new Vector2((w - victorySize.X) / 2, 200), Color.Gold, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 0);

                string prompt = "Press 1 for next level, Esc for menu";
                Vector2 promptSize = font.MeasureString(prompt);
                spriteBatch.DrawString(font, prompt, new Vector2((w - promptSize.X) / 2, 300), Color.White);
            }
        }

        private void DrawMenu(SpriteBatch spriteBatch, SpriteFont font, int width, int height)
        {
            if (font == null) return;
            string title = "TRICKSTER'S LABYRINTH";
            Vector2 titleSize = font.MeasureString(title);
            spriteBatch.DrawString(font, title, new Vector2((width - titleSize.X) / 2, 180), Color.Gold, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 0);

            string[] items = { "1 - Start Game", "2 - Help", "3 - Exit" };
            for (int i = 0; i < items.Length; i++)
            {
                Vector2 size = font.MeasureString(items[i]);
                spriteBatch.DrawString(font, items[i], new Vector2((width - size.X) / 2, 340 + i * 50), Color.LightGreen, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
            }
        }

        private void DrawHelp(SpriteBatch spriteBatch, SpriteFont font, int width, int height)
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