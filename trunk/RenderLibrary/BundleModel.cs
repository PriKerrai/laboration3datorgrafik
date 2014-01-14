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
        public string bPath { get; set; }
        public float bScale { get; set; }
        public List<Texture2D> TexturePath { get; set; }

        public BundleModel(Vector3 position, string path, float scale, List<Texture2D> tPath) 
        {
            bPosition = position;
            bPath = path;
            bScale = scale;
            TexturePath = tPath;
        }
            
    }
}
