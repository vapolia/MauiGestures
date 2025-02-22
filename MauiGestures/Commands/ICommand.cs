namespace MauiGestures.Commands;

/// <summary>
/// Interface for a command.
/// </summary>
public interface ICommand : System.Windows.Input.ICommand
{

}

/// <summary>
/// Interface for a command with a generic parameter.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICommand<T> : System.Windows.Input.ICommand
{
    public void Execute(T parameter);
    public bool CanExecute(T parameter);
}