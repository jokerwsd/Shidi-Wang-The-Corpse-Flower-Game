using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using GameFramework;

namespace Shidi_Wang_Project
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : GameFramework.GameHost
    {
        internal GraphicsDeviceManager _graphics;
        internal SpriteBatch _spriteBatch;
        public SoundEffect sound = null;
        public SoundEffectInstance loopsound = null;
        public SoundEffectInstance Bgm = null;
        public SoundEffectInstance Bgm2 = null;
        public BasicEffect _effect;


        SpriteFont Times_New_Roman;

        


        public List<BulletObject> BulletsObjects = new List<BulletObject>();
        public Model modelbullet;
        public Matrix projection;
        public Matrix view; 


       

        public Mode_Game mgame;


        Random rnd = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected async override void Initialize()
        {

            //Add game mode handlers 
            AddGameModeHandler(new Mode_Menu(this));
            AddGameModeHandler(mgame =  new Mode_Game(this));
            AddGameModeHandler(new Mode_HighScores(this));
            // Initialize and load the high scores
            HighScores.InitializeTable("Normal", 10);
            await HighScores.LoadScoresAsync();
            TouchPanel.EnableMouseTouchPoint = true;


            // Calculate the screen aspect ratio
            float aspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
            // Create a projection matrix
           
              projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 0.1f, 500000.0f);

            // Calculate a view matrix (where we are looking from and to)
              view = Matrix.CreateLookAt(new Vector3(0, 5, 5), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            // Create and initialize the effect
            _effect = new BasicEffect(GraphicsDevice);
            _effect.VertexColorEnabled = false;
            _effect.TextureEnabled = true;
            _effect.Projection = projection;
            _effect.View = view;
            _effect.World = Matrix.Identity;
            _effect.LightingEnabled = true;

            _effect.EnableDefaultLighting();
            //_effect.DirectionalLight0.Enabled = true;
            //_effect.DirectionalLight0.Direction = new Vector3(0, 0, -1);
            //_effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            //_effect.DirectionalLight0.SpecularColor = Color.White.ToVector3();

            _effect.FogEnabled = true;
            _effect.FogStart = 300.0f;
            _effect.FogEnd = 1000.0f;
            _effect.FogColor = Color.Black.ToVector3();
           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            Textures.Add("Grass", Content.Load<Texture2D>("DryLand"));
            Textures.Add("NightSky", Content.Load<Texture2D>("NightSky"));
            Textures.Add("Smoke", Content.Load<Texture2D>("Smoke"));
            Textures.Add("sign", Content.Load<Texture2D>("sign"));
            Textures.Add("bg4", Content.Load<Texture2D>("bg4"));
            Textures.Add("bg5", Content.Load<Texture2D>("bg5"));

            // Load models
            Models.Add("CartoonTree", Content.Load<Model>("CartoonTree"));
            Models.Add("StrangeFlower", Content.Load<Model>("StrangeFlower"));
            Models.Add("HorrorTree", Content.Load<Model>("HorrorTree"));
            Models.Add("Ghost", Content.Load<Model>("Nintendoghost"));
            Models.Add("Bullet", Content.Load<Model>("Heart"));
            Models.Add("Mashroom", Content.Load<Model>("MarioShroom_Red"));
            Models.Add("Orange", Content.Load<Model>("Orange"));
            Models.Add("Root", Content.Load<Model>("root fbx"));
            Models.Add("Cross", Content.Load<Model>("Cross"));


            SoundEffects.Add("shot", Content.Load<SoundEffect>("shot"));
            SoundEffects.Add("shot2", Content.Load<SoundEffect>("shot2"));
            SoundEffects.Add("hit", Content.Load<SoundEffect>("hit"));
            SoundEffects.Add("gfire", Content.Load<SoundEffect>("GhostFire"));
            SoundEffects.Add("geto", Content.Load<SoundEffect>("geto"));
            SoundEffects.Add("Thurder", Content.Load<SoundEffect>("Thurder"));
            SoundEffects.Add("Thurder2", Content.Load<SoundEffect>("Thurder2"));
            SoundEffects.Add("collect", Content.Load<SoundEffect>("collect"));

            Times_New_Roman = Content.Load<SpriteFont>("Times New Roman");
            Fonts.Add("Miramonte", Content.Load<SpriteFont>("Miramonte"));




            // Load songs.
            // Are we in control of the media player?
            if (MediaPlayer.GameHasControl)
            {
                // Load our song
                Songs.Add("bgmusic", Content.Load<Song>("scary"));

                // Play the song, repeating
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(Songs["bgmusic"]);
                MediaPlayer.Volume =0.3f;
            }


            // Reset the game
            SetGameMode<Mode_Menu>();
            CurrentGameModeHandler.Reset();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {  
     //       for (int i = 0; i < BulletsObjects.Count; i++)
       //     BulletsObjects[i].Update(gameTime);
            UpdateAll(gameTime);

       
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
     

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

          
            base.Draw(gameTime);
            

        }

     
      
    }
}
