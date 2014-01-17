using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RenderLibrary
{
    public class BundleModel
    {

        public Model bModel { get; set; }
        public Vector3 bPosition { get; set; }
        public string bModelPath { get; set; }
        public float bScale { get; set; }
        public Texture2D bTexturePath { get; set; }
        public Texture2D bNormalMap {get; set; }

        public BundleModel(Vector3 position, string modelPath, float scale, Texture2D tPath)
        {
            this.bPosition = position;
            this.bModelPath = modelPath;
            this.bScale = scale;
            this.bTexturePath = tPath;
        }

        public BundleModel(Vector3 position, string modelPath, float scale, Texture2D texture, Texture2D normalMap) 
        {
            this.bPosition = position;
            this.bModelPath = modelPath;
            this.bScale = scale;
            this.bTexturePath = texture;
            this.bNormalMap = normalMap;
        }
            
    }
}
