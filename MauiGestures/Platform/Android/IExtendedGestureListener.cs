using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiGestures.Platform.Android
{
    /// <summary>
    /// Gesture listener with an additional method.
    /// </summary>
    interface IExtendedGestureListener
    {
        void OnUp(MotionEvent? e);
    }
}
