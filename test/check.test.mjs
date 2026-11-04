import assert from "node:assert/strict"
import { mkdirSync, mkdtempSync, rmSync, writeFileSync } from "node:fs"
import { tmpdir } from "node:os"
import { dirname, join } from "node:path"
import { test } from "node:test"
import { allProblems, docProblems, guidProblems, metaProblems, siteProblems, worldProblems } from "../scripts/check.mjs"

function repo(files) {
  const root = mkdtempSync(join(tmpdir(), "learn-unity-"))
  for (const [path, content] of Object.entries(files)) {
    mkdirSync(dirname(join(root, path)), { recursive: true })
    writeFileSync(join(root, path), content)
  }
  return root
}

function withRepo(files, run) {
  const root = repo(files)
  try {
    run(root)
  } finally {
    rmSync(root, { recursive: true })
  }
}

const meta = (guid) => `fileFormatVersion: 2\nguid: ${guid}\n`

const healthy = {
  "Assets/Worlds.meta": meta("a1"),
  "Assets/Worlds/Tree.meta": meta("a2"),
  "Assets/Worlds/Tree/Tree.unity": "scene",
  "Assets/Worlds/Tree/Tree.unity.meta": meta("a3"),
  "Assets/Worlds/Tree/README.md": "# Tree",
  "Assets/Worlds/Tree/README.md.meta": meta("a4"),
  "docs/setup.md": "See [the world](../Assets/Worlds/Tree/README.md) and `Assets/Worlds/Tree/Tree.unity`.",
  "site/index.html": '<a href="tree/">Tree</a>',
  "site/tree/index.html": "game",
  "site/tree/Build/tree.wasm.unityweb": "code",
}

test("a healthy repo has no problems", () => {
  withRepo(healthy, (root) => assert.deepEqual(allProblems(root), []))
})

test("finds an asset with no .meta file", () => {
  const { "Assets/Worlds/Tree/Tree.unity.meta": _, ...files } = healthy
  withRepo(files, (root) => assert.match(metaProblems(root)[0], /Tree\.unity has no \.meta/))
})

test("finds a .meta file with no asset", () => {
  withRepo({ ...healthy, "Assets/Gone.png.meta": meta("b1") }, (root) => assert.match(metaProblems(root)[0], /Gone\.png\.meta has no matching asset/))
})

test("finds two assets sharing a GUID", () => {
  withRepo({ ...healthy, "Assets/Copy.png": "x", "Assets/Copy.png.meta": meta("a3") }, (root) => assert.match(guidProblems(root)[0], /same GUID/))
})

test("finds a world with no scene", () => {
  const { "Assets/Worlds/Tree/Tree.unity": _, ...files } = healthy
  withRepo(files, (root) => assert.match(worldProblems(root)[0], /needs Tree\.unity/))
})

test("finds a world with no README", () => {
  const { "Assets/Worlds/Tree/README.md": _, ...files } = healthy
  withRepo(files, (root) => assert.match(worldProblems(root)[0], /needs README\.md/))
})

test("finds a broken link in the docs", () => {
  withRepo({ ...healthy, "docs/bad.md": "[gone](missing.md)" }, (root) => assert.match(docProblems(root)[0], /does not exist/))
})

test("finds a path in the docs that no longer exists", () => {
  withRepo({ ...healthy, "docs/bad.md": "Open `Assets/Core/Gone.cs`." }, (root) => assert.match(docProblems(root)[0], /Assets\/Core\/Gone\.cs/))
})

test("ignores web links and placeholder paths in the docs", () => {
  withRepo({ ...healthy, "docs/ok.md": "[web](https://vosslabs.org) [top](#top) `Assets/Worlds/<Name>/` `Assets/Worlds/*`" }, (root) => assert.deepEqual(docProblems(root), []))
})

test("finds a web build the landing page does not link to", () => {
  withRepo({ ...healthy, "site/other/index.html": "game", "site/other/Build/x": "code" }, (root) => assert.match(siteProblems(root)[0], /does not link to other/))
})

test("finds an incomplete web build", () => {
  const { "site/tree/Build/tree.wasm.unityweb": _, ...files } = healthy
  withRepo(files, (root) => assert.match(siteProblems(root)[0], /not a complete web build/))
})
