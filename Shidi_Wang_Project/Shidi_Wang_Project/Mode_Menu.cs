using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;
using GameFramework;

namespace Shidi_Wang_Project
{
    class Mode_Menu : GameModeBase
    {

        private Game1 _game;
       
        public Mode_Menu(Game1 game)
            : base(game)
        {
            _game = game;
        }


        public override void Reset()
        {
            base.Reset();

            TextObject gameText;

            // Clear any existing objects
            GameObjects.Clear();
            // Add the "new game" option
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                            new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f, _game.GraphicsDevice.Viewport.Height * 0.5f),
                            "Start new game", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(1.0f);
            gameText.Tag = "NewGame";
            GameObjects.Add(gameText);

            // Add the "High Scores" option
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                            new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f, _game.GraphicsDevice.Viewport.Height * 0.6f),
                            "View the high scores", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(1.0f);
            gameText.Tag = "HighScores";
            GameObjects.Add(gameText);

            // Add the "Exit" option
            gameText = new TextObject(_game, _game.Fonts["Miramonte"],
                            new Vector2(_game.GraphicsDevice.Viewport.Width * 0.5f, _game.GraphicsDevice.Viewport.Height * 0.7f),
                            "Exit", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            gameText.Scale = new Vector2(1.0f);
            gameText.Tag = "Exit";
            GameObjects.Add(gameText);
            
            
        }


        public override void Activate()
        {
            base.Activate();

            // Have the game objects been loaded yet?
            if (GameObjects.Count == 0)
            {
                // No, so exit
                return;
            }

            
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            TouchCollection touches;
            SpriteObject touchedText;

            // Update all game objects
            _game.UpdateAll(gameTime);
            
            // Has the player touched the screen?
            touches = TouchPanel.GetState();
            if (touches.Count == 1 && touches[0].State == TouchLocationState.Pressed)
            {
                // Find which text object (if any) has been touched
                touchedText = Game.GetSpriteAtPoint(touches[0].Position);
                // Did we get something?
                if (touchedText != null)
                {
                    
                    // See what it was
                    switch (touchedText.Tag)
                    {
                        case "NewGame":
                            // Switch to gameplay mode
                            Game.SetGameMode<Mode_Game>();
                            _game.IsMouseVisible = false;
                            // Reset for a new game
                            _game.mgame.Ingamescore = 0;
                            _game.mgame.level = 1;

                            Game.CurrentGameModeHandler.Reset();
                            break;

                        case "HighScores":
                            // Switch to High Scores mode
                            Game.SetGameMode<Mode_HighScores>();
                            break;

                        case "Exit":
                            App.Current.Exit();
                            break;

                       

                    }

                }
            }

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.SteelBlue);

            _game._spriteBatch.Begin();
            _game._spriteBatch.Draw(_game.Textures["bg5"], new Vector2(_game.GraphicsDevice.Viewport.Bounds.Width / 2, _game.GraphicsDevice.Viewport.Bounds.Height / 2), null, Color.White, 0, new Vector2(_game.Textures["bg5"].Width / 2, _game.Textures["bg5"].Height / 2), (float)_game.GraphicsDevice.Viewport.Bounds.Width / (float)_game.Textures["bg5"].Width, SpriteEffects.None, 0);

            _game.DrawText(gameTime, _game._spriteBatch);
            _game._spriteBatch.End();

            base.Draw(gameTime);
        }



    }
}
