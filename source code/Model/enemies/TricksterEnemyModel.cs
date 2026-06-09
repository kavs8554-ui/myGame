namespace myGame.Model.enemies
{
    public class TricksterEnemyModel : EnemyModel
    {
        public bool IsVulnerable { get; set; }
        public bool HasTriggeredSwap { get; set; }

        public float ShootCooldown { get; set; } = 2.0f;
        public float ShootTimer { get; set; }
        public float VisionRange { get; set; } = 250f;
        public float BulletSpeed { get; set; } = 180f;
    }
}
