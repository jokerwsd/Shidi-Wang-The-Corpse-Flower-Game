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
    class HealthObject : GameFramework.MatrixModelObject
    {
        Game1 game;
        public Vector3 position;
        public float distance;
        public Vector3 towards;
        public int amount = 0;

        public HealthObject(Game1 game, Vector3 position, Model model)
            : base(game, position, model)
        {
            this.game = game;
            this.position = position;
        }

        public override void Update(GameTime gameTime)
        {
            distance = (position - game.mgame._ghost.Position).Length();
            towards = game.mgame._ghost.Position - position;
            towards.Normalize();
            if (distance <= 30)
            {
                game.SoundEffects["geto"].Play(0.3f, 0, 0);
                if (game.mgame.health < 100 && game.mgame.health > 0)
                    game.mgame.health++;
                game.GameObjects.Remove(this);
                return;
            }
            
            Transformation = Matrix.CreateWorld(position, new Vector3(0, 0, 1), Vector3.Up);

        }

        public override void Draw(GameTime gameTime, Effect effect)
        {

            base.Draw(gameTime, effect);
        }
    }
}