using System.ComponentModel;
using MauiGestures.Commands;
using MauiGestures.GestureArgs;
using Microsoft.Maui.Controls.Platform;

namespace MauiGestures.Platform
{
    internal partial class GestureEffect : PlatformEffect
    {
        #region Fields

        private object? commandParameter;

        private ICommand? tapCommand;
        private ICommand? rightTabCommand;
        private ICommand? doubleTapCommand;
        private ICommand? longPressCommand;
        private ICommand? panCommand;
        private ICommand? pinchCommand; 

        private ICommand<PointArgs>? tapPointCommand;
        private ICommand<PointArgs>? rightTapPointCommand;
        private ICommand<PointArgs>? doubleTapPointCommand;
        private ICommand<PointArgs>? longPressPointCommand;
        private ICommand<PanArgs>? panPointCommand;
        private ICommand<PinchArgs>? pinchPointCommand;
        private ICommand<SwipeArgs>? swipeCommand;

        private event Action<PointArgs>? tapEvent;
        private event Action<PointArgs>? doubleTapEvent;
        private event Action<PointArgs>? rightTapEvent;
        private event Action<PointArgs>? longPressEvent;
        private event Action<PanArgs>? panEvent;
        private event Action<PinchArgs>? pinchEvent;
        private event Action<SwipeArgs>? swipeEvent;

        #if WINDOWS
        private bool returnAllPointsOnWindows;
        #endif

        #endregion Fields

        #region Constructors

        #endregion Constructors

        #region Methods
        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName is not "X" and not "Y" and not "Width" and not "Height")
            {
                var element = Element;

                tapCommand = Gesture.GetTapCommand(element);
                doubleTapCommand = Gesture.GetDoubleTapCommand(element);
                rightTabCommand = Gesture.GetRightTapCommand(element);
                longPressCommand = Gesture.GetLongPressCommand(element);
                panCommand = Gesture.GetPanCommand(element);
                pinchCommand = Gesture.GetPinchCommand(element);
                swipeCommand = Gesture.GetSwipeCommand(element);

                tapPointCommand = Gesture.GetTapPointCommand(element);
                doubleTapPointCommand = Gesture.GetDoubleTapPointCommand(element);
                rightTapPointCommand = Gesture.GetRightTapPointCommand(element);
                longPressPointCommand = Gesture.GetLongPressPointCommand(element);
                panPointCommand = Gesture.GetPanPointCommand(element);
                pinchPointCommand = Gesture.GetPinchPointCommand(element);

                commandParameter = Gesture.GetCommandParameter(element);

                tapEvent = Gesture.GetTapEvent(element);
                doubleTapEvent = Gesture.GetDoubleTapEvent(element);
                rightTapEvent = Gesture.GetRightTapEvent(element);
                longPressEvent = Gesture.GetLongPressEvent(element);
                panEvent = Gesture.GetPanEvent(element);
                pinchEvent = Gesture.GetPinchEvent(element);
                swipeEvent = Gesture.GetSwipeEvent(element);

                #if WINDOWS
                returnAllPointsOnWindows = Gesture.GetReturnAllPointsOnWindows(element);
                #endif

                #if IOS || MACCATALYST
                panDetector.IsImmediate = Gesture.GetIsPanImmediate(element);
                pinchDetector.IsImmediate = Gesture.GetIsPinchImmediate(element);
                #endif
            }
        }

        protected override partial void OnAttached();
        protected override partial void OnDetached();

        private void TriggerCommand(ICommand? command, object? parameter)
        {
            if (command?.CanExecute(parameter) == true)
                command.Execute(parameter);
        }

        private void TriggerCommand<T>(ICommand<T>? command, T parameter)
        {
            if (command?.CanExecute(parameter) == true)
                command.Execute(parameter);
        }

        private void TriggerEvent<T>(Action<T>? action, T args)
        {
            action?.Invoke(args);
        }

        #endregion Methods
    }
}