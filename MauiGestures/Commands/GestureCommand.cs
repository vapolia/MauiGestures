
namespace MauiGestures.Commands;

/// <summary>
/// Custom and generic command implementation.
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class GestureCommand<T> : ICommand<T>
{
    #region Fields
    private readonly Action<T> _execute;
    private readonly Func<T, bool>? _canExecute;

    #endregion Fields

    #region Methods
    /// <summary>
    /// Initializes a new instance of the <see cref="GestureCommand{T}"/> class.
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GestureCommand(Action<T> execute, Func<T, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Determines whether the command can be executed.
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(object? parameter)
    {
        if (parameter is T t)
        {
            return CanExecute(t);
        }
        return false;
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Execute(object? parameter)
    {
        if (parameter is T t)
        {
            Execute(t);
        }
        else
        {
            throw new ArgumentException($"Ungültiger Parameter. Erwartet wird {typeof(T).Name}.", nameof(parameter));
        }
    }

    /// <summary>
    /// Determines whether the command can be executed.
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(T parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter"></param>
    public void Execute(T parameter)
    {
        _execute(parameter);
    }

    /// <summary>
    /// Event that is raised when the <see cref="RaiseCanExecuteChanged"/> method changes.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Manually raises the <see cref="CanExecuteChanged"/> event.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}

/// <summary>
/// Custom command implementation.
/// </summary>
public partial class GestureCommand : ICommand
{
    #region Fields
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    #endregion Fields

    #region Methods
    /// <summary>
    /// Initializes a new instance of the <see cref="GestureCommand"/> class.
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GestureCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Determines whether the command can be executed.
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() ?? true;
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter"></param>
    public void Execute(object? parameter)
    {
        _execute();
    }

    /// <summary>
    ///  Event that is raised when the <see cref="CanExecute"/> method changes.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Manually raises the <see cref="CanExecuteChanged"/> event.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion Methods
}