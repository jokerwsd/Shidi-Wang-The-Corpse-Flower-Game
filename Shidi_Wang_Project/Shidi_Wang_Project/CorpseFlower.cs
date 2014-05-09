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
    public class CorpseFlower : GameFramework.MatrixModelObject
    {
        Game1 game;
        public Vector3 position;
        public float distance;
        public Vector3 towards;
        public int fhealth = 10;

        int upcount = 0;

        public CorpseFlower(Game1 game, Vector3 position, Model model) : base(game,position,model)
        {
            this.game = game;
            this.position = position;
        }
        public override void Update(GameTime gameTime)
        {
            distance = (position - game.mgame._ghost.Position).Length();
            towards = game.mgame._ghost.Position - position;
            towards.Normalize();

            if (distance <= 200)
            {
                game.SoundEffects["gfire"].Play(1 / distance, 0, 0);

            }
            
            if (distance <= 300)
            {
                
                if (upcount % 100 == 0)
                {
                    game.GameObjects.Add(new BulletObject2(game, position, game.Models["Orange"], towards));
                    game.SoundEffects["shot"].Play(0.3f, 0, 0);
                }
            }
            position.Y = 7 - distance / 60f;
            if (fhealth <= 0)
            {
                game.mgame.kill++;
                game.mgame.Ingamescore += 10;
                if(game.mgame.bulletleft<=990)
                game.mgame.bulletleft += 10;
                if(game.mgame.health<=95)
                game.mgame.health += 5;
                game.GameObjects.Remove(this);

                
                return;
            }

            
            
            Transformation = Matrix.CreateWorld(position,new Vector3(0,0,1), Vector3.Up);
            upcount++;
        }

        public override void Draw(GameTime gameTime, Effect effect)
        {

            base.Draw(gameTime, effect);
        }
    }
}
