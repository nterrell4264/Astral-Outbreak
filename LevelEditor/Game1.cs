using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace LevelEditor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public LevelInterface Level { get; set; }
        private bool unClicked;
        private KeyboardState kbLast;

        private int lastScroll;

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
            graphics.ToggleFullScreen();
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();
            // TODO: Add your initialization logic here
            if (File.Exists("LevelMap.dat"))
                Level = new LevelInterface("LevelMap.dat");
            else
                Level = new LevelInterface("MapData.dat");
            unClicked = true;
            kbLast = Keyboard.GetState();
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
            LevelInterface.DemonTexture = Content.Load<Texture2D>("rect");
            LevelInterface.SlugTexture = Content.Load<Texture2D>("rect");
            LevelInterface.WallTexture = Content.Load<Texture2D>("rect");
            LevelInterface.PlayerStartTexture = Content.Load<Texture2D>("rect");
            LevelInterface.RoundTexture = Content.Load<Texture2D>("rounded");
            LevelInterface.GridTexture = Content.Load<Texture2D>("grid");
            LevelInterface.Font = Content.Load<SpriteFont>("font");



            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            if(IsActive)
                HandleInput();

            base.Update(gameTime);
        }

        public void HandleInput()
        {
            //Mouse
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (unClicked)
                {
                    Level.Click(Mouse.GetState().Position.X, Mouse.GetState().Position.Y, GraphicsDevice.Viewport.Width / Level.Scale - 2, GraphicsDevice.Viewport.Height / Level.Scale - 2);
                }
                else
                {
                    unClicked = true;
                }
            }

            if(Mouse.GetState().ScrollWheelValue - lastScroll > 0 && Level.Scale < 32)
            {
                Level.Scale++;
            }
            else if(Mouse.GetState().ScrollWheelValue - lastScroll < 0 && Level.Scale > 1)
            {
                Level.Scale--;
            }

            lastScroll = Mouse.GetState().ScrollWheelValue;

            //Keyboard
            KeyboardState kb = Keyboard.GetState();
            if(kb.IsKeyDown(Keys.D) && (kbLast.IsKeyUp(Keys.D) || kb.IsKeyDown(Keys.LeftShift)))
            {
                if(Level.MapX < Level.MapData.Width)
                    Level.MapX++;
                if(kb.IsKeyDown(Keys.LeftControl))
                    if (Level.MapX < Level.MapData.Width - 2)
                        Level.MapX += 3;

            }
            if (kb.IsKeyDown(Keys.A) && (kbLast.IsKeyUp(Keys.A) || kb.IsKeyDown(Keys.LeftShift)))
            {
                if (Level.MapX > 0)
                    Level.MapX--;
                if (kb.IsKeyDown(Keys.LeftControl))
                    if (Level.MapX > 2)
                        Level.MapX -= 3;
            }
            if (kb.IsKeyDown(Keys.S) && (kbLast.IsKeyUp(Keys.S) || kb.IsKeyDown(Keys.LeftShift)))
            {
                if (Level.MapY < Level.MapData.Height)
                    Level.MapY++;
                if (kb.IsKeyDown(Keys.LeftControl))
                    if (Level.MapY < Level.MapData.Height - 2)
                        Level.MapY += 3;
            }
            if (kb.IsKeyDown(Keys.W) && (kbLast.IsKeyUp(Keys.W) || kb.IsKeyDown(Keys.LeftShift)))
            {
                if (Level.MapY > 0)
                    Level.MapY--;
                if (kb.IsKeyDown(Keys.LeftControl))
                    if (Level.MapY > 2)
                        Level.MapY -= 3;
            }
            if (kb.IsKeyDown(Keys.Up) && (kbLast.IsKeyUp(Keys.Up)))
            {
                if (Level.CursorSize < 5)
                    Level.CursorSize++;
            }
            if (kb.IsKeyDown(Keys.Down) && (kbLast.IsKeyUp(Keys.Down)))
            {
                if (Level.CursorSize > 1)
                    Level.CursorSize--;
            }
            if (kb.IsKeyDown(Keys.Left) && (kbLast.IsKeyUp(Keys.Left)))
            {
                if (Level.CursorValue > 0)
                    Level.CursorValue--;
            }
            if (kb.IsKeyDown(Keys.Right) && (kbLast.IsKeyUp(Keys.Right)))
            {
                if (Level.CursorValue < 6)
                    Level.CursorValue++;
            }

            if (kb.IsKeyDown(Keys.D1))
                Level.CursorItem = CursorMode.Erase;
            if (kb.IsKeyDown(Keys.D2))
                Level.CursorItem = CursorMode.Wall;
            if (kb.IsKeyDown(Keys.D3))
                Level.CursorItem = CursorMode.Demon;
            if (kb.IsKeyDown(Keys.D4))
                Level.CursorItem = CursorMode.Slug;
            if (kb.IsKeyDown(Keys.D5))
                Level.CursorItem = CursorMode.Player;
            if (kb.IsKeyDown(Keys.D6))
                Level.CursorItem = CursorMode.Item;
            if (kb.IsKeyDown(Keys.D7))
                Level.CursorItem = CursorMode.Boss;

            if (kb.IsKeyDown(Keys.Enter) && kbLast.IsKeyUp(Keys.Enter))
                Level.Save("LevelMap.dat");
            if(kb.IsKeyDown(Keys.LeftShift) && kb.IsKeyDown(Keys.LeftControl) && kb.IsKeyDown(Keys.Delete))
            {
                int i = 0;
                while (File.Exists("Backup" + i + ".dat"))
                    i++;
                Level.Save("Backup" + i + ".dat");
                Level = new LevelInterface("");
            }
            kbLast = kb;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            Level.Draw(spriteBatch, Mouse.GetState().Position.X, Mouse.GetState().Position.Y, GraphicsDevice.Viewport.Width / Level.Scale - 2, GraphicsDevice.Viewport.Height / Level.Scale - 2);
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
