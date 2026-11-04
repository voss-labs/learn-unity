# Lesson 5: Light and mood

Take the same room and furniture, change only the light, and it goes from cozy to a hospital. Light does more for how a world feels than any model.

**Open:** `Assets/Worlds/Home/Home.unity`

## Four kinds of light in this room

| Light | Where to find it | What it does |
| --- | --- | --- |
| Directional | **Sun** in the Hierarchy | Parallel rays from far away, like the sun. Only its rotation and colour matter, not its position |
| Point | **Rooms > Stylized room > Ceiling light**, **Floor lamp light**, **Kitchen light** | Glows in all directions from one spot, like a bulb, and fades with distance |
| Ambient | **Window > Rendering > Lighting**, **Environment** tab | A base level everywhere, standing in for light bouncing around the room |
| Emissive surface | Not used in Home. Look for the bright strips in Campus | A material that looks lit from inside but does not light anything else |

## Why this room feels cozy

Nothing here is automatic. These are choices you can copy:

1. **Warm colour.** Select **Ceiling light** and look at **Color**, `#ffc58f`. Real bulbs are orange compared with daylight, and we read that as evening and home. Try a pure white and feel the difference.
2. **Low ambient.** The ambient colours are dark brown. Corners stay in shadow, so the lamps create pools of light. In a flatly lit room nothing stands out.
3. **Several small lights, not one big one.** A ceiling light, a floor lamp and a kitchen light make three places in one room.
4. **A contrast.** The sun comes in low through the window and cuts a bright shape across the floor.
5. **The final grade.** Select **Global Volume**. Its profile runs over the finished picture: **Tonemapping** handles very bright values gently, **Color Adjustments** sets the overall exposure, and **White Balance** pushes everything a little warmer.

Compare `Assets/Worlds/Campus/Campus.unity`. It is a big hall under a glass roof, so it uses strong, even ambient light and a gentle sun: soft daylight with barely any drama. Same engine and same tools, opposite mood.

## Shadows have a price

A shadow means rendering the scene again from the light's point of view. The sun's shadow is one extra pass. A point light shines in all directions, so its shadow needs up to six. In this project the point lights have shadows off, and you can see how much the room still works.

Unity keeps different settings per platform in **Edit > Project Settings > Quality**. Web builds use the **Mobile** pipeline settings in `Assets/Settings`, because a student's laptop running this in a browser tab has far less to spare than the Unity Editor. Always test how it looks where people will really run it.

## Try it

**1. Turn it to night.** Set **Sun** intensity to 0. The room gets dramatic immediately. Now the window is a dark hole. What would you change so it still feels cozy?

**2. Make it a hospital.** Set all the point lights to white at higher intensity and raise the ambient colours. Walk around. Nothing moved, yet nobody would want to sit on that sofa. Undo with Ctrl+Z, or close the scene without saving.

**3. See the price.** Turn on **Soft Shadows** for one point light, and open the **Stats** panel in the Game view toolbar. Compare the frames per second, and how many batches it draws.

**4. Light your own corner.** This is the real exercise. Add a lamp to the bookshelf corner of a room: a model, a point light inside its shade, a warm colour, and a short range so it only lights that corner. Getting a lamp to feel right takes longer than you expect, and that is the skill.

## Check yourself

- Why does the sun's position not matter, only its rotation?
- What makes a room feel warm? Name three things that are not furniture.
- Why are point light shadows turned off in a build that runs in a browser?

Next: [Lesson 6: Web builds](06-web-builds.md).
