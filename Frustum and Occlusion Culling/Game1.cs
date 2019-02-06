using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sample;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Frustum_and_Occlusion_Culling
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        InputEngine input;
        DebugEngine debug;
        ImmediateShapeDrawer shapeDrawer;

        List<GameObject3D> gameObjects = new List<GameObject3D>();
        Camera mainCamera;

        SpriteFont sfont;
        int objectsDrawn = 0;

        OcclusionQuery occQuery;
        private OctTree octTree;
        Stopwatch timer = new Stopwatch();
        long totalTime, totalObjects;

        Effect colorEffect;
        private int counter;
        private bool drawAlt;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1080;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();

            input = new InputEngine(this);
            debug = new DebugEngine();
            shapeDrawer = new ImmediateShapeDrawer();

            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        private bool FrustumContains(SimpleModel go)
        {
            if (mainCamera.Frustum.Contains(go.AABB) != ContainmentType.Disjoint)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsOcculded(SimpleModel go)
        {
            bool value = true;

            timer.Start();
            occQuery.Begin();
            shapeDrawer.DrawBoundingBox(go.AABB, mainCamera);
            occQuery.End();

            while (!occQuery.IsComplete)
            {

            }

            if (occQuery.IsComplete && occQuery.PixelCount > 0)
            {
                value = false;
                timer.Stop();
            }

            totalTime += timer.ElapsedMilliseconds;
            return value;
        }

        protected override void Initialize()
        {
            GameUtilities.Content = Content;
            GameUtilities.GraphicsDevice = GraphicsDevice;

            debug.Initialize();
            shapeDrawer.Initialize();

            mainCamera = new Camera("cam", new Vector3(0, 5, 10), new Vector3(0, 0, -1));
            mainCamera.Initialize();

            occQuery = new OcclusionQuery(GraphicsDevice);
            octTree = new OctTree(100, Vector3.Zero, 5);

            base.Initialize();
        }

        protected void AddModel(SimpleModel model)
        {
            model.Initialize();
            model.LoadContent();

            //replace this effect on the model


            octTree.AddObject(model);
            totalObjects++;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sfont = Content.Load<SpriteFont>("debug");

            colorEffect = Content.Load<Effect>("Effects/BasicTexture");

            AddModel(new TextureModel("wall", new Vector3(0, 0, 0)));

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            GameUtilities.Time = gameTime;

            if (InputEngine.IsKeyHeld(Keys.Escape))
            {
                Exit();
            }

            mainCamera.Update();
            gameObjects.Clear();
            //gameObjects.ForEach(go => go.Update());
            //quadTree.Process(mainCamera.Frustum, ref gameObjects);
            octTree.Process(mainCamera.Frustum, ref gameObjects);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //debug.Draw(mainCamera);

            foreach (SimpleModel go in gameObjects)
            {
                if (FrustumContains(go))
                {
                    go.Draw(mainCamera);
                }
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(sfont, "Objects Drawn: " + objectsDrawn, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(sfont, "Occulsion Time: " + totalTime, new Vector2(10, 25), Color.White);

            spriteBatch.End();

            objectsDrawn = 0;
            GameUtilities.SetGraphicsDeviceFor3D();

            base.Draw(gameTime);
        }
    }
}
