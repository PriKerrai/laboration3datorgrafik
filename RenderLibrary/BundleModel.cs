using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

        public Vector3 bDirectLightningDirection { get; set; }


        public BundleModel(Model model, Vector3 position, float scale)
        {
            this.bDirectLightningDirection = new Vector3(0f, 1f, 0f);
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

        public BundleModel(Vector3 position, string modelPath, float scale, Texture2D texture, Texture2D normalMap) 
        {
            this.bPosition = position;
            this.bModelPath = modelPath;
            this.bScale = scale;
            this.bTexture = texture;
            this.bNormalMap = normalMap;
        }

        public BundleModel()
        {
            
        }
        public void Draw(Camera camera)
        {

            foreach (ModelMesh mesh in bModel.Meshes)
            {
                foreach (BasicEffect meshEffect in mesh.Effects)
                {
                    meshEffect.View = camera.ViewMatrix;
                    meshEffect.World = ((camera.WorldMatrix * mesh.ParentBone.Transform * Matrix.CreateScale(bScale) * Matrix.CreateRotationY(bRotation) * Matrix.CreateTranslation(bPosition)));
                    meshEffect.Projection = camera.ProjectionMatrix;
                    meshEffect.DirectionalLight0.Direction = bDirectLightningDirection;

                    
                }
                mesh.Draw();
            }
        }

        public void SetBasicEffectParameters()
        {
            foreach (ModelMesh mesh in bModel.Meshes)
            {
                foreach (BasicEffect meshEffect in mesh.Effects)
                {
                    meshEffect.LightingEnabled = true;
                    meshEffect.Texture = meshEffect.Texture;
                    meshEffect.DirectionalLight0.DiffuseColor = meshEffect.DiffuseColor;
                    meshEffect.SpecularColor = meshEffect.SpecularColor;
                    meshEffect.SpecularPower = meshEffect.SpecularPower;
                    meshEffect.AmbientLightColor = new Vector3(1,1,1);
                    
                }
            }
        }
        public void SetEffectParameters(Effect customEffect)
        {
            if (bModel != null)
            {
                foreach (ModelMesh mesh in bModel.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        

                        float alpha = ((BasicEffect)part.Effect).Alpha;
                        Texture2D texture = ((BasicEffect)part.Effect).Texture;
                        Vector3 diffuseColor = ((BasicEffect)part.Effect).DiffuseColor;
                        Vector3 specularColor = ((BasicEffect)part.Effect).SpecularColor;
                        float SpecularIntensity = ((BasicEffect)part.Effect).SpecularPower;

                        part.Effect = customEffect.Clone();

                        part.Effect.Parameters["DiffuseColor"].SetValue(new Vector4(diffuseColor, 0));
                        part.Effect.Parameters["Alpha"].SetValue(alpha);
                        part.Effect.Parameters["ModelTexture"].SetValue(texture);
                        part.Effect.Parameters["SpecularColor"].SetValue(specularColor);
                        part.Effect.Parameters["SpecularIntensity"].SetValue(SpecularIntensity);

                        if (bNormalMap != null && !bEnvironmentTextured)
                        {
                            part.Effect.Parameters["NormalBumpMapEnabled"].SetValue(true);
                            part.Effect.Parameters["EnvironmentTextureEnabled"].SetValue(false);
                            part.Effect.Parameters["NormalMap"].SetValue(bNormalMap);
                        }
                        else if (bEnvironmentTextured)
                        {
                            part.Effect.Parameters["NormalBumpMapEnabled"].SetValue(true);
                            part.Effect.Parameters["EnvironmentTextureEnabled"].SetValue(true);
                        }
                        else
                        {
                            part.Effect.Parameters["NormalBumpMapEnabled"].SetValue(false);
                            part.Effect.Parameters["EnvironmentTextureEnabled"].SetValue(false);
                        }

                    }
                    
                }
            }
        }
        public void DrawSpecialEffect(Effect customEffect, Camera camera)
        {
            if (bModel != null)
            {
                foreach (ModelMesh mesh in bModel.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect.Parameters["World"].SetValue(camera.WorldMatrix * mesh.ParentBone.Transform * Matrix.CreateScale(bScale) * Matrix.CreateRotationY(bRotation) * Matrix.CreateTranslation(bPosition));
                        part.Effect.Parameters["View"].SetValue(camera.ViewMatrix);
                        part.Effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                        part.Effect.Parameters["EyePosition"].SetValue(camera.Position);
                        //part.Effect.Parameters["DirectLightDirection"].SetValue(bDirectLightningDirection);
                    }
                    mesh.Draw();
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            ProcessLightPositionChange(gameTime);
        }

        public void ProcessLightPositionChange(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.NumPad1))
                this.bDirectLightningDirection += new Vector3(0.5f, 0f, 0f);
            if (keys.IsKeyDown(Keys.NumPad3))
                this.bDirectLightningDirection -= new Vector3(0.5f, 0f, 0f); ;
            if (keys.IsKeyDown(Keys.NumPad4))
                this.bDirectLightningDirection += new Vector3(0f, 0.5f, 0f); ;
            if (keys.IsKeyDown(Keys.NumPad6))
                this.bDirectLightningDirection -= new Vector3(0f, 0.5f, 0f); ;
            if (keys.IsKeyDown(Keys.NumPad7))
               this.bDirectLightningDirection += new Vector3(0f, 0f, 0.5f); ;
            if (keys.IsKeyDown(Keys.NumPad9))
                this.bDirectLightningDirection -= new Vector3(0f, 0f, 0.5f); ;
        }

            
    }
}
