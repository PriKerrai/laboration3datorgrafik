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
                    part.Effect = effect.Clone();

                    part.Effect.Parameters["Alpha"].SetValue(1);
                    part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(new Vector3(0.8f, 0.8f, 0.8f), 0.0f));
                    part.Effect.Parameters["SpecularColor"].SetValue(new Vector3(0f, 0f, 0f));
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
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Backward, Vector3.Up);
            renderManager.Draw(camera);

            graphics.SetRenderTarget(cube, CubeMapFace.NegativeZ);
            graphics.Clear(Color.Gray);
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Forward, Vector3.Up);
            renderManager.Draw(camera);

            graphics.SetRenderTarget(cube, CubeMapFace.NegativeX);
            graphics.Clear(Color.Gray);
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Left, Vector3.Up);
            renderManager.Draw(camera);

            graphics.SetRenderTarget(cube, CubeMapFace.PositiveX);
            graphics.Clear(Color.Gray);
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Right, Vector3.Up);
            renderManager.Draw(camera);

            graphics.SetRenderTarget(cube, CubeMapFace.PositiveY);
            graphics.Clear(Color.Gray);
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Up, Vector3.Forward);
            renderManager.Draw(camera);

            graphics.SetRenderTarget(cube, CubeMapFace.NegativeY);
            graphics.Clear(Color.Gray);
            camera.ViewMatrix = Matrix.CreateLookAt(position, position + Vector3.Down, Vector3.Backward);
            renderManager.Draw(camera);

            cubeMap = cube;
            graphics.SetRenderTarget(null);
        }
    }
}
