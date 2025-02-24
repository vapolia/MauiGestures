using System.ComponentModel;
using Microsoft.Maui.Controls.Platform;
using Microsoft.UI.Xaml.Input;
using MauiGestures.GestureArgs;
using Microsoft.UI.Input;
using GestureRecognizer = Microsoft.UI.Input.GestureRecognizer;
using Microsoft.Maui.Platform;

namespace MauiGestures.Platform
{
    internal partial class GestureEffect : PlatformEffect
    {
        #region Fields
        private bool isHolding;
        private readonly GestureRecognizer _gestureRecognizer;
        private readonly SwipeGestureRecognizer _swipeGestureRecognizer;
        private readonly PinchGestureRecognizer _pinchGestureRecognizer;

        #endregion Fields

        #region Constructors
        public GestureEffect()
        {
            _gestureRecognizer = new()
            {
                GestureSettings =
                   GestureSettings.Tap
                   | GestureSettings.RightTap
                   | GestureSettings.Drag
                   | GestureSettings.DoubleTap
                   | GestureSettings.Hold
                   | GestureSettings.HoldWithMouse
                   | GestureSettings.ManipulationScale
            };

            _swipeGestureRecognizer = new SwipeGestureRecognizer();
            _pinchGestureRecognizer = new PinchGestureRecognizer();

            _swipeGestureRecognizer.Swiped += (sender, e) =>
            {
                var swipeArgs = new SwipeArgs(e.Direction);
                TriggerCommand(swipeCommand, swipeArgs);
                TriggerEvent(SwipeEvent, swipeArgs);
            };

            _swipeGestureRecognizer.Direction = SwipeDirection.Left | SwipeDirection.Right | SwipeDirection.Up | SwipeDirection.Down;

            _pinchGestureRecognizer.PinchUpdated += (sender, e) =>
            {
                var startingPoints = (e.ScaleOrigin, e.ScaleOrigin);
                var currentPoints = (e.ScaleOrigin, e.ScaleOrigin);
                var pinchArgs = new PinchArgs(e.Status, currentPoints, startingPoints);
                TriggerCommand(pinchCommand, pinchArgs);
                TriggerEvent(PinchEvent, pinchArgs);
            };

            _gestureRecognizer.Dragging += (sender, args) =>
            {
                var currentPoint = new Point(args.Position.X, args.Position.Y);

                if (args.DraggingState == DraggingState.Started)
                {
                    return;
                }

                if (args.DraggingState == DraggingState.Continuing)
                {
                    var gestureStatus = GestureStatus.Running;
                    var parameters = new PanArgs(currentPoint, gestureStatus);
                    TriggerCommand(panCommand, commandParameter);
                    TriggerCommand(panPointCommand, parameters);
                    TriggerEvent(PanEvent, parameters);
                    return;
                }

                if (args.DraggingState == DraggingState.Completed)
                {
                    ResetStates();
                }
            };

            _gestureRecognizer.Tapped += (sender, args) =>
            {
                if (args.TapCount == 1)
                {
                    TriggerCommand(tapCommand, commandParameter);
                    var pointArgs = new PointArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                    TriggerCommand(tapPointCommand, pointArgs);
                    TriggerEvent(TapEvent, pointArgs);
                }
                else if (args.TapCount == 2)
                {
                    TriggerCommand(doubleTapCommand, commandParameter);
                    var pointArgs = new PointArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                    TriggerCommand(doubleTapPointCommand, pointArgs);
                    TriggerEvent(DoubleTapEvent, pointArgs);
                }
            };

            _gestureRecognizer.RightTapped += (sender, args) =>
            {
                if (!isHolding)
                {
                    TriggerCommand(rightTapCommand, commandParameter);
                    var pointArgs = new PointArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                    TriggerCommand(rightTapPointCommand, pointArgs);
                    TriggerEvent(RightTapEvent, pointArgs);
                }
            };

            _gestureRecognizer.Holding += (sender, args) =>
            {
                if (args.HoldingState == HoldingState.Started)
                {
                    isHolding = true;
                    TriggerCommand(longPressCommand, commandParameter);
                    var pointArgs = new PointArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                    TriggerCommand(longPressPointCommand, pointArgs);
                    TriggerEvent(LongPressEvent, pointArgs);
                }
            };

            _gestureRecognizer.ManipulationUpdated+= (sender, args) =>
            {
                TriggerCommand(pinchCommand, commandParameter);
                var startingPoints = (args.Position.ToPoint(), args.Position.ToPoint());
                var currentPoints = (args.Position.ToPoint(), args.Position.ToPoint());
                var pinchArgs = new PinchArgs(GestureStatus.Running, currentPoints, startingPoints);
                TriggerCommand(pinchCommand, pinchArgs);
                TriggerEvent(PinchEvent, pinchArgs);
            };
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods
        protected override partial void OnAttached()
        {
            var control = Control ?? Container;

            if (control is Microsoft.UI.Xaml.UIElement uiElement)
            {
                uiElement.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.TranslateInertia | ManipulationModes.All;
            }

            control.PointerMoved += ControlOnPointerMoved;
            control.PointerPressed += ControlOnPointerPressed;
            control.PointerReleased += ControlOnPointerReleased;
            control.PointerCanceled += ControlOnPointerCanceled;
            control.PointerCaptureLost += ControlOnPointerCanceled;
            control.PointerExited += ControlOnPointerExited;
            control.PointerEntered += ControlOnPointerEntered;

            OnElementPropertyChanged(new PropertyChangedEventArgs(String.Empty));
        }

        protected override partial void OnDetached()
        {
            var control = Control ?? Container;
            control.PointerMoved -= ControlOnPointerMoved;
            control.PointerPressed -= ControlOnPointerPressed;
            control.PointerReleased -= ControlOnPointerReleased;
            control.PointerCanceled -= ControlOnPointerCanceled;
            control.PointerCaptureLost -= ControlOnPointerCanceled;
            control.PointerExited -= ControlOnPointerExited;
            control.PointerEntered -= ControlOnPointerEntered;
        }

        private void ControlOnPointerPressed(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            _gestureRecognizer.CompleteGesture();
            _gestureRecognizer.ProcessDownEvent(pointerRoutedEventArgs.GetCurrentPoint(Control ?? Container));
            pointerRoutedEventArgs.Handled = true;
        }

        private void ControlOnPointerExited(object sender, PointerRoutedEventArgs args)
        {
            args.Handled = true;
        }

        private void ControlOnPointerEntered(object sender, PointerRoutedEventArgs args)
        {
            args.Handled = true;
        }

        private void ControlOnPointerMoved(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            _gestureRecognizer.ProcessMoveEvents(returnAllPointsOnWindows ?
                pointerRoutedEventArgs.GetIntermediatePoints(Control ?? Container)
                : new List<PointerPoint> { pointerRoutedEventArgs.GetCurrentPoint(Control ?? Container) });
            pointerRoutedEventArgs.Handled = true;
        }

        private void ControlOnPointerCanceled(object sender, PointerRoutedEventArgs args)
        {
            _gestureRecognizer.CompleteGesture();
            ResetStates();
            args.Handled = true;
        }

        private void ControlOnPointerReleased(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            _gestureRecognizer.ProcessUpEvent(pointerRoutedEventArgs.GetCurrentPoint(Control ?? Container));
            ResetStates();
            pointerRoutedEventArgs.Handled = true;
        }

        private void ResetStates()
        {
            isHolding = false;
        }

        #endregion Methods
    }
}