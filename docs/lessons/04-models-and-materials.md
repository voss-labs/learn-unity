# Lesson 4: Models and materials

Nobody models every chair themselves. This lesson is about using free 3D models well, and fixing what is wrong with them when they arrive.

**Open:** `Assets/Worlds/Home/Home.unity`

Home is the same room built twice, on purpose. Press Play and use the **1** and **2** keys to compare them.

## Where the models came from

| | Stylized room | Realistic room |
| --- | --- | --- |
| Source | [Kenney Furniture Pack](https://kenney.nl/assets/furniture-kit) | [Poly Haven](https://polyhaven.com/models) |
| Format | FBX | glTF |
| Folder | `Assets/Worlds/Home/Furniture/Stylized` | `Assets/Worlds/Home/Furniture/Realistic` |
| Colour comes from | Flat colours in the model | Texture images |

Both are **CC0**, which means free for anything with no credit required. Always check the license before you use a model. "Free to download" is not the same thing as free to use.

Unity understands FBX out of the box. glTF needs a package, which is why `Packages/manifest.json` lists `com.unity.cloud.gltfast`.

## Three things that are always wrong

Models rarely arrive ready to use. The Kenney pack is a good example because it has all three problems:

1. **Wrong size.** A Kenney sofa is about 10 units wide. In Unity one unit is one metre, so that is a 10-metre sofa. We scale them to 0.2.
2. **Wrong pivot.** The pivot is the point an object moves and turns around. Kenney's models were exported in place on one big sheet, so each pivot is far away from the model. Rotating a sofa would swing it across the room.
3. **Wrong direction.** Kenney's furniture faces the opposite way to Poly Haven's. Our first version had every sofa facing the wall.

Here is the fix for all three. In the Hierarchy, open **Rooms > Stylized room > Furniture > loungeSofa**. There are two GameObjects:

- **loungeSofa** is an empty holder. Its position and rotation are where the sofa goes in the room.
- Its child is the actual model, with the scale and offset that fix it.

Now you can move and turn the holder like a sensible object, and the model's quirks are hidden one level down. Use this trick whenever a model misbehaves. Scale and offset the child, place the parent.

## Materials

A **material** says how a surface reacts to light. Select **Rooms > Realistic room > Shell > Floor** and scroll down the Inspector to the **Parquet** material. It uses the URP **Lit** shader:

| Property | What it does |
| --- | --- |
| Base Map | The colour image |
| Normal Map | Fakes small bumps by changing how light bounces, with no extra geometry |
| Smoothness | Low is matte like plaster. High is shiny like marble |
| Tiling | How many times the image repeats |

Tiling is the setting beginners miss. A texture covers a whole face by default, so a 7-metre floor would stretch one image of wood blocks across the whole room. Parquet repeats 3.5 by 3 times to bring the blocks back to real size. Look at the **Kitchen wood** material too. Its grain is still a little large, because the same material is used on boxes of different sizes. That is the cost of sharing one material.

## Try it

**1. Move the furniture.** Select the sofa's holder in the realistic room and press W for the move tool. Rearrange the room. It is your room.

**2. Change a surface.** Select the **Marble** material in `Assets/Worlds/Home/Materials`, drag its **Smoothness** up and down, and look at the counter while playing. Change **Tiling** on **Rug** and watch the pattern change.

**3. Replace the fridge.** This is the real exercise. The realistic room's fridge is a plain white box, because Poly Haven does not have one. Find a CC0 fridge, for example on [Poly Pizza](https://poly.pizza) or [Kenney](https://kenney.nl) (check each model's license). Drop it into `Assets/Worlds/Home/Furniture/Realistic`, add it to the scene, and use the holder trick to fix its size, pivot and direction. Add a **Box Collider** to the holder so people cannot walk through it, and delete the box. Then credit it in the world's README, and open a pull request: the room gets better for everyone.

## Check yourself

- What are the three problems you should check for on any imported model?
- Why use an empty parent instead of moving the model directly?
- What does Tiling do, and what goes wrong without it?

Next: [Lesson 5: Light and mood](05-light-and-mood.md).
