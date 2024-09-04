namespace MonoGame.Utilities.StateMachine;

public class StateMachineManager<T>
{
    public IState<T> CurrentState { get; private set; }
    public T Parent { get; private set; }

    public StateMachineManager(IState<T> startingState, T parent)
    {
        Parent = parent;
        ChangeState(startingState);
    }

    public void ChangeState(IState<T> state)
    {
        if (CurrentState != null)
            CurrentState.Exit();
        
        CurrentState = state;
        CurrentState.Parent = Parent;
        CurrentState.Enter();
    }

    public void Update(float delta)
    {
        IState<T> newState = CurrentState.Update(delta);
        if (newState != null) ChangeState(newState);
    }
}
