using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Laboration3Datorgrafik;


namespace RenderLibrary
{
    public class Floor : BundleModel
    {

        private VertexBuffer _vertexBuffer;
        private Texture2D _texture;
        private Texture2D _normalMap;
        private Effect effect;
        private ushort _primitiveCount = 0;
        protected Vector3 _position;
        protected Matrix World { get; set; }

        public Floor(GraphicsDevice graphics, Texture2D texture, Texture2D normalMap, ushort width, ushort height, Vector3 position)
            : base(null, position, 0)
        {
            _texture = texture;
            _normalMap = normalMap;
            _position = position;

            VertexPositionNormalTextureTangentBinormal[] verticeData = new VertexPositionNormalTextureTangentBinormal[width * height * 6];
            ushort index = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    verticeData[index] = new VertexPositionNormalTextureTangentBinormal();
                    verticeData[index].Position = new Vector3(x, 0, y);
                    verticeData[index].TextureCoordinate = new Vector2(0, 0);
                    verticeData[index].Normal = Vector3.Up;
                    verticeData[index].Tangent = Vector3.Forward;
                    verticeData[index].Binormal = Vector3.Cross(Vector3.Up, Vector3.Forward);
                    index++;

                    verticeData[index] = new VertexPositionNormalTextureTangentBinormal();
                    verticeData[index].Position = new Vector3(x + 1, 0, y);
                    verticeData[index].TextureCoordinate = new Vector2(1, 0);
                    verticeData[index].Normal = Vector3.Up;
                    verticeData[index].Tangent = Vector3.Forward;
                    verticeData[index].Binormal = Vector3.Cross(Vector3.Up, Vector3.Forward);
                    index++;

                    verticeData[index] = new VertexPositionNormalTextureTangentBinormal();
                    verticeData[index].Position = new Vector3(x, 0, y + 1);
                    verticeData[index].TextureCoordinate = new Vector2(0, 1);
                    verticeData[index].Normal = Vector3.Up;
                    verticeData[index].Tangent = Vector3.Forward;
                    verticeData[index].Binormal = Vector3.Cross(Vector3.Up, Vector3.Forward);
                    index++;

                    _primitiveCount++;

                    verticeData[index] = new VertexPositionNormalTextureTangentBinormal();
                    verticeData[index].Position = new Vector3(x, 0, y + 1);
                    verticeData[index].TextureCoordinate = new Vector2(0, 1);
                    verticeData[index].Normal = Vector3.Up;
                    verticeData[index].Tangent = Vector3.Forward;
                    verticeData[index].Binormal = Vector3.Cross(Vector3.Up, Vector3.Forward);
                    index++;

                    verticeData[index] = new VertexPositionNormalTextureTangentBinormal();
                    verticeData[index].Position = new Vector3(x + 1, 0, y);
                    verticeData[index].TextureCoordinate = new Vector2(1, 0);
                    verticeData[index].Normal = Vector3.Up;
                    verticeData[index].Tangent = Vector3.Forward;
                    verticeData[index].Binormal = Vector3.Cross(Vector3.Up, Vector3.Forward);
                    index++;

                    verticeData[index] = new VertexPositionNormalTextureTangentBinormal();
                    verticeData[index].Position = new Vector3(x + 1, 0, y + 1);
                    verticeData[index].TextureCoordinate = new Vector2(1, 1);
                    verticeData[index].Normal = Vector3.Up;
                    verticeData[index].Tangent = Vector3.Forward;
                    verticeData[index].Binormal = Vector3.Cross(Vector3.Up, Vector3.Forward);
                    index++;

                    _primitiveCount++;
                }
            }

            _vertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionNormalTextureTangentBinormal), verticeData.Length, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(verticeData);
        }
        public void SetEffectParameters(Effect effect) 
        {
            this.effect = effect;
            effect.Parameters["DiffuseColor"].SetValue(new Vector4(1f, 1f, 1f, 0f));
            effect.Parameters["Alpha"].SetValue(1);
            effect.Parameters["SpecularColor"].SetValue(new Vector3(1f, 1f, 1f));
            effect.Parameters["SpecularIntensity"].SetValue(0.4f);
            effect.Parameters["ModelTexture"].SetValue(_texture);
            effect.Parameters["NormalMap"].SetValue(_normalMap);
            effect.Parameters["NormalBumpMapEnabled"].SetValue(true);

        }
        public void Draw(GraphicsDevice graphics, Effect effect, Camera camera)
        {

            this.effect.Parameters["World"].SetValue(camera.WorldMatrix * Matrix.CreateScale(1) * Matrix.CreateTranslation(new Vector3(-25, 0, -25)));
            this.effect.Parameters["View"].SetValue(camera.ViewMatrix);
            this.effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            this.effect.Parameters["EyePosition"].SetValue(camera.Position);

            graphics.SetVertexBuffer(_vertexBuffer);

            this.effect.CurrentTechnique.Passes[0].Apply();

            graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, _primitiveCount);
        }
        
                            

    }
}
