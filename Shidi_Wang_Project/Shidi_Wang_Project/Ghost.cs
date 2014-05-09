using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameFramework;

namespace Shidi_Wang_Project
{
    public class Ghost : GameFramework.MatrixModelObject
    {
        Game1 game;
        public float VangleX = 0;
        public float VangleY = 0;

        float x = 0;
        float y = 0;
        float z = 0;

        public SoundEffectInstance thurder = null;
        public SoundEffectInstance thurder2 = null;


        public Ghost(Game1 game, Vector3 position, Model model) 
            : base(game,position,model)
        {
            this.game = game;
            this.x = position.X;
            this.y = position.Y;
            this.z = position.Z;
            thurder = game.SoundEffects["Thurder"].CreateInstance();
            thurder.Volume = 1.0f;
            thurder2 = game.SoundEffects["Thurder2"].CreateInstance();
            thurder2.Volume = 1.0f;
            

        }
        public override void Update(GameTime gameTime)
        {
            Vector3 delta;


            base.Update(gameTime);
            
            
            if (Keyboard.GetState().IsKeyDown(Keys.S) == true)
            {
                z += (float)Math.Cos(VangleY);
                x -= (float)Math.Sin(VangleY);

            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) == true)
            {
                z -= (float)Math.Cos(VangleY);
                x += (float)Math.Sin(VangleY);

            }
            if (Keyboard.GetState().IsKeyDown(Keys.A) == true)
            {
               // x += (float)Math.Cos(VangleY);
                VangleY += MathHelper.ToRadians(-1);
                z -= (float)Math.Cos(VangleY) ;
                x += (float)Math.Sin(VangleY) ;
                thurder2.Play();

            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) == true)
            {
                //x -= (float)Math.Cos(VangleY);
                VangleY += MathHelper.ToRadians(1);
                z -= (float)Math.Cos(VangleY) ;
                x += (float)Math.Sin(VangleY) ;
                thurder.Play();
            }

            Position = new Vector3(x, y, z);

            delta = new Vector3(-(float)Math.Sin(VangleY), 0, (float)Math.Cos(VangleY));

            Transformation = Matrix.CreateWorld(Position, delta, Vector3.Up);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)          
            {
                if (game.mgame.bulletleft >= 0 && UpdateCount%3 == 0)
                {
                    game.GameObjects.Add(new BulletObject(game, Position, game.Models["Bullet"], VangleY));
                    game.SoundEffects["shot2"].Play(0.3f, 0, 0);
                    game.mgame.bulletleft--;
                }
                
            }     
        }


        public override void Draw(GameTime gameTime, Effect effect)
        {



            base.Draw(gameTime, effect);
        }
    }
}
