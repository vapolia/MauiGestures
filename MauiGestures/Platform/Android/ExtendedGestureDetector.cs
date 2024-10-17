using Android.Views;
using Android.Content;
using Android.Runtime;

namespace MauiGestures.Platform.Android
{
    internal sealed class ExtendedGestureDetector : GestureDetector
    {
        #region Fields
        private readonly IExtendedGestureListener? gestureListener;

        #endregion Fields

        #region Constructors
        private ExtendedGestureDetector(IntPtr javaRef, JniHandleOwnership transfer) : base(javaRef, transfer)
        {
        }

        internal ExtendedGestureDetector(Context context, IOnGestureListener listener) : base(context, listener)
        {
            if (listener is IExtendedGestureListener my)
                gestureListener = my;
        }

        #endregion Constructors

        #region Methods
        /// <summary>
        /// Touch event handler.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool OnTouchEvent(MotionEvent e)
        {
            if (gestureListener != null && e.Action == MotionEventActions.Up)
                gestureListener.OnUp(e);
            return base.OnTouchEvent(e);
        }

        #endregion Methods
    }
}
