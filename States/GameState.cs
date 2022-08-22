﻿using System;
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

        int score = 0;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager Content)
          : base(game, graphicsDevice, Content)
        {
            background = Content.Load<Texture2D>("background");
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
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(_camera);

            spriteBatch.Draw(background, new Vector2(-500, -500), Color.White);
            spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(player.Position.X - 600, player.Position.Y - 325), Color.White);

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
            }

            spriteBatch.End();
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
                        score++;
                    }
                }
            }

            Projectile.projectiles.RemoveAll(p => p.Collided);
            Enemy.enemies.RemoveAll(e => e.Dead);
        }
    }
}