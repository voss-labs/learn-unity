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
- **Phones:** it picks a smaller data file, draws at a lower resolution, and stops the browser from scrolling or zooming when you drag your thumbs.
- **Failure:** it says what went wrong instead of leaving a blank page.

## Why a phone gets its own data file

A phone is the hardest place to run a world, for two reasons you can measure.

**Texture formats.** Graphics chips read compressed textures directly, but computers and phones read different formats. Computers use one called DXT and phones use ASTC. Send DXT to a phone and Unity has to unpack every texture in software, which takes several times the memory and can crash the browser tab.

**Download size vs memory.** A small download does not mean a small world. Campus once had a 14 MB download that unpacked to about 290 MB of textures in memory, because its images were embedded in the model and Unity could not compress them. Exporting them as separate image files in `Assets/Worlds/Campus/textures` let Unity compress them like any other texture.

So `WebBuild.cs` builds every world twice: once with DXT, and once with ASTC and smaller textures. Both builds have the same code, so only the phone's data file is copied across, as `<world>.mobile.data.unityweb`. The page chooses which one to load:

```javascript
const phone = navigator.maxTouchPoints > 0 && matchMedia("(pointer: coarse)").matches;
```

It first draws one tiny ASTC texture and checks the result. The iPhone simulator claims to support ASTC, but its sRGB ASTC textures turn black in every mip level below the largest, so anything that fails this check gets the computer's data file instead. That file still works and just uses more memory.

When the world starts on a phone, `Assets/Core/PhoneSettings.cs` also lowers the shadow quality, so the graphics chip has less to do.

It also runs a small test from `Assets/Core/DepthComparison.jslib`. Shadows depend on the graphics chip comparing depth values, and the iPhone simulator's GPU cannot do that: every comparison says "in shadow", so the sun disappears and the world goes dark. If a device fails the test, the world turns shadows off instead. Finding this took a tiny test page that asked the graphics API one question directly, with Unity out of the picture. That is a good way to debug any graphics problem: remove everything you can until only the question is left.

## Run it on your phone

Your phone and laptop must be on the same Wi-Fi. Start the local server, find your laptop's address (`ipconfig` on Windows, `ipconfig getifaddr en0` on Mac), and open `http://THAT-ADDRESS:8080/starter/` on your phone.

Always test on a real phone before you call a world finished. An emulator has your laptop's memory behind it, so it will not crash where a real phone would.

## Why our builds need no server setup

This is a real decision made for this repo, so it is worth understanding.

Unity compresses builds with Brotli. Normally the files end in `.br` and the server must send a `Content-Encoding: br` header, so the browser unpacks them. That needs server configuration, and Vercel can [compress such files a second time](https://community.vercel.com/t/breaking-change-pre-compressed-assets-re-compressed-at-the-edge/18744), which some browsers cannot read.

So we turn on **Decompression Fallback** in **Edit > Project Settings > Player > Web > Publishing Settings**. The files end in `.unityweb`, no header is needed, and a small JavaScript decompressor inside the loader unpacks them. Startup costs a little more. In exchange it runs on any static host with zero configuration, which also means anyone can fork this repo and deploy it. Simple over clever.

## Why you do not commit your build

Web builds are large, and Git keeps every version forever, so committing them in every pull request would make the repo enormous for everyone. Your pull request contains only your world's folder. A maintainer builds the site after merging.

## Try it

**1. Build Starter and run it locally.** Open the browser's developer tools with F12, look at the **Network** tab, and reload. Which file is the largest? What is it?

**2. Compare sizes.** Build a second world. Look in `site/<world>/Build`. Which file grows the most between Starter and Home? Why not the code file? How much smaller is the phone's data file than the computer's?

**3. Shrink a world.** This is the real exercise. Select a large texture in your world. In the Inspector, lower **Max Size** and click **Apply**. Rebuild and compare the file sizes, then walk up to that texture. How small can it go before anyone would notice?

## Check yourself

- Why does a build only contain some of the project's assets?
- Why can't you double-click `index.html`?
- Why does a phone need a different data file from a computer?
- What did we give up, and what did we gain, by using Decompression Fallback?

Next: [Add your own world](../add-a-world.md). You have everything you need.
