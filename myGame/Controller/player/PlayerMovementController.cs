using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using myGame.Model.player;


namespace myGame.Controller.player
{
    public class PlayerMovementController
    {
        private float _speed = 300f;
        private float _friction = 0.95f;
        public void Update(PlayerModel player, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 input = Vector2.Zero;
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up)) input.Y -= 1;
            if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down)) input.Y += 1;
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left)) input.X -= 1;
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right)) input.X += 1;
            if (input != Vector2.Zero) input.Normalize();

            player.Velocity += input * _speed * dt;
            player.Velocity *= _friction;
            player.Position += player.Velocity * dt;
        }
    }
}
