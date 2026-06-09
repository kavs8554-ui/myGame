
namespace myGame.Model.map
{
    public enum TileType
    {
        Wall, Floor
    }

    public class TileModel
    {
        public TileType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
