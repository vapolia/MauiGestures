using System.ComponentModel;
using Android.Util;
using Android.Views;
using MauiGestures.GestureArgs;
using MauiGestures.Platform.Android;
using View = Android.Views.View;

namespace MauiGestures.Platform
{
    internal partial class GestureEffect
    {
        #region Fields
        private GestureDetector? gestureRecognizer;
        private readonly InternalGestureDetector tapDetector;
        private DisplayMetrics? displayMetrics;

        #endregion Fields

        #region Constructors
        public GestureEffect()
        {
            tapDetector = new InternalGestureDetector()
            {
                SwipeThresholdInPoints = 80,
                MaxSwipeDuration = 200,

                TapAction = motionEvent =>
                {
                    var x = motionEvent!.GetX();
                    var y = motionEvent!.GetY();
                    var point = PxToDp(new Point(x, y));
                    var args = new PointArgs(point, Element, Element.BindingContext);

                    TriggerCommand(tapPointCommand, args);
                    TriggerCommand(tapCommand, commandParameter);
                    TriggerEvent(tapEvent, args);
                },
                DoubleTapAction = motionEvent =>
                {
                    var x = motionEvent!.GetX();
                    var y = motionEvent!.GetY();
                    var point = PxToDp(new Point(x, y));
                    var args = new PointArgs(point, Element, Element.BindingContext);

                    TriggerCommand(doubleTapPointCommand, args);
                    TriggerCommand(doubleTapCommand, commandParameter);
                    TriggerEvent(doubleTapEvent, args);
                },
                SwipeLeftAction = motionEvent =>
                {
                    var x = motionEvent!.GetX();
                    var y = motionEvent!.GetY();
                    var point = PxToDp(new Point(x, y));
                    var distance = motionEvent.GetX() - motionEvent.GetX(1);
                    var args = new SwipeArgs(SwipeDirection.Left, distance, point);

                    TriggerCommand(swipeCommand, args);
                    TriggerEvent(swipeEvent, args);
                },
                SwipeRightAction = motionEvent =>
                {
                    var x = motionEvent!.GetX();
                    var y = motionEvent!.GetY();
                    var point = PxToDp(new Point(x, y));
                    var distance = motionEvent.GetX() - motionEvent.GetX(1);
                    var args = new SwipeArgs(SwipeDirection.Left, distance, point);

                    TriggerCommand(swipeCommand, args);
                    TriggerEvent(swipeEvent, args);
                },
                SwipeTopAction = motionEvent =>
                {
                    var x = motionEvent!.GetX();
                    var y = motionEvent!.GetY();
                    var point = PxToDp(new Point(x, y));
                    var distance = motionEvent.GetX() - motionEvent.GetX(1);
                    var args = new SwipeArgs(SwipeDirection.Left, distance, point);

                    TriggerCommand(swipeCommand, args);
                    TriggerEvent(swipeEvent, args);
                },
                SwipeBottomAction = motionEvent =>
                {
                    var x = motionEvent!.GetX();
                    var y = motionEvent!.GetY();
                    var point = PxToDp(new Point(x, y));
                    var distance = motionEvent.GetX() - motionEvent.GetX(1);
                    var args = new SwipeArgs(SwipeDirection.Left, distance, point);

                    TriggerCommand(swipeCommand, args);
                    TriggerEvent(swipeEvent, args);
                },
                PanAction = (initialDown, currentMove) =>
                {
                    var continueGesture = true;
                    var x = currentMove!.GetX();
                    var y = currentMove!.GetY();
                    var point = PxToDp(new Point(x, y));

                    var status = currentMove.Action switch
                    {
                        MotionEventActions.Down => GestureStatus.Started,
                        MotionEventActions.Move => GestureStatus.Running,
                        MotionEventActions.Up => GestureStatus.Completed,
                        MotionEventActions.Cancel => GestureStatus.Canceled,
                        _ => GestureStatus.Canceled
                    };

                    var parameter = new PanArgs(point, status);
                    TriggerCommand(panPointCommand, parameter);
                    TriggerCommand(panCommand, commandParameter);
                    TriggerEvent(panEvent, parameter);

                    if (parameter.CancelGesture)
                        continueGesture = false;

                    return continueGesture;
                },
                PinchAction = (initialDown, currentMove) =>
                {
                    if (pinchCommand != null && currentMove != null)
                    {
                        var origin0 = PxToDp(new Point(initialDown.GetX(0), initialDown.GetY(0)));
                        var origin1 = PxToDp(new Point(initialDown.GetX(1), initialDown.GetY(1)));
                        var current0 = PxToDp(new Point(currentMove.GetX(0), currentMove.GetY(0)));
                        var current1 = PxToDp(new Point(currentMove.GetX(1), currentMove.GetY(1)));

                        var status = currentMove.Action switch
                        {
                            MotionEventActions.Down => GestureStatus.Started,
                            MotionEventActions.Move => GestureStatus.Running,
                            MotionEventActions.Up => GestureStatus.Completed,
                            MotionEventActions.Cancel => GestureStatus.Canceled,
                            _ => GestureStatus.Canceled
                        };

                        var parameters = new PinchArgs(status, (current0, current1), (origin0, origin1));
                        TriggerCommand(pinchCommand, parameters);
                        TriggerCommand(pinchPointCommand, parameters);
                        TriggerEvent(pinchEvent, parameters);
                    }
                },
                LongPressAction = motionEvent =>
                {
                    var x = motionEvent!.GetX();
                    var y = motionEvent!.GetY();
                    var point = PxToDp(new Point(x, y));
                    var args = new PointArgs(point, Element, Element.BindingContext);

                    TriggerCommand(longPressPointCommand, args);
                    TriggerCommand(longPressCommand, commandParameter);
                },
            };
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods
        private Point PxToDp(Point point)
        {
            point.X /= displayMetrics?.Density ?? 1;
            point.Y /= displayMetrics?.Density ?? 1;
            return point;
        }

        protected override partial void OnAttached()
        {
            var control = Control ?? Container;

            var context = control.Context;
            if (context?.Resources != null)
            {
                displayMetrics = context.Resources?.DisplayMetrics;
                tapDetector.Density = displayMetrics != null ? displayMetrics.Density : 1;
                gestureRecognizer ??= new ExtendedGestureDetector(context, tapDetector);
            }

            control.Touch += ControlOnTouch;
            control.Clickable = true;

            OnElementPropertyChanged(new PropertyChangedEventArgs(string.Empty));
        }

        private void ControlOnTouch(object? sender, View.TouchEventArgs touchEventArgs)
        {
            if (touchEventArgs.Event != null)
                gestureRecognizer?.OnTouchEvent(touchEventArgs.Event);
            touchEventArgs.Handled = false;
        }

        protected override partial void OnDetached()
        {
            var control = Control ?? Container;
            control.Touch -= ControlOnTouch;

            var g = gestureRecognizer;
            gestureRecognizer = null;
            g?.Dispose();
            displayMetrics = null;
        }

        #endregion Methods
    }
}