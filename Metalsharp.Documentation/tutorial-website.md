# Create a Website with Metalsharp

This tutorial walks through (almost) all the pieces of Metalsharp you're likely to use in the regular course of building a website. We'll build a small personal site with a homepage, a couple of static pages, and a blog — using nothing but the core Metalsharp library and a single C# file.

We'll write the whole project as a [.NET file-based app](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview): one `.cs` file, run directly with `dotnet run`, no `.csproj` or solution required. This is a great fit for a static site generator — you get a real, compiled C# program with full IntelliSense and no build ceremony to maintain.

First, we'll walk through adding each of the site's pages and the reasoning behind the layout. Then we'll write the Metalsharp program that turns them into a website.

## Content

* [Project Files](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#project-files)
  * [Website Content](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#website-content)
  * [Static Content](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#static-content)
* [Build it with Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#build-it-with-metalsharp)
  * [Starting the Script](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#starting-the-script)
  * [Adding Files](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#adding-files)
  * [Using our First Plugins](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#using-our-first-plugins)
  * [Handling the Blog Posts](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#handling-the-blog-posts)
  * [Generating the Blog Page](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#generating-the-blog-page)
  * [Rendering the Layout](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#rendering-the-layout)
  * [Building the Site](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#building-the-site)
  * [Conclusion](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md#conclusion)

## Project Files

### Website Content

We'll write our website content in Markdown. The first file we need is `index.md`, which Metalsharp will translate to `index.html`. Let's create a project folder — we'll call it `MyProject` — and inside it a `Site` folder to hold all of the site's content. That's where `index.md` will go.

```plaintext
MyProject
└── Site
    └── index.md
```

Write whatever you like in `index.md`. Let's also add an `about.md` page and a `contact.md` page:

```plaintext
MyProject
└── Site
    ├── about.md
    ├── contact.md
    └── index.md
```

Now let's put our blog posts in their own subdirectory. This makes them easy to categorize, and just as importantly, easy to select as a group once we start writing Metalsharp code. Each post will also carry some frontmatter — an author, a date, and a title are natural fits for metadata rather than page content. Here's an example post:

> If you're unfamiliar with frontmatter, it's a common convention for attaching metadata to a content file, popularized by tools like [Jekyll](https://jekyllrb.com/docs/front-matter/). Metalsharp's `Frontmatter` plugin understands both YAML and JSON frontmatter.

```markdown
---
author: Some Body
date: 2024-01-15
title: My First Post
description: The first post on my new blog
---

# This Is My First Post

How exciting!
```

> Using an ISO 8601 date (`YYYY-MM-DD`) keeps dates sortable as plain strings, which is what we'll do later in this tutorial.

Let's add this post and a couple of others to a `Posts` folder:

```plaintext
MyProject
└── Site
    ├── Posts
    │   ├── my-first-post.md
    │   └── i-love-blogging.md
    ├── about.md
    ├── contact.md
    └── index.md
```

That covers all the content we need to write by hand. Notice there's no `blog.md` — we'll generate that page from the posts themselves. This way, adding a new post is as simple as dropping a file into `Posts`; nothing else needs to change.

### Static Content

Some files — a stylesheet, for example — shouldn't be touched at all; they should just be copied straight to the output. Let's add a `Static` folder alongside `Site` for exactly that. For now we just need a `style.css`:

```plaintext
MyProject
├── Site
│   ├── Posts
│   │   ├── my-first-post.md
│   │   └── i-love-blogging.md
│   ├── about.md
│   ├── contact.md
│   └── index.md
└── Static
    └── style.css
```

Write whatever styling you like — this tutorial doesn't depend on any particular CSS.

This concludes the content for the site itself. All that's left is the Metalsharp program that builds it, which we'll write as a single `app.cs` file at the root of `MyProject`.

```plaintext
MyProject
├── Site
│   └── ...
├── Static
│   └── style.css
└── app.cs
```

## Build it with Metalsharp

A [file-based app](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview) is just a C# file with top-level statements — the same code you'd put in `Program.cs` in a console project — plus an optional set of directives at the top of the file for things like package references. `dotnet run app.cs` compiles and runs it directly, with no project file needed.

To pull in Metalsharp, add a `#:package` directive at the top of `app.cs`:

```c#
#:package Metalsharp@1.0.1
```

This tutorial will build up `app.cs` piece by piece. Each step of the way, this tutorial will explain a bit more of the Metalsharp API — the full generated API reference is [available here](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/api.md).

### Starting the Script

Let's start with the package directive and the `using` statements we'll need throughout:

```c#
#:package Metalsharp@1.0.1

using Metalsharp;
using System.Text;
```

`System.Text` is needed later, for `Encoding`, when we rewrite a file's contents.

### Adding Files

> This tutorial uses Windows-style directory separators (`\`) in strings passed to Metalsharp, since Metalsharp accepts whatever separator you give it and stores it as-is in a file's virtual path. If you'd rather write platform-independent code, use `Path.DirectorySeparatorChar` or `Path.Combine` instead of a literal `\` or `/`.

First, we need a `MetalsharpProject` with our site content read in. We also know we want to build to a `build` directory and clear it out on every run, and both of those are configured when the project is constructed:

```c#
var project = new MetalsharpProject(clearOutputDirectory: true, outputDirectory: "build")
    .AddInput("Site", ".");
```

`AddInput`'s second argument is the virtual directory Metalsharp should use for the files it reads — here, `.`, so the contents of `Site` land at the root of our virtual file list instead of nested inside a `Site` directory. Once this runs, our input list looks like this:

- `.\about.md`
- `.\contact.md`
- `.\index.md`
- `.\Posts\my-first-post.md`
- `.\Posts\i-love-blogging.md`

This virtual directory structure mirrors the one on disk, but it's worth remembering that it's just that — virtual. `MetalsharpFile` objects aren't tied to real files on disk; they simply carry a `FilePath` that Metalsharp uses when it eventually writes output.

It's important to note that `MetalsharpProject` keeps two lists of files: `InputFiles` and `OutputFiles`. Only files in `OutputFiles` are written to disk when we build. Reading a directory into the project (via `AddInput`) only ever populates the input list — this gives us a staging area, so we can decide precisely which files make it into the output and in what shape.

Next, let's add `Static`. Since we want those files copied straight through, they go directly into the output list with `AddOutput`:

```c#
project.AddOutput("Static", ".");
```

Now that we've added our files, it's worth knowing that `MetalsharpProject` also has methods to move or remove files after the fact, in case you need to restructure things mid-pipeline. We won't need them for this project, but here's what they look like:

```c#
project
    // Move files from the Posts directory to an Articles directory
    .MoveInput(".\\Posts", ".\\Articles")
    .MoveOutput(".\\Posts", ".\\Articles")

    // Remove any .txt files
    .RemoveInput(file => file.Extension == ".txt")
    .RemoveOutput(file => file.Extension == ".txt");
```

### Using our First Plugins

With our files in place, it's time to start processing them. Let's run two plugins:

```c#
project
    .UseFrontmatter()
    .UseMarkdown();
```

`Frontmatter` parses each file's frontmatter and merges it into that file's metadata. `Markdown` converts Markdown files in the input into HTML files in the output. The order matters here: when `Markdown` generates an HTML file, it copies over the metadata that was already on the source file — so `Frontmatter` needs to run first.

> *Metadata* is any data associated with a file that isn't part of the file's own text. `MetalsharpFile.Metadata` is a plain `Dictionary<string, object>`.

There's more than one way to invoke a plugin. Above, we used the extension methods that ship alongside each plugin. Every plugin is a class implementing `IMetalsharpPlugin`, so you can also invoke one by referencing its type:

```c#
project
    .Use<Frontmatter>()
    .Use<Markdown>();
```

This only works for plugins with a public parameterless constructor. What both of the forms above actually do is construct an instance of the plugin and call its `Execute` method — you can do that yourself directly, too:

```c#
project
    .Use(new Frontmatter())
    .Use(new Markdown());
```

None of these forms is more "correct" than the others; pick whichever reads best to you. A reasonable rule of thumb: invoking by instance is the most explicit and consistent way, since every plugin is a class either way, while an extension method may read more naturally because the plugin author had the chance to design a purpose-built API around it.

After running `Frontmatter` and `Markdown`, our output list looks like this, while the input remains untouched:

- `.\about.html`
- `.\contact.html`
- `.\index.html`
- `.\Posts\my-first-post.html`
- `.\Posts\i-love-blogging.html`
- `.\style.css` *(copied through from `Static`)*

### Handling the Blog Posts

Just as each file carries its own metadata, `MetalsharpProject` carries project-level metadata to pass information between plugins. The `Collections` plugin uses this to group related files — in both the input and output lists — under a name you choose. We'll use it to gather our blog posts.

```c#
project.UseCollections("posts", file => file.IsChildOf("Posts"));
```

`IsChildOf` checks whether a file's immediate parent directory matches the one given — so this selects every file directly inside `Posts`, in both the input and output lists, and stores them in the project's metadata under the key `"collections"`. If you wanted to reach in and grab this yourself, you could:

```c#
// Getting at your collection - the hard way
var postsCollection = project.Metadata["collections"] as Dictionary<string, Dictionary<string, string[]>>;
var postPaths = postsCollection["posts"]["output"];
```

But Metalsharp gives us a much friendlier way to do this:

```c#
var posts = project.GetOutputFilesFromCollection("posts")
    .OrderByDescending(file => (string)file.Metadata["date"])
    .ToList();
```

`GetOutputFilesFromCollection` handles the lookup and casting for us and hands back the actual `MetalsharpFile` objects from the `"posts"` collection's output list. There's also `GetInputFilesFromCollection` and `GetFilesFromCollection`, for the input files and all files respectively. Since our post dates are plain ISO 8601 strings, sorting them as strings sorts them chronologically for free.

### Generating the Blog Page

Now let's build `blog.html` from the list of posts we just gathered. We'll write a small function that renders a summary for each post, and use it to build the page's content:

```c#
string RenderPostList(IEnumerable<MetalsharpFile> posts) =>
    string.Join("\n", posts.Select(post => $"""
        <article>
            <h2><a href="Posts/{post.Name}.html">{post.Metadata["title"]}</a></h2>
            <p>{post.Metadata["date"]} &mdash; {post.Metadata["author"]}</p>
            <p>{post.Metadata["description"]}</p>
        </article>
        """));
```

Then we add `blog.html` to the output list ourselves, using `MetalsharpFile`'s metadata constructor to attach a title:

```c#
project.AddOutput(new MetalsharpFile(
    RenderPostList(posts),
    Path.Combine(".", "blog.html"),
    new Dictionary<string, object> { ["title"] = "Blog" }
));
```

> We build the path with `Path.Combine(".", "blog.html")` rather than the literal string `"blog.html"` so that the file's virtual directory is `.`, matching the rest of our root-level pages. This matters in the next step, where we compute how deep each page sits in the site.

### Rendering the Layout

We now have five HTML files in the output — `index`, `about`, `contact`, `blog`, and two posts — and none of them look like a real page yet; they're just fragments of body content. Let's wrap all of them in a shared layout.

Two of our pages (the posts) are nested one directory deep, inside `Posts`, so their links back to the rest of the site need a `../` prefix that the root-level pages don't need. Metalsharp's `Leveller` plugin computes exactly this: it adds a `level` metadata record to every file, counting how many directories deep it sits.

```c#
project.UseLeveller();
```

Now we can write a function that wraps every HTML file's content in a shared page shell, using each file's `level` to compute the right number of `../` segments:

```c#
void RenderLayout(MetalsharpProject proj)
{
    foreach (var file in proj.OutputFiles.Where(f => f.Extension == ".html"))
    {
        var depth = (int)file.Metadata["level"];
        var relativeRoot = string.Concat(Enumerable.Repeat("../", depth));
        var title = file.Metadata.TryGetValue("title", out var t) ? t : "MyProject";

        var page = $"""
            <!DOCTYPE html>
            <html>
            <head>
                <title>{title}</title>
                <link rel="stylesheet" href="{relativeRoot}style.css">
            </head>
            <body>
                <nav>
                    <a href="{relativeRoot}index.html">Home</a>
                    <a href="{relativeRoot}about.html">About</a>
                    <a href="{relativeRoot}blog.html">Blog</a>
                    <a href="{relativeRoot}contact.html">Contact</a>
                </nav>
                <main>
                    {file.Text}
                </main>
            </body>
            </html>
            """;

        file.Contents = Encoding.UTF8.GetBytes(page);
    }
}
```

`MetalsharpFile.Text` is a read-only view over `MetalsharpFile.Contents`, so to replace a file's content we assign new bytes back to `Contents` — here, the UTF-8 bytes of our rendered page.

This `RenderLayout` function has exactly the shape `MetalsharpProject.Use` expects — a method taking a single `MetalsharpProject` and returning nothing — so we can invoke it as a plugin without writing a class at all:

```c#
project.Use(RenderLayout);
```

> This tutorial keeps templating deliberately simple so the whole project stays self-contained in one file. If you want a more capable templating story — layout inheritance, partials, and so on — nothing stops you from pulling in a templating library of your choice and calling it from a `Use` function exactly like this one.

### Building the Site

We've already configured our output directory and clear-on-build behavior back when we constructed `project`, so all that's left is to call `Build`:

```c#
project.Build();
```

### Conclusion

Putting it all together, here's the complete `app.cs`:

```c#
#:package Metalsharp@1.0.1

using Metalsharp;
using System.Text;

var project = new MetalsharpProject(clearOutputDirectory: true, outputDirectory: "build")
    .AddInput("Site", ".")
    .AddOutput("Static", ".")
    .UseFrontmatter()
    .UseMarkdown()
    .UseCollections("posts", file => file.IsChildOf("Posts"));

var posts = project.GetOutputFilesFromCollection("posts")
    .OrderByDescending(file => (string)file.Metadata["date"])
    .ToList();

project.AddOutput(new MetalsharpFile(
    RenderPostList(posts),
    Path.Combine(".", "blog.html"),
    new Dictionary<string, object> { ["title"] = "Blog" }
));

project
    .UseLeveller()
    .Use(RenderLayout)
    .Build();

string RenderPostList(IEnumerable<MetalsharpFile> posts) =>
    string.Join("\n", posts.Select(post => $"""
        <article>
            <h2><a href="Posts/{post.Name}.html">{post.Metadata["title"]}</a></h2>
            <p>{post.Metadata["date"]} &mdash; {post.Metadata["author"]}</p>
            <p>{post.Metadata["description"]}</p>
        </article>
        """));

void RenderLayout(MetalsharpProject proj)
{
    foreach (var file in proj.OutputFiles.Where(f => f.Extension == ".html"))
    {
        var depth = (int)file.Metadata["level"];
        var relativeRoot = string.Concat(Enumerable.Repeat("../", depth));
        var title = file.Metadata.TryGetValue("title", out var t) ? t : "MyProject";

        var page = $"""
            <!DOCTYPE html>
            <html>
            <head>
                <title>{title}</title>
                <link rel="stylesheet" href="{relativeRoot}style.css">
            </head>
            <body>
                <nav>
                    <a href="{relativeRoot}index.html">Home</a>
                    <a href="{relativeRoot}about.html">About</a>
                    <a href="{relativeRoot}blog.html">Blog</a>
                    <a href="{relativeRoot}contact.html">Contact</a>
                </nav>
                <main>
                    {file.Text}
                </main>
            </body>
            </html>
            """;

        file.Contents = Encoding.UTF8.GetBytes(page);
    }
}
```

Run it from the `MyProject` directory with:

```plaintext
dotnet run app.cs
```

And your site will be generated in `build`.

---

Now you're all set — your site is up and running! Congrats!

Did you write something you think could be a useful plugin for others? Consider publishing it — [Create a Plugin for Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md) explains how to develop and release one of your own.

Did you notice something odd with this tutorial — a typo, outdated information, or something that could be explained better? [Editing this page](https://github.com/IanWold/Metalsharp/edit/master/Metalsharp.Documentation/tutorial-website.md) and submitting a PR would be a great way to contribute to Metalsharp!
