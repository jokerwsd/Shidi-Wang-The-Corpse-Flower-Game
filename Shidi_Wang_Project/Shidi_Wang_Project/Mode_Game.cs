using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameFramework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace Shidi_Wang_Project
{
    public class Mode_Game : GameModeBase
    {
        public float AverageScale = 0.1f;
        public Game1 _game;
        public int bulletleft = 1000;
        public int health = 100;
        public int kill = 0;
        public bool GameIsActive { get; set; }
        Random rnd = new Random();
        public int Ingamescore=0;
        public int Score { get; set; }
        public Ghost ghost;
        public Ghost _ghost;
        public CorpseFlower cflower;
        public int level = 1;
        public Mode_Game(Game1 game)
            : base(game)
        {
            _game = game;
            _game.IsMouseVisible = true;
            // Indicate that the game is not yet active
            GameIsActive = false;
        }

        public Song music = null;

        public enum State {playing,gameover}
        public State state = State.playing; 

        public float level_bannar_Y=-20;
        public float level_bannar_Y_a=0;
        public int over_index;
        

        // The player's final score
        



        /// <summary>
        /// Reset the game to its initial state
        /// </summary>
        public override void Reset()
        {


            state = State.playing;


            // Load songs.
            // Are we in control of the media player?
            MediaPlayer.Play(_game.Songs["bgmusic"]);

            GameObjects.Clear();
            _game.BulletsObjects.Clear();
            _game.GameObjects.Add(new GroundObject(_game, _game.Textures["Grass"]));
            base.Reset();

            //Add some Trees
            for (int i = 0; i < 1000; i++)
            {

                int j = rnd.Next(1, 10000);

                _game.GameObjects.Add(new MatrixModelObject(_game, new Vector3(-j + i * 10, 5, i - j), _game.Models["HorrorTree"]));

            }
            for (int i = 0; i < 100; i++)
            {

                int j = rnd.Next(1, 10000);

                _game.GameObjects.Add(new MatrixModelObject(_game, new Vector3(-j + i * 100, 0, i * 10 - j), _game.Models["Root"]));

            }

            for (int i = 0; i < 100*level; i++)
            {
                int h = rnd.Next(1, 10000);

                _game.GameObjects.Add(new CorpseFlower(_game, new Vector3(-h + i * 100, 7, i * 10 - h), _game.Models["StrangeFlower"]));
            }

            for (int i = 0; i < 100-level*20; i++)
            {
                int h = rnd.Next(1, 10000);

                _game.GameObjects.Add(new HealthObject(_game, new Vector3(-h + i * 100, -20, i * 10 - h), _game.Models["Mashroom"]));
            }
            for (int i = 0; i < 10; i++)
            {
                int h = rnd.Next(1, 10000);

                _game.GameObjects.Add(new CollectObject(_game, new Vector3(-h + i * 100, 2, i * 10 - h), _game.Models["Cross"]));
            }


            ghost = new Ghost(_game, new Vector3(0, 7, 0), _game.Models["Ghost"]);
            GameObjects.Add(ghost);
            _ghost = ghost;
            health = 100;
            bulletleft = 1000;



            // Add the camera to the game
            _game.Camera = new CameraObject(_game);
            _game.Camera.PositionY = 7;

            _game.Camera.ChaseObject = _ghost;
            _game.Camera.ChaseDistance = 150;
            _game.Camera.ChaseElevation = 20f;

            // Add the sky box
            _game.Skybox = new MatrixSkyboxObject(_game, _game.Textures["NightSky"], new Vector3(0, 0.2f, 0), new Vector3(1000, 1000, 1000));
            
            GameIsActive = true;
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            TouchCollection touches = TouchPanel.GetState();

            // Has the player touched the screen?

            if (health <= 0)
            {
                MediaPlayer.Stop();
                state = State.gameover;               
                Score = Ingamescore;
                over_index = 0;
                
            }
            if (state == State.gameover)
            {

                if(TouchPanel.GetState().Count==1)
                Game.SetGameMode<Mode_HighScores>();
                _game.IsMouseVisible = true;
                GameIsActive = false;
                this.Deactivate();
                _game.GameObjects.Clear();
            }
            if (Ingamescore > 200 * level)
            {
                level++;
                Reset();
          
            }

                Game.UpdateAll(gameTime);
             //   updatecount++;         
                base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(Color.Black);
                         
            if (state == State.playing)
            {

                _game.DrawObjects(gameTime, _game._effect);
                _game.DrawParticles(gameTime, _game._effect, _game.Textures["Smoke"]);

                _game.StoreStateBeforeSprites();
                _game._spriteBatch.Begin();
                _game._spriteBatch.Draw(_game.Textures["sign"], new Vector2(_game.GraphicsDevice.Viewport.Bounds.Width - 330, _game.GraphicsDevice.Viewport.Height - 60), null, new Color(200 - health * 2, 50 + health * 2, 0), 0, new Vector2(0, 0), new Vector2(3 * health / 100f, 0.5f), SpriteEffects.None, 0);
                _game._spriteBatch.DrawString(_game.Fonts["Miramonte"], "Health:" + health.ToString() + "%", new Vector2(_game.GraphicsDevice.Viewport.Bounds.Width - 310, _game.GraphicsDevice.Viewport.Height - 45), Color.White);

                _game._spriteBatch.Draw(_game.Textures["sign"], new Vector2(10, _game.GraphicsDevice.Viewport.Height - 60), null, new Color(50 + bulletleft / 5, 250 - bulletleft / 5, 0), 0, new Vector2(0, 0), new Vector2(3 * bulletleft / 1000.0f, 0.5f), SpriteEffects.None, 0);
                _game._spriteBatch.DrawString(_game.Fonts["Miramonte"], "Bullet:" + bulletleft.ToString() + "%", new Vector2(30, _game.GraphicsDevice.Viewport.Height - 45), Color.White);

                _game._spriteBatch.DrawString(_game.Fonts["Miramonte"], "Score:" + Ingamescore.ToString(), new Vector2(_game.GraphicsDevice.Viewport.Width-200, 25), Color.White);

                _game._spriteBatch.DrawString(_game.Fonts["Miramonte"], "Level:" + level.ToString(), new Vector2(30, 25), Color.White);

            }
            else if (state == State.gameover)
            {
                Texture2D redbg = new Texture2D(_game.GraphicsDevice, 1, 1);
                redbg.SetData(new Color[] { Color.BlueViolet });


                _game.StoreStateBeforeSprites();
                _game._spriteBatch.Begin();


                _game._spriteBatch.Draw(redbg, new Vector2(0, 0), null,  Color.BlueViolet, 0, new Vector2(0, 0), new Vector2(_game.GraphicsDevice.Viewport.Bounds.Width, _game.GraphicsDevice.Viewport.Bounds.Height), SpriteEffects.None, 0);
                _game._spriteBatch.DrawString(_game.Fonts["Miramonte"], "Final Score: " + Score.ToString(), new Vector2(_game.GraphicsDevice.Viewport.Bounds.Width / 2 - 90, 450), Color.White);
                _game._spriteBatch.DrawString(_game.Fonts["Miramonte"], "GAME OVER !" , new Vector2(_game.GraphicsDevice.Viewport.Bounds.Width / 2 - 220, 350), Color.White,0f,new Vector2(0,0),3.0f,SpriteEffects.None,0);
            }
            // End the spritebatch
            _game._spriteBatch.End();
            _game.RestoreStateAfterSprites();
            _game.GraphicsDevice.BlendState = BlendState.Opaque;
            _game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            base.Draw(gameTime);
        }
    }
}
