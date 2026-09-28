# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repository is

A personal study lab (학습 저장소), not a product. Three **independent** projects sit side by side with no shared code or build between them:

| Path | What it is |
| --- | --- |
| `CppLab/` | C++20 (v145 toolset) console solution — `CppLab.slnx` → `CppLab/CppLab.vcxproj` |
| `CSharpLab/` | Legacy (non-SDK-style) .NET Framework 4.7.2 console solution — `CSharpLab.slnx` → `CSharpLab/CSharpLab.csproj` |
| `UnityLab/` | Unity 6000.3.10f1 URP project |

Both C++ and C# labs are currently empty `main` stubs; `UnityLab/Assets/2_Scripts/` has no scripts yet. Expect to be adding the first real code in a given area rather than fitting into existing structure.

README.md, commit messages, and comments are written in Korean — match that when committing.

## Working conventions

This is a study repository: the owner is here to learn Unity algorithms and design patterns, not to ship features.
**Do not hand over finished implementations.** Explain the approach in steps — what the concept does, what to build
first, what decisions come next — and stop where the owner can write the code themselves. Illustrative snippets and
skeletons are fine; a complete drop-in solution defeats the purpose. Reviewing code they wrote is always welcome.

The README defines the study flow each topic follows: **Concept → Theory → Implementation → Experiment → Result**. The point is not just working code but comparing implementations, measuring, and recording what was learned. When adding a topic, keep alternative implementations side by side rather than replacing one with another, and keep notes on the trade-offs.

## Build & run

Both solutions use the `.slnx` format, which needs the Visual Studio 18 MSBuild. `dotnet build` will **not** work on `CSharpLab.csproj` (non-SDK-style project). Use:

```sh
MSB="/c/Program Files/Microsoft Visual Studio/18/Community/MSBuild/Current/Bin/MSBuild.exe"

# C# → CSharpLab/CSharpLab/bin/Debug/CSharpLab.exe
"$MSB" CSharpLab/CSharpLab.slnx -v:m

# C++ → CppLab/x64/Debug/CppLab.exe   (solution-level x64/, not under CppLab/CppLab/)
"$MSB" CppLab/CppLab.slnx -p:Configuration=Debug -p:Platform=x64 -v:m
```

Neither lab has a test project; there is no lint setup.

## UnityLab

The Unity 6 editor lives at `E:\Unity\6000.3.10f1\` (Hub secondary install path — not under `C:\Program Files\Unity\Hub\Editor`, which only holds older 2019–2022 editors).

**Prefer the UnityMCP tools over editing Unity files by hand.** The project has `com.coplaydev.unity-mcp` as a package dependency, so an open editor exposes `manage_script`, `manage_gameobject`, `manage_scene`, `run_tests`, `read_console`, etc. Scenes, prefabs, and `.asset` files are YAML with GUID references — hand-editing them or writing a `.cs` file without its `.meta` will desync the asset database.

Asset layout is a fixed numbered convention; put new files in the matching bucket:

```
Assets/1_Scenes   2_Scripts   3_Prefabs   4_Model   5_Animations   6_Data
Assets/Settings     — URP pipeline assets (PC_RPAsset / Mobile_RPAsset + renderers), volume profiles
Assets/Plugins      — JetBrains RiderFlow (vendored, do not edit)
Assets/Thirdparty   — empty
```

Only `Assets/1_Scenes/Empty.unity` is in the build settings. There are no `.asmdef` files outside the RiderFlow plugin, so gameplay scripts compile into `Assembly-CSharp`. `com.unity.test-framework` is installed but no test assembly exists yet — creating one means adding a Tests asmdef referencing `UnityEngine.TestRunner`/`UnityEditor.TestRunner` before `run_tests` has anything to run.

### Unity from the command line

The MCP tools need the editor **open**; the CLI below needs it **closed** (the `Library/` lock is exclusive). Pick one — they cannot be used at the same time.

```sh
UNITY="/e/Unity/6000.3.10f1/Editor/Unity.exe"
PROJ="D:/_Project/Programming_Lab/UnityLab"

# EditMode tests; drop -testFilter to run all. Filter matches FullName (Namespace.Class.Method).
"$UNITY" -runTests -batchmode -projectPath "$PROJ" \
  -testPlatform EditMode -testFilter "Some.Namespace.MyTests.SingleCase" \
  -testResults "$PROJ/Logs/results.xml" -logFile -

# PlayMode tests: same, but omit -batchmode (Unity needs a real player loop).
```

After editing a `.cs` file outside the editor while it is open, call `refresh_unity` (or focus the editor) — Unity will not recompile on its own.

When something fails and the editor is not available, the logs are `UnityLab/Logs/AssetImportWorker*.log` (import/compile errors) and `%LOCALAPPDATA%/Unity/Editor/Editor.log` (everything else).

Key packages beyond URP: Input System (new), Cinemachine 3, Character Controller, Unity Physics (DOTS), AI Navigation, Timeline, Recorder, Profile Analyzer.

The `*.csproj`/`.sln` at `UnityLab/` root are Unity-generated — never edit them.

### Gotcha: `UnityLab/Packages/` is not versioned

`.gitignore:130` has a legacy NuGet `packages/` rule that, case-insensitively on Windows, also swallows `UnityLab/Packages/` — so `manifest.json` and `packages-lock.json` are untracked. Package additions will not show up in `git status`. `ProjectSettings/` and `Assets/` are tracked normally.
