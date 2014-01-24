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
        Effect effectAmbient;

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
               // if (bModels[i].bTexture == null)
                    bModels[i].GetTextures(this.effectAmbient);

            }
        }

        public void Draw()
        {
            
            for (int i = 0; i < bModels.Count; i++)
            {
                bModels[i].Draw(camera);
                //foreach (ModelMesh mesh in bModels[i].bModel.Meshes)
                //{
                //    foreach (ModelMeshPart part in mesh.MeshParts)
                //    {

                //      //  part.Effect = effectAmbient.Clone();
                //        //if (bModels[i].bTexture != null)
                //        //{

                //        //    part.Effect = effectAmbient.Clone();
                //        //    part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(1, 1, 1, 1));
                //        //    part.Effect.Parameters["ModelTexture"].SetValue(bModels[i].bTexture);
                //        //}

                //        //part.Effect = effectAmbient;
                //        //if (bModels[i].bTexture != null)
                //        //{
                //        //    part.Effect = effectAmbient.Clone();
                //        //    part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(1, 1, 1, 1));
                //        //    part.Effect.Parameters["ModelTexture"].SetValue(bModels[i].bTexture);
                //        //}

                //        part.Effect.Parameters["World"].SetValue(camera.WorldMatrix * mesh.ParentBone.Transform * Matrix.CreateScale(bModels[i].bScale) * Matrix.CreateRotationY(bModels[i].bRotation) * Matrix.CreateTranslation(bModels[i].bPosition));
                //        part.Effect.Parameters["View"].SetValue(camera.ViewMatrix);
                //        part.Effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                //        part.Effect.Parameters["ViewVector"].SetValue(camera.Position);
                //       // part.Effect.Parameters["ModelTexture"].SetValue(bModels[i].bTexture);

                //        if (bModels[i].bNormalMap != null && !bModels[i].bEnvironmentTextured)
                //        {
                //            part.Effect.Parameters["NormalBumpMapEnabled"].SetValue(true);
                //            part.Effect.Parameters["EnvironmentTextureEnabled"].SetValue(false);
                //            part.Effect.Parameters["NormalMap"].SetValue(bModels[i].bNormalMap);
                //        }
                //        else if (bModels[i].bEnvironmentTextured)
                //        {
                //            part.Effect.Parameters["NormalBumpMapEnabled"].SetValue(true);
                //            part.Effect.Parameters["EnvironmentTextureEnabled"].SetValue(true);
                //        }
                //        else
                //        {
                //            part.Effect.Parameters["NormalBumpMapEnabled"].SetValue(false);
                //            part.Effect.Parameters["EnvironmentTextureEnabled"].SetValue(false);
                //        }

                //        Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * camera.WorldMatrix));
                //        effectAmbient.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                //    }
                //    mesh.Draw();
                //}
                // effectAmbient.Parameters["DiffuseColor"].SetValue(new Vector4(1f, 1f, 1f, 1f));
            }
        }
    }
}
