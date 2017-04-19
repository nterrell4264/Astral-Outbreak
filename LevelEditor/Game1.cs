using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private LevelInterface level;
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
            // TODO: Add your initialization logic here
            level = new LevelInterface("MapData.dat");
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
                    level.Click(Mouse.GetState().Position.X, Mouse.GetState().Position.Y, GraphicsDevice.Viewport.Width / level.Scale - 2, GraphicsDevice.Viewport.Height / level.Scale - 2);
                }
                else
                {
                    unClicked = true;
                }
            }

            if(Mouse.GetState().ScrollWheelValue - lastScroll > 0 && level.Scale < 32)
            {
                level.Scale++;
            }
            else if(Mouse.GetState().ScrollWheelValue - lastScroll < 0 && level.Scale > 1)
            {
                level.Scale--;
            }

            lastScroll = Mouse.GetState().ScrollWheelValue;

            //Keyboard
            KeyboardState kb = Keyboard.GetState();
            if(kb.IsKeyDown(Keys.D) && (kbLast.IsKeyUp(Keys.D) || kb.IsKeyDown(Keys.LeftShift)))
            {
                if(level.MapX < level.MapData.Width)
                    level.MapX++;
            }
            if (kb.IsKeyDown(Keys.A) && (kbLast.IsKeyUp(Keys.A) || kb.IsKeyDown(Keys.LeftShift)))
            {
                if (level.MapX > 0)
                    level.MapX--;
            }
            if (kb.IsKeyDown(Keys.S) && (kbLast.IsKeyUp(Keys.S) || kb.IsKeyDown(Keys.LeftShift)))
            {
                if (level.MapY < level.MapData.Height)
                    level.MapY++;
            }
            if (kb.IsKeyDown(Keys.W) && (kbLast.IsKeyUp(Keys.W) || kb.IsKeyDown(Keys.LeftShift)))
            {
                if (level.MapY > 0)
                    level.MapY--;
            }
            if (kb.IsKeyDown(Keys.Up) && (kbLast.IsKeyUp(Keys.Up)))
            {
                if (level.CursorSize < 5)
                    level.CursorSize++;
            }
            if (kb.IsKeyDown(Keys.Down) && (kbLast.IsKeyUp(Keys.Down)))
            {
                if (level.CursorSize > 1)
                    level.CursorSize--;
            }

            if (kb.IsKeyDown(Keys.D1))
                level.CursorItem = CursorMode.Erase;
            if (kb.IsKeyDown(Keys.D2))
                level.CursorItem = CursorMode.Wall;
            if (kb.IsKeyDown(Keys.D3))
                level.CursorItem = CursorMode.Demon;
            if (kb.IsKeyDown(Keys.D4))
                level.CursorItem = CursorMode.Slug;
            if (kb.IsKeyDown(Keys.D5))
                level.CursorItem = CursorMode.Player;

            if (kb.IsKeyDown(Keys.Enter) && kbLast.IsKeyUp(Keys.Enter))
                level.Save("LevelMap.dat");

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
            level.Draw(spriteBatch, Mouse.GetState().Position.X, Mouse.GetState().Position.Y, GraphicsDevice.Viewport.Width / level.Scale - 2, GraphicsDevice.Viewport.Height / level.Scale - 2);
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
