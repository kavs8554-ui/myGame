using myGame.Model.map;

namespace myGame.Controller.map
{
    public class ProceduralGenerator
    {
        public LevelModel GenerateLevel(int width, int height)
        {
            return new LevelModel { Width = width, Height = height };
        }
    }
}
