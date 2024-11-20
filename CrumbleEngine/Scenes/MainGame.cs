using System;
using System.Globalization;
using Apos.Camera;
using Apos.Input;
using CrumbleEngine.Simulation;
using CrumbleEngine.Simulation.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.ImGuiNet;
using MonoGame.Utilities;
using MonoGame.Utilities.Scene;
using MouseButton = Apos.Input.MouseButton;

namespace CrumbleEngine.Scenes;

public class MainGame : IScene
{
    private World _world;
    private IVector2[,] _intentionMatrix;

    private IVirtualViewport _defaultViewport;
    private Camera _camera;

    private SpriteFont _font;
    private ElementTypes _selectedElement;

    public void Load()
    {
        _selectedElement = ElementTypes.Stone;
        _font = GameRoot.Instance.Content.Load<SpriteFont>("arial");
        _timer = new Timer(1f / 45);
        _world = new(new(100, 100));
        _intentionMatrix = new IVector2[100, 100];
        _defaultViewport = new DensityViewport(GameRoot.Instance.GraphicsDevice, GameRoot.Instance.Window, 100, 100);
        _camera = new Camera(_defaultViewport);

        _camera.Scale = new Vector2(1f, 1f);
        for (int x = 0; x < 128; x++)
        {
            // _world.SetElement(Element.GetElement(ElementTypes.Sand), new(x, 0));
        }
        
        
        _world.SetElement(new(8, 0), Element.GetElement(ElementTypes.Sand));
        _world.SetElement(new(7, 2), Element.GetElement(ElementTypes.Sand));
        _world.SetElement(new(8, 2), Element.GetElement(ElementTypes.Sand));
        _world.SetElement(new(9, 2), Element.GetElement(ElementTypes.Sand));
        
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < y + 1; x++)
            {
                _world.SetElement(new(32+x, 16+y), Element.GetElement(ElementTypes.Stone));
            }
        }
        
        for (int y = 0; y < 20; y++)
        {
            for (int x = -y; x <= 0; x++)
            {
                _world.SetElement(new(73+x, 16+y), Element.GetElement(ElementTypes.Stone));
            }
        }
        
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                _world.SetElement(new(42+x, 5+y), Element.GetElement(ElementTypes.Sand));
            }
        }
        
        
        // for (int y = 0; y < 10; y++)
        // {
        //     for (int x = 0; x < 10; x++)
        //     {
        //         IVector2 pos = new IVector2(x, y);
        //         IVector2 cPos = new IVector2(5, 5);
        //         float dx = pos.X - cPos.X;
        //         float dy = pos.Y - cPos.Y;
        //         float multi = dx * dx + dy * dy;
        //         float dist = MathF.Round(MathF.Sqrt(multi));
        //
        //         if (dist <= 5)
        //             _world.SetElement(new IVector2(x, y) + new IVector2(32+8, 0), Element.GetElement(ElementTypes.Sand, (x == 5 && y == 9)));
        //     }
        // }

        for (int i = 0; i < 128; i++)
        {
            _world.SetElement(new IVector2(i, 64), Element.GetElement(ElementTypes.Stone));
        }
    }

    private bool _started = false;
    private Timer _timer;
    private bool isWater = false;
    
    public void Update(GameTime gameTime)
    {
        if (_startSim.Pressed())
            _started = !_started;

        if (_debugSand.Pressed())
        {
            MouseState state = Mouse.GetState();
            Vector2 clickPos = _camera.ScreenToWorld(state.X, state.Y);
            clickPos += new Vector2(50, 50);
            IVector2 clickPosInt = new IVector2((int)clickPos.X, (int)clickPos.Y);

            _world.GetElement(clickPosInt).ShouldDebug = true;
            _debugSand.Consume();
            _spawnSand.Consume();
        }
        
        if (_started == false)
            return;

        if (_switch.Pressed())
        {
            isWater = !isWater;
        }
        
        if (_spawnSand.Pressed())
        {
            MouseStateExtended mouseState = MouseExtended.GetState();
            Vector2 worldPos = _camera.ScreenToWorld(mouseState.X, mouseState.Y) + new Vector2(50, 50);

            for (int y = 1; y < 10; y++)
            {
                for (int x = 1; x < 10; x++)
                {
                    IVector2 pos = new IVector2(x, y);
                    IVector2 cPos = new IVector2(5, 5);
                    float dx = pos.X - cPos.X;
                    float dy = pos.Y - cPos.Y;
                    float multi = dx * dx + dy * dy;
                    float dist = MathF.Sqrt(multi);

                    if (dist <= 5.0f)
                        _world.SetElement(new IVector2(x, y) + new IVector2((int)worldPos.X, (int)worldPos.Y), Element.GetElement(_selectedElement));
                }
            }
        }

        if (_timer.Update((float)gameTime.ElapsedGameTime.TotalSeconds))
            _world.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _camera.SetViewport();
        spriteBatch.Begin(transformMatrix: _camera.View, samplerState: SamplerState.PointClamp);

        _world.Draw(spriteBatch, _camera.XY);

        
        spriteBatch.End();
        _camera.ResetViewport();
    }

    private readonly AnyCondition _spawnSand = new AnyCondition(new MouseCondition(MouseButton.LeftButton));
    private readonly AnyCondition _switch = new AnyCondition(new MouseCondition(MouseButton.RightButton));
    private readonly AnyCondition _startSim = new AnyCondition(new KeyboardCondition(Keys.Space));
    private readonly AllCondition _debugSand = new AllCondition(new KeyboardCondition(Keys.LeftControl), new KeyboardCondition(Keys.LeftShift), new MouseCondition(MouseButton.LeftButton));
}
