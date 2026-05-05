using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
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

            if (keyboard.IsKeyDown(keys[0]) || keyboard.IsKeyDown(keys[1])) input.Y -= 1; // вверх
            if (keyboard.IsKeyDown(keys[2]) || keyboard.IsKeyDown(keys[3])) input.Y += 1; // вниз
            if (keyboard.IsKeyDown(keys[4]) || keyboard.IsKeyDown(keys[5])) input.X -= 1; // влево
            if (keyboard.IsKeyDown(keys[6]) || keyboard.IsKeyDown(keys[7])) input.X += 1; // вправо

            return input;
        }

        public void Update(GameModel model)
        {
            var currentKeyboardState = Keyboard.GetState();

            if (model.CurrentMode != GameMode.Game)
            {
                if (currentKeyboardState.IsKeyDown(Keys.D1) && !_previousKeyboardState.IsKeyDown(Keys.D1))
                    model.CurrentMode = GameMode.Menu;
                else if (currentKeyboardState.IsKeyDown(Keys.D2) && !_previousKeyboardState.IsKeyDown(Keys.D2))
                    model.CurrentMode = GameMode.Game;
                else if (currentKeyboardState.IsKeyDown(Keys.D3) && !_previousKeyboardState.IsKeyDown(Keys.D3))
                    model.CurrentMode = GameMode.Help;
            }

            _previousKeyboardState = currentKeyboardState;
        }
    }
}
