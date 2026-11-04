# Lesson 3: Collisions

What you see and what you bump into are two separate things in a game. Once that clicks, you can make anything walkable.

**Open:** `Assets/Worlds/Starter/Starter.unity`

## Colliders are invisible shapes

A **collider** is an invisible shape the physics engine uses. It has nothing to do with how an object looks. Select **Ramp** in the Hierarchy under **Collision playground**, and look at the green outline in the Scene view. That is its Box Collider.

Physics is much cheaper with simple shapes, so most games use boxes even when the visible model is detailed. The Home sofa is thousands of triangles to draw and one box to walk into.

## The playground

Press Play, then walk around the **Collision playground** on your left and right. Each object tests one rule of the CharacterController. Select `Player` and find these values on its **Character Controller**:

| Try this | What happens | The setting behind it |
| --- | --- | --- |
| Walk at the **Low step**, 0.2 m tall | You walk up it | **Step Offset** is 0.3. Anything lower is a step |
| Walk at the **High block**, 0.6 m tall | It stops you | Higher than Step Offset, so it counts as a wall |
| Walk up the **Ramp** | You climb smoothly | **Slope Limit** is 50 degrees. Anything gentler is walkable |
| Walk into a block at an angle | You slide along it | The controller keeps the part of your move that is along the surface |
| Walk off the **Platform** | It stops you at the edge | Not a physics rule. That is our code, below |

## A raycast stops you walking off edges

Nothing in physics stops you stepping off a platform. You would simply fall. We stop it because in Campus a fall could drop you into a place with no way back.

```csharp
bool HasGround(Vector3 feet) =>
    Physics.Raycast(feet + Vector3.up * RaycastLift, Vector3.down, RaycastLift + maxDrop + body.skinWidth);
```

A **raycast** fires an invisible line and tells you if it hit a collider. Before each step, `Move` in `Assets/Core/Walker.cs` checks the spot you are about to stand on: is there ground within **Max Drop** below it? If not, that part of the step is cancelled. It checks left-right and forward-back separately, so at an edge you still slide along it instead of freezing.

## How Campus does it

Open `Assets/Worlds/Campus/CampusWorld.cs`. The campus model has hundreds of pieces, and adding colliders by hand would take days and go wrong the moment the model changes. So the model file carries its own collision data, and this script turns it into colliders when the world starts:

```csharp
foreach (Transform solid in Group("Solids"))
    solid.gameObject.AddComponent<BoxCollider>();
foreach (Transform floor in Group("Floors"))
    floor.gameObject.AddComponent<BoxCollider>().center = new Vector3(0f, -0.5f, 0f);
```

`Solids` and `Floors` are lists of empty GameObjects inside `campus.glb`. Each one's Transform is already the right position and size, so a default Box Collider fits exactly. Floors shift their collider down by half, so the top of the box is the surface you walk on.

**The stairs are ramps.** Every staircase you can walk in Campus is a tilted box you never see. Drawing steps and walking on a smooth slope is a very common game trick: real steps would make the camera jerk up and down with every stair. Press Play in Campus, select a child of `Floors`, and look at the tilted green outline in the Scene view.

## Try it

**1. See the invisible.** In Campus, press Play and turn on **Gizmos** in the Game view toolbar. Walk to the stairs by the main gate and look at the colliders around you.

**2. Change a rule.** In Starter, set Player's **Step Offset** to 0.7 and walk at the High block. Set **Max Drop** to 5 and walk off the Platform. Now you can fall. Set them back.

**3. Separate look from shape.** Select the Spinning cube, and in its Box Collider set **Size** to (3, 3, 3). Press Play and walk at it. You are stopped by nothing you can see. Keep this in mind the next time a game blocks you with an invisible wall.

**4. Build a way up.** This is the real exercise. Add a second platform higher than the first, and connect them with ramps or a staircase you can actually climb. Use **GameObject > 3D Object > Cube**, which comes with a Box Collider. Add a spawn on top with Ctrl+D under Spawns. If you get stuck halfway up, check the angle against the Slope Limit and the step height against the Step Offset.

## Check yourself

- Why do games use a box collider around a detailed model?
- Why are Campus's stairs ramps?
- What stops you walking off an edge here: physics, or our code? Which line?

Next: [Lesson 4: Models and materials](04-models-and-materials.md).
