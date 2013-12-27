using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Laboration3Datorgrafik {
    public class FlyingCamera {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        private float MoveSpeed = 0.5f;

        public FlyingCamera() {
            this.Position = new Vector3(0,5,6);
            this.Rotation = Quaternion.Identity;
            //this.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(180));

        }

        public void ProcessInput(GameTime gameTime)
        {
            #region Controlling airplane

            float turningSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            turningSpeed *= 1.6f * MoveSpeed;
            KeyboardState keys = Keyboard.GetState();

            float yawRot = 0;
            if (keys.IsKeyDown(Keys.Q))
                yawRot += turningSpeed;
            if (keys.IsKeyDown(Keys.E))
                yawRot -= turningSpeed;

            float leftRightRot = 0;
            if (keys.IsKeyDown(Keys.D))
                leftRightRot += turningSpeed;
            if (keys.IsKeyDown(Keys.A))
                leftRightRot -= turningSpeed;

            float upDownRot = 0;
            if (keys.IsKeyDown(Keys.W))
                upDownRot += turningSpeed;
            if (keys.IsKeyDown(Keys.S))
                upDownRot -= turningSpeed;

            float speed = 0;
            if (keys.IsKeyDown(Keys.R))
                speed += turningSpeed;
                Vector3 addVectorForward = Vector3.Transform(new Vector3(0, 0, 2), Rotation);
                Position += addVectorForward * speed;
            if (keys.IsKeyDown(Keys.F))
                speed -= turningSpeed;
                Vector3 addVectorBackward = Vector3.Transform(new Vector3(0, 0, 2), Rotation);
                Position += addVectorBackward * speed;

            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), leftRightRot)
                                       * Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), upDownRot)
                                       * Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), yawRot);

            this.Rotation *= additionalRot;

            #endregion
        }

        public void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
        {
            Vector3 addVector = Vector3.Transform(new Vector3(1, 0, 0), rotationQuat);
            position += addVector * speed;
        }


    }
}
