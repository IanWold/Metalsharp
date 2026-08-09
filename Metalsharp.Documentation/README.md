# Metalsharp Documentation

## Tutorials

New to Metalsharp? Start here.

* [Quickstart](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md) — a tour of the core API.
* [Create a Website with Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md) — build a complete site as a single-file C# app.
* [Create a Plugin for Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md) — develop and publish your own plugin.

## API Reference

* [Generated API Documentation](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/api.md) — a full reference generated from the XML comments in the source.

`api.md` is generated, not hand-written — don't edit it directly. If you spot an inaccuracy, fix the corresponding XML doc comment in `Metalsharp/`, then regenerate from the repo root:

```plaintext
dotnet run Metalsharp.Documentation/GenerateApiDoc.cs
```
