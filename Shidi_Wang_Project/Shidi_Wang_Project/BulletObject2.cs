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
    public class BulletObject2 : GameFramework.MatrixModelObject
    {
    
        Game1 game;
        public Vector3 position;
        public Vector3 delta;
        Vector3 towards;
        public int update_count = 0;
        int upcount = 0;
        public float distance;

        public BulletObject2(Game1 game, Vector3 position, Model model, Vector3 angel)
            : base(game, position, model)
        {
            this.position = position;
            this.game = game;
            this.towards = angel;
        }

        public override void Update(GameTime gameTime)
        {
            position += 4*towards; 
            delta = towards;
            Transformation = Matrix.CreateWorld(position, delta, Vector3.Up);
            upcount ++;
            distance = (position - game.mgame._ghost.Position).Length();
            if (distance < 10)
            {
                game.GameObjects.Add(new SmokeObject(game, Game.Textures["Smoke"], game.mgame._ghost.Position, new Vector3(20f)));
                game.SoundEffects["hit"].Play(0.3f, 0, 0);
                game.GameObjects.Remove(this);
                if (game.mgame.health <= 100 && game.mgame.health > 0)
                    game.mgame.health -= 5;
                return;
            }
            if (upcount > 1000)
            {
                game.GameObjects.Remove(this);
                
                
                return;
            }

        }
  /*     private void AddSmokeParticle()
        {
            // First look for an inactive particle that we can re-use
            foreach (GameObjectBase obj in game.GameObjects)
            {
                // Is this an inactive smoke particle?
                if (obj is SmokeObject && ((SmokeObject)obj).IsActive == false)
                {
                    // Yes, so reset it and return it
                    ((SmokeObject)obj).ResetParticle();
                    return;
                }
            }

            // Couldn't find an inactive particle so create a new one
            game.GameObjects.Add(new SmokeObject(game, Game.Textures["Smoke"], game._ghost.Position, new Vector3(0.08f)));
        }*/

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