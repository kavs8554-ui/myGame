using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using myGame.Controller;
using myGame.Model;
using myGame.player;
using myGame.View;
using myGame.View.enemies;
using myGame.View.map;


namespace myGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameModel _model;
        private GameView _view;
        private InputController _inputController;
        private GameController _gameController;
        private MapView _mapView;
        private PlayerView _playerView;
        private EnemyView _enemyView;
        private BulletView _bulletView;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 800;  
            _graphics.PreferredBackBufferHeight = 600; 
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _model = new GameModel();
            _view = new GameView();
            _inputController = new InputController();
            _gameController = new GameController();
            _mapView = new MapView();
            _playerView = new PlayerView();
            _enemyView = new EnemyView();
            _bulletView = new BulletView();
        }

        protected override void Update(GameTime gameTime)
        {
            _inputController.Update(_model);
            _gameController.Update(_model, gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color bgColor = _view.GetBackgroundColor(_model);
            GraphicsDevice.Clear(bgColor);

            if (_model.CurrentMode == GameMode.Game && _model.CurrentLevel != null)
            {
                _spriteBatch.Begin();
                _mapView.Draw(_spriteBatch, _model.CurrentLevel);
                foreach (var enemy in _model.CurrentLevel.Enemies)
                    _enemyView.Draw(_spriteBatch, enemy);
                foreach (var bullet in _model.CurrentLevel.Bullets)
                    _bulletView.Draw(_spriteBatch, bullet);
                _playerView.Draw(_spriteBatch, _model.Player);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
