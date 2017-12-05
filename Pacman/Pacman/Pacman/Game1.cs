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

namespace Pacman
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState oldkb;

        Rectangle bgR;
        Rectangle pacr; //Pac Man Rectangle Placeholder

        Texture2D bgT;
        Texture2D wbx;
        Texture2D pact; //pacman texture

        MouseState clicks;

        Vector2 clickPos;

        SpriteFont coordinates; //font used for pac man's coordinates in the top left

        int pacx, pacy; //pacman's x and y variables for movement

        Boolean pacdown, pacup, pacright, pacleft; //booleans to keep pacman moving in one direction at a time


        Rectangle[] wallBoxes;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 813;
            IsMouseVisible = true;
            
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
            bgR = new Rectangle(0, 115, 813, 736);
            pacx = 387; pacy = 653;
            clicks = Mouse.GetState();
            clickPos = Vector2.Zero;
            wallBoxes = new Rectangle[50];
            wallBoxes[0] = new Rectangle(0,115,813,15);
            wallBoxes[1] = new Rectangle(792,125,23,230);
            wallBoxes[2] = new Rectangle(0,125,23,230);
            wallBoxes[3] = new Rectangle(15,342,145,12);
            wallBoxes[4] = new Rectangle(654,342,145,12);
            wallBoxes[5] = new Rectangle(655,350,13,80);
            wallBoxes[6] = new Rectangle(146,350,13,80);
            wallBoxes[7] = new Rectangle(655,424,155,10);
            wallBoxes[8] = new Rectangle(0,424,155,10);
            wallBoxes[9] = new Rectangle(655,485,155,10);
            wallBoxes[10] = new Rectangle(0,485,155,10);
            wallBoxes[11] = new Rectangle(655,494,10,78);
            wallBoxes[12] = new Rectangle(148,494,10,78);
            wallBoxes[13] = new Rectangle(655,569,158,8);
            wallBoxes[14] = new Rectangle(0,569,158,8);
            wallBoxes[15] = new Rectangle(800,577,10,272);
            wallBoxes[16] = new Rectangle(5,577,10,272);
            wallBoxes[17] = new Rectangle(0,838,813,10);
            oldkb = Keyboard.GetState();
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
            bgT = this.Content.Load<Texture2D>("pacmanBackround");
            wbx = Content.Load<Texture2D>("wallbox");
            pact = this.Content.Load<Texture2D>("gru");
            coordinates = this.Content.Load<SpriteFont>("SpriteFont1");


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

            // TODO: Add your update logic here
            clicks = Mouse.GetState();
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Right) && !oldkb.IsKeyDown(Keys.Right))
            {
                pacdown = false;
                pacleft = false;
                pacup = false;
                pacright = true;
            }
            if (kb.IsKeyDown(Keys.Left) && !oldkb.IsKeyDown(Keys.Left))
            {
                pacdown = false;
                pacup = false;
                pacright = false;
                pacleft = true;
            }
            if (kb.IsKeyDown(Keys.Up) && !oldkb.IsKeyDown(Keys.Up))
            {
                pacdown = false;
                pacleft = false;
                pacright = false;
                pacup = true;
            }
            if (kb.IsKeyDown(Keys.Down) && !oldkb.IsKeyDown(Keys.Down))
            {
                pacup = false;
                pacleft = false;
                pacright = false;
                pacdown = true;
            }
            if (pacdown == true)
                pacy = pacy + 2;
            if (pacup == true)
                pacy = pacy - 2;
            if (pacleft == true)
                pacx = pacx - 2;
            if (pacright == true)
                pacx = pacx + 2;
            pacr = new Rectangle(pacx, pacy, 40, 40);

            if (clicks.LeftButton == ButtonState.Pressed)
            {
                clickPos = new Vector2(clicks.X, clicks.Y);
                System.Console.WriteLine(clickPos.X + ", " + clickPos.Y);
            }
            oldkb = kb;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            spriteBatch.Draw(bgT, bgR, Color.White);

            for (int i = 0; i < wallBoxes.Length; i++)
            {
                spriteBatch.Draw(wbx,wallBoxes[i],Color.White);
            }
            spriteBatch.Draw(pact, pacr, Color.White);
            spriteBatch.DrawString(coordinates, pacx.ToString(), new Vector2(20, 40), Color.White);
            spriteBatch.DrawString(coordinates, pacy.ToString(), new Vector2(60, 40), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
