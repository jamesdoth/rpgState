using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using rpgState.States;
using rpgState;
using System.Reflection.Metadata;

namespace rpgState.States
{
    public class GameState : State
    {
        Texture2D playerSprite;
        Texture2D walkUp;
        Texture2D walkDown;
        Texture2D walkLeft;
        Texture2D walkRight;
        Texture2D ball;
        Texture2D skull;

        Texture2D background;
        SpriteFont gameFont;

        int score = 0;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager Content)
          : base(game, graphicsDevice, Content)
        {
            background = Content.Load<Texture2D>("background");
            playerSprite = Content.Load<Texture2D>("player");
            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(-500, -500), Color.White);
            spriteBatch.Draw(playerSprite, new Vector2(0, 0), Color.White);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}