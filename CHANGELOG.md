# Changelog

## Unreleased

- Every world now runs on iPhone and Android: touch controls in the shared Walker, a phone data file with ASTC textures for each world, lighter graphics on phones, and a wider view in portrait
- Campus textures are now separate files, so Unity compresses them. They were about 290 MB uncompressed in memory
- The website, game pages and on-screen labels now use the VOSS logo and colours: paper, ink and orange, light by default with a dark theme
- Three worlds: Starter, Campus (from vmaps) and Home
- Shared player in `Assets/Core`, used by every world
- New world window and web build in the Learn Unity menu
- Six lessons, setup and add-a-world guides
- Website with a web build of each world, ready for Vercel
- Repo checks that catch missing `.meta` files, duplicate GUIDs, broken doc links and more
