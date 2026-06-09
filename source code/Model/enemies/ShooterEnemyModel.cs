namespace myGame.Model.enemies
{
    public class ShooterEnemyModel : EnemyModel
    {
        public float ShootCooldown { get; set; } = 1.5f;
        public float ShootTimer { get; set; }
        public float VisionRange { get; set; } = 200f;
        public float BulletSpeed { get; set; } = 300f;
        public bool SeesPlayer { get; set; }
    }
}
