using MauiGestures.GestureArgs;
using System.Diagnostics;

namespace MauiGestures.Example
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.InputTransparent = false;
            this.BindingContext = new MainViewModel();

            Gesture.SetTapEvent(EventBox, OnTapped);
            Gesture.SetRightTapEvent(EventBox, OnRightTapped);
            Gesture.SetDoubleTapEvent(EventBox, OnDoubleTapped);
            Gesture.SetLongPressEvent(EventBox, OnLongPressed);
            Gesture.SetPanEvent(EventBox, OnPan);
            Gesture.SetPinchEvent(EventBox, OnPinch);
            Gesture.SetSwipeEvent(EventBox, OnSwiped);
        }

        private void OnPinch(PinchArgs args)
        {
            Debug.WriteLine("Pinched " + args.Scale + ", " + args.Status.ToString());
        }

        private void OnPan(PanArgs args)
        {
            Debug.WriteLine("Panned X:" + args.Point.X + ", Y:" + args.Point.Y + ", " + args.Status.ToString());
        }

        private void OnSwiped(SwipeArgs args)
        {
            Debug.WriteLine("Swiped " + args.Direction + ", " + args.Distance + ", " + args.Position);
        }

        private void OnLongPressed(PointArgs args)
        {
            Debug.WriteLine("Long pressed at " + args.Point);
        }

        private void OnDoubleTapped(PointArgs args)
        {
            Debug.WriteLine("Double tapped at " + args.Point);
        }

        private void OnRightTapped(PointArgs args)
        {
            Debug.WriteLine("Right tapped at " + args.Point);
        }

        private void OnTapped(PointArgs args)
        {
            Debug.WriteLine("Tapped at " + args.Point);
        }
    }

}
