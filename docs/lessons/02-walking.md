# Lesson 2: Walking

Every world in this repo uses the same player. Here is how a few lines of input become a person walking around.

**Open:** `Assets/Worlds/Starter/Starter.unity` and `Assets/Core/Walker.cs`

## The player is a prefab

Click **Player** in the Hierarchy. Its name is blue because it is an instance of a **prefab**, a saved GameObject you can reuse. The original is `Assets/Core/Player.prefab`. Fix a bug in the prefab and every world gets the fix.

An instance can still differ from its prefab. Home walks slower than Campus because the rooms are small. Open `Assets/Worlds/Home/Home.unity`, select Player, and look at **Walk Speed**. It is bold with a blue bar, which means "this world overrides the prefab here". Open Starter again when you have seen it.

Player has one child, **View**, which holds the Camera. You will see why it is separate in a moment.

## One frame of walking

`Update` in `Walker.cs` is the whole story. Read it first, then follow each call:

```csharp
void Update()
{
    touch.Update();
    var move = touch.Move;
    var running = touch.Running;

    var keyboard = Keyboard.current;
    if (keyboard != null)
    {
        UpdateSpawnKeys(keyboard);
        var keys = ReadMove(keyboard);
        if (keys != Vector2.zero)
        {
            move = keys;
            running = keyboard.shiftKey.isPressed;
        }
    }

    var mouse = Mouse.current;
    if (keyboard != null && mouse != null && !touch.Used)
    {
        UpdateCursor(keyboard, mouse);
        if (Cursor.lockState == CursorLockMode.Locked)
            Turn(mouse.delta.ReadValue() * lookDegreesPerPixel);
    }

    Turn(touch.LookDegrees);
    if (touch.NextPlaceTapped)
        GoTo((currentSpawn + 1) % spawns.childCount);
    Move(move, running ? runSpeed : walkSpeed);
    if (transform.position.y < FallLimit)
        GoTo(currentSpawn);
}
```

Notice the shape. Wherever the input comes from, it ends up as the same two things: a direction to `Move` and some degrees to `Turn`. Adding a new kind of input only means producing those two things.

### Reading input

`Keyboard.current` comes from Unity's Input System package. There are two kinds of question you can ask a key, and mixing them up is a classic bug:

- `isPressed` is true for every frame the key is held. Walking uses this.
- `wasPressedThisFrame` is true for only the one frame it went down. Jumping to a place uses this, otherwise you would jump sixty times a second.

A phone may have no keyboard at all, which is why the code checks for `null` before using one. `ReadMove` turns the keys into a direction. Notice `Vector2.ClampMagnitude`. Pressing W and D together gives a vector of (1, 1), which is about 1.41 long. Without the clamp you would walk 41 percent faster diagonally.

### Looking

```csharp
void ApplyLook()
{
    transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    view.localRotation = Quaternion.Euler(pitch, 0f, 0f);
}
```

This is why View is a separate child. Turning left and right (yaw) turns your whole body, so W always means "the way you are facing". Looking up and down (pitch) only tilts your head. If the body tilted too, looking at the ceiling would make W walk you into the ceiling.

### Touch

On a phone, `Assets/Core/TouchControls.cs` does the same job with fingers. A finger that lands on the left half of the screen becomes a thumbstick: the further it drags from where it landed, the faster you walk, and dragging all the way makes you run. A finger on the right half turns the view.

Each finger has an id, and the class remembers which id is doing which job. That is how you can walk and look at once, with two thumbs doing different things. The on-screen stick and the Next place button are drawn by `Assets/Core/Hud.cs`.

### Moving

`Move` uses a **CharacterController**, a component built for exactly this. You tell it "try to move this far" and it handles walls, slopes and steps, sliding you along anything you hit.

```csharp
var step = transform.TransformDirection(new Vector3(input.x, 0f, input.y)) * (speed * Time.deltaTime);
```

`TransformDirection` converts "forward from where I'm facing" into a direction in the world. `speed * Time.deltaTime` is lesson 1 again: metres per second times seconds gives metres.

Gravity is not automatic with a CharacterController. `Move` does it by hand: if you are on the ground, push down gently so you stay stuck to ramps. If you are in the air, add a bit more falling speed every frame. Accelerating every frame is what makes a fall look like a fall.

### Places

The number keys call `GoTo`, which moves you to a child of the `spawns` object. Open **Spawns** in the Hierarchy. Each child is a place, named what the screen shows when you arrive. Every spawn sits at eye height, so `GoTo` subtracts the camera's height to find where your feet go.

## Try it

**1. Feel the numbers.** In Play mode, change **Walk Speed** and **Look Degrees Per Pixel** on Player. Find what feels right, then remember it is not saved.

**2. Add a place.** When stopped, duplicate a child of Spawns with Ctrl+D, move it somewhere and rename it. Press Play and hit its number key.

**3. Add a jump.** This is the real exercise. Add a `jumpSpeed` field, and when Space was pressed this frame while grounded, set `verticalSpeed` to it.

You will hit a bug: nothing happens. Read `Move` carefully. On the ground, it overwrites `verticalSpeed` every frame, so your jump is erased before it is used. Working out where the check has to go is the lesson.

**4. Try it on a phone.** Lesson 6 shows how to run a world on your phone. Is the stick too sensitive? Change `StickRadiusOfScreen` in `TouchControls.cs` and feel the difference.

**5. Add a crouch.** While Left Ctrl is held, lower `view.localPosition` and shrink `body.height`. What goes wrong if you forget `body.center`? What happens if you stand up under something low?

## Check yourself

- Why is the camera on a child object instead of on Player itself?
- What is the difference between `isPressed` and `wasPressedThisFrame`?
- What does a blue, bold value in the Inspector mean on a prefab instance?

Next: [Lesson 3: Collisions](03-collisions.md).
