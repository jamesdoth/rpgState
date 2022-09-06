using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using rpgState.States;
using rpgState;
using System.Reflection.Metadata;

using Comora;
using rpgState.Managers;
using rpgState.Controls;

namespace rpgState.States
{
    enum Dir
    {
        Down,
        Up,
        Left,
        Right,
    }

    public static class MySounds
    {
        public static SoundEffect projectileSound;
        public static SoundEffect enemyHit;
        public static Song bgMusic;
    }

    public class GameState : State
    {
        Texture2D playerSprite;
        Texture2D walkDown;
        Texture2D walkUp;
        Texture2D walkLeft;
        Texture2D walkRight;
        Texture2D ball;
        Texture2D skull;

        Texture2D background;
        SpriteFont gameFont;

        Player player = new Player();

        Camera _camera;

        int _score = 0;
        int score = 0;
        private ScoreManager _scoreManager;

        private List<Component> _components;

        public SpriteFont buttonFont;
        public Texture2D buttonTexture;
        private Button newGameButton;
        private Button highScoreButton;
        private Button quitGameButton;

        //public Texture2D ngBut;
        //public Texture2D hsBut;
        //public Texture2D qgBut;
        //public Texture2D ngText;
        //public Texture2D hsText;
        //public Texture2D qgText;
        //private gsButton ngShit;
        //private gsButton hsShit;
        //private gsButton qgShit;
        //private List<Component> _shitComponent;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager Content)
          : base(game, graphicsDevice, Content)
        {
            background = Content.Load<Texture2D>("background");
            buttonTexture = Content.Load<Texture2D>("button");
            buttonFont = Content.Load<SpriteFont>("spaceFont");

            //ngBut = Content.Load<Texture2D>("button");
            //hsBut = Content.Load<Texture2D>("button");
            //qgBut = Content.Load<Texture2D>("button");

            playerSprite = Content.Load<Texture2D>("player");
            walkDown = Content.Load<Texture2D>("walkDown");
            walkUp = Content.Load<Texture2D>("walkUp");
            walkLeft = Content.Load<Texture2D>("walkLeft");
            walkRight = Content.Load<Texture2D>("walkRight");

            ball = Content.Load<Texture2D>("ball");
            skull = Content.Load<Texture2D>("skull");

            gameFont = Content.Load<SpriteFont>("galleryFont");
            _camera = new Camera(graphicsDevice);

            player.animations[0] = new SpriteAnimation(walkDown, 4, 8);
            player.animations[1] = new SpriteAnimation(walkUp, 4, 8);
            player.animations[2] = new SpriteAnimation(walkLeft, 4, 8);
            player.animations[3] = new SpriteAnimation(walkRight, 4, 8);

            player.anim = player.animations[0];

            MySounds.projectileSound = Content.Load<SoundEffect>("blip"); // .wav = sound effect
            MySounds.enemyHit = Content.Load<SoundEffect>("explode");
            MySounds.bgMusic = Content.Load<Song>("nature"); // .ogg = songs
            MediaPlayer.Play(MySounds.bgMusic); // .stop() .pause()

            newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(/*player.Position.X*/575, 150),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            highScoreButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(/*player.Position.X*/575, 300),
                Text = "High Scores",
            };

            highScoreButton.Click += HighScoreButton_Click;

            quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(/*player.Position.X*/575, 450),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _scoreManager = ScoreManager.Load();

            _components = new List<Component>()
            {
                newGameButton,
                highScoreButton,
                quitGameButton,
            };

            //_shitComponent = new List<Component>() 
            //{
            //    ngShit,
            //    hsShit,
            //    qgShit,
            //};

            //ngShit = new gsButton(buttonTexture, buttonFont, new Vector2(player.Position.X - 70, player.Position.Y - 200))
            //{
            //    _position = new Vector2(player.Position.X - 70, player.Position.Y - 200),
            //    Text = "New Game",
            //};

            //ngShit.Click += NewGameButton_Click;

