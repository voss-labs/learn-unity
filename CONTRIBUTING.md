# Contributing to learn-unity

learn-unity is a VOSS Labs project. Contributions are welcome from all Vidyalankar students, and from anyone else who wants to learn Unity.

New to open source? Start with [voss-labs/first-contributions](https://github.com/voss-labs/first-contributions), then come back.

## Setup

[docs/setup.md](docs/setup.md) walks you through installing Unity and opening the project.

## What you can do here

**Add a world.** The best contribution. [docs/add-a-world.md](docs/add-a-world.md) takes you from one menu click to a pull request.

**Improve a world.** Every world's README lists what it still needs. Good first changes:

- Replace the plain white fridge in Home's realistic room with a real CC0 model
- Add a sound: footsteps, a fan in the canteen, a kitchen hum
- Add a floor lamp or wall art to a room in Home
- Add an interaction: a door that opens, a TV that turns on, a light switch
- Make another staircase walkable in Campus. That change belongs in [vmaps](https://github.com/voss-labs/vmaps), then gets exported here

**Improve a lesson.** If a step confused you, you are the best person to fix it, because the rest of us can no longer see what is confusing. Fix the wording, add to the "Stuck?" table, or add an exercise. Fix anything you find that does not match the code.

**Add a lesson.** Follow the shape of the others: one idea, real code in this repo, things to try, one real exercise, and questions to check yourself. Sound, animation and UI are all waiting for someone.

## Making changes

1. Fork the repo and clone your fork.
2. Create a branch from `main`: `git checkout -b your-branch-name`
3. Make your changes in Unity, and press Play to test them.
4. Run the checks:

```bash
node scripts/check.mjs
node --test
```

5. Commit with a [conventional commit](https://www.conventionalcommits.org) message, such as `feat: add footsteps to Starter` or `docs: explain tiling in lesson 4`.
6. Push and open a pull request against `main`. Add before and after screenshots for anything visual.

## Guidelines

- **Commit `.meta` files.** Unity uses them to identify every asset, so a missing one breaks references for everyone. Move and rename assets inside Unity, not in your file explorer, so the `.meta` file moves too.
- **Stay in your world's folder.** Changing `Assets/Core` affects every world, so open an issue first.
- **Do not commit `site/`.** Web builds are large and live in Git forever. A maintainer builds them after merging.
- **Only use assets you are allowed to share.** Look for CC0, and credit what you used in your world's README.
- **Keep code readable by a second-year student.** Clear names and small methods. Files under 400 lines.
- **No emojis** in code, commits or docs.

## Using AI

AI tools are allowed. Say which ones you used in your pull request. You must be able to explain every line you submit, and a maintainer may ask you to. If you let AI write your world, you skipped the part where you learn Unity.

## For maintainers

After merging a new world or a change to one:

1. Run `./scripts/build.sh WorldName`. You need the Unity CLI and Web Build Support. Or use **Learn Unity > Build this world for the web** in the Editor.
2. For a new world, add its screenshot to `site/images` and its doorway to `site/index.html`.
3. Run `node scripts/check.mjs`, then commit `site/` to `main`. Vercel deploys on push.

Rebuild only worlds that changed, to keep the repo small. If `site/` ever grows too large for this to be sensible, the next step is building in CI.

## Questions

[Open an issue](https://github.com/voss-labs/learn-unity/issues/new/choose) so that the next person finds the answer too.
