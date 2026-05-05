using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using myGame.Model.player;


namespace myGame.Controller.player
{
    public class PlayerMovementController
    {
        private float _speed = 1300f;
        private float _friction = 0.95f;
        private InputController _input = new InputController();
        public void Update(PlayerModel player, GameTime gameTime)
        {
            if (!player.IsAlive) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 input = _input.GetMovementDirection(player);

            player.Velocity += input * _speed * dt;
            player.Velocity *= _friction;
            player.Position += player.Velocity * dt;

            if (player.ControlsSwapped)
            {
                player.SwapTimer -= dt;
                if (player.SwapTimer <= 0)
                {
                    player.ControlsSwapped = false;
                    player.SwapTimer = 0;
                }
            }
        }
    }
}
