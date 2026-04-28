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
                GameMode.Menu => Color.Green,
                GameMode.Game => Color.Blue,
                GameMode.Help => Color.Yellow,
                _ => Color.Black
            };
        }
        public void Draw(SpriteBatch spriteBatch, GameModel model)
        {
            
        }
    }
}