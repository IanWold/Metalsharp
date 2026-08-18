Add a plugin to the plugins list on the website:

1. Ensure your plugin is available on Github and Nuget,
2. Update the [website/Plugins.json] file to add your plugin to the JSON list,
3. Submit the PR with the file update.

JSON example:

```json
    {
        "Nuget": "Nuget.Package.Name",
        "Github": "GithubUsername/GithubRepoName",
        "Description": "Description that will show on the website."
    }
```