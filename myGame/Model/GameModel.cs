using Microsoft.Xna.Framework;
using myGame.Model.map;
using myGame.Model.player;

namespace myGame.Model
{
    public enum GameMode
    {
        Menu, Game, Help
    }
    public class GameModel
    {
        public GameMode CurrentMode { get; set; } = GameMode.Menu;
        public LevelModel CurrentLevel { get; set; }
        public PlayerModel Player { get; set; }
    }
}
