# learn-unity

Learn Unity by walking through real worlds, opening their code, and shipping your own.

A [VOSS Labs](https://vosslabs.org) project at Vidyalankar Institute of Technology, Mumbai. There is no course to finish. You play a world in your browser, open the same world in Unity, change it, and add a world of your own with a pull request. When it merges, it goes live and it is yours.

No Unity experience needed. You need a computer that can run Unity and a free GitHub account.

## The worlds

| World | What it is | Good for learning |
| --- | --- | --- |
| [Starter](Assets/Worlds/Starter/README.md) | A courtyard with a ramp, a step and a spinning cube | Scenes, components, your first script. Every new world begins as a copy of it |
| [Campus](Assets/Worlds/Campus/README.md) | Level 1 of the VIT main building, from [vmaps](https://github.com/voss-labs/vmaps) | Importing a large model, collisions from data, stairs |
| [Home](Assets/Worlds/Home/README.md) | A cozy living room and kitchen, built twice: low-poly and realistic | Models, materials, light and mood |

## Start here

1. [Set up Unity and open the project](docs/setup.md). The first open takes a while, so start it before you need it.
2. Follow the lessons in order. Each one points at real code in this repo and ends with something for you to change.

| Lesson | You will understand | You will change |
| --- | --- | --- |
| [1. Scenes and components](docs/lessons/01-scenes-and-components.md) | What a scene, a GameObject and a component are | Make something move with your own script |
| [2. Walking](docs/lessons/02-walking.md) | Input, frames, cameras, prefabs | Add a jump |
| [3. Collisions](docs/lessons/03-collisions.md) | Colliders, ramps, raycasts | Build a way up to somewhere |
| [4. Models and materials](docs/lessons/04-models-and-materials.md) | Importing models, scale, pivots, textures | Replace the plain fridge with a real model |
| [5. Light and mood](docs/lessons/05-light-and-mood.md) | Why a room feels cozy or cold | Turn a room to night |
| [6. Web builds](docs/lessons/06-web-builds.md) | How a Unity project runs in a browser | Build a world and run it locally |

3. [Add your own world](docs/add-a-world.md). This is the point of the repo. One menu click gives you a world to start from.

## What's in here

```text
Assets/
  Core/          what every world shares: Walker, Hud, Spinner and the Player prefab
  Worlds/        one folder per world, each with its own README
  Editor/        the New World window and the web build
  WebGLTemplates/  the page each world loads in
docs/            setup, the lessons, how to add a world
site/            the website: landing page plus one web build per world
scripts/         build.sh makes the web builds, check.mjs checks the repo
test/            tests for check.mjs
```

The repo folder is the Unity project, so you open it directly in Unity Hub.

## Run the website locally

The site is plain static files. Serve the `site` folder:

```bash
npx http-server site -c-1
```

Double-clicking `index.html` will not work. Browsers only load the game files from a server.

## Check your work

You need Node.js 20 or newer. There is nothing to install. These run on every pull request too:

```bash
node scripts/check.mjs
node --test
```

The check catches the mistakes Unity projects make most often, such as a missing `.meta` file. It tells you which file and what to do.

## Deploying

Vercel serves `site/`, as set in [vercel.json](vercel.json). There is no build step on Vercel because it cannot run Unity, so the web builds are committed. Import the repo in Vercel once and every push to `main` deploys. [Lesson 6](docs/lessons/06-web-builds.md) explains the compression setting that makes this work with no server configuration.

Want this for your own college? Fork it, swap in your own worlds, and import your fork into Vercel.

## Credits

- Stylized furniture: [Kenney Furniture Pack](https://kenney.nl/assets/furniture-kit), CC0.
- Realistic furniture and textures: [Poly Haven](https://polyhaven.com), CC0.
- Campus model: [vmaps](https://github.com/voss-labs/vmaps), MIT. It is a draft built from photos, not a measured map. Read its [model notes](https://github.com/voss-labs/vmaps/blob/main/docs/MODEL_NOTES.md) before presenting it as accurate.

Code is under the [MIT License](LICENSE).
