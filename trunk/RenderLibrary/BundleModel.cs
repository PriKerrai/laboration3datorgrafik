using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RenderLibrary
{
    class BundleModel
    {

        public Model bModel { get; set; }
        public Vector3 bPosition { get; set; }
        public string bPath { get; set; }

        public BundleModel(Vector3 position, string path) 
        {
            bPosition = position;
            bPath = path;
        }
            
    }
}
