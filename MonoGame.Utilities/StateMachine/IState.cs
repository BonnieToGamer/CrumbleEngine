using System;

namespace MonoGame.Utilities.StateMachine;

public interface IState<T>
{
    public T Parent { get; set; }

    public abstract void Enter();
    public abstract void Exit();
    public abstract IState<T> Update(float delta);
}
