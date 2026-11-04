# Set up Unity and open the project

Allow about an hour the first time, most of it waiting for downloads. You can do something else while they run.

New to Git and pull requests? Do [first-contributions](https://github.com/voss-labs/first-contributions) first. It takes fifteen minutes and everything below assumes it.

## 1. Install the tools

1. **Git.** On Windows install [Git for Windows](https://git-scm.com/downloads/win) and use its **Git Bash** terminal for every command in these docs. On Mac and Linux your normal terminal is fine.
2. **Unity Hub** from [unity.com/download](https://unity.com/download). The Hub installs and opens Unity. Sign in or create a free Unity account, and accept the free **Personal** license when it asks.
3. **VS Code** from [code.visualstudio.com](https://code.visualstudio.com), to read and edit scripts.
4. **Node.js** 20 or newer from [nodejs.org](https://nodejs.org). You only need it for the checks and for running the website locally.

## 2. Get the project

Fork [voss-labs/learn-unity](https://github.com/voss-labs/learn-unity) on GitHub, then clone your fork. Replace `YOUR-USERNAME`:

```bash
git clone https://github.com/YOUR-USERNAME/learn-unity.git
```

## 3. Open it in Unity

1. In Unity Hub, go to **Projects**, click **Add**, choose **Add project from disk**, and pick the `learn-unity` folder.
2. The Hub says the editor version is not installed. Click to install it. Use the exact version it offers, because that is the one this project is checked against.
3. Add **Web Build Support** when it asks which modules you want. You need it for lesson 6. If you skip it you can add it later from **Installs** in the Hub.
4. Open the project. The first open takes several minutes because Unity builds a `Library` folder from all the assets. That folder is not in Git. Later opens are fast.

## 4. Walk around

1. In the **Project** window at the bottom, open `Assets/Worlds/Starter`.
2. Double-click the `Starter` scene.
3. Press the **Play** button at the top.
4. Click inside the **Game** view so it can capture your mouse.

| To | Press |
| --- | --- |
| Move | W, A, S, D or the arrow keys |
| Run | Hold Shift |
| Look | Move the mouse |
| Jump to a place | Number keys |
| Get your mouse back | Esc |

Press **Play** again to stop. Anything you changed while playing is thrown away, which catches everybody once.

Now go to [lesson 1](lessons/01-scenes-and-components.md).

## Stuck?

| What you see | What to do |
| --- | --- |
| The keys do nothing | Click inside the Game view first. It only gets the keyboard when it has focus |
| Pink objects | A material's shader did not compile yet. Wait for the bar at the bottom right to finish, then press Play again |
| "Enter Safe Mode?" when opening | A script has an error. Choose Enter Safe Mode, read the error in the Console window, and fix it |
| The Hub cannot find the editor version | Open the project's `ProjectSettings/ProjectVersion.txt` to see the version, then install it from the [Unity download archive](https://unity.com/releases/editor/archive) |
| Something else | [Open an issue](https://github.com/voss-labs/learn-unity/issues/new/choose) with a screenshot. If you are stuck, the next person will be too |
