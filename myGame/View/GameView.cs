using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using myGame.Model;
using myGame.View.menu;

namespace myGame.View
{
    public class GameView
    {
        private MenuView _menuView = new MenuView();
        private HelpView _helpView = new HelpView();

        public Color GetBackgroundColor(GameModel model)
        {
            return model.CurrentMode switch
            {
                GameMode.Menu => new Color(15, 20, 35),   
                GameMode.Game => Color.Black,
                GameMode.Help => Color.DarkSlateGray,
                GameMode.Victory => new Color(10, 20, 40),
                _ => Color.Black
            };
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, GameModel model)
        {
            int w = spriteBatch.GraphicsDevice.Viewport.Width;
            int h = spriteBatch.GraphicsDevice.Viewport.Height;

            switch (model.CurrentMode)
            {
                case GameMode.Menu:
                    _menuView.Draw(spriteBatch, font, w, h);
                    break;
                case GameMode.Help:
                    _helpView.Draw(spriteBatch, font, w, h);
                    break;
            }
        }

        public void DrawHUD(SpriteBatch spriteBatch, SpriteFont font, GameModel model)
        {
            if (font == null) return;
            int w = spriteBatch.GraphicsDevice.Viewport.Width;
            int h = spriteBatch.GraphicsDevice.Viewport.Height;
            if (model.CurrentLevel == null || model.Player == null) return;

            string healthText = $"HP: {model.Player.Health}/{model.Player.MaxHealth}";
            Color healthColor = model.Player.Health > 1 ? Color.White : Color.Red;
            spriteBatch.DrawString(font, healthText, new Vector2(20, 40), healthColor);

            if (model.Player.ControlsSwapped)
            {
                string swapText = $"SWAPPED! {model.Player.SwapTimer:F1}s";
                spriteBatch.DrawString(font, swapText, new Vector2(20, 65), Color.OrangeRed);
            }

            if (model.CurrentMode == GameMode.Victory)
            {
                string victoryText = "VICTORY!";
                Vector2 victorySize = font.MeasureString(victoryText);
                spriteBatch.DrawString(font, victoryText,
                    new Vector2((w - victorySize.X) / 2, 200),
                    Color.Gold, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 0);

                string prompt = "Press 1 for next level, Esc for menu";
                Vector2 promptSize = font.MeasureString(prompt);
                spriteBatch.DrawString(font, prompt,
                    new Vector2((w - promptSize.X) / 2, 300), Color.White);
            }
        }
    }
}