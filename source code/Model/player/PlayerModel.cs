using Microsoft.Xna.Framework;

namespace myGame.Model.player
{
    public class PlayerModel
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; } = 16f;
        public Vector2 Velocity { get; set; }
        public bool IsAlive { get; set; } = true;

        public bool ControlsSwapped { get; set; } = false;
        public float SwapTimer { get; set; } = 0f;
        public float ShootCooldown { get; set; } = 0.5f;
        public float ShootTimer { get; set; }
        public int MaxHealth { get; set; } = 3;
        public int Health { get; set; } = 3;
        public float InvincibilityTimer { get; set; }
        public float InvincibilityDuration { get; set; } = 1.5f;
        public bool IsInvincible => InvincibilityTimer > 0;
    }
}
