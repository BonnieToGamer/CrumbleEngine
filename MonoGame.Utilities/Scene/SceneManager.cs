using System.Collections.Generic;

namespace MonoGame.Utilities.Scene;

public class SceneManager
{
    private readonly Stack<IScene> _sceneStack;

    public SceneManager()
    {
        _sceneStack = new Stack<IScene>();
    }

    public void AddScene(IScene scene)
    {
        _sceneStack.Push(scene);
    }

    public void RemoveScene()
    {
        _sceneStack.Pop();
    }

    public IScene GetCurrentScene()
    {
        return _sceneStack.Peek();
    }

}
