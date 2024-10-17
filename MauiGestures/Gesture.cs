using MauiGestures.Commands;
using MauiGestures.Extensions;
using MauiGestures.GestureArgs;
using MauiGestures.Platform;

namespace MauiGestures
{
    public static class Gesture
    {
        #region Bindable Properties

        internal static readonly BindableProperty CommandParameterProperty = BindableProperty.CreateAttached(
            "CommandParameter",
            typeof(object),
            typeof(Gesture),
            null,
            propertyChanged: BindableObjectChanged);
        public static object GetCommandParameter(BindableObject view) => view.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(BindableObject view, object value) => view.SetValue(CommandParameterProperty, value);


        internal static readonly BindableProperty IsPanImmediateProperty = BindableProperty.CreateAttached(
        "IsPanImmediate",
        typeof(bool),
        typeof(Gesture),
        false,
        propertyChanged: BindableObjectChanged);
        public static bool GetIsPanImmediate(BindableObject view) => (bool)view.GetValue(IsPanImmediateProperty);
        public static void SetIsPanImmediate(BindableObject view, bool isImmediate) => view.SetValue(IsPanImmediateProperty, isImmediate);


        internal static readonly BindableProperty IsPinchImmediateProperty = BindableProperty.CreateAttached(
            "IsPinchImmediate",
            typeof(bool),
            typeof(Gesture),
            false,
            propertyChanged: BindableObjectChanged);
        public static bool GetIsPinchImmediate(BindableObject view) => (bool)view.GetValue(IsPinchImmediateProperty);
        public static void SetIsPinchImmediate(BindableObject view, bool isImmediate) => view.SetValue(IsPinchImmediateProperty, isImmediate);


        internal static readonly BindableProperty ReturnAllPointsOnWindowsProperty = BindableProperty.CreateAttached(
            "ReturnAllPointsOnWindows",
            typeof(bool),
            typeof(Gesture),
            false,
            propertyChanged: BindableObjectChanged);
        public static bool GetReturnAllPointsOnWindows(BindableObject view) => (bool)view.GetValue(ReturnAllPointsOnWindowsProperty);
        public static void SetReturnAllPointsOnWindows(BindableObject view, bool value) => view.SetValue(ReturnAllPointsOnWindowsProperty, value);

        #endregion Bindable Properties

        #region Bindable Commands

        internal static readonly BindableProperty TapCommandProperty = BindableProperty.CreateAttached(
            "TapCommand", 
            typeof(ICommand), 
            typeof(Gesture), 
            null, 
            propertyChanged: BindableObjectChanged);
        public static ICommand GetTapCommand(BindableObject view) => (ICommand)view.GetValue(TapCommandProperty);
        public static void SetTapCommand(BindableObject view, ICommand value) => view.SetValue(TapCommandProperty, value);


        internal static readonly BindableProperty TapPointCommandProperty = BindableProperty.CreateAttached(
            "TapPointCommand", 
            typeof(ICommand<PointArgs>), 
            typeof(Gesture), 
            null, 
            propertyChanged: BindableObjectChanged);
        public static ICommand<PointArgs> GetTapPointCommand(BindableObject view) => (ICommand<PointArgs>)view.GetValue(TapPointCommandProperty);
        public static void SetTapPointCommand(BindableObject view, ICommand<PointArgs> value) => view.SetValue(TapPointCommandProperty, value);


        internal static readonly BindableProperty DoubleTapCommandProperty = BindableProperty.CreateAttached(
            "DoubleTapCommand", 
            typeof(ICommand), 
            typeof(Gesture), 
            null, 
            propertyChanged: BindableObjectChanged);
        public static ICommand GetDoubleTapCommand(BindableObject view) => (ICommand)view.GetValue(DoubleTapCommandProperty);
        public static void SetDoubleTapCommand(BindableObject view, ICommand value) => view.SetValue(DoubleTapCommandProperty, value);


        internal static readonly BindableProperty DoubleTapPointCommandProperty = BindableProperty.CreateAttached(
            "DoubleTapPointCommand", 
            typeof(ICommand<PointArgs>), 
            typeof(Gesture), 
            null, 
            propertyChanged: BindableObjectChanged);
        public static ICommand<PointArgs> GetDoubleTapPointCommand(BindableObject view) => (ICommand<PointArgs>)view.GetValue(DoubleTapPointCommandProperty);
        public static void SetDoubleTapPointCommand(BindableObject view, ICommand<PointArgs> value) => view.SetValue(DoubleTapPointCommandProperty, value);


