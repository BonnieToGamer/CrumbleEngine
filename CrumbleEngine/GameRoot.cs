using System;
using Apos.Input;
using CrumbleEngine.Scenes;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.ImGuiNet;
using MonoGame.Utilities.Scene;

namespace CrumbleEngine;

public class GameRoot : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager _sceneManager;
    public static ImGuiRenderer GuiRenderer;
    internal static GameRoot Instance { get; private set; }

    public GameRoot()
    {
        Instance = this;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        GuiRenderer = new ImGuiRenderer(this);

        _sceneManager = new();
        _sceneManager.AddScene(new MainGame());

        // Window.AllowUserResizing = true;
        _graphics.PreferredBackBufferWidth = 100 * 6;
        _graphics.PreferredBackBufferHeight = 100 * 6;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        GuiRenderer.RebuildFontAtlas();

        _sceneManager.GetCurrentScene().Load();

        InputHelper.Setup(this);
    }

    protected override void Update(GameTime gameTime)
    {
        InputHelper.UpdateSetup();
        MouseExtended.Update();

        if (Input.CloseApplication.Pressed())
            Exit();

        _sceneManager.GetCurrentScene().Update(gameTime);

        base.Update(gameTime);
        InputHelper.UpdateCleanup();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _sceneManager.GetCurrentScene().Draw(gameTime, _spriteBatch);
        
        base.Draw(gameTime);

        GuiRenderer.BeginLayout(gameTime);

        GuiRenderer.EndLayout();
    }
}
