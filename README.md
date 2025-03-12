# Unity - CVar

[CVar](https://github.com/DerKekser/unity-cvar) provides a flexible way to create console variables in Unity projects. With this system, you can mark fields, properties, and methods for console access using a simple attribute.

## Contents
- [Simple Example](#simple-example)
- [Supported Types](#supported-types)
- [Static Members](#static-members)
- [Methods](#methods)
- [Manual Registration](#manual-registration)
- [Console](#console)
    - [Creation](#creation)
    - [Commands](#commands)
- [Install](#install)
    - [Install via Unity Package](#install-via-unity-package)
    - [Install via git URL](#install-via-git-url)
- [License](#license)

### Simple Example

Define a console variable by adding the `CVar` attribute to a field, property, or method:

```csharp
using Kekser.UnityCVar;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    [CVar("player_speed")]
    private float _speed = 5.0f;

    [CVar("player_health", "Sets the player's health")]
    public int Health { get; private set; } = 100;
}
```
### Supported Types

The system supports various types by default, including:  
- Basic types (int, float, bool, string)
- Unity types (Vector2, Vector3, Vector4, Quaternion)
### Creation

### Static Members

You can also mark static fields, properties, and methods as CVars:

```csharp
using Kekser.UnityCVar;

public static class GameSettings
{
    [CVar("game_difficulty")]
    private static int _difficulty = 1;

    [CVar("game_debug_mode")]
    private static bool DebugMode { get; set; } = false;
}
```
### Methods

Methods can also be marked with the CVar attribute:

```csharp
using Kekser.UnityCVar;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [CVar("restart_game")]
    private void RestartGame()
    {
        // Restart game
    }

    [CVar("add_score")]
    private void AddScore(int points)
    {
        // Add points
    }
}
```
### Manual Registration

You can manually register CVars by calling the `CVarAttributeCache.RegisterCVar` method:

```csharp
CVarAttributeCache.RegisterCVar(
    "go_setactive",
    "Enables or disables the current target GameObject (true/false)",
    typeof(GameObject),
    "SetActive"
);
```
### Console

#### Creation

You can create a console by dragging the `Console_Prefab` prefab  from the `UnityCVarConsole` folder into your scene.

#### Commands

To view all available commands, type `con_list` in the console. You can also use `con_list <filter>` to filter the results.
### Install

#### Install via Unity Package

Download the latest [release](https://github.com/DerKekser/unity-cvar/releases) and import the package into your Unity project.
#### Install via git URL

You can add this package to your project by adding these git URLs in the Package Manager:
```
https://github.com/DerKekser/unity-cvar.git?path=Assets/Kekser/UnityCVar
https://github.com/DerKekser/unity-cvar.git?path=Assets/Kekser/UnityCVarConsole
```
![Package Manager](/Assets/Kekser/Screenshots/package_manager.png)
### License

This library is under the MIT License.