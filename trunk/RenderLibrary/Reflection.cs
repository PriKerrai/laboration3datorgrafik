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
        private TextureCube cubeMap;
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
            effect.Parameters["CubeMap"].SetValue(cubeMap);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    //Vector3 diffuse = ((BasicEffect)part.Effect).DiffuseColor;
                    //Vector3 specularColor = ((BasicEffect)part.Effect).SpecularColor;
                    //float specularPower = ((BasicEffect)part.Effect).SpecularPower;
                    //Texture2D texture = ((BasicEffect)part.Effect).Texture;
                    //float alpha = ((BasicEffect)part.Effect).Alpha;

                    //part.Effect = effect.Clone();

                    //part.Effect.Parameters["Alpha"].SetValue(alpha);
                    //part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(diffuse, 0.0f));
                    //part.Effect.Parameters["SpecularColor"].SetValue(specularColor);
                    //part.Effect.Parameters["SpecularPower"].SetValue(specularPower);
                    //part.Effect.Parameters["ModelTexture"].SetValue(texture);
                    //effect.Parameters["NormalTextureEnabled"].SetValue(false);

                    part.Effect = effect.Clone();

                    part.Effect.Parameters["Alpha"].SetValue(1);
                    part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(new Vector3(0.8f, 0.8f, 0.8f), 0.0f));
                    part.Effect.Parameters["SpecularColor"].SetValue(new Vector3(0f, 0f, 0f));                        
                    //part.Effect.Parameters["Shininess"].SetValue(16);
                    //part.Effect.Parameters["ModelTexture"].SetValue(new Texture2D());
                    //effect.Parameters["NormalTextureEnabled"].SetValue(false);
                }
            }
        }

        public void GenerateCubeMap()
        {
            RenderTargetCube cube = new RenderTargetCube(graphics, 1024, true, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1, 1f, 50f);
            Camera camera = new Camera(graphics, position);
            camera.ProjectionMatrix = projection;

            graphics.SetRenderTarget(cube, CubeMapFace.PositiveZ);
            graphics.Clear(Color.Gray);
            //effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Backward, Vector3.Up));
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Backward, Vector3.Up);
            renderManager.Draw(camera);

            graphics.SetRenderTarget(cube, CubeMapFace.NegativeZ);
            graphics.Clear(Color.Gray);
            //effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Forward, Vector3.Up));
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Forward, Vector3.Up);
            renderManager.Draw(camera);

            graphics.SetRenderTarget(cube, CubeMapFace.NegativeX);
            graphics.Clear(Color.Gray);
            //effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Left, Vector3.Up));
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Left, Vector3.Up);
            renderManager.Draw(camera);

            graphics.SetRenderTarget(cube, CubeMapFace.PositiveX);
            graphics.Clear(Color.Gray);
            //effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Right, Vector3.Up));
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Right, Vector3.Up);
            renderManager.Draw(camera);

            graphics.SetRenderTarget(cube, CubeMapFace.PositiveY);
            graphics.Clear(Color.Gray);
            //effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Up, Vector3.Forward));
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Up, Vector3.Forward);
            renderManager.Draw(camera);

            graphics.SetRenderTarget(cube, CubeMapFace.NegativeY);
            graphics.Clear(Color.Gray);
            //effect.Parameters["View"].SetValue(Matrix.CreateLookAt(position, position + Vector3.Down, Vector3.Backward));
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Down, Vector3.Backward);
            renderManager.Draw(camera);

            cubeMap = cube;
            graphics.SetRenderTarget(null);
        }
    }
}
