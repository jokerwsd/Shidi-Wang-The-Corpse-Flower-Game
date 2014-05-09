using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;

namespace Shidi_Wang_Project
{
    class CameraObject : GameFramework.MatrixCameraObject
    {

        //-------------------------------------------------------------------------------------
        // Class constructors
     /*   public Vector3 position, view, up;
        public Matrix projectionMatrix, viewMatrix;
        private float timeLapse = 0.0f;*/
   //     Game1 game;
        public CameraObject(Game1 game)
            : base(game)
        {
    //        this.game = game;
           /* position = new Vector3(0.0f, 0.9f, 0.0f);
            view = new Vector3(0.0f, 0.9f, -0.5f);
            up = new Vector3(0.0f, 1.0f, 0.0f);*/
        }


        //-------------------------------------------------------------------------------------
        // Object Functions




        /// <summary>
        /// Update the ground position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ChaseObject != null)
            {
                // Yes, so no further camera processing is required
                return;
            }


            // Increase the y axis rotation angle for our camera transformation
            AngleY += MathHelper.ToRadians(0.2f);//change radians to float

            // Reset the position using the identity matrix
            SetIdentity();

            // Rotate the camera
            ApplyTransformation(Matrix.CreateRotationY(AngleY));

            // Set the camera position
         //   ApplyTransformation(Matrix.CreateTranslation(game.camera_position));
            ApplyTransformation(Matrix.CreateTranslation(0, 5, -14));
            // Look at the world origin
            //LookAtTarget = Vector3.Zero;//Change
            LookAtTarget = Vector3.Zero;
        }

    }
}
