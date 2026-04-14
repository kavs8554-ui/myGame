using Microsoft.Xna.Framework.Graphics;
using myGame.Model;

namespace myGame.View
{
    public class GameView
    {
        private MapView _mapView = new MapView();
        private PlayerView _playerView = new PlayerView();
        private EnemyView _enemyView = new EnemyView();
        private BulletView _bulletView = new BulletView();
        private MenuView _menuView = new MenuView();
        private HelpView _helpView = new HelpView();

        public void Draw(SpriteBatch spriteBatch, GameModel model)
        {
            
        }
    }
}