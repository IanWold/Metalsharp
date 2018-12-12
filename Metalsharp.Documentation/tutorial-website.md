# Create a Website with Metalsharp

This tutorial will walk you through (almost) all the components of Metalsharp you might use in the regular course of making a website. This tutorial will make a personal/blog website similar to https://ianwold.com. Some layout/style content will be referenced from this project.

> Side note: a demo website that is not an actual personal website for anybody should probably be made for this tutorial. If you might want to do that and submit a PR, that would be a great way to contribute to Metalsharp!

The website we'll be making will have a homepage, a few text pages, and several blog posts. We'll use the [Metalsharp.FluidTemplate](https://github.com/IanWold/Metalsharp.FluidTemplate) plugin to use templates for our site. First, we'll walk through adding each of the pages and the design decisions behind doing things the way they are presented (hopefully allowing you to easier change this structure if you desire). Then, we'll write the Metalsharp code to do all the generation.

> **Please Note:** Until `v0.9.0-rc1`, Metalsharp won't be able to be installed via NuGet. You'll need to build it from source yourself.

## Content

* [Project Files](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#project-files)
  * [Website Content](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#website-content)
  * [Static Content](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#static-content)
  * [Templates](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#templates)
* [Build it with Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#build-it-with-metalsharp)
  * [Adding Files](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#adding-files)
  * [Using our First Plugins](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#using-our-first-plugins)
  * [Handling the Blog Posts](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#handling-the-blog-posts)
  * [Generating the Blog Page](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#generating-the-blog-page)
  * [Using the Templates](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#using-the-templates)
  * [Building the Site](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#building-the-site)
  * [Conclusion](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#conclusion)

## Project Files

### Website Content

We'll write our website in Markdown, and the first file we'll need is `index.md`, which will be translated to `index.html` by Metalsharp. We will first need to create a project folder (let's call it `MyProject`), and in there we'll include a `Site` folder for all the content of the website - that's where `index.md` will go.

```plaintext
MyProject
└── Site
    └── index.md
```

And we can write whatever we want in `index.md`.

Let's also inclue an `about.md` page and a `contact.md` page, again with whatever content you want:

```plaintext
MyProject
└── Site
    ├── about.md
    ├── contact.md
    └── index.md
```

Now, let's put our blog posts in a sub-directory. That's both so that it's easier for us to categorize, but also so that we can easily separate them out when we need to work with them in Metalsharp. In addition, let's include some frontmatter with our posts. Author, date, and title information would go well in the Metadata. Here's an example post:

> If you're unfamiliar with the concept of "frontmatter", it is a common way to include metadata with files. It's used by [Jekyll](https://jekyllrb.com/docs/front-matter/), for example. Metalsharp's frontmatter plugin supports YAML and JSON frontmatter.

```markdown
---
author: Some Body
date: 1 January 1990
title: My First Post
description: The first post in my blog
---

# This is my First Post

How exciting!
```

We'll include this and a couple other posts in a `Posts` folder:

```plaintext
MyProject
└── Site
    ├── Posts
	│   ├── my_first_post.md
    │   ├── i_love_blogging.md
	│   └── on_writers_block.html
    ├── about.md
    ├── contact.md
    └── index.md
```

This covers all the content we need to write for our website. Notice we have not written a `blog.md` file. This is because it would be better to generate that page from the posts we've added. This way, when we need to add a new post, we just need to put the post in the `Posts` folder, and we don't need to update anything else (besides regenerating the website, of course).

### Static Content

There are some files (for example, a `css` style) that we don't want to manipulate in any way, we just want it to go straight to the output directory. Let's add another folder alongside `Site` for these. This makes it easier both to maintain a nice directory structure and to handle in Metalsharp. For now, we just need our `MainStyle.css`:

```plaintext
MyProject
├── Site
│   ├── Posts
│   │   ├── my_first_post.md
│   │   ├── i_love_blogging.md
│   │   └── on_writers_block.html
│   ├── about.md
│   ├── contact.md
│   └── index.md
└── Static
    └── MainStyle.css
```

For the content of `MainStyle.css`, let's just go ahead and use the style directly [from the reference site](https://github.com/IanWold/ianwold.github.io/blob/master/MainStyle.css).

### Templates

This tutorial assumes we're using [Metalsharp.FluidTemplate](https://github.com/IanWold/Metalsharp.FluidTemplate) to handle templating, but you could certainly use another system for templating [or create your own](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md).

We'll place our template files in a `Template` directory alongside the `Site` and `Static` directories, once again because this is a nice directory structure and because it will be easier in Metalsharp. We'll need three templates - a layout file for all of the pages, an article template to provide extra styling for the blog posts, and a blog template to help generate the blog page. This tutorial is going to lift the [template folder of the reference site](https://github.com/IanWold/ianwold.github.io/tree/master/src/Templates), but of course you are welcome to develop your own. Later in the tutorial we'll go over some of the specific structures in these templates.

Here's our directory structure now:

```plaintext
MyProject
├── Site
│   ├── Posts
│   │   ├── my_first_post.md
│   │   ├── i_love_blogging.md
│   │   └── on_writers_block.html
│   ├── about.md
│   ├── contact.md
│   └── index.md
├── Static
│   └── MainStyle.css
└── Templates
    ├── article.liquid
    ├── blog.liquid
    └── layout.liquid
```

This concludes all of the files for the website project. You will, in addition, need to have a C# Metalsharp project which will build this site. How you configure that project and where you put it is up to you. For this tutorial, we'll assume that the Metalsharp program will execute at the `MyProject` directory.

## Build it with Metalsharp

To start using Metalsharp, you'll need a new C# console application. Metalsharp is written in .NET Standard, so you can choose whether you want to create a .NET Framework or a .NET Core console application. In general, if you are running Windows and you will need to rely on the robustness and future-compatibility of .NET Framework (this would probably be in an enterprise environment), then choose .NET Framework. Otherwise, Microsoft really wants you to choose .NET Core. This tutorial will use .NET Core, though it will (probably) make no difference to you. We'll call the C# app `MyProject`.

> When Metalsharp is on NuGet, you will also have the option of using a C# script with NuGet packages (via [scriptcs](http://scriptcs.net/)) to create your project. Until then, this tutorial will only cover a C# project. In addition, you will currently need to build Metalsharp from source and add an assembly reference to your built DLL. This tutorial will be updated when Metalsharp hits NuGet.

Your project will have a `Program.cs` file with a `Main` method inside. In the main method, we will instantiate a `MetalsharpDirectory` object, add our files, apply several plugins to it, and built it to an output directory. Each step of the way this tutorial will explain parts of the Metalsharp API. Full generated API documentation [is available here](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/api.md).

### Adding files

> Note that this tutorial will use Windows directory separator characters (i.e. `\`). Metalsharp is written to handle the directory separator characters specific to your platform. So, if you're not using Windows, use the character(s) native to your platform. If you want to use universal directory separator characters, use `Path.DirectorySeparatorCharacter` and/or `Path.Combine` from .NET.

So, we need to get our Metalsharp project up and going, and we'll want to add the site contents (in `/Site`) right off the bat. When we instantiate our `MetalsharpDirectory`, we can read in the files in that directory right away:

```c#
var directory = new MetalsharpDirectory("Site");
```

When Metalsharp reads files in, it stores them in a single list. That is, there is no `Folder` object now storing our files. All the files are `MetalsharpFile` objects, and they're all stored in a single list. Each `MetalsharpFile` object has a `FilePath` property. Here is what our list of files looks like now, having read all the files in the `Site` directory:

- `Site\about.md`
- `Site\contact.md`
- `Site\index.md`
- `Site\Posts\my_first_post.md`
- `Site\Posts\i_love_blogging.md`
- `Site\Posts\on_writers_block.md`

They have a virtual directory structure which resembles that of the files on disk (we call this a "virtual directory structure" to reinforce the fact that they're really in a single list). There's a slight problem here, and that is that when we output our website, we don't want the website inside a `Site` directory within our output directory. To fix that, we want to move the contents of `Site` up one directory. We can do that by using a different constructor for `MetalsharpDirectory`:

```c#
var directory = new MetalsharpDirectory("Site", ".");
```

This will still read all the files in `Site`, but instead of remembering them in a `Site` directory, Metalsharp will remember them in the "." directory. Windows (and other OSes?) understand "." as a "root" directory - at output there will be no directory created named ".". Thus, our new list of files looks like this:

- `.\about.md`
- `.\contact.md`
- `.\index.md`
- `.\Posts\my_first_post.md`
- `.\Posts\i_love_blogging.md`
- `.\Posts\on_writers_block.md`

It's important to note here that `MetalsharpDirectory` keeps two lists of files, in fact. These are the input and output files (`MetalsharpDirectory.InputFiles` and `MetalsharpDirectory.OutputFiles`, respectively). The distinction is that files in the output list will be written to disk during build, and the input files won't be. When we instantiate `MetalsharpDirectory` and tell it to read in a directory, it places those files in the input list only. This gives us a staging area and allows us to more precisely control which files we output, and how we do so.

Once we've instantiated our `MetalsharpDirectory`, we can continue adding files to the input and output lists. We still need to add our `Templates` and `Static` directories.

`Templates` needs to go in the inputs (because they won't be included with the final site):

```c#
directory.AddInput("Templates");
```

And `Static` can go straight to the output list - we just want to copy them over as-is:

```c#
directory.AddOutput("Static", ".");
```

Notice that as we add `Templates`, it will be placed in a virtual directory called `Templates`, while `Static` will be placed in our root (`.`) directory.

Now that you've added several files to the project, you may need to move some files from one directory to another, or you may need to remove a file or two. `MetalsharpDirectory` includes methods to move and remove files from the input and output. We don't need to do that on this project, but here are some examples in case you do:

```c#
directory
    // Move files in Posts directory to Articles directory
    .MoveInput(".\\Posts", ".\\Articles")
    .MoveOutput(".\\Posts", ".\\Articles")

    // Delete .txt files
    .RemoveInput(file => file.Extension == ".txt")
    .RemoveOutput(file => file.Extension == ".txt");
```

### Using our First Plugins

We've got our files in, now we need to start processing them. Let's run these two plugins first:

```c#
directory
    .UseFrontmatter()
    .UseMarkdown();
```

These two plugins come with Metalsharp and they're pretty self-descriptive. `Frontmatter` parses the frontmatter of a file and places it in the file's metadata, and `Markdown` parses Markdown files into HTML, and places the resulting HTML files in the output list. The order of these two are important: when the markdown is parsed, the generated HTML files are given their source file's metadata. Thus, we need to populate the metadata before we generate the HTML files.

> *Metadata* is any data associated with the file that isn't part of the file's actual text. Metalsharp uses a `Dictionary<string, object>` to store metadata.

There are a few ways to invoke a plugin. Above, the plugins are invoked using extension methods to `MetalsharpDirectory`. Each plugin is a class (which implements `IMetalsharpPlugin`), and these plugins can also be accessed by referencing their type with the `MetalsharpDirectory.Use<T>()` method. The above is equivalent to the following:

```c#
directory
    .Use<Frontmatter>()
    .Use<Markdown>();
```

This can be desirable if you prefer that style of coding. Note though that this only works for plugins with default constructors (a constructor with no arguments). What both the extension methods and generic-type `Use` methods do, though, is they create an instance of the plugin object and call its `Execute` method. You can invoke a plugin by supplying an instance of the plugin object, if that makes more logical sense to you. Again, the above are each equivalent to the following:

```c#
directory
    .Use(new Frontmatter())
    .Use(new Markdown());
```

This latter form is much more akin to the `Metalsmith` library on which Metalsharp is based. If you are not sure which form to choose, the following principle may help: invoking a plugin by instance will be the most consistent and robust, though verbose, way because every plugin is a class, while invoking a plugin by extension method may be a more expressive, though inconsistent, way because the plugin author has more control over the extension method(s).

What is important is that after running our `Markdown` plugin, we get the following files in the output directory, while the input remains just as we left it:

- `.\about.html`
- `.\contact.html`
- `.\index.html`
- `.\Posts\my_first_post.html`
- `.\Posts\i_love_blogging.html`
- `.\Posts\on_writers_block.html`

### Handling the Blog Posts

Just like each file has its own metadata, `MetalsharpDirectory` also has it's own metadata to pass information about the whole project from plugin to plugin. The `Collections` plugin uses this project-level directory to store collections of files, in both the input and the output lists. This can be useful if you have several files that each need some operations performed on them. In our case, we need to do some additional processing to the blog post files, so we will use the `Collections` plugin to group the posts together.

To create a collection, we need a name for the collection and a function to choose which files to include in that collection. Here's how we'll collect the blog post files:

```c#
directory
    .UseCollections("posts", file => file.IsChildOf("Posts"));
```

This will add a new record to the metadata in our `directory` object, which will look like the following mess (using the C# literal dictionary notation):

```c#
["posts"] =
{
    ["input"] =
    {
        ".\\Posts\\my_first_post.md"
        ".\\Posts\\i_love_blogging.md"
        ".\\Posts\\on_writers_block.md"
    },
    ["output"] =
    {
        ".\\Posts\\my_first_post.html"
        ".\\Posts\\i_love_blogging.html"
        ".\\Posts\\on_writers_block.html"
    }
}
```

This selected each of the files, in each the input and output lists, which are children of the "Posts" directory, and put them into a metadata record with the key "posts-collection", which was the name of the collection. If you were to recall the metadata value for the key "posts-collection" from the `MetalsharpDirectory` metadata (as follows), you would be able to get this object, but there are better ways to get this information.

```c#
// Getting at your collection - the hard way
var postsCollection = directory.Metadata["posts-collection"] as Dictionary<string, string[]>;
```

Now we need to add metadata to each post. Specifically, we want each post to reference the `article` template (from our `Templates` directory) in its metadata. The reason for this is specific to the `FluentTemplates` plugin and is explained below. If you are using a different system for templating, your mileage here may vary.

```c#
directory.GetOutputFilesFromCollection("posts-collection").ToList().ForEach(file => file.Metadata.Add("template", ".\\Templates\\article.liquid"));
```

This method, `GetOutputFilesFromCollection` is an extension method for the `Collections` plugin. It does all the typecasting and file-matching for you so that you have all of your files from the "posts-collection" collection - and specifically the outputs, at that. There is also `GetInputFilesFromCollection` and `GetFilesFromCollection`, for the input files and all files, respectively.

### Generating the `Blog` Page

This is perhaps the most complicated bit. This section is mostly in reference to the [blog layout from the reference site](https://github.com/IanWold/ianwold.github.io/blob/master/src/Templates/blog.liquid). The way we will generate our `blog.html` file is that we will create an empty `html` file in the output list, tell it that `Templates\blog.liquid` is its template, and then let the templating engine generate the whole page.

Our specific templating engine ([Metalsharp.FluidTemplates](https://github.com/IanWold/Metalsharp.FluidTemplate), which uses the [Fluid](https://github.com/sebastienros/fluid) library) will pass a file's metadata to the template as a set of objects. As you see in the `blog.liquid` template, it expects there to be a `posts` object, which is a list, and for each object in `posts` to have `Slug`, `Title`, `Date`, and `Description` properties. This `posts` object will need to be in the file's metadata (under the key "posts"). To be able to do this in a way that `Fluid` understands, we will need to create our own `Post` object to store the metadata from each post. Fluid will not work with anonymous objects, but it does use reflection to translate POCOs for its own understanding. For brevity, here is that object:

```c#
class Post
{
    public Post(MetalsharpFile file)
    {
        Title = file.Metadata["title"] as string;
        Date = file.Metadata["date"] as string;
        Description = file.Metadata["description"] as string;
        Author = file.Metadata["author"] as string;
        Slug = file.Metadata["slug"] as string;
    }

    public string Title { get; set; }

    public string Date { get; set; }

    public string Description { get; set; }

    public string Author { get; set; }

    public string Slug { get; set; }
}
```

Now we need to make a `blog.html` file in the output, and we need to set its metadata to have a list of these `Post` objects, as well as a link to the `blog.liquid` template. As we covered before, `MetalsharpDirectory` does have an `AddOutput` method, and we could *just* use that here, but this is an opportunity to demonstrate another feature. `MetalsharpDirectory` has another overload of the `Use` method which allows you to use a lambda function (that is, an anonymous function) as a kind of plugin. If you want to use a function, it'll have one input (the directory) and should have no output. This is what .NET calls an `Action<MetalsharpDirectory>`. Here's how it would look:

```c#
directory.Use(dir => dir.AddOutput("", "blog.html")
{
    Metadata = new Dictionary<string, object>()
    {
        ["posts"] = dir.GetOutputFilesFromList("posts").Select(file => new Post(file)),
        ["template"] = ".\\Templates\\blog.liquid"
    }
});
```

Obviously, because our `MetalsharpDirectory` is in the variable `directory`, we don't need this complexity. The following will achieve the same for us:

```c#
directory.AddOutput("", "blog.html")
{
    Metadata = new Dictionary<string, object>()
    {
        ["posts"] = directory.GetOutputFilesFromList("posts").Select(file => new Post(file)),
        ["template"] = ".\\Templates\\blog.liquid"
    }
});
```

Where the former option is desirable is where you are using `MetalsharpDirectory`'s fluent interface. That is, if you are chaining together all of your plugin invocations rather than creating a new `MetalsharpDirectory` variable, you will need to use the former option to access the current `MetalsharpDirectory` object.

### Using the Templates

Now we've got our templates generating our `blog.html` page, but we need to actually make the call to `Metalsharp.FluidTemplate` to add the templates to our HTML files. Before this, though, we have a slight problem. All of our blog post pages are sitting in a `Posts` directory. The trouble with this is that our templates will need to accommodate for that in the hyperlinks - if our user is on a post page and wants to get home, we need to generate a `../` prefix for each link going back one directory. Now, it would be simple enough for us to take those post pages and raise them one directory, but there are times where that may not be desirable. So, here is the solution.

At the top of our `layout.liquid` template, we'll put the following code:

```liquid
{% assign directoryPrefix = "" %}
{% for i in (1..level) %}
	{% assign directoryPrefix = directoryPrefix + "../" %}
{% endfor %}
```

This expects that our template will be given a `level` object (an int), which will tell the template how many directory levels we are from the root directory. It will then create a `directoryPrefix` variable in the template with the appropriate number of `../` to get us back to the root directory. A hyperlink in the layout will then look like:

```liquid
<a href="{{ directoryPrefix }}index.html">MyProject</a>
```

When we're at the root directory, `directoryPrefix` will be empty, so the link will take us right to `index.html`. However, when we're in a post, `directoryPrefix` will be `../`, which will take us back up one directory to get to `index.html`.

Now we just need to generate this `level` variable in our Metalsharp code, and give every page a new metadata record keyed "level" with the appropriate integer. It should be relatively easy to write a function that goes over every file in both the inputs and outputs in the `MetalsharpDirectory` and assigns an appropriate metadata record, but for brevity here is that function:

```c#
static void LeveledFiles(MetalsharpDirectory directory)
{
    foreach (var file in directory.InputFiles.Concat(directory.OutputFiles))
    {
        // Get the directories from the path of the file as an array
        var dirLevels = file.Directory.Split(Path.DirectorySeparatorChar);
        // If one of the directories in the file is ".", omit that one
        // Because Windows will ignore the "." directory
        var dirLevelCount = dirLevels.Count() - (dirLevels[0] == "." ? 1 : 0);
        // Add the count to the metadata of the file
        file.Metadata.Add("level", dirLevelCount);
    }
}
```

Now we have our method to achieve this, we just need to plug it in to our Metalsharp project. Recall from above where we used `MetalsharpDirectory.Use` to invoke a function on the project - we can do exactly the same here with this method. Becuase the method has the same inputs and outputs as an `Action<MetalsharpDirectory>`, we can use it as a delegate here:

```c#
directory.Use(LeveledFiles);
```

And now, we are ready to hook up our templates. Again, it bears mentioning that this tutorial is covering specifics for the `Metalsharp.FluidTemplate` library, and you may have a slightly different experience if you're using a different system for your templates.

If you've been reading closely, you'll notice that only four of our files have a metadata record keyed "template": each of the three posts, which specify `article.liquid`, and the blog page, which specifies `blog.liquid`. The other three pages - index, about, and contact, do not need one. This is because there is a slight distinction between a *template* and a *layout*. A *template* is specified in the metadata of the file, while a *layout* is specified in our Metalsharp code when we invoke the `FluidTemplate` plugin. If a file specifies a template, then the template is applied first. After the templates are applied and if a layout is specified, then the layout is applied to every file.

Thus, we only need the following addition to our Metalsharp code:

```c#
directory.UseFluidTemplate(".\\Templates\\layout.liquid");
```

### Building the Site

Now we're done doing the processing we need to with Metalsharp, we can build the site! But we have a couple requirements: we want to build the site to a `build` directory, and every time we build the site we want to remove all the files in that directory before we write any files there. The `BuildOptions` class handles these options for us, and we can provide an instance of this class, with these options configured, to the `MetalsharpDirectory.Build` method.

```c#
directory.Build(new BuildOptions
{
    OutputDirectory = "build",
    ClearOutputDirectory = true
});
```

### Conclusion

To reiterate, if you've been following along (and assuming you opted to use the fluent interface), your code probably looks similar to the following:

```c#
namespace MyProject
{
    using Metalsharp;
    using System.Linq;
    using Metalsharp.FluidTemplate;
    using System.IO;
    using System.Collections.Generic;
    using System;

    class Program
    {
        static void Main(string[] args) =>
            new MetalsharpDirectory("Site", ".")
                .AddInput("Templates")
                .AddOutput("Static", ".")
                .UseFrontmatter()
                .UseMarkdown()
                .UseCollections("posts", file => file.IsChildOf("Posts"))
                .Use(dir =>
                    dir.GetOutputFilesFromCollection("posts-collection").ToList()
                    .ForEach(file => file.Metadata.Add("template", ".\\Templates\\article.liquid")))
                .Use(dir => dir.AddOutput("", "blog.html")
                {
                    Metadata = new Dictionary<string, object>()
                    {
                        ["posts"] = dir.GetOutputFilesFromList("posts").Select(file => new Post(file)),
                        ["template"] = ".\\Templates\\blog.liquid"
                    }
                })
                .Use(LeveledFiles)
                .UseFluidTemplate(".\\Templates\\layout.liquid")
                .Build(new BuildOptions
                {
                    OutputDirectory = "build",
                    ClearOutputDirectory = true
                });

        static void LeveledFiles(MetalsharpDirectory directory)
        {
            foreach (var file in directory.InputFiles.Concat(directory.OutputFiles))
            {
                var dirLevels = file.Directory.Split(Path.DirectorySeparatorChar);
                var dirLevelCount = dirLevels.Count() - (dirLevels[0] == "." ? 1 : 0);
                file.Metadata.Add("level", dirLevelCount);
            }
        }
    }

    class Post
    {
        public Post(MetalsharpFile file)
        {
            Title = file.Metadata["title"] as string;
            Date = file.Metadata["date"] as string;
            Description = file.Metadata["description"] as string;
            Author = file.Metadata["author"] as string;
            Slug = file.Metadata["slug"] as string;
        }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Slug { get; set; }
    }
}
```

---

Now you're all set - your site is up and running! Congrats!

Did you write a plugin to help you generate your site? If you did, you may want to consider releasing it. [This tutorial](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md) explains how to develop and release your own plugin.

Did you notice something odd with this tutorial - a typo, old information, or just something that could make it a bit better? [Editing this tutorial](https://github.com/IanWold/Metalsharp/edit/master/Metalsharp.Documentation/tutorial-website.md) and submitting a PR would be a great way to contribute to Metalsharp!
