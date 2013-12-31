using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RenderLibrary
{
    public class Camera
    {
        private GraphicsDevice device;

        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 UpDirection { get; set; }
        public Vector3 CameraPos { get; set; }

        public float AspectRatio { get; set; }
        public float nearPlaneDistance { get; set; }
        public float farPlaneDistance { get; set; }

        public Camera(GraphicsDevice graphics, Vector3 startingPosition)
        {
            this.device = graphics;

            AspectRatio = device.DisplayMode.AspectRatio;
            CameraPos = startingPosition;
            nearPlaneDistance = 1f;
            farPlaneDistance = 200f;

            //startingPosition = new Vector3(500,100000,10);

            this.ViewMatrix = Matrix.CreateLookAt(this.CameraPos, new Vector3(1, 1, 1), new Vector3(0, 10, 0));
            this.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, this.AspectRatio, this.nearPlaneDistance, this.farPlaneDistance);
        }

        public void Update(Vector3 pos, Quaternion rot)
        {
            this.Rotation = Quaternion.Lerp(this.Rotation, rot, 0.1f);

            Vector3 campos = new Vector3(0, 0, -1f);
            campos = Vector3.Transform(campos, Matrix.CreateFromQuaternion(this.Rotation));
            campos += pos;

            Vector3 camup = new Vector3(0, 1, 0);
            camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(this.Rotation));

            this.Position = campos;
            this.UpDirection = camup;

            this.ViewMatrix = Matrix.CreateLookAt(this.Position, pos, this.UpDirection);
            this.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, this.AspectRatio, this.nearPlaneDistance, this.farPlaneDistance);
        }

    }
}
