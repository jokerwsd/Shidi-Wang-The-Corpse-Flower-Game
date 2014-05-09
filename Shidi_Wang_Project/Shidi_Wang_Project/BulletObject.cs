using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameFramework;
using Microsoft.Xna.Framework.Audio;

namespace Shidi_Wang_Project
{
    public class BulletObject : GameFramework.MatrixModelObject
    {
        Model model;
        Game1 game;
        public Vector3 position;
        public Vector3 delta;
        float direction;
        public int update_count = 0;
        public float distance;


        public BulletObject(Game1 game, Vector3 position, Model model, float angel) : base(game, position, model)
        {
            this.model = model;
            this.position = position;
            this.game = game;
            this.direction = angel;
        }

        public override void Update(GameTime gameTime)
        {          
            position.Z -= (float)Math.Cos(direction) * 5;
            position.X += (float)Math.Sin(direction) * 5;
            Position = new Vector3(position.X, position.Y, position.Z);
            update_count++;

            foreach(GameObjectBase f in game.GameObjects)
            {
                if(f is CorpseFlower)
                {
                    float dist;
                    CorpseFlower fobject;
                    fobject = (CorpseFlower)f;
                    dist = (Position - fobject.position).Length();
                    
                    if (dist < 30)
                    {
                        game.GameObjects.Add(new SmokeObject(game, Game.Textures["Smoke"], fobject.position, new Vector3(20f)));
                        game.SoundEffects["hit"].Play(0.3f, 0, 0); 
                        if(fobject.fhealth<=10 && fobject.fhealth>0)
                        fobject.fhealth--;
                        game.GameObjects.Remove(this);
                       
                        return;
                    }
                    if (update_count > 1000)
                    {
                        game.GameObjects.Remove(this);


                        return;
                    }

                }
            }

            delta = new Vector3(-(float)Math.Sin(direction), 0, (float)Math.Cos(direction));
            Transformation = Matrix.CreateWorld(Position, delta, Vector3.Up);
           }

        public void Draw(GameTime gameTime, Effect effect)
        {
            base.Draw(gameTime, effect);
        }


        /// <summary>
        /// Calculate the position of the plane through its movement paths
        /// </summary>
        /// <param name="splineIndex">The first index of the four-point spline segment</param>
        /// <param name="splineWeight">The weight within the spline segment (0 = start, 1 = end)</param>
        /// <returns></returns>
    }
}
