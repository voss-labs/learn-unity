import { existsSync, readdirSync, readFileSync, statSync } from "node:fs"
import { dirname, join, relative, resolve } from "node:path"
import { fileURLToPath } from "node:url"

const LARGE_FILE_MB = 50
const LINK = /\]\((?!https?:|#|mailto:)([^)#\s]+)/g
const ASSET_PATH = /`(Assets\/[^`*<{\s]+)`/g

function walk(dir) {
  return readdirSync(dir, { withFileTypes: true }).flatMap((entry) => {
    if (entry.name.startsWith(".")) return []
    const path = join(dir, entry.name)
    return entry.isDirectory() ? [path, ...walk(path)] : [path]
  })
}

export function metaProblems(root) {
  const assets = join(root, "Assets")
  if (!existsSync(assets)) return []
  const paths = new Set(walk(assets))
  const problems = []
  for (const path of paths) {
    const name = relative(root, path)
    if (path.endsWith(".meta")) {
      if (!paths.has(path.slice(0, -5)))
        problems.push(`${name} has no matching asset. Delete it, or restore the asset it belonged to.`)
    } else if (!paths.has(`${path}.meta`)) {
      problems.push(`${name} has no .meta file. Open the project in Unity so it creates one, then commit it.`)
    }
  }
  return problems
}

export function guidProblems(root) {
  const assets = join(root, "Assets")
  if (!existsSync(assets)) return []
  const seen = new Map()
  const problems = []
  for (const path of walk(assets).filter((p) => p.endsWith(".meta"))) {
    const guid = readFileSync(path, "utf8").match(/^guid: (\w+)/m)?.[1]
    if (!guid) continue
    const name = relative(root, path)
    if (seen.has(guid))
      problems.push(`${name} has the same GUID as ${seen.get(guid)}. Delete one .meta file and let Unity make a new one.`)
    seen.set(guid, name)
  }
  return problems
}

export function worldProblems(root) {
  const worlds = join(root, "Assets", "Worlds")
  if (!existsSync(worlds)) return ["Assets/Worlds is missing."]
  return readdirSync(worlds, { withFileTypes: true })
    .filter((entry) => entry.isDirectory())
    .flatMap(({ name }) =>
      [`${name}.unity`, "README.md"]
        .filter((file) => !existsSync(join(worlds, name, file)))
        .map((file) => `Assets/Worlds/${name} needs ${file}. Every world has a scene named after it and a README.`)
    )
}

export function largeFileProblems(root) {
  return ["Assets", "site"]
    .filter((dir) => existsSync(join(root, dir)))
    .flatMap((dir) => walk(join(root, dir)))
    .filter((path) => statSync(path).size > LARGE_FILE_MB * 1e6)
    .map((path) => `${relative(root, path)} is over ${LARGE_FILE_MB} MB. Shrink it, because GitHub rejects files over 100 MB.`)
}

export function siteProblems(root) {
  const site = join(root, "site")
  const page = join(site, "index.html")
  if (!existsSync(page)) return ["site/index.html is missing."]
  const html = readFileSync(page, "utf8")
  return readdirSync(site, { withFileTypes: true })
    .filter((entry) => entry.isDirectory() && entry.name !== "images")
    .flatMap(({ name }) => {
      const problems = []
      if (!existsSync(join(site, name, "index.html")) || !existsSync(join(site, name, "Build")))
        problems.push(`site/${name} is not a complete web build. Build the world again.`)
      if (!html.includes(`href="${name}/"`))
        problems.push(`site/index.html does not link to ${name}/. Add a doorway for it.`)
      return problems
    })
}

export function docProblems(root) {
  const docs = ["README.md", "CONTRIBUTING.md", "AGENTS.md"]
    .map((file) => join(root, file))
    .filter(existsSync)
    .concat(existsSync(join(root, "docs")) ? walk(join(root, "docs")) : [])
    .concat(existsSync(join(root, "Assets", "Worlds")) ? walk(join(root, "Assets", "Worlds")) : [])
    .filter((path) => path.endsWith(".md"))
  return docs.flatMap((doc) => {
    const text = readFileSync(doc, "utf8")
    const links = [...text.matchAll(LINK)].map((m) => resolve(dirname(doc), m[1]))
    const assets = [...text.matchAll(ASSET_PATH)].map((m) => join(root, m[1]))
    return [...links, ...assets]
      .filter((target) => !existsSync(target))
      .map((target) => `${relative(root, doc)} points to ${relative(root, target)}, which does not exist.`)
  })
}

export function allProblems(root) {
  return [metaProblems, guidProblems, worldProblems, largeFileProblems, siteProblems, docProblems].flatMap((check) => check(root))
}

if (process.argv[1] === fileURLToPath(import.meta.url)) {
  const root = resolve(dirname(fileURLToPath(import.meta.url)), "..")
  const problems = allProblems(root)
  for (const problem of problems) console.error(`- ${problem}`)
  console.log(problems.length ? `\n${problems.length} problems found.` : "All checks passed.")
  process.exit(problems.length ? 1 : 0)
}
