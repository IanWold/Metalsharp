# Examples

These examples demonstrate runnable Metalsharp projects, each a single [.NET file-based app](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview).

## `simple-website/`

The core Metalsharp workflow with nothing else added: `Frontmatter` reads each page's title out of its front matter, `Markdown` converts the page body to HTML, and a plain function wraps the result in a bare HTML shell using that title.

```plaintext
cd examples/simple-website
dotnet run app.cs
```

## `blog/`

A small blog built with two published Metalsharp plugins: [`Metalsharp.SimpleBlog`](https://github.com/IanWold/Metalsharp.SimpleBlog) collects and sorts the posts in `posts/` by their front-matter `date`, and [`Metalsharp.LiquidTemplates`](https://github.com/IanWold/Metalsharp.LiquidTemplates) renders the blog index and each post through the Liquid templates in `templates/`. `static/` holds a stylesheet that's copied straight through to the output rather than processed by any plugin.

Posts sit one directory below the blog index (`posts/some-post.html` vs. `blog.html`), so their links back to the index and to `static/style.css` can't just be the same relative path everywhere. The built-in `Leveller` plugin adds a `level` metadata field to every file (how many directories deep it is) and `templates/layout.liquid` uses that to compute the right number of `../` for each file, however deep it ends up being.

```plaintext
cd examples/blog
dotnet run app.cs
```

## Trying the output

Run the build script for any of the examples:

```plaintext
cd examples/<directory>
dotnet run app.cs
```

The result will be built to `examples/<directory>/output`.
