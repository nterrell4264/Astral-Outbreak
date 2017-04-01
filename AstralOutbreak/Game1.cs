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
            CurrentState = GameState.Playing;

            RoomManager.Active = new Room(2000, 2000, new Vector2(0, 3f));
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
            spriteManager = SpriteManager.Instance;
            spriteManager.AddTexture(Content.Load<Texture2D>("rect"));
            spriteManager.AddTexture(Content.Load<Texture2D>("mnuStart"));
            CurrentState = GameState.MainMenu;
            menuManager = new MenuManager();
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            // TEST DRAW WILL REMOVE WHEN SPRITEMANAGER WORKS
            if(CurrentState == GameState.Playing)
            {
                for (int i = 0; i < RoomManager.Active.PhysicsObjects.Count; i++)
                {
                    if (RoomManager.Active.PhysicsObjects[i] is Player)
                        spriteBatch.Draw(spriteManager.masterList["rect"],
                        new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Position.X - (int)RoomManager.Active.CameraX, 
                        (int)RoomManager.Active.PhysicsObjects[i].Position.Y - (int)RoomManager.Active.CameraY,
                        (int)RoomManager.Active.PhysicsObjects[i].Width, (int)RoomManager.Active.PhysicsObjects[i].Height),
                        Color.Blue);
                    else
                        spriteBatch.Draw(spriteManager.masterList["rect"],
                            new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Position.X - (int)RoomManager.Active.CameraX, 
                            (int)RoomManager.Active.PhysicsObjects[i].Position.Y - (int)RoomManager.Active.CameraY,
                            (int)RoomManager.Active.PhysicsObjects[i].Width, (int)RoomManager.Active.PhysicsObjects[i].Height),
                            Color.Black);
                }
            }
            foreach (MenuContent menuPart in menuManager.items) {
                Texture2D texture = spriteManager.masterList[menuPart.TextureName];
                spriteBatch.Draw(texture, new Rectangle(menuPart.Location, new Point(texture.Width, texture.Height)), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
