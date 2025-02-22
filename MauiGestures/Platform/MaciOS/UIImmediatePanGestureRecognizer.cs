using Foundation;
using UIKit;

namespace MauiGestures.Platform.MaciOS;

internal class UIImmediatePanGestureRecognizer : UIPanGestureRecognizer
{
    #region Constructors
    public UIImmediatePanGestureRecognizer()
    {
    }

    public UIImmediatePanGestureRecognizer(Action action) : base(action)
    {
    }

    public UIImmediatePanGestureRecognizer(Action<UIPanGestureRecognizer> action) : base(action)
    {
    }

    [Preserve]
    protected internal UIImmediatePanGestureRecognizer(IntPtr handle) : base(handle)
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