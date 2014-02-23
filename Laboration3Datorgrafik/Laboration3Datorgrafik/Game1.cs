using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RenderLibrary;

namespace Laboration3Datorgrafik
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Constants
        const int number_of_vertices = 8;
        const int number_of_indices = 36;
        const float rotation_factor = 0.01f;
        const float translation_factor = 0.05f;
        const float scale_factor = 1.01f;
        #endregion

        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        Camera camera;
        Floor floor;
        FlyingCamera fCamera;
        Effect customEffect;
        RenderManager renderManager;

        Vector3 jeepPosition = Vector3.Zero;
        Vector3 speherePosition;
        BundleModel sphereBundle;
        Reflection sphereReflection;
        Vector3 cameraPosition = new Vector3(5f, 2f, 2f);
        float aspectRatio;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;
            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //this.IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;

            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;
            //graphics.IsFullScreen = false;
            //graphics.ApplyChanges();

            GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.CullCounterClockwiseFace };
            GraphicsDevice.BlendState = BlendState.AlphaBlend;


            Window.Title = "Datorgrafik Lab 3";
            this.camera = new Camera(GraphicsDevice, new Vector3(0, 0, -10));
            
            device = GraphicsDevice;
           
            
            
            renderManager = new RenderManager(Content, camera, device);
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            floor = new Floor(GraphicsDevice, Content.Load<Texture2D>("Models\\setts"), Content.Load<Texture2D>("Models\\setts-normalmap"), 50, 50, new Vector3(0, 0, 0));
            renderManager.AddBundleModelWithCustomEffect(new BundleModel(new Vector3(1, 1.001f, 1), "Models\\snowplow", 0.7f));
            renderManager.AddBundleModelWithCustomEffect(new BundleModel(new Vector3(1, 1.001f, -1),"Models\\snowplow", 0.7f));

    
            renderManager.AddBundleModelWithCustomEffect(new BundleModel(new Vector3(1, 1.001f, -3), "Models\\snowplow", 0.7f));
            renderManager.AddBundleModelWithCustomEffect(new BundleModel(new Vector3(1, 1.001f, -5), "Models\\snowplow", 0.7f));
            
            renderManager.AddBundleModelWithCustomEffect(new BundleModel(new Vector3(-6, 0, 2), "Models\\jeep", 0.8f));
            renderManager.AddBundleModelWithCustomEffect(new BundleModel(new Vector3(5, 5, 2), "Models\\Helicopter", 0.8f));
            renderManager.AddBundleModelWithCustomEffect(new BundleModel(new Vector3(-1, 5, 3), "Models\\BeachBall", 0.4f));
            renderManager.AddBundleModelWithCustomEffect(new BundleModel(new Vector3(0, 0, 10), "Models\\moffett-old-building-a", 1));
            


            customEffect = Content.Load<Effect>("customEffect");

            customEffect.Parameters["AmbientLightIntensity"].SetValue(new Vector3(0.45f, 0.45f, 0.45f));
            customEffect.Parameters["DirectLightDirection"].SetValue(new Vector3(0, -1, 0));
            customEffect.Parameters["DirectLightDiffuseIntensity"].SetValue(new Vector3(0.5f, 0.5f, 0.5f));
            customEffect.Parameters["DirectLightSpecularIntensity"].SetValue(new Vector3(0.55f, 0.55f, 0.55f));

            customEffect.Parameters["Alpha"].SetValue(1);
            customEffect.Parameters["FogEnabled"].SetValue(true);
            customEffect.Parameters["FogStart"].SetValue(10);
            customEffect.Parameters["FogEnd"].SetValue(25);
            customEffect.Parameters["FogColor"].SetValue(Color.DarkGray.ToVector3());
            
            // EnvironmentTextured
            //speherePosition = new Vector3(-2, 2, -3);

            //sphereBundle = new BundleModel(speherePosition,"Models\\sphere_mapped", 0.8f, Content.Load<Texture2D>("Models\\BeachBallNormalMap"), Content.Load<Texture2D>("Models\\normal_4"));
            //sphereBundle.bEnvironmentTextured = true;
            //sphereBundle.bModel = Content.Load<Model>("Models\\sphere_mapped");

            //renderManager.AddBundleModelWithCustomEffect(sphereBundle);
            renderManager.CustomEffect = customEffect;
            renderManager.Graphics = graphics;
            renderManager.Load();

            floor = new Floor(GraphicsDevice, Content.Load<Texture2D>("Models\\setts"), Content.Load<Texture2D>("Models\\setts-normalmap"), 50, 50, new Vector3(0, 0, 0));
            floor.SetEffectParameters(customEffect);
            renderManager.Floor = floor;

            //sphereReflection = new Reflection(speherePosition, graphics.GraphicsDevice, renderManager, customEffect);

            fCamera = new FlyingCamera();

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
        }

        #region VertexBuffer
        VertexBuffer vertices;

        void CreateCubeVertexBuffer()
        {
            Vector3[] cubeVertices = new Vector3[number_of_vertices];

            cubeVertices[0] = new Vector3(-1, -1, -1);
            cubeVertices[1] = new Vector3(-1, -1, 1);
            cubeVertices[2] = new Vector3(1, -1, 1);
            cubeVertices[3] = new Vector3(1, -1, -1);
            cubeVertices[4] = new Vector3(-1, 1, -1);
            cubeVertices[5] = new Vector3(-1, 1, 1);
            cubeVertices[6] = new Vector3(1, 1, 1);
            cubeVertices[7] = new Vector3(1, 1, -1);

            VertexDeclaration VertexPositionDeclaration = new VertexDeclaration
                (
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0)
                );

            vertices = new VertexBuffer(GraphicsDevice, VertexPositionDeclaration, number_of_vertices, BufferUsage.WriteOnly);
            vertices.SetData<Vector3>(cubeVertices);
        }
        #endregion

        #region IndexBuffer
        IndexBuffer indices;

        void CreateCubeIndexBuffer()
        {
            UInt16[] cubeIndices = new UInt16[number_of_indices];

            //bottom
            cubeIndices[0] = 0;
            cubeIndices[1] = 2;
            cubeIndices[2] = 3;
            cubeIndices[3] = 0;
            cubeIndices[4] = 1;
            cubeIndices[5] = 2;

            //top
            cubeIndices[6] = 4;
            cubeIndices[7] = 6;
            cubeIndices[8] = 5;
            cubeIndices[9] = 4;
            cubeIndices[10] = 7;
            cubeIndices[11] = 6;

            //front
            cubeIndices[12] = 5;
            cubeIndices[13] = 2;
            cubeIndices[14] = 1;
            cubeIndices[15] = 5;
            cubeIndices[16] = 6;
            cubeIndices[17] = 2;

            //back
            cubeIndices[18] = 0;
            cubeIndices[19] = 7;
            cubeIndices[20] = 4;
            cubeIndices[21] = 0;
            cubeIndices[22] = 3;
            cubeIndices[23] = 7;

            //left
            cubeIndices[24] = 0;
            cubeIndices[25] = 4;
            cubeIndices[26] = 1;
            cubeIndices[27] = 1;
            cubeIndices[28] = 4;
            cubeIndices[29] = 5;

            //right
            cubeIndices[30] = 2;
            cubeIndices[31] = 6;
            cubeIndices[32] = 3;
            cubeIndices[33] = 3;
            cubeIndices[34] = 6;
            cubeIndices[35] = 7;

            indices = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, number_of_indices, BufferUsage.WriteOnly);
            indices.SetData<UInt16>(cubeIndices);
        }
        #endregion

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {   
            fCamera.ProcessInput(gameTime);
            camera.Update(fCamera.Position, fCamera.Rotation);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkGray, 1.0f, 0);

            renderManager.Draw();
          //  sphereReflection.RemapModel(customEffect, sphereBundle.bModel);
            base.Draw(gameTime);
        }
    }
}
