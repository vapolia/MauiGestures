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

    /// <summary>
    /// Event handlers
    /// </summary>
    private EventHandler? tapEvent, doubleTapEvent, longPressEvent, panEvent, swipeLeftEvent, swipeRightEvent, swipeTopEvent, swipeBottomEvent;
    private EventHandler<PinchEventArgs>? pinchEvent;
    private EventHandler<PointEventArgs>? tapPointEvent, doubleTapPointEvent, longPressPointEvent;
    private EventHandler<PanEventArgs>? panPointEvent;

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

            // Get event handlers
            tapEvent = Gesture.GetTapEvent(element);
            doubleTapEvent = Gesture.GetDoubleTapEvent(element);
            longPressEvent = Gesture.GetLongPressEvent(element);
            panEvent = Gesture.GetPanEvent(element);
            pinchEvent = Gesture.GetPinchEvent(element);
            swipeLeftEvent = Gesture.GetSwipeLeftEvent(element);
            swipeRightEvent = Gesture.GetSwipeRightEvent(element);
            swipeTopEvent = Gesture.GetSwipeTopEvent(element);
            swipeBottomEvent = Gesture.GetSwipeBottomEvent(element);
            tapPointEvent = Gesture.GetTapPointEvent(element);
            doubleTapPointEvent = Gesture.GetDoubleTapPointEvent(element);
            longPressPointEvent = Gesture.GetLongPressPointEvent(element);
            panPointEvent = Gesture.GetPanPointEvent(element);

#if WINDOWS
            processIntermediatePoints = Gesture.WindowsProcessIntermediatePoints(element);
            if (detector != null)
            {
                try
                {
                    detector.CrossSlideHorizontally = Gesture.WindowsCrossSlideHorizontally(element);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to set CrossSlideHorizontally: {e.Message}");
                    Console.WriteLine(e);
                }
            }
#endif

#if IOS || MACCATALYST
            if(panDetector != null)
                panDetector!.IsImmediate = Gesture.GetIsPanImmediate(element);
            if(pinchDetector != null)
                pinchDetector!.IsImmediate = Gesture.GetIsPinchImmediate(element);
#endif
        }
    }

    // Helper methods to trigger events
    protected void OnTap(object sender, EventArgs e)
    {
        tapEvent?.Invoke(sender, e);
    }

    protected void OnDoubleTap(object sender, EventArgs e)
    {
        doubleTapEvent?.Invoke(sender, e);
    }

    protected void OnLongPress(object sender, EventArgs e)
    {
        longPressEvent?.Invoke(sender, e);
    }

    protected void OnPan(object sender, EventArgs e)
    {
        panEvent?.Invoke(sender, e);
    }

    protected void OnPinch(object sender, PinchEventArgs e)
    {
        pinchEvent?.Invoke(sender, e);
    }

    protected void OnSwipeLeft(object sender, EventArgs e)
    {
        swipeLeftEvent?.Invoke(sender, e);
    }

    protected void OnSwipeRight(object sender, EventArgs e)
    {
        swipeRightEvent?.Invoke(sender, e);
    }

    protected void OnSwipeTop(object sender, EventArgs e)
    {
        swipeTopEvent?.Invoke(sender, e);
    }

    protected void OnSwipeBottom(object sender, EventArgs e)
    {
        swipeBottomEvent?.Invoke(sender, e);
    }

    protected void OnTapPoint(object sender, PointEventArgs e)
    {
        tapPointEvent?.Invoke(sender, e);
    }

    protected void OnDoubleTapPoint(object sender, PointEventArgs e)
    {
        doubleTapPointEvent?.Invoke(sender, e);
    }

    protected void OnLongPressPoint(object sender, PointEventArgs e)
    {
        longPressPointEvent?.Invoke(sender, e);
    }

    protected void OnPanPoint(object sender, PanEventArgs e)
    {
        panPointEvent?.Invoke(sender, e);
    }
}
