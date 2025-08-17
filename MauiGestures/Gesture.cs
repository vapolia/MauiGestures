using System.Windows.Input;

namespace MauiGestures;

/// <summary>
/// Attached properties for gesture
/// </summary>
/// <example>
///   &lt;Grid Gesture.TapCommand="{Binding GridTappedCommand}"&gt;
///      ...
///   &lt;/Grid&gt;
/// </example>
public static class Gesture
{
    public static readonly BindableProperty LongPressCommandProperty = BindableProperty.CreateAttached("LongPressCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty TapCommandProperty = BindableProperty.CreateAttached("TapCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty DoubleTapCommandProperty = BindableProperty.CreateAttached("DoubleTapCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty PanCommandProperty = BindableProperty.CreateAttached("PanCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty PinchCommandProperty = BindableProperty.CreateAttached("PinchCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    /// <summary>
    /// Determines whether pinch gesture should fire immediately on iOS/macOS.
    /// </summary>
    public static readonly BindableProperty IsPinchImmediateProperty = BindableProperty.CreateAttached("IsPinchImmediate", typeof(bool), typeof(Gesture), false, propertyChanged: CommandChanged);
    
    public static readonly BindableProperty SwipeLeftCommandProperty = BindableProperty.CreateAttached("SwipeLeftCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty SwipeRightCommandProperty = BindableProperty.CreateAttached("SwipeRightCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty SwipeTopCommandProperty = BindableProperty.CreateAttached("SwipeTopCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty SwipeBottomCommandProperty = BindableProperty.CreateAttached("SwipeBottomCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);

    /// <summary>
    /// Take a Point parameter 
    /// </summary>
    public static readonly BindableProperty LongPressPointCommandProperty = BindableProperty.CreateAttached("LongPressPointCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    /// <summary>
    /// Take a Point parameter 
    /// </summary>
    public static readonly BindableProperty TapPointCommandProperty = BindableProperty.CreateAttached("TapPointCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    /// <summary>
    /// Take a Point parameter 
    /// </summary>
    public static readonly BindableProperty DoubleTapPointCommandProperty = BindableProperty.CreateAttached("DoubleTapPointCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    /// <summary>
    /// Take a (Point,GestureStatus) parameter (it's a tuple) 
    /// </summary>
    public static readonly BindableProperty PanPointCommandProperty = BindableProperty.CreateAttached("PanPointCommand", typeof(ICommand), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty IsPanImmediateProperty = BindableProperty.CreateAttached("IsPanImmediate", typeof(bool), typeof(Gesture), false, propertyChanged: CommandChanged);

    /// <summary>
    /// Android only: min distance to trigger a swipe
    /// </summary>
    public static readonly BindableProperty AndroidSwipeThresholdProperty = BindableProperty.CreateAttached("SwipeThreshold", typeof(int), typeof(Gesture), 40, propertyChanged: CommandChanged);

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(Gesture), null);

    public static readonly BindableProperty WindowsProcessIntermediatePointsProperty = BindableProperty.CreateAttached(nameof(WindowsProcessIntermediatePoints), typeof(bool), typeof(Gesture), false, propertyChanged: CommandChanged);
    public static readonly BindableProperty WindowsCrossSlideHorizontallyProperty = BindableProperty.CreateAttached(nameof(WindowsCrossSlideHorizontally), typeof(bool), typeof(Gesture), true, propertyChanged: CommandChanged);

    // Event properties
    public static readonly BindableProperty TapEventProperty = BindableProperty.CreateAttached("TapEvent", typeof(EventHandler), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty DoubleTapEventProperty = BindableProperty.CreateAttached("DoubleTapEvent", typeof(EventHandler), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty LongPressEventProperty = BindableProperty.CreateAttached("LongPressEvent", typeof(EventHandler), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty PanEventProperty = BindableProperty.CreateAttached("PanEvent", typeof(EventHandler), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty PinchEventProperty = BindableProperty.CreateAttached("PinchEvent", typeof(EventHandler<PinchEventArgs>), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty SwipeLeftEventProperty = BindableProperty.CreateAttached("SwipeLeftEvent", typeof(EventHandler), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty SwipeRightEventProperty = BindableProperty.CreateAttached("SwipeRightEvent", typeof(EventHandler), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty SwipeTopEventProperty = BindableProperty.CreateAttached("SwipeTopEvent", typeof(EventHandler), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty SwipeBottomEventProperty = BindableProperty.CreateAttached("SwipeBottomEvent", typeof(EventHandler), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty TapPointEventProperty = BindableProperty.CreateAttached("TapPointEvent", typeof(EventHandler<PointEventArgs>), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty DoubleTapPointEventProperty = BindableProperty.CreateAttached("DoubleTapPointEvent", typeof(EventHandler<PointEventArgs>), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty LongPressPointEventProperty = BindableProperty.CreateAttached("LongPressPointEvent", typeof(EventHandler<PointEventArgs>), typeof(Gesture), null, propertyChanged: CommandChanged);
    public static readonly BindableProperty PanPointEventProperty = BindableProperty.CreateAttached("PanPointEvent", typeof(EventHandler<PanEventArgs>), typeof(Gesture), null, propertyChanged: CommandChanged);

    public static ICommand GetLongPressCommand(BindableObject view) => (ICommand)view.GetValue(LongPressCommandProperty);
    public static ICommand GetTapCommand(BindableObject view) => (ICommand)view.GetValue(TapCommandProperty);
    public static ICommand GetDoubleTapCommand(BindableObject view) => (ICommand)view.GetValue(DoubleTapCommandProperty);
    public static ICommand GetPanCommand(BindableObject view) => (ICommand)view.GetValue(PanCommandProperty);
    public static ICommand GetPinchCommand(BindableObject view) => (ICommand)view.GetValue(PinchCommandProperty);
    public static bool GetIsPinchImmediate(BindableObject view) => (bool)view.GetValue(IsPinchImmediateProperty);
    public static bool GetIsPanImmediate(BindableObject view) => (bool)view.GetValue(IsPanImmediateProperty);
    public static ICommand GetSwipeLeftCommand(BindableObject view) => (ICommand)view.GetValue(SwipeLeftCommandProperty);
    public static ICommand GetSwipeRightCommand(BindableObject view) => (ICommand)view.GetValue(SwipeRightCommandProperty);
    public static ICommand GetSwipeTopCommand(BindableObject view) => (ICommand)view.GetValue(SwipeTopCommandProperty);
    public static ICommand GetSwipeBottomCommand(BindableObject view) => (ICommand)view.GetValue(SwipeBottomCommandProperty);

    /// <summary>
    /// Take a Point parameter 
    /// </summary>
    public static ICommand GetLongPressPointCommand(BindableObject view) => (ICommand)view.GetValue(LongPressPointCommandProperty);
    /// <summary>
    /// Take a Point parameter 
    /// </summary>
    public static ICommand GetTapPointCommand(BindableObject view) => (ICommand)view.GetValue(TapPointCommandProperty);
    /// <summary>
    /// Take a Point parameter 
    /// </summary>
    public static ICommand GetDoubleTapPointCommand(BindableObject view) => (ICommand)view.GetValue(DoubleTapPointCommandProperty);
    /// <summary>
    /// Take a (Point,GestureStatus) parameter (it is a tuple) 
    /// </summary>
    public static ICommand GetPanPointCommand(BindableObject view) => (ICommand)view.GetValue(PanPointCommandProperty);

    public static bool WindowsProcessIntermediatePoints(BindableObject view) => (bool)view.GetValue(WindowsProcessIntermediatePointsProperty);
    public static bool WindowsCrossSlideHorizontally(BindableObject view) => (bool)view.GetValue(WindowsCrossSlideHorizontallyProperty);

    public static void SetLongPressCommand(BindableObject view, ICommand value) => view.SetValue(LongPressCommandProperty, value);
    public static void SetTapCommand(BindableObject view, ICommand value) => view.SetValue(TapCommandProperty, value);
    public static void SetDoubleTapCommand(BindableObject view, ICommand value) => view.SetValue(DoubleTapCommandProperty, value);
    public static void SetPanCommand(BindableObject view, ICommand value) => view.SetValue(PanCommandProperty, value);
    public static void SetPinchCommand(BindableObject view, ICommand value) => view.SetValue(PinchCommandProperty, value);
    public static void SetIsPinchImmediate(BindableObject view, bool isImmediate) => view.SetValue(IsPinchImmediateProperty, isImmediate);
    public static void SetIsPanImmediate(BindableObject view, bool isImmediate) => view.SetValue(IsPanImmediateProperty, isImmediate);
    public static void SetSwipeLeftCommand(BindableObject view, ICommand value) => view.SetValue(SwipeLeftCommandProperty, value);
    public static void SetSwipeRightCommand(BindableObject view, ICommand value) => view.SetValue(SwipeRightCommandProperty, value);
    public static void SetSwipeTopCommand(BindableObject view, ICommand value) => view.SetValue(SwipeTopCommandProperty, value);
    public static void SetSwipeBottomCommand(BindableObject view, ICommand value) => view.SetValue(SwipeBottomCommandProperty, value);
    /// <summary>
    /// Take a Point parameter 
    /// </summary>
    public static void SetLongPressPointCommand(BindableObject view, ICommand value) => view.SetValue(LongPressPointCommandProperty, value);
    /// <summary>
    /// Take a Point parameter 
    /// </summary>
    public static void SetTapPointCommand(BindableObject view, ICommand value) => view.SetValue(TapPointCommandProperty, value);
    /// <summary>
    /// Take a Point parameter 
    /// </summary>
    public static void SetDoubleTapPointCommand(BindableObject view, ICommand value) => view.SetValue(DoubleTapPointCommandProperty, value);
    /// <summary>
    /// Take a Point parameter 
    /// </summary>
    public static void SetPanPointCommand(BindableObject view, ICommand value) => view.SetValue(PanPointCommandProperty, value);

    public static int GetAndroidSwipeThreshold(BindableObject view) => (int)view.GetValue(AndroidSwipeThresholdProperty);
    public static void SetAndroidSwipeThreshold(BindableObject view, int value) => view.SetValue(AndroidSwipeThresholdProperty, value);

    public static object GetCommandParameter(BindableObject view) => view.GetValue(CommandParameterProperty);
    public static void SetCommandParameter(BindableObject view, object value) => view.SetValue(CommandParameterProperty, value);

    // Event getters and setters
    public static EventHandler GetTapEvent(BindableObject view) => (EventHandler)view.GetValue(TapEventProperty);
    public static void SetTapEvent(BindableObject view, EventHandler value) => view.SetValue(TapEventProperty, value);

    public static EventHandler GetDoubleTapEvent(BindableObject view) => (EventHandler)view.GetValue(DoubleTapEventProperty);
    public static void SetDoubleTapEvent(BindableObject view, EventHandler value) => view.SetValue(DoubleTapEventProperty, value);

    public static EventHandler GetLongPressEvent(BindableObject view) => (EventHandler)view.GetValue(LongPressEventProperty);
    public static void SetLongPressEvent(BindableObject view, EventHandler value) => view.SetValue(LongPressEventProperty, value);

    public static EventHandler GetPanEvent(BindableObject view) => (EventHandler)view.GetValue(PanEventProperty);
    public static void SetPanEvent(BindableObject view, EventHandler value) => view.SetValue(PanEventProperty, value);

    public static EventHandler<PinchEventArgs> GetPinchEvent(BindableObject view) => (EventHandler<PinchEventArgs>)view.GetValue(PinchEventProperty);
    public static void SetPinchEvent(BindableObject view, EventHandler<PinchEventArgs> value) => view.SetValue(PinchEventProperty, value);

    public static EventHandler GetSwipeLeftEvent(BindableObject view) => (EventHandler)view.GetValue(SwipeLeftEventProperty);
    public static void SetSwipeLeftEvent(BindableObject view, EventHandler value) => view.SetValue(SwipeLeftEventProperty, value);

    public static EventHandler GetSwipeRightEvent(BindableObject view) => (EventHandler)view.GetValue(SwipeRightEventProperty);
    public static void SetSwipeRightEvent(BindableObject view, EventHandler value) => view.SetValue(SwipeRightEventProperty, value);

    public static EventHandler GetSwipeTopEvent(BindableObject view) => (EventHandler)view.GetValue(SwipeTopEventProperty);
    public static void SetSwipeTopEvent(BindableObject view, EventHandler value) => view.SetValue(SwipeTopEventProperty, value);

    public static EventHandler GetSwipeBottomEvent(BindableObject view) => (EventHandler)view.GetValue(SwipeBottomEventProperty);
    public static void SetSwipeBottomEvent(BindableObject view, EventHandler value) => view.SetValue(SwipeBottomEventProperty, value);

    public static EventHandler<PointEventArgs> GetTapPointEvent(BindableObject view) => (EventHandler<PointEventArgs>)view.GetValue(TapPointEventProperty);
    public static void SetTapPointEvent(BindableObject view, EventHandler<PointEventArgs> value) => view.SetValue(TapPointEventProperty, value);

    public static EventHandler<PointEventArgs> GetDoubleTapPointEvent(BindableObject view) => (EventHandler<PointEventArgs>)view.GetValue(DoubleTapPointEventProperty);
    public static void SetDoubleTapPointEvent(BindableObject view, EventHandler<PointEventArgs> value) => view.SetValue(DoubleTapPointEventProperty, value);

    public static EventHandler<PointEventArgs> GetLongPressPointEvent(BindableObject view) => (EventHandler<PointEventArgs>)view.GetValue(LongPressPointEventProperty);
    public static void SetLongPressPointEvent(BindableObject view, EventHandler<PointEventArgs> value) => view.SetValue(LongPressPointEventProperty, value);

    public static EventHandler<PanEventArgs> GetPanPointEvent(BindableObject view) => (EventHandler<PanEventArgs>)view.GetValue(PanPointEventProperty);
    public static void SetPanPointEvent(BindableObject view, EventHandler<PanEventArgs> value) => view.SetValue(PanPointEventProperty, value);

    // Attached events for XAML usage
    public static readonly BindableProperty TapProperty = BindableProperty.CreateAttached("Tap", typeof(EventHandler), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty DoubleTapProperty = BindableProperty.CreateAttached("DoubleTap", typeof(EventHandler), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty LongPressProperty = BindableProperty.CreateAttached("LongPress", typeof(EventHandler), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty PanProperty = BindableProperty.CreateAttached("Pan", typeof(EventHandler), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty PinchProperty = BindableProperty.CreateAttached("Pinch", typeof(EventHandler<PinchEventArgs>), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty SwipeLeftProperty = BindableProperty.CreateAttached("SwipeLeft", typeof(EventHandler), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty SwipeRightProperty = BindableProperty.CreateAttached("SwipeRight", typeof(EventHandler), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty SwipeTopProperty = BindableProperty.CreateAttached("SwipeTop", typeof(EventHandler), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty SwipeBottomProperty = BindableProperty.CreateAttached("SwipeBottom", typeof(EventHandler), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty TapPointProperty = BindableProperty.CreateAttached("TapPoint", typeof(EventHandler<PointEventArgs>), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty DoubleTapPointProperty = BindableProperty.CreateAttached("DoubleTapPoint", typeof(EventHandler<PointEventArgs>), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty LongPressPointProperty = BindableProperty.CreateAttached("LongPressPoint", typeof(EventHandler<PointEventArgs>), typeof(Gesture), null, propertyChanged: OnEventChanged);
    public static readonly BindableProperty PanPointProperty = BindableProperty.CreateAttached("PanPoint", typeof(EventHandler<PanEventArgs>), typeof(Gesture), null, propertyChanged: OnEventChanged);

    // Getters and setters for attached events
    public static EventHandler GetTap(BindableObject view) => (EventHandler)view.GetValue(TapProperty);
    public static void SetTap(BindableObject view, EventHandler value) => view.SetValue(TapProperty, value);

    public static EventHandler GetDoubleTap(BindableObject view) => (EventHandler)view.GetValue(DoubleTapProperty);
    public static void SetDoubleTap(BindableObject view, EventHandler value) => view.SetValue(DoubleTapProperty, value);

    public static EventHandler GetLongPress(BindableObject view) => (EventHandler)view.GetValue(LongPressProperty);
    public static void SetLongPress(BindableObject view, EventHandler value) => view.SetValue(LongPressProperty, value);

    public static EventHandler GetPan(BindableObject view) => (EventHandler)view.GetValue(PanProperty);
    public static void SetPan(BindableObject view, EventHandler value) => view.SetValue(PanProperty, value);

    public static EventHandler<PinchEventArgs> GetPinch(BindableObject view) => (EventHandler<PinchEventArgs>)view.GetValue(PinchProperty);
    public static void SetPinch(BindableObject view, EventHandler<PinchEventArgs> value) => view.SetValue(PinchProperty, value);

    public static EventHandler GetSwipeLeft(BindableObject view) => (EventHandler)view.GetValue(SwipeLeftProperty);
    public static void SetSwipeLeft(BindableObject view, EventHandler value) => view.SetValue(SwipeLeftProperty, value);

    public static EventHandler GetSwipeRight(BindableObject view) => (EventHandler)view.GetValue(SwipeRightProperty);
    public static void SetSwipeRight(BindableObject view, EventHandler value) => view.SetValue(SwipeRightProperty, value);

    public static EventHandler GetSwipeTop(BindableObject view) => (EventHandler)view.GetValue(SwipeTopProperty);
    public static void SetSwipeTop(BindableObject view, EventHandler value) => view.SetValue(SwipeTopProperty, value);

    public static EventHandler GetSwipeBottom(BindableObject view) => (EventHandler)view.GetValue(SwipeBottomProperty);
    public static void SetSwipeBottom(BindableObject view, EventHandler value) => view.SetValue(SwipeBottomProperty, value);

    public static EventHandler<PointEventArgs> GetTapPoint(BindableObject view) => (EventHandler<PointEventArgs>)view.GetValue(TapPointProperty);
    public static void SetTapPoint(BindableObject view, EventHandler<PointEventArgs> value) => view.SetValue(TapPointProperty, value);

    public static EventHandler<PointEventArgs> GetDoubleTapPoint(BindableObject view) => (EventHandler<PointEventArgs>)view.GetValue(DoubleTapPointProperty);
    public static void SetDoubleTapPoint(BindableObject view, EventHandler<PointEventArgs> value) => view.SetValue(DoubleTapPointProperty, value);

    public static EventHandler<PointEventArgs> GetLongPressPoint(BindableObject view) => (EventHandler<PointEventArgs>)view.GetValue(LongPressPointProperty);
    public static void SetLongPressPoint(BindableObject view, EventHandler<PointEventArgs> value) => view.SetValue(LongPressPointProperty, value);

    public static EventHandler<PanEventArgs> GetPanPoint(BindableObject view) => (EventHandler<PanEventArgs>)view.GetValue(PanPointProperty);
    public static void SetPanPoint(BindableObject view, EventHandler<PanEventArgs> value) => view.SetValue(PanPointProperty, value);

    private static void OnEventChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is View view)
        {
            GetOrCreateBehavior(view);

            // Copy the event handler to the corresponding Event property for internal use
            if (newValue is EventHandler eventHandler)
            {
                var property = GetEventPropertyFromAttachedProperty(bindable, oldValue, newValue);
                if (property != null)
                    view.SetValue(property, eventHandler);
            }
            else if (newValue is EventHandler<PointEventArgs> pointEventHandler)
            {
                var property = GetEventPropertyFromAttachedProperty(bindable, oldValue, newValue);
                if (property != null)
                    view.SetValue(property, pointEventHandler);
            }
            else if (newValue is EventHandler<PanEventArgs> panEventHandler)
            {
                var property = GetEventPropertyFromAttachedProperty(bindable, oldValue, newValue);
                if (property != null)
                    view.SetValue(property, panEventHandler);
            }
            else if (newValue is EventHandler<PinchEventArgs> pinchEventHandler)
            {
                var property = GetEventPropertyFromAttachedProperty(bindable, oldValue, newValue);
                if (property != null)
                    view.SetValue(property, pinchEventHandler);
            }
        }
    }

    private static BindableProperty? GetEventPropertyFromAttachedProperty(BindableObject bindable, object oldValue, object newValue)
    {
        // Find which property was changed by checking the stack trace or using reflection
        // This is a simplified approach - in practice, you might want to use a more robust method
        var frame = new System.Diagnostics.StackFrame(3);
        var method = frame.GetMethod();
        var propertyName = method?.Name?.Replace("Set", "");

        return propertyName switch
        {
            "Tap" => TapEventProperty,
            "DoubleTap" => DoubleTapEventProperty,
            "LongPress" => LongPressEventProperty,
            "Pan" => PanEventProperty,
            "Pinch" => PinchEventProperty,
            "SwipeLeft" => SwipeLeftEventProperty,
            "SwipeRight" => SwipeRightEventProperty,
            "SwipeTop" => SwipeTopEventProperty,
            "SwipeBottom" => SwipeBottomEventProperty,
            "TapPoint" => TapPointEventProperty,
            "DoubleTapPoint" => DoubleTapPointEventProperty,
            "LongPressPoint" => LongPressPointEventProperty,
            "PanPoint" => PanPointEventProperty,
            _ => null
        };
    }

    private static void GetOrCreateBehavior(View view)
    {
        var behavior = view.Behaviors.OfType<GestureBehavior>().FirstOrDefault();
        if (behavior == null)
        {
            behavior = new GestureBehavior();
            view.Behaviors.Add(behavior);
        }
    }

    private static void CommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is View view)
            GetOrCreateBehavior(view);
    }

    /// <summary>
    /// This method is no longer needed as gestures now use behaviors instead of effects.
    /// You can safely remove this call from your MauiProgram.cs file.
    /// </summary>
    [Obsolete("This method is no longer needed as gestures now use behaviors instead of effects. You can safely remove this call from your MauiProgram.cs file.", false)]
    public static MauiAppBuilder UseAdvancedGestures(this MauiAppBuilder builder)
    {
        // No longer needed - behaviors are automatically attached when gesture properties are set
        return builder;
    }
}
