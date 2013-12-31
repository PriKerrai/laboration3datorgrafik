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

        public RenderManager(ContentManager content, Camera camera) 
        {
            Content = content;
            this.camera = camera;
        }

        public void AddModelToWorldWithPosition(Vector3 position, string modelPath)
        { 

            bModels.Add(new BundleModel(position, modelPath));
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
                    foreach (BasicEffect effecT in mesh.Effects)
                    {
                        effecT.EnableDefaultLighting();

                        effecT.World = Matrix.Identity * Matrix.CreateTranslation(new Vector3(10, 1, 10));

                        effecT.View = camera.ViewMatrix;

                        effecT.Projection = camera.ProjectionMatrix;


                    }
                    mesh.Draw();
                }
            }
        }
        


    }
}
