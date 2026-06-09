using Microsoft.Xna.Framework;
using myGame.Model.map;
using myGame.Model.player;

namespace myGame.Model
{
    public enum GameMode
    {
        Menu, Game, Help, Victory, GameOver
    }
    public class GameModel
    {
        public GameMode CurrentMode { get; set; } = GameMode.Menu;
        public LevelModel CurrentLevel { get; set; }
        public PlayerModel Player { get; set; }
        public bool ExitGame { get; set; } = false;
        public int ViewportWidth { get; set; } = 1920;   
        public int ViewportHeight { get; set; } = 1080;
    }
}
