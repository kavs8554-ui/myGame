using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using myGame.Model;
using myGame.Model.player;

namespace myGame.Controller
{
    public class InputController
    {
        private KeyboardState _previousKeyboardState;
        private Keys[] _normalMoveKeys = { Keys.W, Keys.Up, Keys.S, Keys.Down, Keys.A, Keys.Left, Keys.D, Keys.Right };
        private Keys[] _swappedMoveKeys = { Keys.S, Keys.Down, Keys.W, Keys.Up, Keys.D, Keys.Right, Keys.A, Keys.Left };

        public Vector2 GetMovementDirection(PlayerModel player)
        {
            Keys[] keys = player.ControlsSwapped ? _swappedMoveKeys : _normalMoveKeys;
            var keyboard = Keyboard.GetState();
            Vector2 input = Vector2.Zero;
            if (keyboard.IsKeyDown(keys[0]) || keyboard.IsKeyDown(keys[1])) input.Y -= 1;
            if (keyboard.IsKeyDown(keys[2]) || keyboard.IsKeyDown(keys[3])) input.Y += 1;
            if (keyboard.IsKeyDown(keys[4]) || keyboard.IsKeyDown(keys[5])) input.X -= 1;
            if (keyboard.IsKeyDown(keys[6]) || keyboard.IsKeyDown(keys[7])) input.X += 1;
            return input;
        }

        public void Update(GameModel model)
        {
            var currentKeyboardState = Keyboard.GetState();

            // Esc – всегда возврат в меню (кроме самого меню)
            if (currentKeyboardState.IsKeyDown(Keys.Escape) && !_previousKeyboardState.IsKeyDown(Keys.Escape)
                && model.CurrentMode != GameMode.Menu)
            {
                model.CurrentMode = GameMode.Menu;
                model.CurrentLevel = null;
                model.Player = null;
                _previousKeyboardState = currentKeyboardState;
                return;
            }

            if (model.CurrentMode != GameMode.Game)
            {
                if (currentKeyboardState.IsKeyDown(Keys.D1) && !_previousKeyboardState.IsKeyDown(Keys.D1))
                {
                    model.CurrentMode = GameMode.Game;
                    model.CurrentLevel = null;
                    model.Player = null;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.D2) && !_previousKeyboardState.IsKeyDown(Keys.D2))
                {
                    System.Diagnostics.Debug.WriteLine("Switching to Help");
                    model.CurrentMode = GameMode.Help;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.D3) && !_previousKeyboardState.IsKeyDown(Keys.D3))
                {
                    // В меню – выход из игры, в остальных – возврат в меню
                    if (model.CurrentMode == GameMode.Menu)
                        model.ExitGame = true;
                    else
                        model.CurrentMode = GameMode.Menu;
                }
            }

            _previousKeyboardState = currentKeyboardState;
        }
    }
}