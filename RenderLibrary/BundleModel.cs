﻿using System;
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
        public Texture2D bTexture { get; set; }
        public Texture2D bNormalMap {get; set; }
        public float bRotation { get; set; }
        public bool bEnvironmentTextured = false;

        public BundleModel(Model model, Vector3 position, float scale)
        {
            bPosition = position;
            bModel = model;
            bScale = scale;
        }

        public BundleModel(Vector3 position, string modelPath, float scale)
        {
            bPosition = position;
            bModelPath = modelPath;
            bScale = scale;
        }

        public BundleModel(Vector3 position, string modelPath, float scale, Model model)
        {
            bPosition = position;
            bModelPath = modelPath;
            bScale = scale;
        }

        public BundleModel(Vector3 position, string modelPath, float scale, Texture2D tPath, float radians)
        {
            this.bPosition = position;
            this.bModelPath = modelPath;
            this.bScale = scale;
            this.bTexture = tPath;
            this.bRotation = radians;
        }

        public BundleModel(Vector3 position, string modelPath, float scale, Texture2D texture, Texture2D normalMap) 
        {
            this.bPosition = position;
            this.bModelPath = modelPath;
            this.bScale = scale;
            this.bTexture = texture;
            this.bNormalMap = normalMap;
        }

        public void GetTextures(Effect effectAmbient)
        {
            if (bModel != null)
            {
                foreach (ModelMesh mesh in bModel.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        Texture2D texture = ((BasicEffect)part.Effect).Texture;
                        part.Effect = effectAmbient.Clone();
                        //part.Effect.Parameters["isColor2"].SetValue(true);
                        part.Effect.Parameters["ModelTexture"].SetValue(texture);
                    }
                }
            }
        }
            
    }
}