        internal static readonly BindableProperty RightTapCommandProperty = BindableProperty.CreateAttached(
            "RightTapCommand", 
            typeof(ICommand), 
            typeof(Gesture), 
            null, 
            propertyChanged: BindableObjectChanged);
        public static ICommand GetRightTapCommand(BindableObject view) => (ICommand)view.GetValue(RightTapCommandProperty);
        public static void SetRightTapCommand(BindableObject view, ICommand value) => view.SetValue(RightTapCommandProperty, value);


        internal static readonly BindableProperty RightTapPointCommandProperty = BindableProperty.CreateAttached(
            "RightTapPointCommand", 
            typeof(ICommand<PointArgs>), 
            typeof(Gesture), 
            null, 
            propertyChanged: BindableObjectChanged);
        public static ICommand<PointArgs> GetRightTapPointCommand(BindableObject view) => (ICommand<PointArgs>)view.GetValue(RightTapPointCommandProperty);
        public static void SetRightTapPointCommand(BindableObject view, ICommand<PointArgs> value) => view.SetValue(RightTapPointCommandProperty, value);


        internal static readonly BindableProperty LongPressCommandProperty = BindableProperty.CreateAttached(
            "LongPressCommand", 
            typeof(ICommand), 
            typeof(Gesture), 
            null, 
            propertyChanged: BindableObjectChanged);
        public static ICommand GetLongPressCommand(BindableObject view) => (ICommand)view.GetValue(LongPressCommandProperty);
        public static void SetLongPressCommand(BindableObject view, ICommand value) => view.SetValue(LongPressCommandProperty, value);

        internal static readonly BindableProperty LongPressPointCommandProperty = BindableProperty.CreateAttached(
            "LongPressPointCommand", 
            typeof(ICommand<PointArgs>), 
            typeof(Gesture), 
            null, 
            propertyChanged: BindableObjectChanged);
        public static ICommand<PointArgs> GetLongPressPointCommand(BindableObject view) => (ICommand<PointArgs>)view.GetValue(LongPressPointCommandProperty);
        public static void SetLongPressPointCommand(BindableObject view, ICommand<PointArgs> value) => view.SetValue(LongPressPointCommandProperty, value);


        internal static readonly BindableProperty PanCommandProperty = BindableProperty.CreateAttached(
            "PanCommand", 
            typeof(ICommand), 
            typeof(Gesture), 
            null, 
            propertyChanged: BindableObjectChanged);
        public static ICommand GetPanCommand(BindableObject view) => (ICommand)view.GetValue(PanCommandProperty);
        public static void SetPanCommand(BindableObject view, ICommand value) => view.SetValue(PanCommandProperty, value);


        internal static readonly BindableProperty PanPointCommandProperty = BindableProperty.CreateAttached(
            "PanPointCommand",
            typeof(ICommand<PanArgs>),
            typeof(Gesture),
            null,
            propertyChanged: BindableObjectChanged);
        public static ICommand<PanArgs> GetPanPointCommand(BindableObject view) => (ICommand<PanArgs>)view.GetValue(PanPointCommandProperty);
        public static void SetPanPointCommand(BindableObject view, ICommand<PanArgs> value) => view.SetValue(PanPointCommandProperty, value);


        internal static readonly BindableProperty PinchCommandProperty = BindableProperty.CreateAttached(
            "PinchCommand",
            typeof(ICommand),
            typeof(Gesture),
            null,
            propertyChanged: BindableObjectChanged);
        public static ICommand GetPinchCommand(BindableObject view) => (ICommand)view.GetValue(PinchCommandProperty);
        public static void SetPinchCommand(BindableObject view, ICommand value) => view.SetValue(PinchCommandProperty, value);

        internal static readonly BindableProperty PinchPointCommandProperty = BindableProperty.CreateAttached(
            "PinchPointCommand", 
            typeof(ICommand<PinchArgs>), 
            typeof(Gesture), 
            null, 
            propertyChanged: BindableObjectChanged);
        public static ICommand<PinchArgs> GetPinchPointCommand(BindableObject view) => (ICommand<PinchArgs>)view.GetValue(PinchPointCommandProperty);
        public static void SetPinchPointCommand(BindableObject view, ICommand<PinchArgs> value) => view.SetValue(PinchPointCommandProperty, value);


