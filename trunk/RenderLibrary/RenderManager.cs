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
        Effect effect;
        Texture2D normalMap;

        public RenderManager(ContentManager content, Camera camera) 
        {
            Content = content;
            this.camera = camera;
            effect = Content.Load<Effect>("effects");
            normalMap = Content.Load<Texture2D>("Models\\normal_4");
        }


        public void AddBundleModel(BundleModel bModel)
        {
            bModels.Add(bModel);
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
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {

                        part.Effect = effect;
                        effect.Parameters["World"].SetValue(camera.WorldMatrix * mesh.ParentBone.Transform * Matrix.CreateScale(bModels[i].bScale) * Matrix.CreateTranslation(bModels[i].bPosition));
                        effect.Parameters["View"].SetValue(camera.ViewMatrix);
                        effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                        effect.Parameters["ViewVector"].SetValue(camera.viewVector);
                        effect.Parameters["ModelTexture"].SetValue(bModels[i].TexturePath);
                        effect.Parameters["NormalMap"].SetValue(normalMap);
                        
                        //Matrix scale = Matrix.CreateScale(bModels[i].bScale);
                        Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * camera.WorldMatrix));
                        effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
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
