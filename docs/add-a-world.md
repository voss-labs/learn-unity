# Add your own world

This is what the repo is for. Build a place people can walk around, open a pull request, and it goes live on the site with your name on it.

Your first world can be tiny. A single room you made yourself beats a huge plan that never ships.

## Ideas

Pick something you actually want to walk around in:

- Your hostel room or your classroom, recreated
- The canteen, the library or a lab that Campus does not cover yet
- A treehouse, a spaceship corridor, a tiny island
- A maze, or a gallery of things you have made
- A playground that teaches one thing: gravity, light, sound

## 1. Create your world

1. Make a branch: `git checkout -b world/treehouse`
2. In Unity, choose **Learn Unity > New world**.
3. Type a name in plain letters with no spaces, such as `Treehouse`, and click **Create**.

You now have a folder called `Assets/Worlds/<YourWorld>/` with its own scene, materials and README. It is a copy of Starter that you are free to tear apart. Nothing you do there can break anyone else's world.

## 2. Build it

Press Play often. Keep what works, and delete the Collision playground once you are done with it.

- Keep the **Player** in the scene. Everything about walking comes from it.
- Keep at least one child under **Spawns**, so people have a place to start. Spawns sit at eye height, so about 1.65 m above the floor.
- Put everything of yours inside your world's folder.
- Give any script you write a name that is yours, like `TreehouseLift`. Unity does not allow two classes with the same name in the project.
- Make sure there is a floor under every spawn, or people fall.

## 3. Use only assets you are allowed to use

Models, textures and sounds must be free to share in an open-source project. Good sources: [Kenney](https://kenney.nl), [Poly Haven](https://polyhaven.com), [Poly Pizza](https://poly.pizza), [ambientCG](https://ambientcg.com). Look for **CC0**. Do not use ripped game assets or anything without a clear license.

List what you used in your world's README, with links. Keep your world's folder under about 100 MB.

## 4. Fill in your README

Your world has a `README.md`. Say what it is, who made it, what you are proud of, and where you got your assets. Someone reading it should learn something. Tell them the trick you worked out.

## 5. Check it, then open a pull request

Run the repo's checks:

```bash
node scripts/check.mjs
```

Commit your world folder **and every `.meta` file in it**. Unity uses those files to identify assets, so a missing one breaks the world for everyone else. Check with `git status`. You should only see files inside your world's folder, plus `ProjectSettings/EditorBuildSettings.asset`.

Do not commit anything under `site/`. A maintainer builds the website after merging.

```bash
git add Assets/Worlds/Treehouse ProjectSettings/EditorBuildSettings.asset
git commit -m "feat: add the Treehouse world"
git push origin world/treehouse
```

Then open a pull request on GitHub with two or three screenshots. The template asks for them. The same check runs there automatically and tells you exactly what to fix.

## 6. What happens next

A maintainer walks your world and leaves feedback. Once it merges, they build it and add it to the site, and you can send your friends the link.
