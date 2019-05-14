using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brain
{
    public class BrainGame : Game
    {
        public static Texture2D Pixel { get; set; }
        public static Random Random { get; private set; }
        public static BrainGame Instance { get; private set; }
        public static ContentManager ContentManager { get; private set; }

        private readonly GraphicsDeviceManager graphics;
        private readonly Matrix camera;
        public static int gameWidth = 320;
        public static int gameHeight = 180;
        RenderTarget2D gameFrame;
        Rectangle gameFrameLocation;
        SpriteBatch spriteBatch;
        Level currentScene;
        private Point previousResolution;
        private bool switchingScreenMode;
        private SoundEffectInstance music;

        private bool isOnSplash = true; 

        public BrainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Data";
            Random = new Random();
            camera = Matrix.CreateTranslation(new Vector3(gameWidth / 2f, gameHeight / 2f, 0));
            Instance = this;
        }

        protected override void Initialize()
        {
            base.Initialize();
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            ContentManager = Content;
            currentScene = new InfoScreen(GraphicsDevice, camera, "splashscreen");
            GameSpaceMouse.Setup(gameWidth, gameHeight, camera, GraphicsDevice);
        }

        protected override void LoadContent()
        {
            gameFrame = new RenderTarget2D(GraphicsDevice, gameWidth, gameHeight);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });

            music = Content.Load<SoundEffect>("music").CreateInstance();
            music.IsLooped = true;
            music.Volume = 0.3f;
            music.Play();
        }

        protected override void UnloadContent()
        {
        }

        public void Restart()
        {
            GameState.Reset();
            currentScene = new GameScene(GraphicsDevice, camera);
        }

        protected override void Update(GameTime gameTime)
        {
            ImprovedKeyboard.Update();
            ImprovedMouse.Update();
            currentScene.Update(gameTime);

            HandleFullscreenSwitch();

            if(ImprovedKeyboard.DidJustPress(Keys.Q))
                Exit();

            if (currentScene is InfoScreen splash && splash.Continue)
            {
                if(isOnSplash)
                    currentScene = new InfoScreen(GraphicsDevice, camera, "infoscreen");
                else
                    currentScene = new GameScene(GraphicsDevice, camera);
                isOnSplash = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            #region Black bar position

            var screenW = GraphicsDevice.PresentationParameters.BackBufferWidth;
            var screenH = GraphicsDevice.PresentationParameters.BackBufferHeight;

            if (screenW == gameWidth && screenH == gameHeight)
            {
                gameFrameLocation = new Rectangle(0, 0, gameFrame.Width, gameFrame.Height);
            }
            else
            {
                var aspect = (float)gameWidth / gameHeight;

                var x = (float)screenW / gameWidth;
                var y = (float)screenH / gameHeight;

                if (x < y)
                {
                    var h = (int)(screenW / aspect);
                    gameFrameLocation = new Rectangle(0, screenH / 2 - h / 2, screenW, h);
                }
                else
                {
                    var w = (int)(screenH * aspect);
                    gameFrameLocation = new Rectangle(screenW / 2 - w / 2, 0, w, screenH);
                }
            }

            #endregion

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRenderTarget(gameFrame);
            currentScene.Draw(gameTime);
            GraphicsDevice.SetRenderTarget(null);

            // Draw render target to screen in correct scale
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            spriteBatch.Draw(gameFrame, gameFrameLocation, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleFullscreenSwitch()
        {
            if (!switchingScreenMode && ImprovedKeyboard.Pressed(Keys.LeftAlt) && ImprovedKeyboard.Pressed(Keys.Enter))
            {
                switchingScreenMode = true;
                if (Window.IsBorderlessEXT)
                {
                    Window.IsBorderlessEXT = false;
                    graphics.PreferredBackBufferWidth = previousResolution.X;
                    graphics.PreferredBackBufferHeight = previousResolution.Y;
                }
                else
                {
                    previousResolution =
                        new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    Window.IsBorderlessEXT = true;
                    graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                    graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
                }

                graphics.ApplyChanges();
            }

            if (switchingScreenMode && ImprovedKeyboard.Released(Keys.LeftAlt) && ImprovedKeyboard.Released(Keys.Enter))
                switchingScreenMode = false;
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            using (BrainGame game = new BrainGame())
            {
                game.Run();
            }
        }
    }
}
