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
            effectNormalMap = Content.Load<Effect>("effects");
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
                        if (bModels[i].bNormalMap != null)
                        {
                            part.Effect = effectNormalMap;
                            effectNormalMap.Parameters["World"].SetValue(camera.WorldMatrix * mesh.ParentBone.Transform * Matrix.CreateScale(bModels[i].bScale) * Matrix.CreateTranslation(bModels[i].bPosition));
                            effectNormalMap.Parameters["View"].SetValue(camera.ViewMatrix);
                            effectNormalMap.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                            effectNormalMap.Parameters["ViewVector"].SetValue(camera.viewVector);
                            effectNormalMap.Parameters["ModelTexture"].SetValue(bModels[i].bTexturePath);
                            effectNormalMap.Parameters["NormalMap"].SetValue(bModels[i].bNormalMap);

                            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * camera.WorldMatrix));
                            effectNormalMap.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                        }
                        else
                        {
                            part.Effect = effectAmbient;
                            effectAmbient.Parameters["World"].SetValue(camera.WorldMatrix * mesh.ParentBone.Transform * Matrix.CreateScale(bModels[i].bScale) * Matrix.CreateTranslation(bModels[i].bPosition));
                            effectAmbient.Parameters["View"].SetValue(camera.ViewMatrix);
                            effectAmbient.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                            effectAmbient.Parameters["ViewVector"].SetValue(camera.viewVector);
                            effectAmbient.Parameters["ModelTexture"].SetValue(bModels[i].bTexturePath);

                            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * camera.WorldMatrix));
                            effectAmbient.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                        }
                        
                    }
                    mesh.Draw();
                }
            }
        }


        public void DrawModel()
        {
            for (int i = 0; i < bModels.Count; i++)
            {
                foreach (ModelMesh mesh in bModels[i].bModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        if (i == 0)
                        {
                            effect.DiffuseColor = new Vector3(1f, 1f, 1f);   
                            effect.AmbientLightColor = new Vector3(1f, 1f, 1f);
                        }
                        effect.FogEnabled = true;
                        effect.FogStart = 20f;
                        effect.FogEnd = 30f;
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.World = Matrix.Identity * mesh.ParentBone.Transform * Matrix.CreateScale(bModels[i].bScale) * Matrix.CreateTranslation(bModels[i].bPosition);
                        effect.View = camera.ViewMatrix;
                        effect.Projection = camera.ProjectionMatrix;
                        
                    }
                    mesh.Draw();
                }
            }
        }
        


    }
}
