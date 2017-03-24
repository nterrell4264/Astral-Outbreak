using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AstralOutbreak
{
    //Enum that handles all of the states our game can be in
    public enum GameState { MainMenu, OptionsMenu, PauseMenu, LoadMenu, SaveMenu, Playing, GameOverMenu }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //TEST
        Texture2D testTexture;
        //END TEST



        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        FileManager fileManager;
        public static InputManager Inputs { get; set; }
        SoundManager soundManager;
        SpriteManager spriteManager;
        MenuManager menuManager;
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
            Inputs = new InputManager();
            IsMouseVisible = true;
            CurrentState = GameState.Playing;
            RoomManager.Active = new Room(2000, 2000, new Vector2(0, 3f));
            RoomManager.Active.PhysicsObjects.Add(new Wall(new Vector2(0, 64), 300, 20));
            RoomManager.Active.PhysicsObjects.Add(new Player(new Vector2(4, 4), 20, 20, 10));

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
            testTexture = Content.Load<Texture2D>("rect");
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
            if (CurrentState == GameState.Playing && Inputs.PauseButtonState == ButtonStatus.Pressed) CurrentState = GameState.PauseMenu;
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
                        spriteBatch.Draw(testTexture,
                        new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Position.X, (int)RoomManager.Active.PhysicsObjects[i].Position.Y,
                        (int)RoomManager.Active.PhysicsObjects[i].Width, (int)RoomManager.Active.PhysicsObjects[i].Height),
                        Color.Blue);
                    else
                        spriteBatch.Draw(testTexture,
                            new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Position.X, (int)RoomManager.Active.PhysicsObjects[i].Position.Y,
                            (int)RoomManager.Active.PhysicsObjects[i].Width, (int)RoomManager.Active.PhysicsObjects[i].Height),
                            Color.Black);
                }
            }
            foreach (MenuContent menuPart in menuManager.items)
                spriteBatch.Draw(testTexture, new Rectangle(menuPart.Location, new Point(testTexture.Width, testTexture.Height)), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
