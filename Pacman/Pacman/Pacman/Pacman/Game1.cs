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

        readonly Rectangle PACMAN = new Rectangle(0, 36, 17, 17);
        readonly Rectangle SMALL_PELLET = new Rectangle(35, 54, 8, 8);
        readonly Rectangle LARGE_PELLET = new Rectangle(104, 68, 20, 20);

        readonly int[] GRID_SIZE = { 28, 36 };
        const int BLOCK_SIZE = 20;
        const int X = 0, Y = 1;
        const int UP = 0, RIGHT = 1, DOWN = 2, LEFT = 3;

        int[] pacmanPos = new int[] { 14, 26 };
        bool[] dirctPacman = new bool[] { false, false, false, false };
        int timer = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = GRID_SIZE[Y] * BLOCK_SIZE;
            graphics.PreferredBackBufferWidth = GRID_SIZE[X] * BLOCK_SIZE;
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
            restGrid();
            base.Initialize();
        }

        private void restGrid()
        {
            strgrid = new string[,]
            {
                {"c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c"},
                {"c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c"},
                {"c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c"},

                {"w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w"},
                {"w","p","p","p","p","p","p","p","p","p","p","p","p","w","w","p","p","p","p","p","p","p","p","p","p","p","p","w"},
                {"w","p","w","w","w","w","p","w","w","w","w","w","p","w","w","p","w","w","w","w","w","p","w","w","w","w","p","w"},
                {"w","P","w","w","w","w","p","w","w","w","w","w","p","w","w","p","w","w","w","w","w","p","w","w","w","w","P","w"},
                {"w","p","w","w","w","w","p","w","w","w","w","w","p","w","w","p","w","w","w","w","w","p","w","w","w","w","p","w"},
                {"w","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","w"},
                {"w","p","w","w","w","w","p","w","w","p","w","w","w","w","w","w","w","w","p","w","w","p","w","w","w","w","p","w"},
                {"w","p","w","w","w","w","p","w","w","p","w","w","w","w","w","w","w","w","p","w","w","p","w","w","w","w","p","w"},
                {"w","p","p","p","p","p","p","w","w","p","p","p","p","w","w","p","p","p","p","w","w","p","p","p","p","p","p","w"},
                {"w","w","w","w","w","w","p","w","w","w","w","w","c","w","w","c","w","w","w","w","w","p","w","w","w","w","w","w"},
                {"c","c","c","c","c","w","p","w","w","w","w","w","c","w","w","c","w","w","w","w","w","p","w","c","c","c","c","c"},
                {"c","c","c","c","c","w","p","w","w","c","c","c","c","c","c","c","c","c","c","w","w","p","w","c","c","c","c","c"},
                {"c","c","c","c","c","w","p","w","w","c","w","w","w","0","0","w","w","w","c","w","w","p","w","c","c","c","c","c"},
                {"w","w","w","w","w","w","p","w","w","c","w","c","c","c","c","c","c","w","c","w","w","p","w","w","w","w","w","w"},
                {"c","c","c","c","c","c","p","c","c","c","w","c","c","c","c","c","c","w","c","c","c","p","c","c","c","c","c","c"},
                {"w","w","w","w","w","w","p","w","w","c","w","c","c","c","c","c","c","w","c","w","w","p","w","w","w","w","w","w"},
                {"c","c","c","c","c","w","p","w","w","c","w","w","w","w","w","w","w","w","c","w","w","p","w","c","c","c","c","c"},
                {"c","c","c","c","c","w","p","w","w","c","c","c","c","c","c","c","c","c","c","w","w","p","w","c","c","c","c","c"},
                {"c","c","c","c","c","w","p","w","w","c","w","w","w","w","w","w","w","w","c","w","w","p","w","c","c","c","c","c"},
                {"w","w","w","w","w","w","p","w","w","c","w","w","w","w","w","w","w","w","c","w","w","p","w","w","w","w","w","w"},
                {"w","p","p","p","p","p","p","p","p","p","p","p","p","w","w","p","p","p","p","p","p","p","p","p","p","p","p","w"},
                {"w","p","w","w","w","w","p","w","w","w","w","w","p","w","w","p","w","w","w","w","w","p","w","w","w","w","p","w"},
                {"w","p","w","w","w","w","p","w","w","w","w","w","p","w","w","p","w","w","w","w","w","p","w","w","w","w","p","w"},
                {"w","P","p","p","w","w","p","p","p","p","p","p","p","c","c","p","p","p","p","p","p","p","w","w","p","p","P","w"},
                {"w","w","w","p","w","w","p","w","w","p","w","w","w","w","w","w","w","w","p","w","w","p","w","w","p","w","w","w"},
                {"w","w","w","p","w","w","p","w","w","p","w","w","w","w","w","w","w","w","p","w","w","p","w","w","p","w","w","w"},
                {"w","p","p","p","p","p","p","w","w","p","p","p","p","w","w","p","p","p","p","w","w","p","p","p","p","p","p","w"},
                {"w","p","w","w","w","w","w","w","w","w","w","w","p","w","w","p","w","w","w","w","w","w","w","w","w","w","p","w"},
                {"w","p","w","w","w","w","w","w","w","w","w","w","p","w","w","p","w","w","w","w","w","w","w","w","w","w","p","w"},
                {"w","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","p","w"},
                {"w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w","w"},

                {"c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c"},
                {"c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c","c"},
            };
            strgrid[pacmanPos[Y], pacmanPos[X]] = "1";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int x = 0; x < GRID_SIZE[X]; x++)
                for (int y = 0; y < GRID_SIZE[Y]; y++)
                {
                    gameGrid[x, y] = new Rectangle(x * BLOCK_SIZE, y * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE);
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
            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            timer++;

            if (kb.IsKeyDown(Keys.Up) &&
                !(oldKb.IsKeyDown(Keys.Up)))
            {
                for (int i = 0; i < dirctPacman.Length; i++)
                    dirctPacman[i] = false;
                dirctPacman[UP] = true;
            }
            else if (kb.IsKeyDown(Keys.Down) &&
                !(oldKb.IsKeyDown(Keys.Down)))
            {
                for (int i = 0; i < dirctPacman.Length; i++)
                    dirctPacman[i] = false;
                dirctPacman[DOWN] = true;
            }

            if (kb.IsKeyDown(Keys.Right) &&
                !(oldKb.IsKeyDown(Keys.Right)))
            {
                for (int i = 0; i < dirctPacman.Length; i++)
                    dirctPacman[i] = false;
                dirctPacman[RIGHT] = true;
            }
            else if (kb.IsKeyDown(Keys.Left) &&
                !(oldKb.IsKeyDown(Keys.Left)))
            {
                for (int i = 0; i < dirctPacman.Length; i++)
                    dirctPacman[i] = false;
                dirctPacman[LEFT] = true;
            }

            if ((timer / 60.0) % 0.25 == 0)
                if (dirctPacman[UP])
                    pacmanPos[Y]--;
                else if (dirctPacman[RIGHT])
                    pacmanPos[X]++;
                else if (dirctPacman[DOWN])
                    pacmanPos[Y]++;
                else if (dirctPacman[LEFT])
                    pacmanPos[X]--;

            if (pacmanPos[Y] == 17 && pacmanPos[X] == -1)
                pacmanPos[X] = 27;
            else if (pacmanPos[Y] == 17 && pacmanPos[X] == 28)
                pacmanPos[X] = 0;
            else
                checkHitWall();

            Console.WriteLine("PACMAN " + pacmanPos[X] + " " + pacmanPos[Y]);

            restGrid();

            oldKb = kb;
            base.Update(gameTime);
        }

        private void checkHitWall()
        {
            if (dirctPacman[UP])
            {
                if (strgrid[pacmanPos[Y], pacmanPos[X]] == "w")
                {
                    pacmanPos[Y]++;
                    dirctPacman[UP] = false;
                }
            }
            else if (dirctPacman[RIGHT])
            {
                if (strgrid[pacmanPos[Y], pacmanPos[X]] == "w")
                {
                    pacmanPos[X]--;
                    dirctPacman[RIGHT] = false;
                }
            }
            else if (dirctPacman[DOWN])
            {
                if (strgrid[pacmanPos[Y], pacmanPos[X]] == "w" ||
                    strgrid[pacmanPos[Y], pacmanPos[X]] == "0")
                {
                    pacmanPos[Y]--;
                    dirctPacman[DOWN] = false;
                }
            }
            else if (dirctPacman[LEFT])
            {
                if (strgrid[pacmanPos[Y], pacmanPos[X]] == "w")
                {
                    pacmanPos[X]++;
                    dirctPacman[LEFT] = false;
                }
            }
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
                    else if (strgrid[y, x] == "P")
                        spriteBatch.Draw(spritesheet, gameGrid[x, y], SMALL_PELLET, Color.White);
                    else if (strgrid[y, x] == "p")
                        spriteBatch.Draw(spritesheet, gameGrid[x, y], LARGE_PELLET, Color.White);
                    else if (strgrid[y, x] == "1")
                        spriteBatch.Draw(spritesheet, gameGrid[x, y], PACMAN, Color.White);
                    else
                        spriteBatch.Draw(t, gameGrid[x, y], Color.LightPink);
                }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
