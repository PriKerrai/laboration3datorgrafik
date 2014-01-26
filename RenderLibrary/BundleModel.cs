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
                foreach (BasicEffect meshEffect in mesh.Effects)
                {

                    //  part.Effect = customEffect.Clone();
                    //if (bModels[i].bTexture != null)
                    //{

                    //    part.Effect = customEffect.Clone();
                    //    part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(1, 1, 1, 1));
                    //    part.Effect.Parameters["ModelTexture"].SetValue(bModels[i].bTexture);
                    //}

                    //part.Effect = customEffect;
                    //if (bModels[i].bTexture != null)
                    //{
                    //    part.Effect = customEffect.Clone();
                    //    part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(1, 1, 1, 1));
                    //    part.Effect.Parameters["ModelTexture"].SetValue(bModels[i].bTexture);
                    //}
                    meshEffect.View = camera.ViewMatrix;
                    meshEffect.World = ((camera.WorldMatrix * mesh.ParentBone.Transform * Matrix.CreateScale(bScale) * Matrix.CreateRotationY(bRotation) * Matrix.CreateTranslation(bPosition)));
                    meshEffect.Projection = camera.ProjectionMatrix;
                    meshEffect.LightingEnabled = true;
                    //meshEffect.Parameters["World"].SetValue(camera.WorldMatrix * mesh.ParentBone.Transform * Matrix.CreateScale(bScale) * Matrix.CreateRotationY(bRotation) * Matrix.CreateTranslation(bPosition));
                    //meshEffect.Parameters["View"].SetValue(camera.ViewMatrix);
                    //meshEffect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    //meshEffect.Parameters["EyePosition"].SetValue(camera.Position);
                    // part.Effect.Parameters["ModelTexture"].SetValue(bModels[i].bTexture);

                    //if (bNormalMap != null && !bEnvironmentTextured)
                    //{
                    //    meshEffect.Parameters["NormalBumpMapEnabled"].SetValue(true);
                    //    meshEffect.Parameters["EnvironmentTextureEnabled"].SetValue(false);
                    //    meshEffect.Parameters["NormalMap"].SetValue(bNormalMap);
                    //}
                    //else if (bEnvironmentTextured)
                    //{
                    //    meshEffect.Parameters["NormalBumpMapEnabled"].SetValue(true);
                    //    meshEffect.Parameters["EnvironmentTextureEnabled"].SetValue(true);
                    //}
                    //else
                    //{
                    //    meshEffect.Parameters["NormalBumpMapEnabled"].SetValue(false);
                    //    meshEffect.Parameters["EnvironmentTextureEnabled"].SetValue(false);
                    //}
                }
                mesh.Draw();
            }
        }
        public void DrawSpecialEffect(Effect customEffect)
        {
            if (bModel != null)
            {
                foreach (ModelMesh mesh in bModel.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {

                        Texture2D texture = ((BasicEffect)part.Effect).Texture;
                        float alpha = ((BasicEffect)part.Effect).Alpha;
                        Vector3 diffuseColor = ((BasicEffect)part.Effect).DiffuseColor;
                        Vector3 specularColor = ((BasicEffect)part.Effect).SpecularColor;
                        float SpecularIntensity = ((BasicEffect)part.Effect).SpecularPower;

                        part.Effect = customEffect.Clone();

                        part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(diffuseColor, 0));
                        part.Effect.Parameters["Alpha"].SetValue(alpha);
                        part.Effect.Parameters["ModelTexture"].SetValue(bTexture);
                        part.Effect.Parameters["SpecularColor"].SetValue(specularColor);
                        part.Effect.Parameters["SpecularIntensity"].SetValue(SpecularIntensity);
                                                
                    }
                    mesh.Draw();
                }
            }
        }
            
    }
}
