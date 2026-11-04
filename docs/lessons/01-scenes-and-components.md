# Lesson 1: Scenes and components

Everything in Unity is built from one idea. Learn it here and the rest of the engine stops looking like magic.

**Open:** `Assets/Worlds/Starter/Starter.unity`

## The idea

A **scene** is one place, saved as a file. Starter is a scene. Campus is another.

A scene holds **GameObjects**. A GameObject does nothing by itself. It is an empty thing with a position.

What a GameObject can do comes from its **components**. Want it visible? Add a renderer. Solid? Add a collider. Spinning? Add a script. Every script you write is a component.

## See it

The **Hierarchy** window on the left lists every GameObject in the scene. Click **Courtyard**, open it, and click **Spinning cube**.

The **Inspector** on the right shows its components, top to bottom:

| Component | What it gives the cube |
| --- | --- |
| Transform | Where it is, how it is turned, how big it is. Every GameObject has one |
| Mesh Filter | Its shape |
| Mesh Renderer | Draws that shape with a material |
| Box Collider | Makes it solid |
| Spinner | Makes it spin. This one is ours |

Untick **Mesh Renderer** and press Play. The cube is invisible but you can still bump into it. Untick **Box Collider** and it is visible but you walk through it. Same object, different components. Tick them both back.

## Read the script

Open `Assets/Core/Spinner.cs`. This is all of it:

```csharp
public class Spinner : MonoBehaviour
{
    [SerializeField] float degreesPerSecond = 45f;

    void Update()
    {
        transform.Rotate(0f, degreesPerSecond * Time.deltaTime, 0f);
    }
}
```

- **`MonoBehaviour`** is what makes a class a component you can attach to a GameObject.
- **`[SerializeField]`** shows the private field in the Inspector, so you can tune it without touching code. Look for **Degrees Per Second** on the cube.
- **`Update`** is called by Unity once every frame. You do not call it. You fill it in.
- **`Time.deltaTime`** is how many seconds the last frame took.

That last one matters most. A fast laptop runs more frames per second than a slow one. Rotating by a fixed amount per frame would make the cube spin faster on the better laptop. Multiplying by the frame's duration makes it the same speed everywhere: "45 degrees per second" stays true.

## Try it

**1. Tune it live.** Press Play, select the cube, and change Degrees Per Second. It changes immediately. Press Play again to stop and watch your change vanish. Edits made in Play mode are not saved. To keep a value, change it when stopped and save with Ctrl+S (Cmd+S on Mac).

**2. Spin a different way.** Change the code so the cube tumbles around another axis. Which of the three numbers in `Rotate` is which?

**3. Break it on purpose.** Remove `* Time.deltaTime`. What happens to the speed? Why? Put it back.

**4. Write your own component.** This is the real exercise. In the Project window, right-click and choose **Create > MonoBehaviour Script**, name it `Bobber`, and make something float up and down. Attach it by dragging the script onto a GameObject.

You need two things:

- `Time.time` is the seconds since the game started.
- `Mathf.Sin` turns a growing number into a smooth wave between -1 and 1.

Set `transform.position` every frame. Your cube needs to remember where it started, so store that in a field during `Start`, which Unity calls once before the first frame. If your object flies away or jumps to the world centre, you found the reason for storing the start position.

## Check yourself

You are done when you can answer these without looking:

- What is the difference between a GameObject and a component?
- Why does `Rotate` multiply by `Time.deltaTime`?
- Which method does Unity call once at the start, and which every frame?

Next: [Lesson 2: Walking](02-walking.md).
