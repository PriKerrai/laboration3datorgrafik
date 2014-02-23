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
        public static RasterizerState cullingState = new RasterizerState { CullMode = CullMode.CullCounterClockwiseFace };
        public static RasterizerState noCullingState = new RasterizerState { CullMode = CullMode.None };

        public static DepthStencilState zBufferState = new DepthStencilState { DepthBufferEnable = true, DepthBufferWriteEnable = true };
        public static DepthStencilState noZBufferState = new DepthStencilState { DepthBufferEnable = true, DepthBufferWriteEnable = false };

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
            Graphics.GraphicsDevice.DepthStencilState =zBufferState;
            Graphics.GraphicsDevice.RasterizerState = cullingState;
            for (int i = 0; i < bModelsWithSpecialEffect.Count; i++){
                bModelsWithSpecialEffect[i].DrawSpecialEffect(Graphics.GraphicsDevice, camera, this);
            }
            Graphics.GraphicsDevice.DepthStencilState = noZBufferState;
            Graphics.GraphicsDevice.RasterizerState = noCullingState;
            for (int i = 0; i < bModelsWithSpecialEffect.Count; i++)
            {
                bModelsWithSpecialEffect[i].DrawTranslucentMeshes( camera, this);
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
                bModelsWithSpecialEffect[i].DrawSpecialEffect(Graphics.GraphicsDevice, camera, this);
            }
        }
    }
}
