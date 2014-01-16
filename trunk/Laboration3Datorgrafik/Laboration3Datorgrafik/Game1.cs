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
        VertexBuffer vertexBuffer;
        Ground ground;


        FlyingCamera fCamera;

        Effect effect, ambient;

        RenderManager renderManager;

        Model jeep;
        Texture2D texture;
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
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Datorgrafik Lab 3";
            this.camera = new Camera(GraphicsDevice, new Vector3(0, 0, 0));

            renderManager = new RenderManager(Content, camera);
            List<Texture2D> jeepTexturePaths = new List<Texture2D>();
            List<Texture2D> hangarTexturePaths = new List<Texture2D>();
            //jeepTexturePaths.Add(Content.Load<Texture2D>("Models\\fbx\\jeep-1"));
            //hangarTexturePaths.Add(Content.Load<Texture2D>("Models\\moffett-hangar2-concrete-b"));
            //hangarTexturePaths.Add(Content.Load<Texture2D>("Models\\moffett-hangar2-concrete"));
            //hangarTexturePaths.Add(Content.Load<Texture2D>("Models\\moffett-hangar2-numbers"));
            //hangarTexturePaths.Add(Content.Load<Texture2D>("Models\\moffett-hangar2-red-light"));
            //hangarTexturePaths.Add(Content.Load<Texture2D>("Models\\moffett-hangar2-roof"));
            //hangarTexturePaths.Add(Content.Load<Texture2D>("Models\\moffett-hangar2-windows")); 
            renderManager.AddBundleModel(new BundleModel(new Vector3(0, 0, 0), "Models\\jeep", 5f, jeepTexturePaths));
            renderManager.AddBundleModel(new BundleModel(new Vector3(10, 0, 10), "Models\\moffett-hangar2", 0.1f, hangarTexturePaths));
            renderManager.AddBundleModel(new BundleModel(new Vector3(10, 10, 10), "Models\\sphere", 1, null));
            ground = new Ground(this.graphics.GraphicsDevice);
            //renderManager.AddModelToWorldWithPosition(new Vector3(0, 10, 5), "Models\\Zeppelin_NT", 0.5f);
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

            device = GraphicsDevice;
            fCamera = new FlyingCamera();

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            
            ground = new Ground(GraphicsDevice);
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
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);
            
            effect.Parameters["View"].SetValue(camera.ViewMatrix);
            effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            effect.Parameters["World"].SetValue(camera.WorldMatrix);
            effect.Parameters["ModelTexture"].SetValue(texture);
            renderManager.DrawModel();
            //renderManager.Draw();
          //  ground.Draw(camera);
            base.Draw(gameTime);
        }
    }
}
