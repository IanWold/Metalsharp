# Create a Plugin for Metalsharp

Plugins manipulate files in small and understandable ways. Metalsharp works best with many plugins, and it will get better with more plugins made by more people. No matter how simple or complicated, creating (and maybe even publishing) your own plugin could be very beneficial to others. This tutorial will show how to develop and publish a plugin. As an example, this tutorial will create a version of the `Collections` plugin that only acts on input files.

> Side note: a practical demo plugin that is more generic than the Collections plugin should probably be made for this tutorial. If you might want to do that and submit a PR, that would be a great way to contribute to Metalsharp!

## Contents

* [Developing the Plugin](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md#developing-the-plugin)
  * [Implementing `IMetalsharpPlugin`](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md#implementing-imetalsharpplugin)
  * [Extending `MetalsharpProject`](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md#extending-metalsharpdirectory)
* [Publishing the Plugin](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md#publishing-the-plugin)

## Developing the Plugin

Metalsharp is developed as a .NET Standard library, and your library would be the most portable if you are able to target .NET Standard as well. However, there is nothing stopping you from exclusively targeting .NET Core or Framework. If you are planning on publishing your plugin - which you definitely should - your plugin should be in a class library.

It is recommended that your plugin sit in a namespace within the `Metalsharp` namespace. In this tutorial, we'll call our version of the `Collections` plugin `MyPlugin`, so your plugin should sit in the namespace `Metalsharp.MyPlugin`. It is easiest if this is also the name of your project (for example, [`Metalsharp.FluidTemplate`](https://github.com/IanWold/Metalsharp.FluidTemplate)). This project will need an assembly reference to Metalsharp, which you must build from source. When Metalsharp reaches `v0.9.0-rc1`, it will be on NuGet, and this tutorial will be updated accordingly.

### Implementing `IMetalsharpPlugin`

This interface is easy to implement - it has one method, `Execute`, and that's where our code will go. Visual Studio's automatic implementation gets us most of the way here.

```c#
namespace Metalsharp.MyPlugin
{
    public class MyPlugin : IMetalsharpPlugin
    {
        public void Execute(MetalsharpProject directory)
        {
        
        }
    }
}
```

Considering the collections plugin, which is documented [here](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/api.md#collections), we will need a constructor to accept the definition of the collection, and our plugin won't be able to be invoked by referencing its type. The definition is a string to name the collection and a `Predicate<IMetalsharpFile>` to select the files that will go into the collection.

```c#
namespace Metalsharp.MyPlugin
{
    public class MyPlugin : IMetalsharpPlugin
    {
        private string _name;
        private Predicate<IMetalsharpFile> _predicate;

        public MyPlugin(string name, Predicate<IMetalsharpFile> predicate)
        {
            _name = name;
            _predicate = predicate;
        }

        public void Execute(MetalsharpProject project)
        {
            
        }
    }
}
```

Now, we want to select each of the files from the input matching the predicate. `MetalsharpProject.InputFiles` and `LINQ` gives us this access.

```c#
public void Execute(MetalsharpProject project)
{
    ...
    project.InputFiles.Where(file => _predicate(file));
    ...
}
```

The `Collections` plugin stores the full path of each file in an array in the `MetalsharpProject`'s metadata, so we want to select the `FilePath` from each matching file, and we want to add a record to the metadata of `directory` keyed by the name of the collection.


```c#
public void Execute(MetalsharpProject project) =>
    project.Meta(_name, project.InputFiles.Where(file => _predicate(file)).Select(file => file.FilePath).ToArray());
```

And that's all for the `Execute` method! Ideally, all Metalsharp plugins should be this simple. Truthfully though, most plugins are *slightly* more complicated and will need more functionality in the `Execute` method. We can, however, add more functionality by adding extensions to `MetalsharpProject` for this plugin.

### Extending `MetalsharpProject`

As the plugins that come with Metalsharp demonstrate, it's easy to add an extension to `MetalsharpProject` to invoke the plugin,

```c#
public static class MetalsharpExtensions
{
    public static MetalsharpProject UseMyPlugin(this MetalsharpProject project, string name, Predicate<IMetalsharpFile> predicate) =>
        project.Use(new MyPlugin(name, predicate));
}
```

This is nice to include because you can add more customization to the construction of your plugin if you need. Another use case specific to the `Collections` plugin is that we can introduce more methods to help our user use the collections stored in the `MetalsharpProject`. It is not too useful to our users to have an array of the paths of files in the collections - most of the time they are going to want to access the actual file objects themselves. We can add an extension to deliver this for us:

```c#
public static class MetalsharpExtensions
{
    // Dumb method name but call it what you want!
    public static IMetalsharpFile[] GetMyPluginFiles(this MetalsharpProject project, string name) =>
        project.Metadata[name] is string[] filePaths
            ? project.InputFiles.Where(file => filePaths.Contains(file.FilePath))
            : throw new ArgumentException("There is no collection by the name " + name);
}
```

And now we have a useful plugin that is easy for our users to consume!

## Publishing the Plugin

Before considering publishing your plugin, make sure you have suitable documentation. Metalsharp is billed as an easy-to-use and well-documented library, and your plugin will surely benefit from good documentation too.

There is a small handful of plugins that comes with Metalsharp, but your plugin almost certainly doesn't belong there. If you develop a plugin for Metalsharp, that should be your own project. In fact, it would be desirable for there to be no plugins that come with Metalsharp (and, to that end, if you think you can redo a plugin that comes with Metalsharp and make it better - do it, and then we can shave one more off of Metalsharp). The reason is to keep Metalsharp as slim as possible and to make it a sort of platform for plugins - Metalsharp is not a solution, it enables solutions.

From here, there are several ways to get your plugin into the world

**Open Source**

If you want to keep your source closed (or if you're in an enterprise and you need to keep your source closed) by all means do so, and publish your plugin elsewhere. However, if you can, you should consider making your plugin open source on a platform like GitHub. This enables a greater level of community involvement around Metalsharp.

**NuGet**

NuGet is the de facto package manager for .NET, and that's how Metalsharp will eventually be released. Indeed, until Metalsharp is on NuGet it doesn't make sense to release here. But that's coming around the corner, so preparing your package for NuGet might make sense. And, it'll give other users an easier time consuming your plugin.

**DLL Download**

This option kind of sucks, but sometimes your hands are tied and the only way to release a library is by allowing others to download a DLL from your site. Unless totally necessary, this practice is discouraged.

---

Now you've developed and published your own Metalsharp plugin! Congrats!

Did you notice something odd with this tutorial - a typo, old information, or just something that could make it a bit better? [Editing this tutorial](https://github.com/IanWold/Metalsharp/edit/master/Metalsharp.Documentation/tutorial-plugin.md) and submitting a PR would be a great way to contribute to Metalsharp!
