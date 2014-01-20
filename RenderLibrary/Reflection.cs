using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RenderLibrary
{
    public class Reflection
    {
        private TextureCube _cubeMap;
        private Vector3 position;
        private GraphicsDevice graphics;
        private RenderManager renderManager;
        private Effect effect;

        public Reflection(Vector3 position, GraphicsDevice graphics, RenderManager renderManager, Effect effect)
        {
            this.position = position;
            this.graphics = graphics;
            this.renderManager = renderManager;
            this.effect = effect;

            GenerateCubeMap();
        }

        public void RemapModel(Effect effect, Model model)
        {
            effect.Parameters["EnvironmentTextureEnabled"].SetValue(true);
            effect.Parameters["CubeMap"].SetValue(_cubeMap);

            if (model != null)
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        Vector3 diffuse = ((BasicEffect)part.Effect).DiffuseColor;
                        Vector3 specularColor = ((BasicEffect)part.Effect).SpecularColor;
                        float specularPower = ((BasicEffect)part.Effect).SpecularPower;
                        Texture2D texture = ((BasicEffect)part.Effect).Texture;
                        float alpha = ((BasicEffect)part.Effect).Alpha;

                        part.Effect = effect.Clone();

                        part.Effect.Parameters["Alpha"].SetValue(alpha);
                        part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(diffuse, 0.0f));
                        part.Effect.Parameters["SpecularColor"].SetValue(specularColor);
                        part.Effect.Parameters["SpecularPower"].SetValue(specularPower);
                        part.Effect.Parameters["ModelTexture"].SetValue(texture);
                        effect.Parameters["NormalTextureEnabled"].SetValue(false);
                    }
                }
            }
        }

        public void GenerateCubeMap()
        {
            RenderTargetCube cube = new RenderTargetCube(graphics, 1024, true, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            effect.Parameters["Projection"].SetValue(Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1, 1f, 50f));
            effect.Parameters["ViewVector"].SetValue(position);

            graphics.SetRenderTarget(cube, CubeMapFace.PositiveZ);
            graphics.Clear(Color.Gray);
            effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Backward, Vector3.Up));
            renderManager.Draw();

            graphics.SetRenderTarget(cube, CubeMapFace.NegativeZ);
            graphics.Clear(Color.Gray);
            effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Forward, Vector3.Up));
            renderManager.Draw();

            graphics.SetRenderTarget(cube, CubeMapFace.NegativeX);
            graphics.Clear(Color.Gray);
            effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Left, Vector3.Up));
            renderManager.Draw();

            graphics.SetRenderTarget(cube, CubeMapFace.PositiveX);
            graphics.Clear(Color.Gray);
            effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Right, Vector3.Up));
            renderManager.Draw();

            graphics.SetRenderTarget(cube, CubeMapFace.PositiveY);
            graphics.Clear(Color.Gray);
            effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Up, Vector3.Forward));
            renderManager.Draw();

            graphics.SetRenderTarget(cube, CubeMapFace.NegativeY);
            graphics.Clear(Color.Gray);
            effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Down, Vector3.Backward));
            renderManager.Draw();

            _cubeMap = cube;

            graphics.SetRenderTarget(null);
            //renderManager.SetViewAndProjection();
            MirrorTextures();
        }

        private void MirrorTextures()
        {
            Color[] data = new Color[1024 * 1024];

            _cubeMap.GetData(CubeMapFace.NegativeX, data);
            MirrorTexture(data, 1024, 1024);
            _cubeMap.SetData(CubeMapFace.NegativeX, data);

            _cubeMap.GetData(CubeMapFace.NegativeY, data);
            MirrorTexture(data, 1024, 1024);
            _cubeMap.SetData(CubeMapFace.NegativeY, data);

            _cubeMap.GetData(CubeMapFace.NegativeZ, data);
            MirrorTexture(data, 1024, 1024);
            _cubeMap.SetData(CubeMapFace.NegativeZ, data);

            _cubeMap.GetData(CubeMapFace.PositiveX, data);
            MirrorTexture(data, 1024, 1024);
            _cubeMap.SetData(CubeMapFace.PositiveX, data);

            _cubeMap.GetData(CubeMapFace.PositiveY, data);
            MirrorTexture(data, 1024, 1024);
            _cubeMap.SetData(CubeMapFace.PositiveY, data);

            _cubeMap.GetData(CubeMapFace.PositiveZ, data);
            MirrorTexture(data, 1024, 1024);
            _cubeMap.SetData(CubeMapFace.PositiveZ, data);
        }

        private void MirrorTexture(Color[] data, int width, int heigth)
        {
            Color temp;
            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width * 0.5f; x++)
                {
                    temp = data[x + (y * width)];
                    data[x + (y * width)] = data[(width - 1 - x) + (y * width)];
                    data[(width - 1 - x) + (y * width)] = temp;
                }
            }
        }
    }
}
