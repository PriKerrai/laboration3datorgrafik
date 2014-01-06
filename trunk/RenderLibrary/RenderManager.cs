using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Content;

namespace RenderLibrary
{
    public class RenderManager
    {
        private List<BundleModel> bModels = new List<BundleModel>();
        private ContentManager Content;
        private Camera camera;
        Effect ambientEffect;

        public RenderManager(ContentManager content, Camera camera) 
        {
            Content = content;
            this.camera = camera;
        }

        public void AddModelToWorldWithPosition(Vector3 position, string modelPath, float scale)
        {
            ambientEffect = Content.Load<Effect>("Ambient");
            bModels.Add(new BundleModel(position, modelPath, scale));
        }

        public void Load() 
        {
            for (int i = 0; i < bModels.Count; i++)
            {
                bModels[i].bModel = Content.Load<Model>(bModels[i].bPath);
            }
        }

        public void Draw()
        {
            for (int i = 0; i < bModels.Count; i++)
            {
                foreach (ModelMesh mesh in bModels[i].bModel.Meshes)
                {
                    //foreach (BasicEffect effecT in mesh.Effects)
                    //{
                    //    effecT.EnableDefaultLighting();
                    //    effecT.AmbientLightColor = Color.Red.ToVector3();
                    //    effecT.View = camera.ViewMatrix;
                    //    effecT.World = Matrix.Identity * Matrix.CreateScale(bModels[i].bScale) * Matrix.CreateTranslation(bModels[i].bPosition);
                    //    effecT.Projection = camera.ProjectionMatrix;

                    
                    //}
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = ambientEffect;

                        ambientEffect.Parameters["xView"].SetValue(camera.ViewMatrix);
                        ambientEffect.Parameters["xProjection"].SetValue(camera.ProjectionMatrix);
                        ambientEffect.Parameters["xWorld"].SetValue(Matrix.Identity * Matrix.CreateScale(bModels[i].bScale) * Matrix.CreateTranslation(bModels[i].bPosition));
                        ambientEffect.Parameters["AmbientColor"].SetValue(Color.Green.ToVector4());
                        ambientEffect.Parameters["AmbientIntensity"].SetValue(0.1f);
                    }
                    mesh.Draw();
                }
            }
        }
        


    }
}