        internal static readonly BindableProperty SwipeCommandProperty = BindableProperty.CreateAttached(
            "SwipeCommand",
            typeof(ICommand<SwipeArgs>),
            typeof(Gesture),
            null,
            propertyChanged: BindableObjectChanged);
        public static ICommand<SwipeArgs> GetSwipeCommand(BindableObject view) => (ICommand<SwipeArgs>)view.GetValue(SwipeCommandProperty);
        public static void SetSwipeCommand(BindableObject view, ICommand<SwipeArgs> value) => view.SetValue(SwipeCommandProperty, value);

        #endregion Bindable Commands

        #region Bindable Events

        internal static readonly BindableProperty TapEventProperty = BindableProperty.CreateAttached(
            "TapEvent",
            typeof(Action<PointArgs>),
            typeof(Gesture),
            null,
            propertyChanged: BindableObjectChanged);

        public static Action<PointArgs>? GetTapEvent(BindableObject view) => (Action<PointArgs>?)view.GetValue(TapEventProperty);
        public static void SetTapEvent(BindableObject view, Action<PointArgs>? value) => view.SetValue(TapEventProperty, value);


        internal static readonly BindableProperty DoubleTapEventProperty = BindableProperty.CreateAttached(
            "DoubleTapEvent",
            typeof(Action<PointArgs>),
            typeof(Gesture),
            null,
            propertyChanged: BindableObjectChanged);
        public static Action<PointArgs>? GetDoubleTapEvent(BindableObject view) => (Action<PointArgs>?)view.GetValue(DoubleTapEventProperty);
        public static void SetDoubleTapEvent(BindableObject view, Action<PointArgs>? value) => view.SetValue(DoubleTapEventProperty, value);


        internal static readonly BindableProperty RightTapEventProperty = BindableProperty.CreateAttached(
            "RightTapEvent",
            typeof(Action<PointArgs>),
            typeof(Gesture),
            null,
            propertyChanged: BindableObjectChanged);
        public static Action<PointArgs>? GetRightTapEvent(BindableObject view) => (Action<PointArgs>?)view.GetValue(RightTapEventProperty);
        public static void SetRightTapEvent(BindableObject view, Action<PointArgs>? value) => view.SetValue(RightTapEventProperty, value);


        internal static readonly BindableProperty LongPressEventProperty = BindableProperty.CreateAttached(
            "LongPressEvent",
            typeof(Action<PointArgs>),
            typeof(Gesture),
            null,
            propertyChanged: BindableObjectChanged);
        public static Action<PointArgs>? GetLongPressEvent(BindableObject view) => (Action<PointArgs>?)view.GetValue(LongPressEventProperty);
        public static void SetLongPressEvent(BindableObject view, Action<PointArgs>? value) => view.SetValue(LongPressEventProperty, value);


        internal static readonly BindableProperty PanEventProperty = BindableProperty.CreateAttached(
            "PanEvent",
            typeof(Action<PanArgs>),
            typeof(Gesture),
            null,
            propertyChanged: BindableObjectChanged);

        public static Action<PanArgs>? GetPanEvent(BindableObject view) => (Action<PanArgs>?)view.GetValue(PanEventProperty);
        public static void SetPanEvent(BindableObject view, Action<PanArgs>? value) => view.SetValue(PanEventProperty, value);


        internal static readonly BindableProperty PinchEventProperty = BindableProperty.CreateAttached(
            "PinchEvent",
            typeof(Action<PinchArgs>),
            typeof(Gesture),
            null,
            propertyChanged: BindableObjectChanged);
        public static Action<PinchArgs>? GetPinchEvent(BindableObject view) => (Action<PinchArgs>?)view.GetValue(PinchEventProperty);
        public static void SetPinchEvent(BindableObject view, Action<PinchArgs>? value) => view.SetValue(PinchEventProperty, value);


        internal static readonly BindableProperty SwipeEventProperty = BindableProperty.CreateAttached(
            "SwipeEvent",
            typeof(Action<SwipeArgs>),
            typeof(Gesture),
            null,
            propertyChanged: BindableObjectChanged);
        public static Action<SwipeArgs>? GetSwipeEvent(BindableObject view) => (Action<SwipeArgs>?)view.GetValue(SwipeEventProperty);
        public static void SetSwipeEvent(BindableObject view, Action<SwipeArgs>? value) => view.SetValue(SwipeEventProperty, value);

        #endregion Bindable Events

        #region Methods

        private static void BindableObjectChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is View view)
                view.GetOrCreateGestureEffect();
        }

        public static MauiAppBuilder UseMauiGestures(this MauiAppBuilder builder)
        {
            builder.ConfigureEffects(effects =>
            {
                effects.Add<GestureRoutingEffect, GestureEffect>();
            });
            return builder;
        }

        #endregion Methods
    }
}
