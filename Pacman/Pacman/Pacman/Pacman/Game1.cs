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

        SpriteFont sf;
        Texture2D spritesheet;
        Rectangle[,] gameGrid;
        string[,] strgrid;
        KeyboardState oldKb = Keyboard.GetState();

        Rectangle PACMAN = new Rectangle(0, 36, 17, 17);
        Rectangle BLINKY = new Rectangle(49, 18, 14, 14);
        Rectangle CLYDE = new Rectangle(84, 48, 14, 14);
        Rectangle INKY = new Rectangle(0, 99, 14, 14);
        Rectangle PINKY = new Rectangle(70, 63, 14, 14);

        readonly Rectangle SMALL_PELLET = new Rectangle(35, 54, 8, 8);
        readonly Rectangle LARGE_PELLET = new Rectangle(104, 68, 20, 20);

        int[,] pellets = new int[244, 2];
        int index = 0;

        readonly int[] GRID_SIZE = { 28, 36 };
        const int BLOCK_SIZE = 20;
        const int X = 0, Y = 1;
        const int UP = 0, RIGHT = 1, DOWN = 2, LEFT = 3;

        int[] pacmanPos = new int[] { 14, 26 };
        int[] blinkyPos = new int[] { 14, 14 };
        int[] clydePos = new int[] { 12, 17 };
        int[] inkyPos = new int[] { 14, 17 };
        int[] pinkyPos = new int[] { 16, 17 };

        bool[] dirctPacman = new bool[] { false, false, false, false };
        bool[] dirctBlinky = new bool[] { false, false, false, false };
        bool blinkyMoving = false;
        bool[] dirctClyde = new bool[] { false, false, false, false };
        bool[] dirctInky = new bool[] { false, false, false, false };
        bool[] dirctPinky = new bool[] { false, false, false, false };

        int points = 0;
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

            for (int i = 0; i < index; i++)
            {
                strgrid[pellets[i, Y], pellets[i, X]] = "c";
            }

            strgrid[pacmanPos[Y], pacmanPos[X]] = "1";
            strgrid[blinkyPos[Y], blinkyPos[X]] = "B";
            strgrid[clydePos[Y], clydePos[X]] = "C";
            strgrid[inkyPos[Y], inkyPos[X]] = "I";
            strgrid[pinkyPos[Y], pinkyPos[X]] = "Pi";
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
            sf = this.Content.Load<SpriteFont>("PacmanText");
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

            if (!blinkyMoving)
            {
                Random rnd = new Random();
                blinkyMoving = true;
                switch (rnd.Next(2))
                {
                    case 0:
                        dirctBlinky[LEFT] = true;
                        break;
                    case 1:
                        dirctBlinky[RIGHT] = true;
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            }
            if (timer / 60.0 == 5.0)
            {
                clydePos[X] = clydePos[Y] = 14;
                Random rnd = new Random();
                switch (rnd.Next(2))
                {
                    case 0:
                        dirctClyde[LEFT] = true;
                        break;
                    case 1:
                        dirctClyde[RIGHT] = true;
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            }
            if (timer / 60.0 == 15.0)
            {
                inkyPos[X] = inkyPos[Y] = 14;
                Random rnd = new Random();
                switch (rnd.Next(2))
                {
                    case 0:
                        dirctInky[LEFT] = true;
                        break;
                    case 1:
                        dirctInky[RIGHT] = true;
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            }
            if (timer / 60.0 == 20.0)
            {
                pinkyPos[X] = pinkyPos[Y] = 14;
                Random rnd = new Random();
                switch (rnd.Next(2))
                {
                    case 0:
                        dirctPinky[LEFT] = true;
                        break;
                    case 1:
                        dirctPinky[RIGHT] = true;
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            }

            if (kb.IsKeyDown(Keys.Up) &&
                !(oldKb.IsKeyDown(Keys.Up)))
            {
                for (int i = 0; i < dirctPacman.Length; i++)
                    dirctPacman[i] = false;
                dirctPacman[UP] = true;
                PACMAN = new Rectangle(49, 0, 17, 17);
            }
            else if (kb.IsKeyDown(Keys.Down) &&
                !(oldKb.IsKeyDown(Keys.Down)))
            {
                for (int i = 0; i < dirctPacman.Length; i++)
                    dirctPacman[i] = false;
                dirctPacman[DOWN] = true;
                PACMAN = new Rectangle(0, 0, 17, 17);
            }

            if (kb.IsKeyDown(Keys.Right) &&
                !(oldKb.IsKeyDown(Keys.Right)))
            {
                for (int i = 0; i < dirctPacman.Length; i++)
                    dirctPacman[i] = false;
                dirctPacman[RIGHT] = true;
                PACMAN = new Rectangle(0, 36, 17, 17);
            }
            else if (kb.IsKeyDown(Keys.Left) &&
                !(oldKb.IsKeyDown(Keys.Left)))
            {
                for (int i = 0; i < dirctPacman.Length; i++)
                    dirctPacman[i] = false;
                dirctPacman[LEFT] = true;
                PACMAN = new Rectangle(0, 18, 17, 17);
            }

            if ((timer / 60.0) % 0.25 == 0)
            {
                if (dirctPacman[UP])
                    pacmanPos[Y]--;
                else if (dirctPacman[RIGHT])
                    pacmanPos[X]++;
                else if (dirctPacman[DOWN])
                    pacmanPos[Y]++;
                else if (dirctPacman[LEFT])
                    pacmanPos[X]--;

                if (dirctBlinky[UP])
                    blinkyPos[Y]--;
                else if (dirctBlinky[LEFT])
                    blinkyPos[X]--;
                else if (dirctBlinky[DOWN])
                    blinkyPos[Y]++;
                else if (dirctBlinky[RIGHT])
                    blinkyPos[X]++;

                if (dirctClyde[UP])
                    clydePos[Y]--;
                else if (dirctClyde[LEFT])
                    clydePos[X]--;
                else if (dirctClyde[DOWN])
                    clydePos[Y]++;
                else if (dirctClyde[RIGHT])
                    clydePos[X]++;

                if (dirctInky[UP])
                    inkyPos[Y]--;
                else if (dirctInky[LEFT])
                    inkyPos[X]--;
                else if (dirctInky[DOWN])
                    inkyPos[Y]++;
                else if (dirctInky[RIGHT])
                    inkyPos[X]++;

                if (dirctPinky[UP])
                    pinkyPos[Y]--;
                else if (dirctPinky[LEFT])
                    pinkyPos[X]--;
                else if (dirctPinky[DOWN])
                    pinkyPos[Y]++;
                else if (dirctPinky[RIGHT])
                    pinkyPos[X]++;
            }

            if (pacmanPos[Y] == 17 && pacmanPos[X] == -1)
                pacmanPos[X] = 27;
            else if (pacmanPos[Y] == 17 && pacmanPos[X] == 28)
                pacmanPos[X] = 0;
            else
                checkHitWall();

            checkGhostHitWall();
            checkHitPellet();

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

        private void checkGhostHitWall()
        {
            if (dirctBlinky[UP])
            {
                if (strgrid[blinkyPos[Y], blinkyPos[X]] == "w")
                {
                    blinkyPos[Y]++;
                    dirctBlinky[UP] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[blinkyPos[Y] - 1, blinkyPos[X]] != "w")
                                {
                                    dirctBlinky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[blinkyPos[Y] + 1, blinkyPos[X]] != "w")
                                {
                                    dirctBlinky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[blinkyPos[Y], blinkyPos[X] - 1] != "w")
                                {
                                    dirctBlinky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[blinkyPos[Y], blinkyPos[X] + 1] != "w")
                                {
                                    dirctBlinky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: BLINKY_DRCT_UP");
                                break;
                        }
                    }
                }
            }
            else if (dirctBlinky[RIGHT])
            {
                if (strgrid[blinkyPos[Y], blinkyPos[X]] == "w")
                {
                    blinkyPos[X]--;
                    dirctBlinky[RIGHT] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[blinkyPos[Y] - 1, blinkyPos[X]] != "w")
                                {
                                    dirctBlinky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[blinkyPos[Y] + 1, blinkyPos[X]] != "w")
                                {
                                    dirctBlinky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[blinkyPos[Y], blinkyPos[X] - 1] != "w")
                                {
                                    dirctBlinky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[blinkyPos[Y], blinkyPos[X] + 1] != "w")
                                {
                                    dirctBlinky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: BLINKY_DRCT_RIGHT");
                                break;
                        }
                    }
                }
            }
            else if (dirctBlinky[DOWN])
            {
                if (strgrid[blinkyPos[Y], blinkyPos[X]] == "w")
                {
                    blinkyPos[Y]--;
                    dirctBlinky[DOWN] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[blinkyPos[Y] - 1, blinkyPos[X]] != "w")
                                {
                                    dirctBlinky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[blinkyPos[Y] + 1, blinkyPos[X]] != "w")
                                {
                                    dirctBlinky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[blinkyPos[Y], blinkyPos[X] - 1] != "w")
                                {
                                    dirctBlinky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[blinkyPos[Y], blinkyPos[X] + 1] != "w")
                                {
                                    dirctBlinky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: BLINKY_DRCT_DOWN");
                                break;
                        }
                    }
                }
            }
            else if (dirctBlinky[LEFT])
            {
                if (strgrid[blinkyPos[Y], blinkyPos[X]] == "w")
                {
                    blinkyPos[X]++;
                    dirctBlinky[LEFT] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[blinkyPos[Y] - 1, blinkyPos[X]] != "w")
                                {
                                    dirctBlinky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[blinkyPos[Y] + 1, blinkyPos[X]] != "w")
                                {
                                    dirctBlinky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[blinkyPos[Y], blinkyPos[X] - 1] != "w")
                                {
                                    dirctBlinky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[blinkyPos[Y], blinkyPos[X] + 1] != "w")
                                {
                                    dirctBlinky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: BLINKY_DRCT_LEFT");
                                break;
                        }
                    }
                }
            }

            if (dirctClyde[UP])
            {
                if (strgrid[clydePos[Y], clydePos[X]] == "w")
                {
                    clydePos[Y]++;
                    dirctClyde[UP] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[clydePos[Y] - 1, clydePos[X]] != "w")
                                {
                                    dirctClyde[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[clydePos[Y] + 1, clydePos[X]] != "w")
                                {
                                    dirctClyde[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[clydePos[Y], clydePos[X] - 1] != "w")
                                {
                                    dirctClyde[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[clydePos[Y], clydePos[X] + 1] != "w")
                                {
                                    dirctClyde[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: CLYDE_DRCT_UP");
                                break;
                        }
                    }
                }
            }
            else if (dirctClyde[RIGHT])
            {
                if (strgrid[clydePos[Y], clydePos[X]] == "w")
                {
                    clydePos[X]--;
                    dirctClyde[RIGHT] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[clydePos[Y] - 1, clydePos[X]] != "w")
                                {
                                    dirctClyde[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[clydePos[Y] + 1, clydePos[X]] != "w")
                                {
                                    dirctClyde[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[clydePos[Y], clydePos[X] - 1] != "w")
                                {
                                    dirctClyde[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[clydePos[Y], clydePos[X] + 1] != "w")
                                {
                                    dirctClyde[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: CLYDE_DRCT_RIGHT");
                                break;
                        }
                    }
                }
            }
            else if (dirctClyde[DOWN])
            {
                if (strgrid[clydePos[Y], clydePos[X]] == "w")
                {
                    clydePos[Y]--;
                    dirctClyde[DOWN] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[clydePos[Y] - 1, clydePos[X]] != "w")
                                {
                                    dirctClyde[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[clydePos[Y] + 1, clydePos[X]] != "w")
                                {
                                    dirctClyde[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[clydePos[Y], clydePos[X] - 1] != "w")
                                {
                                    dirctClyde[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[clydePos[Y], clydePos[X] + 1] != "w")
                                {
                                    dirctClyde[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: CLYDE_DRCT_DOWN");
                                break;
                        }
                    }
                }
            }
            else if (dirctClyde[LEFT])
            {
                if (strgrid[clydePos[Y], clydePos[X]] == "w")
                {
                    clydePos[X]++;
                    dirctClyde[LEFT] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[clydePos[Y] - 1, clydePos[X]] != "w")
                                {
                                    dirctClyde[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[clydePos[Y] + 1, clydePos[X]] != "w")
                                {
                                    dirctClyde[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[clydePos[Y], clydePos[X] - 1] != "w")
                                {
                                    dirctClyde[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[clydePos[Y], clydePos[X] + 1] != "w")
                                {
                                    dirctClyde[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: CLYDE_DRCT_LEFT");
                                break;
                        }
                    }
                }
            }

            if (dirctInky[UP])
            {
                if (strgrid[inkyPos[Y], inkyPos[X]] == "w")
                {
                    inkyPos[Y]++;
                    dirctInky[UP] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[inkyPos[Y] - 1, inkyPos[X]] != "w")
                                {
                                    dirctInky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[inkyPos[Y] + 1, inkyPos[X]] != "w")
                                {
                                    dirctInky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[inkyPos[Y], inkyPos[X] - 1] != "w")
                                {
                                    dirctInky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[inkyPos[Y], inkyPos[X] + 1] != "w")
                                {
                                    dirctInky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: INKY_DRCT_UP");
                                break;
                        }
                    }
                }
            }
            else if (dirctInky[RIGHT])
            {
                if (strgrid[inkyPos[Y], inkyPos[X]] == "w")
                {
                    inkyPos[X]--;
                    dirctInky[RIGHT] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[inkyPos[Y] - 1, inkyPos[X]] != "w")
                                {
                                    dirctInky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[inkyPos[Y] + 1, inkyPos[X]] != "w")
                                {
                                    dirctInky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[inkyPos[Y], inkyPos[X] - 1] != "w")
                                {
                                    dirctInky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[inkyPos[Y], inkyPos[X] + 1] != "w")
                                {
                                    dirctInky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: INKY_DRCT_RIGHT");
                                break;
                        }
                    }
                }
            }
            else if (dirctInky[DOWN])
            {
                if (strgrid[inkyPos[Y], inkyPos[X]] == "w")
                {
                    inkyPos[Y]--;
                    dirctInky[DOWN] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[inkyPos[Y] - 1, inkyPos[X]] != "w")
                                {
                                    dirctInky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[inkyPos[Y] + 1, inkyPos[X]] != "w")
                                {
                                    dirctInky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[inkyPos[Y], inkyPos[X] - 1] != "w")
                                {
                                    dirctInky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[inkyPos[Y], inkyPos[X] + 1] != "w")
                                {
                                    dirctInky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: INKY_DRCT_DOWN");
                                break;
                        }
                    }
                }
            }
            else if (dirctInky[LEFT])
            {
                if (strgrid[inkyPos[Y], inkyPos[X]] == "w")
                {
                    inkyPos[X]++;
                    dirctInky[LEFT] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[inkyPos[Y] - 1, inkyPos[X]] != "w")
                                {
                                    dirctInky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[inkyPos[Y] + 1, inkyPos[X]] != "w")
                                {
                                    dirctInky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[inkyPos[Y], inkyPos[X] - 1] != "w")
                                {
                                    dirctInky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[inkyPos[Y], inkyPos[X] + 1] != "w")
                                {
                                    dirctInky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: INKY_DRCT_LEFT");
                                break;
                        }
                    }
                }
            }

            if (dirctPinky[UP])
            {
                if (strgrid[pinkyPos[Y], pinkyPos[X]] == "w")
                {
                    pinkyPos[Y]++;
                    dirctPinky[UP] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[pinkyPos[Y] - 1, pinkyPos[X]] != "w")
                                {
                                    dirctPinky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[pinkyPos[Y] + 1, pinkyPos[X]] != "w")
                                {
                                    dirctPinky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[pinkyPos[Y], pinkyPos[X] - 1] != "w")
                                {
                                    dirctPinky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[pinkyPos[Y], pinkyPos[X] + 1] != "w")
                                {
                                    dirctPinky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: PINKY_DRCT_UP");
                                break;
                        }
                    }
                }
            }
            else if (dirctPinky[RIGHT])
            {
                if (strgrid[pinkyPos[Y], pinkyPos[X]] == "w")
                {
                    pinkyPos[X]--;
                    dirctPinky[RIGHT] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[pinkyPos[Y] - 1, pinkyPos[X]] != "w")
                                {
                                    dirctPinky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[pinkyPos[Y] + 1, pinkyPos[X]] != "w")
                                {
                                    dirctPinky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[pinkyPos[Y], pinkyPos[X] - 1] != "w")
                                {
                                    dirctPinky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[pinkyPos[Y], pinkyPos[X] + 1] != "w")
                                {
                                    dirctPinky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: PINKY_DRCT_RIGHT");
                                break;
                        }
                    }
                }
            }
            else if (dirctPinky[DOWN])
            {
                if (strgrid[pinkyPos[Y], pinkyPos[X]] == "w")
                {
                    pinkyPos[Y]--;
                    dirctPinky[DOWN] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[pinkyPos[Y] - 1, pinkyPos[X]] != "w")
                                {
                                    dirctPinky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[pinkyPos[Y] + 1, pinkyPos[X]] != "w")
                                {
                                    dirctPinky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[pinkyPos[Y], pinkyPos[X] - 1] != "w")
                                {
                                    dirctPinky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[pinkyPos[Y], pinkyPos[X] + 1] != "w")
                                {
                                    dirctPinky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: PINKY_DRCT_DOWN");
                                break;
                        }
                    }
                }
            }
            else if (dirctPinky[LEFT])
            {
                if (strgrid[pinkyPos[Y], pinkyPos[X]] == "w")
                {
                    pinkyPos[X]++;
                    dirctPinky[LEFT] = false;

                    Random rnd = new Random();
                    bool repeat = true;
                    while (repeat)
                    {
                        switch (rnd.Next(4))
                        {
                            case UP:
                                if (strgrid[pinkyPos[Y] - 1, pinkyPos[X]] != "w")
                                {
                                    dirctPinky[UP] = true;
                                    repeat = false;
                                }
                                break;
                            case DOWN:
                                if (strgrid[pinkyPos[Y] + 1, pinkyPos[X]] != "w")
                                {
                                    dirctPinky[DOWN] = true;
                                    repeat = false;
                                }
                                break;
                            case LEFT:
                                if (strgrid[pinkyPos[Y], pinkyPos[X] - 1] != "w")
                                {
                                    dirctPinky[LEFT] = true;
                                    repeat = false;
                                }
                                break;
                            case RIGHT:
                                if (strgrid[pinkyPos[Y], pinkyPos[X] + 1] != "w")
                                {
                                    dirctPinky[RIGHT] = true;
                                    repeat = false;
                                }
                                break;
                            default:
                                Console.WriteLine("ERR: PINKY_DRCT_LEFT");
                                break;
                        }
                    }
                }
            }
        }

        private void checkHitPellet()
        {
            if (strgrid[pacmanPos[Y], pacmanPos[X]] == "p")
            {
                points += 10;

                pellets[index, X] = pacmanPos[X];
                pellets[index, Y] = pacmanPos[Y];
                index++;
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
                    else if (strgrid[y, x] == "B")
                        spriteBatch.Draw(spritesheet, gameGrid[x, y], BLINKY, Color.White);
                    else if (strgrid[y, x] == "C")
                        spriteBatch.Draw(spritesheet, gameGrid[x, y], CLYDE, Color.White);
                    else if (strgrid[y, x] == "I")
                        spriteBatch.Draw(spritesheet, gameGrid[x, y], INKY, Color.White);
                    else if (strgrid[y, x] == "Pi")
                        spriteBatch.Draw(spritesheet, gameGrid[x, y], PINKY, Color.White);
                    else
                        spriteBatch.Draw(t, gameGrid[x, y], Color.LightPink);
                }

            spriteBatch.DrawString(sf, "Points: " + points, new Vector2(20, 20), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
