using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AstralOutbreak
{
    //Enum that handles all of the states our game can be in
    public enum GameState { MainMenu, OptionsMenu, PauseMenu, LoadMenu, SaveMenu, Playing, GameOverMenu }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Managers
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        FileManager fileManager;
        SoundManager soundManager;
        SpriteManager spriteManager;
        MenuManager menuManager;
        public static InputManager Inputs { get; set; }

        //Current game state
        public static GameState CurrentState { get; set; }
        public static GameState prevMenu { get; set; } //Tracks previous menu state for options and new games.
        public static GraphicsDeviceManager Graphics { get; set; }

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
            if (File.Exists("config.txt"))
                try
                {
                    using (StreamReader input = new StreamReader(File.OpenRead("config.txt")))
                    {
                        Inputs = (JsonConvert.DeserializeObject<InputManager>(input.ReadToEnd()));
                        input.Close();
                    }
                }
                catch
                {
                    Inputs = new InputManager();
                }
            else
                Inputs = new InputManager();
            IsMouseVisible = true;
            Graphics = graphics;
            //graphics.ToggleFullScreen();
            CurrentState = GameState.MainMenu;

            RoomManager.Active = new Room(2000, 2000, new Vector2(0, 9f));
            if (File.Exists("MapData.dat"))
            {
                StreamReader input = null;
                try
                {
                    input = new StreamReader(File.OpenRead("MapData.dat"));
                    RoomManager.Active.LoadRoom((JsonConvert.DeserializeObject<Map>(input.ReadToEnd())));
                }
                catch(Exception e)
                {
                    RoomManager.Active.PhysicsObjects.Add(new Wall(new Vector2(0, 100), 1000, 5));
                    RoomManager.Active.PlayerOne = new Player(new Vector2(0, 0), 20, 20, 20);
                    RoomManager.Active.PhysicsObjects.Add(RoomManager.Active.PlayerOne);
                }
                finally
                {
                    if (input != null)
                        input.Close();
                    RoomManager.Active.Width = GraphicsDevice.Viewport.Width;
                    RoomManager.Active.Height = GraphicsDevice.Viewport.Height;

                }
            }
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
            menuManager = new MenuManager(this);
            spriteManager = new SpriteManager();
            spriteManager.font = Content.Load<SpriteFont>("font");
            spriteManager.AddTexture(Content.Load<Texture2D>("rect"));
            spriteManager.AddTexture(Content.Load<Texture2D>("PlayerSprites"));
            spriteManager.AddTexture(Content.Load<Texture2D>("JackrabbitSprites"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/StartButton"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/OptionsButton"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/ResumeButton"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/RetryButton"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/QuitButton"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/SmallMenuBG"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/LargeMenuBG"));

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
            if (Inputs.PauseButtonState == ButtonStatus.Pressed)
            {
                switch (CurrentState)
                {
                    case (GameState.PauseMenu):
                        CurrentState = GameState.Playing;
                        break;
                    case (GameState.Playing):
                        CurrentState = GameState.PauseMenu;
                        break;
                }
            }
            Inputs.Update();
            if (CurrentState == GameState.Playing) //Game time updating
            {
                if (RoomManager.Active.PlayerOne.IsDead) CurrentState = GameState.GameOverMenu;
                RoomManager.Active.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            menuManager.Update();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();
            spriteManager.Update(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
