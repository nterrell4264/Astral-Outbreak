using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AstralOutbreak
{
    //Enum that handles all of the states our game can be in
    public enum GameState { MainMenu, PauseMenu, LoadMenu, SaveMenu, Playing, GameOver }

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
        public InputManager Inputs { get; set; }
        SoundManager soundManager;
        SpriteManager spriteManager;
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
            CurrentState = GameState.MainMenu;
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

            //RoomManager.Data.Current.Step((float) gameTime.ElapsedGameTime.TotalSeconds);
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
            if(false)
            for(int i = 0; i < RoomManager.Data.Current.PhysicsObjects.Count; i++)
            {
                spriteBatch.Draw(testTexture, 
                    new Rectangle((int)RoomManager.Data.Current.PhysicsObjects[i].Position.X, (int)RoomManager.Data.Current.PhysicsObjects[i].Position.Y,
                    (int)RoomManager.Data.Current.PhysicsObjects[i].Width, (int)RoomManager.Data.Current.PhysicsObjects[i].Height), 
                    Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
