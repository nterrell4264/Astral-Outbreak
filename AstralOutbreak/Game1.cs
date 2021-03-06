﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AstralOutbreak
{
    //Enum that handles all of the states our game can be in
    public enum GameState { MainMenu, OptionsMenu, PauseMenu, LoadMenu, SaveMenu, Playing, GameOverMenu, Victory }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Managers
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SoundManager soundManager;
        SpriteManager spriteManager;
        public static MenuManager menuManager;
        public static InputManager Inputs { get; set; }
        public static Random Rand { get; set; }

        //Victory boolean
        public static bool WonGame { get; set; }
        //Dialogue update timer
        public static float DialogueTimer { get; set; }

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
            graphics.PreferredBackBufferWidth = graphics.PreferredBackBufferWidth * 12 / 10;
            graphics.PreferredBackBufferHeight = graphics.PreferredBackBufferHeight * 12 / 10;
            graphics.ApplyChanges();
            Rand = new Random();
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
            Graphics = graphics;
            graphics.ToggleFullScreen();
            CurrentState = GameState.MainMenu;
            ResetGame();
            RoomManager.Active.Width = GraphicsDevice.Viewport.Width;
            RoomManager.Active.Height = GraphicsDevice.Viewport.Height;
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
            spriteManager = new SpriteManager(this);
            spriteManager.AddFont("font", Content.Load<SpriteFont>("font"));
            spriteManager.AddFont("textfont", Content.Load<SpriteFont>("font"));
            spriteManager.AddFont("UIfont", Content.Load<SpriteFont>("UIfont"));
            //Level sprites
            spriteManager.AddTexture(Content.Load<Texture2D>("rect"));
            spriteManager.AddTexture(Content.Load<Texture2D>("MoreSprites"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Avatars"));
            spriteManager.AddTexture(Content.Load<Texture2D>("TextBox"));
            spriteManager.AddTexture(Content.Load<Texture2D>("PlayerSprites"));
            spriteManager.AddTexture(Content.Load<Texture2D>("JackrabbitSprites"));
            spriteManager.AddTexture(Content.Load<Texture2D>("SlugSprites"));
            spriteManager.AddTexture(Content.Load<Texture2D>("MiscSprites"));
            //Buttons
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/NewButton"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/OptionsButton"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/SaveButton"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/LoadButton"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/ResumeButton"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/RetryButton"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/QuitButton"));
            //Other UI
            spriteManager.AddTexture(Content.Load<Texture2D>("BestTitleScreenEver"));
            spriteManager.AddTexture(Content.Load<Texture2D>("BestYouWinScreenEver"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/SmallMenuBG"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/LargeMenuBG"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/HudBG"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/UpgradeBG"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/rollIcon"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/dashIcon"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/spreadIcon"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/batIcon"));
            spriteManager.AddTexture(Content.Load<Texture2D>("Menus/BossHealthBar"));
            spriteManager.AddTexture(Content.Load<Texture2D>("TileSheet"));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Inputs.Update();
            if (Inputs[ActionButton.PauseButton].Status == ButtonStatus.Pressed)
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
            if (CurrentState == GameState.Playing) //Game time updating
            {
                if (RoomManager.Active.PlayerOne.IsDead && !DialogueManager.Active) CurrentState = GameState.GameOverMenu;
                RoomManager.Active.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
                if (DialogueManager.Active)
                {
                    DialogueTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (DialogueTimer > 5)
                    {
                        DialogueManager.Update();
                        DialogueTimer = 0;
                    }
                }
                else
                {
                    DialogueTimer = 0;
                    
                }
                if (WonGame)
                {
                    CurrentState = GameState.MainMenu;
                }
            }
            else if (DialogueManager.DisplayOnMenu)
            {
                if (DialogueManager.Active)
                {
                    DialogueTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (DialogueTimer > 3)
                    {
                        DialogueManager.Update();
                        DialogueTimer = 0;
                    }
                }
                else
                {
                    DialogueTimer = 0;
                }
            }
            menuManager.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin(SpriteSortMode.BackToFront);
            spriteManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void ResetGame()
        {
            if(RoomManager.Active != null)
                RoomManager.Active = new Room(RoomManager.Active.Width, RoomManager.Active.Height, RoomManager.Active.Gravity);
            else
                RoomManager.Active = new Room(1000, 1000, new Vector2(0, 9f));
            if (File.Exists("MapData.dat"))
            {
                StreamReader input = null;
                try
                {
                    input = new StreamReader(File.OpenRead("MapData.dat"));
                    RoomManager.Active.LoadRoom((JsonConvert.DeserializeObject<Map>(input.ReadToEnd())));
                }
                catch (Exception e)
                {
                    RoomManager.Active.PhysicsObjects.Add(new Wall(new Vector2(0, 100), 1000, 5));
                    RoomManager.Active.PlayerOne = new Player(new Vector2(0, 0), 20, 20, 20);
                    RoomManager.Active.PhysicsObjects.Add(RoomManager.Active.PlayerOne);
                }
                finally
                {
                    if (input != null)
                        input.Close();
                }
            }
        }
        //Hooray!
        public static void Victory()
        {
            if(!WonGame)
                DialogueManager.Update(Triggers.Victory);
            WonGame = true;
        }

        public static Tuple<float, float> Score(Map map)
        {
            if (File.Exists("MapData.dat"))
            {
                int countEnemy = 0;
                int countMaxEnemy = 0;
                int countItems = 0;
                int countMaxItems = 0;
                StreamReader input = null;
                try
                {
                    input = new StreamReader(File.OpenRead("MapData.dat"));
                    Map m = ((JsonConvert.DeserializeObject<Map>(input.ReadToEnd())));
                    countEnemy = RoomManager.MapData.CountEnemies();
                    countMaxEnemy = m.CountEnemies();
                    countItems = RoomManager.MapData.CountItems();
                    countMaxItems = m.CountItems();

                }
                catch (Exception e)
                {
                    
                }
                finally
                {
                    if (input != null)
                        input.Close();
                }
                if (countMaxItems == 0 || countMaxEnemy == 0)
                    return null;

                return new Tuple<float, float>(1 - (countEnemy / (float)countMaxEnemy), 1 - (countItems / (float)countMaxItems));
            }
            else
                return null;
        }
    }
}
