using Foundation;
using UIKit;

namespace MauiGestures.Platform.MaciOS;

internal class UIImmediatePinchGestureRecognizer : UIPinchGestureRecognizer
{
    #region Constructors
    public UIImmediatePinchGestureRecognizer()
    {
    }

    public UIImmediatePinchGestureRecognizer(Action action) : base(action)
    {
    }

    public UIImmediatePinchGestureRecognizer(Action<UIPinchGestureRecognizer> action) : base(action)
    {
    }

    [Preserve]
    protected internal UIImmediatePinchGestureRecognizer(IntPtr handle) : base(handle)
    {
    }

    #endregion Constructors

    #region Properties
    internal bool IsImmediate { get; set; } = false;

    #endregion Properties

    #region Methods
    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
        base.TouchesBegan(touches, evt);
        if (IsImmediate)
            State = UIGestureRecognizerState.Began;
    }

    #endregion Methods
}