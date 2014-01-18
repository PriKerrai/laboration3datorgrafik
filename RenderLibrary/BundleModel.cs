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
        public float bRotation { get; set; }

        public BundleModel(Vector3 position, Model model, float scale)
        {
            bPosition = position;
            bModel = model;
            bScale = scale;

        }

        public BundleModel(Vector3 position, string modelPath, float scale, Texture2D tPath, float radians)
        {
            this.bPosition = position;
            this.bModelPath = modelPath;
            this.bScale = scale;
            this.bTexturePath = tPath;
            this.bRotation = radians;
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
