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
        private Texture2D texture;

        public RenderManager(ContentManager content, Camera camera) 
        {
            Content = content;
            this.camera = camera;
        }

        public void AddModelToWorldWithPosition(Vector3 position, string modelPath, float scale)
        {
            effect = Content.Load<Effect>("Ambient");
            texture = Content.Load<Texture2D>("Models\\fbx\\jeep-1");
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
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {

                        part.Effect = effect;
                        effect.Parameters["World"].SetValue(camera.WorldMatrix * mesh.ParentBone.Transform);
                        effect.Parameters["View"].SetValue(camera.ViewMatrix);
                        effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                        effect.Parameters["ViewVector"].SetValue(camera.viewVector);
                        effect.Parameters["ModelTexture"].SetValue(texture);

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
                        
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.World = Matrix.Identity * mesh.ParentBone.Transform;
                        effect.View = camera.ViewMatrix;
                        effect.Projection = camera.ProjectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }
        


    }
}