            //hsShit = new gsButton(buttonTexture, buttonFont, new Vector2(player.Position.X - 70, player.Position.Y - 50))
            //{
            //    _position = new Vector2(player.Position.X - 70, player.Position.Y - 50),
            //    Text = "High Scores",
            //};

            //hsShit.Click += HighScoreButton_Click;

            //qgShit = new gsButton(buttonTexture, buttonFont, new Vector2(player.Position.X - 70, player.Position.Y + 100))
            //{
            //    _position = new Vector2(player.Position.X - 70, player.Position.Y + 100),
            //    Text = "Quit Game",
            //};

            //qgShit.Click += QuitGameButton_Click;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!player.dead)
            {
                spriteBatch.Begin(_camera);
            } else
            {
                spriteBatch.Begin();
            }

            spriteBatch.Draw(background, new Vector2(-500, -500), Color.White);
            if (!player.dead)
            {
                spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(player.Position.X - 600, player.Position.Y - 325), Color.White);
            } else
            {
                spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(40, 35), Color.White);
            }
            


            foreach (Enemy e in Enemy.enemies)
            {
                e.anim.Draw(spriteBatch);
            }

            foreach (Projectile proj in Projectile.projectiles)
            {
                spriteBatch.Draw(ball, new Vector2(proj.Position.X - 48, proj.Position.Y - 48), Color.White);
            }

            if (!player.dead)
            {
                player.anim.Draw(spriteBatch);
            } else
            {
                foreach (var component in _components)
                {
                    component.Draw(gameTime, spriteBatch);
                }


                //foreach (var comp in _shitComponent)
                //{
                //    comp.Draw(gameTime, spriteBatch);
                //}

                //ngShit.Draw(gameTime, spriteBatch);
                //hsShit.Draw(gameTime, spriteBatch);
                //qgShit.Draw(gameTime, spriteBatch);


                //spriteBatch.Draw(ngBut, new Vector2(player.Position.X - 70, player.Position.Y - 200), Color.White);
                //spriteBatch.Draw(hsBut, new Vector2(player.Position.X - 70, player.Position.Y - 50), Color.White);
                //spriteBatch.Draw(qgBut, new Vector2(player.Position.X - 70, player.Position.Y + 100), Color.White);
                //spriteBatch.DrawString(buttonFont, "New Game", new Vector2(player.Position.X - 129, player.Position.Y - 171), Color.Black);
                //spriteBatch.DrawString(buttonFont, "High Scores", new Vector2(player.Position.X - 129, player.Position.Y - 21), Color.Black);
                //spriteBatch.DrawString(buttonFont, "Quit Game", new Vector2(player.Position.X - 129, player.Position.Y + 129), Color.Black);                
            }

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

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            if (!player.dead)
            {
                Controller.Update(gameTime, skull);
            }
            _camera.Position = player.Position;
            _camera.Update(gameTime);

            foreach (Projectile proj in Projectile.projectiles)
            {
                proj.Update(gameTime);
            }

            foreach (Enemy e in Enemy.enemies)
            {
                e.Update(gameTime, player.Position, player.dead);
                int sum = 32 + e.radius;
                if (Vector2.Distance(player.Position, e.Position) < sum)
                {
                    player.dead = true;
                }
            }

            foreach (Projectile proj in Projectile.projectiles)
            {
                foreach (Enemy enemy in Enemy.enemies)
                {
                    int sum = proj.radius + enemy.radius;
                    if (Vector2.Distance(proj.Position, enemy.Position) < sum)
                    {
                        MySounds.enemyHit.Play(1f, -1.0f, 0f);
                        proj.Collided = true;
                        enemy.Dead = true;
                        _score++;
                        score++;
                    }
                }
            }

            Projectile.projectiles.RemoveAll(p => p.Collided);
            Enemy.enemies.RemoveAll(e => e.Dead);

            if (player.dead)
            {
                _scoreManager.Add(new Models.Score()
                {
                    PlayerName = "jimbo",
                    Value = _score,
                });
                ScoreManager.Save(_scoreManager);
                _score = 0;
                Enemy.enemies.Clear();

                foreach (var component in _components)
                {
                    component.Update(gameTime);
                }
            }
        }
    }
}