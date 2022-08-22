using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using rpgState.States;
using rpgState;
using rpgState.Controls;
using System.Reflection.Metadata;

namespace rpgState.States
{
    public class MenuState : State
    {
        private List<Component> _components;

        public Texture2D buttonTexture;
        public SpriteFont buttonFont;
        private Texture2D background;
        private Button newGameButton;
        private Button highScoreButton;
        private Button quitGameButton;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            buttonTexture = _content.Load<Texture2D>("button");
            buttonFont = _content.Load<SpriteFont>("spaceFont");
            background = _content.Load<Texture2D>("space");           

            newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(575, 150),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            highScoreButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(575, 300),
                Text = "High Scores",
            };

            highScoreButton.Click += HighScoreButton_Click;

            quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(575, 450),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
            {
                newGameButton,
                highScoreButton,
                quitGameButton,
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Vector2(-500, -500), Color.White);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        private void HighScoreButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new HighScoreState(_game, _graphicsDevice, _content));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}