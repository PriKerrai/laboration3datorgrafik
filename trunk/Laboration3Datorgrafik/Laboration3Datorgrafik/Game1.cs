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
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        Camera camera;

        Floor floor;


        FlyingCamera fCamera;

        Effect effect, normalMapEffect;

        RenderManager renderManager;

        Vector3 jeepPosition = Vector3.Zero;
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f);
        float aspectRatio;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Datorgrafik Lab 3";
            this.camera = new Camera(GraphicsDevice, new Vector3(0, 0, -10));
            device = GraphicsDevice;
            renderManager = new RenderManager(Content, camera);

            renderManager.AddBundleModel(new BundleModel(new Vector3(1, 0, 2), "Models\\jeep", 0.8f, Content.Load<Texture2D>("Models\\fbx\\jeep-1"), MathHelper.ToRadians(0)));
            renderManager.AddBundleModel(new BundleModel(new Vector3(5, 5, 2), "Models\\Helicopter", 0.8f, Content.Load<Texture2D>("Models\\fbx\\HelicopterTexture"), MathHelper.ToRadians(0)));
            renderManager.AddBundleModel(new BundleModel(new Vector3(-1, 3, 3), "Models\\BeachBall", 0.4f, Content.Load<Texture2D>("Models\\fbx\\BeachBallTexture"), MathHelper.ToRadians(0)));
            renderManager.AddBundleModel(new BundleModel(new Vector3(5, 2, 2), "Models\\sphere_mapped", 0.8f, Content.Load<Texture2D>("Models\\BeachBallNormalMap"), Content.Load<Texture2D>("Models\\normal_4")));
            renderManager.AddBundleModel(new BundleModel(new Vector3(0, 0, 10), "Models\\moffett-old-building-a", 1, Content.Load<Texture2D>("Models\\fbx\\textures-obs-tower-knuq"), MathHelper.ToRadians(0)));
            renderManager.AddBundleModel(new BundleModel(new Vector3(4, 1.001f, -2), "Models\\snowplow", 0.7f));
            floor = new Floor(GraphicsDevice, Content.Load<Texture2D>("Models\\setts"), Content.Load<Texture2D>("Models\\setts-normalmap"), 100, 100, new Vector3(0, 0, 0));
            
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
            
            effect = Content.Load<Effect>("Ambient");
            normalMapEffect = Content.Load<Effect>("effects");

            effect.Parameters["FogEnabled"].SetValue(true);
            effect.Parameters["FogStart"].SetValue(15);
            effect.Parameters["FogEnd"].SetValue(30);
            effect.Parameters["FogColor"].SetValue(Color.DarkGray.ToVector3());

            normalMapEffect.Parameters["FogEnabled"].SetValue(true);
            normalMapEffect.Parameters["FogStart"].SetValue(15);
            normalMapEffect.Parameters["FogEnd"].SetValue(30);
            normalMapEffect.Parameters["FogColor"].SetValue(Color.DarkGray.ToVector3());

            fCamera = new FlyingCamera();

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            renderManager.Load();
            // TODO: use this.Content to load your game content here
        }

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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            fCamera.ProcessInput(gameTime);
            camera.Update(fCamera.Position, fCamera.Rotation);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkGray, 1.0f, 0);

            floor.Draw(graphics.GraphicsDevice, effect, camera);
            renderManager.Draw();
            base.Draw(gameTime);
        }
    }
}
