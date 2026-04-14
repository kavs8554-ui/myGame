using Microsoft.Xna.Framework;

namespace myGame.Model.player
{
    public class PlayerModel
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; } = 16f;
        public Vector2 Velocity { get; set; }
        public bool IsAlive { get; set; } = true;
        public bool ControlsSwapped { get; set; }
        public float SwapTimer { get; set; }
    }
}
