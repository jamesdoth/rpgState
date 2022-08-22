using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using rpgState.Controls;
using rpgState.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rpgState.States
{
    public class HighScoreState : State
    {
        private ScoreManager _scoreManager;
        private SpriteFont _font;

        private List<Component> _components;
        public SpriteFont buttonFont;
        public Texture2D buttonTexture;
        private Button newGameButton;
        private Button quitGameButton;

        public HighScoreState(Game1 game, GraphicsDevice graphicsDevice, ContentManager Content)
            : base(game, graphicsDevice, Content)
        {
            _scoreManager = ScoreManager.Load();
            _font = Content.Load<SpriteFont>("galleryFont");
            buttonTexture = _content.Load<Texture2D>("button");
            buttonFont = _content.Load<SpriteFont>("spaceFont");

            newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(575, 300),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(575, 450),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
            {
                newGameButton,
                quitGameButton,
            };
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Highscores:\n" + string.Join("\n", _scoreManager.Highscores.Select(c => c.PlayerName + ": " + c.Value).ToArray()), new Vector2(680, 10), Color.Red);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
