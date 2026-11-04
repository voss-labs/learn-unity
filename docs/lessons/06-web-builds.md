# Lesson 6: Web builds

Your world is no use to anyone if it only runs on your laptop. Here is how a Unity project becomes a link you can send to a friend.

**You need:** the **Web Build Support** module. Add it from **Installs** in Unity Hub if you skipped it.

## What a build is

In the Editor, Unity runs your scripts directly. A **build** packages one world so that it runs without Unity at all. For the web, it does this:

1. Picks the scene, and only the models and textures that scene really uses. That is why Starter's build is far smaller than Home's.
2. Compiles your C# to **WebAssembly**, a fast format browsers run directly.
3. Compresses everything and writes a folder of static files with a page to load them.

That folder needs no server code. Any static host can serve it, which is why our whole website costs nothing to run.

## Build a world

Open the world and choose **Learn Unity > Build this world for the web**. The first build takes a long time (expect 10 minutes or more) because Unity switches every texture to a web format and compiles the code. Later builds are quicker.

The menu item is in `Assets/Editor/WebBuild.cs`. Read it. It is short: pick the scene, call `BuildPipeline.BuildPlayer`, and report the result. The result goes to `site/<world>`.

Maintainers use `./scripts/build.sh` to run the same code from a terminal.

## Run it

Browsers refuse to load game files from a double-clicked file, so you need a small local server:

```bash
npx http-server site -c-1
```

Open the address it prints, usually http://localhost:8080, then add the world's name: `/starter/`. Keep the slash at the end, because the page finds its game files relative to its own address.

## The page around the game

`Assets/WebGLTemplates/Walk/index.html` is the page every world loads in. Unity fills in the `{{{ DATA_FILENAME }}}` style placeholders during the build. Read how it handles three moments people actually hit:

- **Loading:** a progress bar, because a world is many megabytes.
- **Phones:** it does not start the download at all, because walking needs a keyboard and mouse, and nobody wants to spend their data to find that out.
- **Failure:** it says what went wrong instead of leaving a blank page.

## Why our builds need no server setup

This is a real decision made for this repo, so it is worth understanding.

Unity compresses builds with Brotli. Normally the files end in `.br` and the server must send a `Content-Encoding: br` header, so the browser unpacks them. That needs server configuration, and Vercel can [compress such files a second time](https://community.vercel.com/t/breaking-change-pre-compressed-assets-re-compressed-at-the-edge/18744), which some browsers cannot read.

So we turn on **Decompression Fallback** in **Edit > Project Settings > Player > Web > Publishing Settings**. The files end in `.unityweb`, no header is needed, and a small JavaScript decompressor inside the loader unpacks them. Startup costs a little more. In exchange it runs on any static host with zero configuration, which also means anyone can fork this repo and deploy it. Simple over clever.

## Why you do not commit your build

Web builds are large, and Git keeps every version forever, so committing them in every pull request would make the repo enormous for everyone. Your pull request contains only your world's folder. A maintainer builds the site after merging.

## Try it

**1. Build Starter and run it locally.** Open the browser's developer tools with F12, look at the **Network** tab, and reload. Which file is the largest? What is it?

**2. Compare sizes.** Build a second world. Look in `site/<world>/Build`. Which file grows the most between Starter and Home? Why not the code file?

**3. Shrink a world.** This is the real exercise. Select a large texture in your world. In the Inspector, lower **Max Size** and click **Apply**. Rebuild and compare the file sizes, then walk up to that texture. How small can it go before anyone would notice?

## Check yourself

- Why does a build only contain some of the project's assets?
- Why can't you double-click `index.html`?
- What did we give up, and what did we gain, by using Decompression Fallback?

Next: [Add your own world](../add-a-world.md). You have everything you need.
