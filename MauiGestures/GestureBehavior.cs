using System.ComponentModel;
using System.Windows.Input;

namespace MauiGestures;

internal partial class GestureBehavior : PlatformBehavior<View>
{
    private object? commandParameter;
    
    /// <summary>
    /// Takes a Point parameter
    /// Except panPointCommand which takes a (Point,GestureStatus) parameter (it's a tuple) 
    /// </summary>
    private ICommand? tapPointCommand, panPointCommand, doubleTapPointCommand, longPressPointCommand;
    
    /// <summary>
    /// Takes a CommandParameter parameter
    /// </summary>
    private ICommand? tapCommand, panCommand, doubleTapCommand, longPressCommand, swipeLeftCommand, swipeRightCommand, swipeTopCommand, swipeBottomCommand;
    
    /// <summary>
    /// 1 parameter: PinchEventArgs
    /// </summary>
    private ICommand? pinchCommand;

#if WINDOWS
    private bool processIntermediatePoints;
#endif

    void OnAttached(View view)
    {
        view.PropertyChanged += OnViewPropertyChanged;
    }

    void OnDetach(View view)
    {
        view.PropertyChanged -= OnViewPropertyChanged;
    }

    void OnViewPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName is not "X" and not "Y" and not "Width" and not "Height")
        {
            var element = (View)sender!;
            
            tapCommand = Gesture.GetTapCommand(element);
            panCommand = Gesture.GetPanCommand(element);

            pinchCommand = Gesture.GetPinchCommand(element);
            doubleTapCommand = Gesture.GetDoubleTapCommand(element);
            longPressCommand = Gesture.GetLongPressCommand(element);

            swipeLeftCommand = Gesture.GetSwipeLeftCommand(element);
            swipeRightCommand = Gesture.GetSwipeRightCommand(element);
            swipeTopCommand = Gesture.GetSwipeTopCommand(element);
            swipeBottomCommand = Gesture.GetSwipeBottomCommand(element);

            tapPointCommand = Gesture.GetTapPointCommand(element);
            panPointCommand = Gesture.GetPanPointCommand(element);
            doubleTapPointCommand = Gesture.GetDoubleTapPointCommand(element);
            longPressPointCommand = Gesture.GetLongPressPointCommand(element);

            commandParameter = Gesture.GetCommandParameter(element);

#if WINDOWS
            processIntermediatePoints = Gesture.GetWindowsProcessIntermediatePoints(element);
            if (detector != null)
                detector.CrossSlideHorizontally = Gesture.GetWindowsCrossSlideHorizontally(element);
#endif

#if IOS || MACCATALYST
            if(panDetector != null)
                panDetector!.IsImmediate = Gesture.GetIsPanImmediate(element);
            if(pinchDetector != null)
                pinchDetector!.IsImmediate = Gesture.GetIsPinchImmediate(element);
#endif
        }
    }
}
