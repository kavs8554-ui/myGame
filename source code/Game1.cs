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
        private SpriteFont _font;
        private Texture2D _enemyTexture;
        private Texture2D _tricksterTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            Window.Title = "Trickster's Labyrinth";
        }

        protected override void Initialize()
        {
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
            _enemyTexture = Content.Load<Texture2D>("Sprites/enemy");
            _tricksterTexture = Content.Load<Texture2D>("Sprites/trickster");
            _bulletView = new BulletView();
            _font = Content.Load<SpriteFont>("Arial");
            _model.ViewportWidth = _graphics.PreferredBackBufferWidth;
            _model.ViewportHeight = _graphics.PreferredBackBufferHeight;
        }

        protected override void Update(GameTime gameTime)
        {
            _inputController.Update(_model);
            if (_model.ExitGame)
                Exit();

            _gameController.Update(_model, gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color bgColor = _view.GetBackgroundColor(_model);
            GraphicsDevice.Clear(bgColor);

            bool drawLevel = _model.CurrentMode == GameMode.Game && _model.CurrentLevel != null;

            if (drawLevel)
            {
                _spriteBatch.Begin();
                _mapView.Draw(
                    _spriteBatch,
                    _model.CurrentLevel,
                    _model.Player.Position
                );
                foreach (var enemy in _model.CurrentLevel.Enemies)
                {
                    if (!IsVisible(enemy.Position))
                        continue;

                    _enemyView.Draw(
                        _spriteBatch, enemy, _enemyTexture, _tricksterTexture);
                }
                foreach (var bullet in _model.CurrentLevel.Bullets)
                {
                    if (!IsVisible(bullet.Position))
                        continue;

                    _bulletView.Draw(_spriteBatch, bullet);
                }
                _playerView.Draw(_spriteBatch, _model.Player);
                _view.DrawHUD(_spriteBatch, _font, _model);
                _spriteBatch.End();
            }
            else
            {
                _spriteBatch.Begin();
                _view.Draw(_spriteBatch, _font, _model);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private bool IsVisible(Vector2 position)
        {
            int visionCells = 8;

            int playerCellX =
                (int)(_model.Player.Position.X /
                      _model.CurrentLevel.CellSize);

            int playerCellY =
                (int)(_model.Player.Position.Y /
                      _model.CurrentLevel.CellSize);

            int objectCellX =
                (int)(position.X /
                      _model.CurrentLevel.CellSize);

            int objectCellY =
                (int)(position.Y /
                      _model.CurrentLevel.CellSize);

            int dx = objectCellX - playerCellX;
            int dy = objectCellY - playerCellY;

            return dx * dx + dy * dy
                   <= visionCells * visionCells;
        }
    }
}
