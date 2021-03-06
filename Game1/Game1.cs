﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game1.AssetClasses;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region "Declarations"
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Enemy enemy;
        Vector2 position;
        Texture2D[,] spriteSheetBackground = new Texture2D[20,20];        
        float time;
        protected List<Enemy> enemyList;
        float frameTime = 0.1f;
        int frameIndex;
        const int totalFrames = 3;
        public int screenX, screenY;
        private bool pressed;
        //private bool bStartUp = true;
        private GameManager gameManager;
        public delegate void EventHandler();
        public event EventHandler SpawnEnemyEvent;
        #endregion

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

            base.Initialize();
            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 64, graphics
                .GraphicsDevice.Viewport.Height / 2 - 64);
            screenX = graphics.GraphicsDevice.Viewport.Width / 2;
            screenY = graphics.GraphicsDevice.Viewport.Height / 2;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);            
            for (int i = 0; i < spriteSheetBackground.Length / 20; i++)
            {
                for (int j = 0; j < spriteSheetBackground.Length / 20; j++)
                {
                    spriteSheetBackground[i, j] = this.Content.Load<Texture2D>("Landscape/landscape_21");
                }
            }
            // TODO: use this.Content to load your game content here            

            enemy = new Enemy(new Vector2(700f,25f));
            enemy.LoadContent(Content);
            enemyList = new List<Enemy>();
            enemyList.Add(enemy);
            gameManager = new GameManager();
            gameManager.LoadContent(Content);
            SpawnEnemyEvent += new EventHandler(gameManager.SpawnEnemy);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.
                Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState state = Keyboard.GetState();
            MouseState mstate = Mouse.GetState();
            if (state.IsKeyDown(Keys.S))
            {
                gameManager.BeginGame(ref enemyList);
            }
            if (state.IsKeyDown(Keys.T))
            {
                pressed = true;
            }
            if(state.IsKeyUp(Keys.T) && pressed)
            {
               pressed = false;
               gameManager.AddTower(screenX, screenY);             
            }            
            if (mstate.LeftButton == ButtonState.Pressed)
            {
                gameManager.activeTower.firstClick = false;
            }

            enemy.Update(gameTime);
            gameManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (time > frameTime)
            {
                frameIndex++;
                time = 0f;
            }
            spriteBatch.Begin();            
            //spriteBatch.Draw(spriteSheet, position);
            for (int i = 0; i < spriteSheetBackground.Length / 20; i++)
            {
                for(int j = spriteSheetBackground.Length / 20 - 1; j >= 0; j--)
                {
                    spriteBatch.Draw(spriteSheetBackground[i, j], new Vector2((i * spriteSheetBackground[i, j].Width / 2.02f) + (j * spriteSheetBackground[i, j].Width / 2.02f)-500,
                        ((i * spriteSheetBackground[i,j].Height / 2.5f) - (j * spriteSheetBackground[i,j].Height / 2.5f))+200));                                        
                }
            }
            //bStartUp = false;
            enemy.Draw(spriteBatch);
            gameManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
