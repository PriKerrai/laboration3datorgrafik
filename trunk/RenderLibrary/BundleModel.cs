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
        public void Draw(Camera camera)
        {

            foreach (ModelMesh mesh in bModel.Meshes)
            {
                foreach (Effect meshEffect in mesh.Effects)
                {

                    //  part.Effect = effectAmbient.Clone();
                    //if (bModels[i].bTexture != null)
                    //{

                    //    part.Effect = effectAmbient.Clone();
                    //    part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(1, 1, 1, 1));
                    //    part.Effect.Parameters["ModelTexture"].SetValue(bModels[i].bTexture);
                    //}

                    //part.Effect = effectAmbient;
                    //if (bModels[i].bTexture != null)
                    //{
                    //    part.Effect = effectAmbient.Clone();
                    //    part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(1, 1, 1, 1));
                    //    part.Effect.Parameters["ModelTexture"].SetValue(bModels[i].bTexture);
                    //}

                    meshEffect.Parameters["World"].SetValue(camera.WorldMatrix * mesh.ParentBone.Transform * Matrix.CreateScale(bScale) * Matrix.CreateRotationY(bRotation) * Matrix.CreateTranslation(bPosition));
                    meshEffect.Parameters["View"].SetValue(camera.ViewMatrix);
                    meshEffect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    meshEffect.Parameters["ViewVector"].SetValue(camera.Position);
                    // part.Effect.Parameters["ModelTexture"].SetValue(bModels[i].bTexture);

                    if (bNormalMap != null && !bEnvironmentTextured)
                    {
                        meshEffect.Parameters["NormalBumpMapEnabled"].SetValue(true);
                        meshEffect.Parameters["EnvironmentTextureEnabled"].SetValue(false);
                        meshEffect.Parameters["NormalMap"].SetValue(bNormalMap);
                    }
                    else if (bEnvironmentTextured)
                    {
                        meshEffect.Parameters["NormalBumpMapEnabled"].SetValue(true);
                        meshEffect.Parameters["EnvironmentTextureEnabled"].SetValue(true);
                    }
                    else
                    {
                        meshEffect.Parameters["NormalBumpMapEnabled"].SetValue(false);
                        meshEffect.Parameters["EnvironmentTextureEnabled"].SetValue(false);
                    }

                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * camera.WorldMatrix));
                    meshEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                }
                mesh.Draw();
            }
        }
        public void GetTextures(Effect effectAmbient)
        {
            if (bModel != null)
            {
                foreach (ModelMesh mesh in bModel.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {

                        //Texture2D texture = ((BasicEffect)part.Effect).Texture;
                        float alpha = ((BasicEffect)part.Effect).Alpha;
                        Vector3 diffuseColor = ((BasicEffect)part.Effect).DiffuseColor;

                        part.Effect = effectAmbient.Clone();

                        part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(diffuseColor, 0));
                        part.Effect.Parameters["Alpha"].SetValue(alpha);
                        part.Effect.Parameters["ModelTexture"].SetValue(bTexture);
                        //if (bTexture != null)
                        //{
                        //    
                        //}
                        //else
                        //{
                        //    part.Effect.Parameters["ModelTexture"].SetValue(texture);
                        //}
                        
                    }
                }
            }
        }
            
    }
}