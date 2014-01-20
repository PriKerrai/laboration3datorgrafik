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
        Effect effectNormalMap,
               effectAmbient;

        public RenderManager(ContentManager content, Camera camera) 
        {
            Content = content;
            this.camera = camera;
            effectAmbient = Content.Load<Effect>("Ambient");
        }


        public void AddBundleModel(BundleModel bModel)
        {
            bModels.Add(bModel);
        }
        public void Load() 
        {
            for (int i = 0; i < bModels.Count; i++)
            {
                bModels[i].bModel = Content.Load<Model>(bModels[i].bModelPath);
                if (bModels[i].bTexture == null)
                    bModels[i].GetTextures(this.effectAmbient);

            }
        }

        public void Draw()
        {
            for (int i = 0; i < bModels.Count; i++)
            {
                foreach (ModelMesh mesh in bModels[i].bModel.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effectAmbient;
                        effectAmbient.Parameters["World"].SetValue(camera.WorldMatrix * mesh.ParentBone.Transform * Matrix.CreateScale(bModels[i].bScale) * Matrix.CreateRotationY(bModels[i].bRotation) * Matrix.CreateTranslation(bModels[i].bPosition));
                        effectAmbient.Parameters["View"].SetValue(camera.ViewMatrix);
                        effectAmbient.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                        effectAmbient.Parameters["ViewVector"].SetValue(camera.Position);
                        effectAmbient.Parameters["ModelTexture"].SetValue(bModels[i].bTexture);
                        
                        if (bModels[i].bNormalMap != null)
                        {
                            effectAmbient.Parameters["NormalBumpMapEnabled"].SetValue(true);
                            effectAmbient.Parameters["NormalMap"].SetValue(bModels[i].bNormalMap);
                        }
                        else
                        {
                            effectAmbient.Parameters["NormalBumpMapEnabled"].SetValue(false);
                        }

                        Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * camera.WorldMatrix));
                        effectAmbient.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                    }
                    mesh.Draw();
                }
            }
        }
    }
}
