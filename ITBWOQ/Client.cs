using ImGuiNET;
using ImGuiNET.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ITBWOQ
{
    public class Client : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private ImGuiRenderer imGuiRenderer;
        private State state = new();

        public Client()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            Window.Title = "Wise One's Quest";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            imGuiRenderer = new(this);
            imGuiRenderer.RebuildFontAtlas();

            ImGui.GetStyle().WindowRounding = 0.0f;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            imGuiRenderer.BeforeLayout(gameTime);

            ImGuiUi.RenderWindow(state);

            imGuiRenderer.AfterLayout();

            base.Draw(gameTime);
        }
    }
}