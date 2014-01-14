using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RenderLibrary
{
    public class Ground
    {
        GraphicsDevice device;
        VertexBuffer groundBuffer;

        public const int groundWidth = 50;
        public const int groundHeight = 50;
        Color[] floorColors = new Color[2] { Color.White, Color.Black };

        public Ground(GraphicsDevice device)
        {
            this.device = device;
            BuildFloorBuffer();
        }

        public void BuildFloorBuffer()
        {
            List<VertexPositionColor> groundVertices = new List<VertexPositionColor>();

            int counter = 0;

            for (int x = 0; x < groundWidth; x++)
            {
                counter++;
                for (int z = 0; z < groundHeight; z++)
                {
                    counter++;
                    foreach (VertexPositionColor vertex in FloorTile(x, z, floorColors[counter % 2]))
                    {
                        groundVertices.Add(vertex);
                    }
                }
            }

            groundBuffer = new VertexBuffer(device, typeof(VertexPositionColor), groundVertices.Count, BufferUsage.WriteOnly);
            groundBuffer.SetData<VertexPositionColor>(groundVertices.ToArray());
        }

        private List<VertexPositionColor> FloorTile(int xOffset, int zOffset, Color tileColor)
        {
            List<VertexPositionColor> vList = new List<VertexPositionColor>();

            vList.Add(new VertexPositionColor(new Vector3(0 + xOffset, 0, 0 + zOffset), tileColor));
            vList.Add(new VertexPositionColor(new Vector3(1 + xOffset, 0, 0 + zOffset), tileColor));
            vList.Add(new VertexPositionColor(new Vector3(0 + xOffset, 0, 1 + zOffset), tileColor));
            vList.Add(new VertexPositionColor(new Vector3(1 + xOffset, 0, 0 + zOffset), tileColor));
            vList.Add(new VertexPositionColor(new Vector3(1 + xOffset, 0, 1 + zOffset), tileColor));
            vList.Add(new VertexPositionColor(new Vector3(0 + xOffset, 0, 1 + zOffset), tileColor));

            return vList;
        }

        public void Draw(Camera camera)
        {
            BasicEffect effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;
            effect.World = Matrix.Identity;
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();         
                device.SetVertexBuffer(groundBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, groundBuffer.VertexCount / 3);
                
            }
        }
    }
}
