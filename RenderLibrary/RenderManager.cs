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
        BundleModel bundleModel;
        private List<BundleModel> bModels = new List<BundleModel>();
        private List<BundleModel> bModelsWithSpecialEffect = new List<BundleModel>();
        private ContentManager Content;
        public RasterizerState rasterizerStateNormal;
        public RasterizerState rasterizerStateNone;
        public GraphicsDevice device;
        private Camera camera;
        public Effect CustomEffect { get; set; }
        public Floor Floor;
        public GraphicsDeviceManager Graphics;

        public RenderManager(ContentManager content, Camera camera, GraphicsDevice device)
        {
            Content = content;
            this.camera = camera;
            bundleModel = new BundleModel();
            this.device = device;
            rasterizerStateNone = new RasterizerState();
            rasterizerStateNormal = new RasterizerState();
            rasterizerStateNone = RasterizerState.CullNone;
            rasterizerStateNormal = RasterizerState.CullCounterClockwise;
        }

        public void AddBundleModelWithNoEffect(BundleModel bModel)
        {
            bModels.Add(bModel);
        }
        public void AddBundleModelWithCustomEffect(BundleModel bModel)
        {
            bModelsWithSpecialEffect.Add(bModel);
        }

        public void Load() 
        {
            for (int i = 0; i < bModels.Count; i++)
            {
                bModels[i].bModel = Content.Load<Model>(bModels[i].bModelPath);
                bModels[i].SetBasicEffectParameters();
            }
            for (int i = 0; i < bModelsWithSpecialEffect.Count; i++)
            {
                bModelsWithSpecialEffect[i].bModel = Content.Load<Model>(bModelsWithSpecialEffect[i].bModelPath);
                bModelsWithSpecialEffect[i].SetEffectParameters(CustomEffect);
            }
        }

        public void Draw()
        {
            Floor.Draw(Graphics.GraphicsDevice, CustomEffect, camera);

            for (int i = 0; i < bModels.Count; i++)
            {
                bModels[i].Draw(camera);
            }
            for (int i = 0; i < bModelsWithSpecialEffect.Count; i++){
                bModelsWithSpecialEffect[i].DrawSpecialEffect(CustomEffect, camera, this);
            }
            for (int i = 0; i < bModelsWithSpecialEffect.Count; i++)
            {
                bModelsWithSpecialEffect[i].DrawTranslucentMeshes(camera, this);
            }
        }

        public void Draw(Camera camera)
        {
            Floor.Draw(Graphics.GraphicsDevice, CustomEffect, camera);

            for (int i = 0; i < bModels.Count; i++)
            {
                bModels[i].Draw(camera);
            }
            for (int i = 0; i < bModelsWithSpecialEffect.Count; i++)
            {
                bModelsWithSpecialEffect[i].DrawSpecialEffect(CustomEffect, camera, this);
            }
        }
    }
}
