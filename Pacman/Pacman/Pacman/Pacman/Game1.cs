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
using System.IO;

namespace Pacman
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D spritesheet;
        Rectangle[,] gameGrid;
        string[,] strgrid;
        KeyboardState oldKb = Keyboard.GetState();

        readonly int[] GRID_SIZE = { 28, 31 };
        const int X = 0, Y = 1;

        int[] pacmanPos = new int[] { 0, 0 };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 620;
            graphics.PreferredBackBufferWidth = 560;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            // TODO: Add your initialization logic here
            gameGrid = new Rectangle[GRID_SIZE[X], GRID_SIZE[Y]];
            strgrid = new string[,]
            {
                {"w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w"},
                {"w","0","0","0","0","0","0","0","0","0","0","0","0","w","w","0","0","0","0","0","0","0","0","0","0","0","0","w"},
                {"w","0","w","w","w","w","0","w","w","w","w","w","0","w","w","0","w","w","w","w","w","0","w","w","w","w","0","w"},
                {"w","0","w","w","w","w","0","w","w","w","w","w","0","w","w","0","w","w","w","w","w","0","w","w","w","w","0","w"},
                {"w","0","w","w","w","w","0","w","w","w","w","w","0","w","w","0","w","w","w","w","w","0","w","w","w","w","0","w"},
                {"w","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","w"},
                {"w","0","w","w","w","w","0","w","w","0","w","w","w","w","w","w","w","w","0","w","w","0","w","w","w","w","0","w"},
                {"w","0","w","w","w","w","0","w","w","0","w","w","w","w","w","w","w","w","0","w","w","0","w","w","w","w","0","w"},
                {"w","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","w"},
                {"w","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","w"},
                {"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                {"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                {"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                {"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                {"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                {"w","w","w","w","w","w","0","w","w","0","w","0","0","0","0","0","0","w","0","w","w","0","w","w","w","w","w","w"},
                {"0","0","0","0","0","w","0","w","w","0","w","w","w","w","w","w","w","w","0","w","w","0","w","0","0","0","0","0"},
                {"0","0","0","0","0","w","0","w","w","0","0","0","0","0","0","0","0","0","0","w","w","0","w","0","0","0","0","0"},
                {"0","0","0","0","0","w","0","w","w","0","w","w","w","w","w","w","w","w","0","w","w","0","w","0","0","0","0","0"},
                {"w","w","w","w","w","w","0","w","w","0","w","w","w","w","w","w","w","w","0","w","w","0","w","w","w","w","w","w"},
                {"w","0","0","0","0","0","0","0","0","0","0","0","0","w","w","0","0","0","0","0","0","0","0","0","0","0","0","w"},
                {"w","0","w","w","w","w","0","w","w","w","w","w","0","w","w","0","w","w","w","w","w","0","w","w","w","w","0","w"},
                {"w","0","w","w","w","w","0","w","w","w","w","w","0","w","w","0","w","w","w","w","w","0","w","w","w","w","0","w"},
                {"w","0","0","0","w","w","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","w","w","0","0","0","w"},
                {"w","w","w","0","w","w","0","w","w","0","w","w","w","w","w","w","w","w","0","w","w","0","w","w","0","w","w","w"},
                {"w","w","w","0","w","w","0","w","w","0","w","w","w","w","w","w","w","w","0","w","w","0","w","w","0","w","w","w"},
                {"w","0","0","0","0","0","0","w","w","0","0","0","0","w","w","0","0","0","0","w","w","0","0","0","0","0","0","w"},
                {"w","0","w","w","w","w","w","w","w","w","w","w","0","w","w","0","w","w","w","w","w","w","w","w","w","w","0","w"},
                {"w","0","w","w","w","w","w","w","w","w","w","w","0","w","w","0","w","w","w","w","w","w","w","w","w","w","0","w"},
                {"w","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","w"},
                {"w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w"},
            };
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
            int Gridsize = 20;

            for (int x = 0; x < GRID_SIZE[X]; x++)
                for (int y = 0; y < GRID_SIZE[Y]; y++)
                {
                    gameGrid[x, y] = new Rectangle(x * Gridsize, y * Gridsize, Gridsize, Gridsize);
                }
            // TODO: use this.Content to load your game content here
            spritesheet = this.Content.Load<Texture2D>("spritesheet");
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
            //KeyboardState kb = Keyboard.GetState();
            //// Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //// TODO: Add your update logic here

            //if (kb.IsKeyDown(Keys.Up) &&
            //    !(oldKb.IsKeyDown(Keys.Up)))
            //    pacmanPos[0] -= 1;
            //else if (kb.IsKeyDown(Keys.Down) &&
            //    !(oldKb.IsKeyDown(Keys.Down)))
            //    pacmanPos[0] += 1;

            //if (kb.IsKeyDown(Keys.Right) &&
            //    !(oldKb.IsKeyDown(Keys.Right)))
            //    pacmanPos[1] += 1;
            //else if (kb.IsKeyDown(Keys.Left) &&
            //    !(oldKb.IsKeyDown(Keys.Left)))
            //    pacmanPos[1] -= 1;

            //if (pacmanPos[0] > 9)
            //    pacmanPos[0] = 9;
            //else if (pacmanPos[0] < 0)
            //    pacmanPos[0] = 0;

            //if (pacmanPos[1] > 9)
            //    pacmanPos[1] = 9;
            //else if (pacmanPos[1] < 0)
            //    pacmanPos[1] = 0;

            //for (int y = 0; y < 31; y++)
            //{
            //    for (int x = 0; x < 28; x++)
            //    {
            //        strgrid[x, y] = "0";
            //    }
            //}

            //strgrid[pacmanPos[0], pacmanPos[1]] = "1";

            //oldKb = kb;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            var t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });

            for (int x = 0; x < GRID_SIZE[X]; x++)
                for (int y = 0; y < GRID_SIZE[Y]; y++)
                {
                    if (strgrid[y, x] == "w")
                        spriteBatch.Draw(t, gameGrid[x, y], Color.Blue);
                    else if (strgrid[y, x] == "c")
                        spriteBatch.Draw(t, gameGrid[x, y], Color.Black);
                    else if (strgrid[y, x] == "p")
                        spriteBatch.Draw(t, gameGrid[x, y], Color.Black);
                    else
                        spriteBatch.Draw(t, gameGrid[x, y], Color.LightPink);
                }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
