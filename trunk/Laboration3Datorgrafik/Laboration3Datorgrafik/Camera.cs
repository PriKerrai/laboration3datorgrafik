using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laboration3Datorgrafik
{
        
    

    class Camera
    {
        private GraphicsDevice device;

        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }

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

            this.ViewMatrix = Matrix.CreateLookAt(this.CameraPos, new Vector3(0, 0, 1), new Vector3(0, 1, 0));
            this.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, this.AspectRatio, this.nearPlaneDistance, this.farPlaneDistance);
        }
    }
}
