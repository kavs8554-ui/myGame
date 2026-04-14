using Microsoft.Xna.Framework;
using myGame.Model.map;

namespace myGame.Model
{
    public enum GameMode
    {
        Menu, Game, Help
    }
    public class GameModel
    {
        public GameMode CurrentMode { get; set; } = GameMode.Menu;
        public int Score { get; set; }
        public bool IsPaused { get; set; }
        public float GlobalTimer { get; set; }
        public LevelModel CurrentLevel { get; set; }
    }
}
