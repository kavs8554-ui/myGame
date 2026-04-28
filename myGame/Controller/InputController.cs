using Microsoft.Xna.Framework.Input;
using myGame.Model;

namespace myGame.Controller
{
    public class InputController
    {
        private KeyboardState _previousKeyboardState;

        public void Update(GameModel model)
        {
            var currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.D1) && !_previousKeyboardState.IsKeyDown(Keys.D1))
                model.CurrentMode = GameMode.Menu;
            else if (currentKeyboardState.IsKeyDown(Keys.D2) && !_previousKeyboardState.IsKeyDown(Keys.D2))
                model.CurrentMode = GameMode.Game;
            else if (currentKeyboardState.IsKeyDown(Keys.D3) && !_previousKeyboardState.IsKeyDown(Keys.D3))
                model.CurrentMode = GameMode.Help;

            _previousKeyboardState = currentKeyboardState;
        }
    }
}
