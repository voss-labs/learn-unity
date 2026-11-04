using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchControls
{
    const int NoFinger = -1;
    const float StickRadiusOfScreen = 0.14f;
    const float RunAbove = 0.92f;
    const float LookDegreesPerScreen = 170f;

    int moveFinger = NoFinger;
    int lookFinger = NoFinger;
    Vector2 stickCentre, stickTip;

    public bool Used { get; private set; }
    public Vector2 Move { get; private set; }
    public Vector2 LookDegrees { get; private set; }
    public bool NextPlaceTapped { get; private set; }
    public bool Running => Move.magnitude > RunAbove;
    public bool StickHeld => moveFinger != NoFinger;
    public Vector2 StickCentre => stickCentre;
    public Vector2 StickTip => stickCentre + Move * StickRadius;
    public float StickRadius => Mathf.Min(Screen.width, Screen.height) * StickRadiusOfScreen;

    public void Enable() => EnhancedTouchSupport.Enable();

    public void Disable() => EnhancedTouchSupport.Disable();

    public void Update()
    {
        LookDegrees = Vector2.zero;
        NextPlaceTapped = false;
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.phase == TouchPhase.Began)
                Begin(touch);
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                End(touch);
            else
                Continue(touch);
        }
        Move = StickHeld ? Vector2.ClampMagnitude((stickTip - stickCentre) / StickRadius, 1f) : Vector2.zero;
    }

    void Begin(Touch touch)
    {
        Used = true;
        if (Hud.NextPlaceButton.Contains(touch.screenPosition))
            NextPlaceTapped = true;
        else if (touch.screenPosition.x < Screen.width / 2f && !StickHeld)
        {
            moveFinger = touch.touchId;
            stickCentre = stickTip = touch.screenPosition;
        }
        else if (lookFinger == NoFinger)
            lookFinger = touch.touchId;
    }

    void Continue(Touch touch)
    {
        if (touch.touchId == moveFinger)
            stickTip = touch.screenPosition;
        else if (touch.touchId == lookFinger)
            LookDegrees += touch.delta / Mathf.Min(Screen.width, Screen.height) * LookDegreesPerScreen;
    }

    void End(Touch touch)
    {
        if (touch.touchId == moveFinger)
            moveFinger = NoFinger;
        else if (touch.touchId == lookFinger)
            lookFinger = NoFinger;
    }
}
