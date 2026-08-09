# Create a Plugin for Metalsharp

Plugins manipulate files in small, understandable ways, and Metalsharp gets more useful with every plugin the community builds. No matter how simple or complex, developing — and maybe publishing — your own plugin can be a real contribution to the ecosystem. This tutorial walks through developing and publishing a plugin. As an example, we'll build a simplified version of the `Collections` plugin that only acts on input files.

## Contents

* [Developing the Plugin](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md#developing-the-plugin)
  * [Implementing `IMetalsharpPlugin`](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md#implementing-imetalsharpplugin)
  * [Extending `MetalsharpProject`](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md#extending-metalsharpproject)
* [Publishing the Plugin](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md#publishing-the-plugin)

## Developing the Plugin

Metalsharp targets .NET 10. If you're planning on publishing your plugin — and you should — it should live in its own class library project, targeting .NET 10 or later.

It's conventional for a plugin to sit in a namespace nested under `Metalsharp`, matching the name of the project. In this tutorial we'll call our version of the `Collections` plugin `MyPlugin`, so our code will live in the `Metalsharp.MyPlugin` namespace (this mirrors real published plugins, such as [`Metalsharp.FluidTemplate`](https://github.com/IanWold/Metalsharp.FluidTemplate)). Your project will, of course, need a reference to Metalsharp:

```plaintext
dotnet add package Metalsharp
```

### Implementing `IMetalsharpPlugin`

This interface is easy to implement — it has a single method, `Execute`, and that's where our logic goes.

```c#
namespace Metalsharp.MyPlugin;

public class MyPlugin : IMetalsharpPlugin
{
    public void Execute(MetalsharpProject project)
    {

    }
}
```

Looking at how the real `Collections` plugin works (documented [here](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/api.md#collections)), we'll need a constructor that accepts the collection's definition: a name, and a `Predicate<MetalsharpFile>` to select which files belong in it. Since our plugin needs constructor arguments, it can't be invoked by referencing its type alone (`Use<MyPlugin>()`) — callers will need to pass an instance.

```c#
namespace Metalsharp.MyPlugin;

public class MyPlugin(string name, Predicate<MetalsharpFile> predicate) : IMetalsharpPlugin
{
    public void Execute(MetalsharpProject project)
    {

    }
}
```

Now we want to select every file from the input matching the predicate. `MetalsharpProject.InputFiles` combined with LINQ gets us there:

```c#
public void Execute(MetalsharpProject project)
{
    var matches = project.InputFiles.Where(file => predicate(file));
}
```

For this simplified version of `Collections`, we'll store the matching files' paths as a string array directly in the project's metadata, keyed by the collection's name:

```c#
public void Execute(MetalsharpProject project) =>
    project.Meta(name, project.InputFiles.Where(file => predicate(file)).Select(file => file.FilePath).ToArray());
```

And that's the entire `Execute` method! Ideally, every Metalsharp plugin should be about this simple — though in practice, most plugins need a bit more than a one-liner. We can add that extra functionality without touching `Execute` at all, by adding extension methods to `MetalsharpProject`.

### Extending `MetalsharpProject`

As the plugins that ship with Metalsharp demonstrate, it's easy to add an extension method to `MetalsharpProject` for invoking your plugin:

```c#
public static class MyPluginExtensions
{
    public static MetalsharpProject UseMyPlugin(this MetalsharpProject project, string name, Predicate<MetalsharpFile> predicate) =>
        project.Use(new MyPlugin(name, predicate));
}
```

This is worth doing even beyond convenience, since it gives you room to customize how the plugin is constructed later without a breaking change. It's also a good place to add helper methods for consuming what your plugin produces. An array of file paths isn't very useful on its own — users will usually want the actual `MetalsharpFile` objects. We can provide that:

```c#
public static class MyPluginExtensions
{
    public static MetalsharpProject UseMyPlugin(this MetalsharpProject project, string name, Predicate<MetalsharpFile> predicate) =>
        project.Use(new MyPlugin(name, predicate));

    public static IEnumerable<MetalsharpFile> GetMyPluginFiles(this MetalsharpProject project, string name) =>
        project.Metadata[name] is string[] filePaths
            ? project.InputFiles.Where(file => filePaths.Contains(file.FilePath))
            : throw new ArgumentException($"There is no collection by the name {name}");
}
```

And now we have a small but genuinely useful plugin.

## Publishing the Plugin

Before publishing, make sure your plugin is well documented. Metalsharp is billed as an easy-to-use, well-documented library, and your plugin should hold to the same standard.

Only a small handful of plugins ship with Metalsharp itself, and that's intentional — your plugin almost certainly belongs in its own package rather than in core. If you can write a better version of one of the built-in plugins, we'd genuinely welcome that; the goal is for Metalsharp to stay as small as possible and act as a platform other plugins build on, rather than a monolith that tries to solve everything itself.

From here, there are a few ways to get your plugin out into the world.

**Open Source**

If you need to keep your source closed — for example, in an enterprise setting — that's fine, distribute it however makes sense for you. But if you're able to, consider open-sourcing your plugin on a platform like GitHub. It makes it easier for the community to contribute back and builds a stronger ecosystem around Metalsharp as a whole.

**NuGet**

NuGet is the standard package manager for .NET, and it's how Metalsharp itself is distributed. Publishing your plugin there gives other developers the easiest possible path to using it — a single `dotnet add package` away.

**Direct Distribution**

Sometimes circumstances mean the only practical option is to distribute a compiled DLL directly. This works, but it's discouraged unless it's genuinely your only option — it loses you the discoverability and dependency management that NuGet provides.

---

Now you've developed and published your own Metalsharp plugin! Congrats!

Did you notice something odd with this tutorial — a typo, outdated information, or something that could be explained better? [Editing this page](https://github.com/IanWold/Metalsharp/edit/master/Metalsharp.Documentation/tutorial-plugin.md) and submitting a PR would be a great way to contribute to Metalsharp!
