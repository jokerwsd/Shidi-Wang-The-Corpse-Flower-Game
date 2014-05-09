using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameFramework;

namespace Shidi_Wang_Project
{
    class Mode_HighScores : GameModeBase
    {

        private Game1 _game;

        public Mode_HighScores(Game1 game)
            : base(game)
        {
            _game = game;
        }

        private int GetLatestScore()
        {
            return _game.GetGameModeHandler<Mode_Game>().Score;
        }
        public override void Reset()
        {
            TextObject gameText;

            base.Reset();

            // Clear existing objects
            GameObjects.Clear();

           
        }


        public override void Activate()
        {
            base.Activate();
            // Clear any existing game objects
            GameObjects.Clear();

            // Did the player's score qualify?
            if (_game.HighScores.GetTable("Normal").ScoreQualifies(GetLatestScore()))
            {
                // Yes, so display the input dialog
                GameHelper.BeginShowKeyboardInput(_game._graphics, KeyboardInputCallback, "High score achieved", "Please enter your name", "Name");
            }
            else
            {
                // Show the highscores now. No score added so nothing to highlight
                ResetHighscoreTableDisplay(null);
            }
            
            // Reset the settings
       //     Reset();
        }
        private async void KeyboardInputCallback(bool result, string text)
        {
            HighScoreEntry newEntry = null;

            // Did we get a name from the player?
            if (result && !string.IsNullOrEmpty(text))
            {
                // Add the name to the highscore
                newEntry = _game.HighScores.GetTable("Normal").AddEntry(text, GetLatestScore());
                // Save the scores
                await _game.HighScores.SaveScoresAsync();
                // Store the name so that we can recall it the next time a high score is achieved
                //SettingsManager.SetValue("PlayerName", text);
            }

            // Show the highscores now and highlight the new entry if we have one
            ResetHighscoreTableDisplay(newEntry);
        }

        /// <summary>
        /// Set the game objects collection to display the high score table
        /// </summary>
        /// <param name="highlightEntry">If a new score has been added, pass its entry here and it will be highlighted</param>
        private void ResetHighscoreTableDisplay(HighScoreEntry highlightEntry)
        {
            // Add the title
            GameObjects.Add(new TextObject(_game, _game.Fonts["Miramonte"], new Vector2(_game.GraphicsDevice.Viewport.Bounds.Width / 2, 150), "High Scores", TextObject.TextAlignment.Center, TextObject.TextAlignment.Far));

            // Add the score objects
            _game.HighScores.CreateTextObjectsForTable("Normal", _game.Fonts["Miramonte"], 1f, 200, 45, Color.Gold, Color.LightGray, highlightEntry, Color.Orange);
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            TouchCollection touches;

            // Update all game objects
            _game.UpdateAll(gameTime);

            // Has the player touched the screen?
            touches = TouchPanel.GetState();
            if (touches.Count == 1 && touches[0].State == TouchLocationState.Pressed)
            {
                // Return back to the menu
                Game.SetGameMode<Mode_Menu>();
                _game.GetGameModeHandler<Mode_Game>().Score = 0;
            }

            base.Update(gameTime);
        }


        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Color black_a = Color.Black;
            black_a.A = 100;
            Texture2D black_table=new Texture2D(_game.GraphicsDevice, 1, 1);
            black_table.SetData(new Color[] { Color.White });
            _game._spriteBatch.Begin();
            
            _game._spriteBatch.Draw(_game.Textures["bg4"], new Vector2(_game.GraphicsDevice.Viewport.Bounds.Width / 2, _game.GraphicsDevice.Viewport.Bounds.Height / 2), null, Color.White, 0, new Vector2(_game.Textures["bg4"].Width / 2, _game.Textures["bg4"].Height / 2), (float)_game.GraphicsDevice.Viewport.Bounds.Width / (float)_game.Textures["bg4"].Width, SpriteEffects.None, 0);
            _game._spriteBatch.Draw(black_table, new Vector2(_game.GraphicsDevice.Viewport.Bounds.Width / 2-250, _game.GraphicsDevice.Viewport.Bounds.Height / 2-300), null, black_a, 0, new Vector2(black_table.Width/2, black_table.Height/2), new Vector2(500, 600), SpriteEffects.None, 0);
            _game.DrawText(gameTime, _game._spriteBatch);
            _game._spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
