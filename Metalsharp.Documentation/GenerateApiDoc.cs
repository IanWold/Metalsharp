using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

string ScriptPath([System.Runtime.CompilerServices.CallerFilePath] string path = "") => path;

var docsDir = Path.GetDirectoryName(ScriptPath())!;
var repoRoot = Path.GetFullPath(Path.Combine(docsDir, ".."));
var csprojPath = Path.Combine(repoRoot, "Metalsharp", "Metalsharp.csproj");
var apiMdPath = Path.Combine(docsDir, "api.md");

Console.WriteLine("Building Metalsharp with XML documentation generation enabled...");
var build = Process.Start(new ProcessStartInfo("dotnet", $"build \"{csprojPath}\" -p:GenerateDocumentationFile=true -v quiet")
{
    RedirectStandardOutput = false,
    RedirectStandardError = false,
})!;
build.WaitForExit();

if (build.ExitCode != 0)
{
    Console.Error.WriteLine("Build failed; aborting.");
    Environment.Exit(build.ExitCode);
}

var outputDir = Path.Combine(repoRoot, "Metalsharp", "bin", "Debug");
var targetFrameworkDir = Directory.GetDirectories(outputDir).OrderByDescending(d => d).First();
var asmPath = Path.Combine(targetFrameworkDir, "Metalsharp.dll");
var xmlPath = Path.Combine(targetFrameworkDir, "Metalsharp.xml");

var assembly = Assembly.LoadFrom(asmPath);
var doc = XDocument.Load(xmlPath);

var members = doc.Descendants("member")
    .ToDictionary(m => (string)m.Attribute("name")!, m => m);

var sb = new StringBuilder();
sb.AppendLine("# Metalsharp API Documentation");
sb.AppendLine();
sb.AppendLine("This file is generated from the XML documentation comments in the Metalsharp source. If you notice an inaccuracy here, please fix the corresponding XML comment in the source and regenerate this file, rather than editing it directly.");
sb.AppendLine();

var types = assembly.GetExportedTypes()
    .Where(t => t.Namespace == "Metalsharp")
    .OrderBy(t => t.Name, StringComparer.Ordinal)
    .ToList();

foreach (var type in types)
{
    WriteType(type);
}

var final = Regex.Replace(sb.ToString(), @"\n{3,}", "\n\n");
File.WriteAllText(apiMdPath, final);
Console.WriteLine($"Wrote {apiMdPath} with {types.Count} types.");

void WriteType(Type type)
{
    sb.AppendLine($"## {type.Name}");
    sb.AppendLine();

    var typeId = "T:" + type.FullName;
    if (members.TryGetValue(typeId, out var typeDoc))
    {
        WriteDocBody(typeDoc);
    }

    if (type.IsEnum)
    {
        WriteEnumFields(type);
        return;
    }

    var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
        .Where(c => !IsCompilerGenerated(c))
        .ToList();

    if (ctors.Count > 0)
    {
        sb.AppendLine("### Constructors");
        sb.AppendLine();

        foreach (var ctor in ctors.OrderBy(c => c.GetParameters().Length))
        {
            WriteMember(MemberId(ctor), $"{type.Name}({ParamList(ctor.GetParameters())})");
        }
    }

    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .Where(m => !m.IsSpecialName && !IsCompilerGenerated(m))
        .OrderBy(m => m.Name, StringComparer.Ordinal)
        .ToList();

    if (methods.Count > 0)
    {
        sb.AppendLine("### Methods");
        sb.AppendLine();

        foreach (var method in methods)
        {
            var name = method.IsGenericMethodDefinition
                ? $"{method.Name}<{string.Join(", ", method.GetGenericArguments().Select(a => a.Name))}>({ParamList(method.GetParameters())})"
                : $"{method.Name}({ParamList(method.GetParameters())})";

            WriteMember(MemberId(method), name, returns: HasReturn(method));
        }
    }

    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .OrderBy(p => p.Name, StringComparer.Ordinal)
        .ToList();

    if (properties.Count > 0)
    {
        sb.AppendLine("### Properties");
        sb.AppendLine();

        foreach (var property in properties)
        {
            WriteMember("P:" + type.FullName + "." + property.Name, property.Name);
        }
    }

    var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .Where(f => f.IsLiteral)
        .OrderBy(f => f.Name, StringComparer.Ordinal)
        .ToList();

    if (fields.Count > 0)
    {
        sb.AppendLine("### Constants");
        sb.AppendLine();

        foreach (var field in fields)
        {
            WriteMember("F:" + type.FullName + "." + field.Name, field.Name);
        }
    }

    var events = type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .OrderBy(e => e.Name, StringComparer.Ordinal)
        .ToList();

    if (events.Count > 0)
    {
        sb.AppendLine("### Events");
        sb.AppendLine();

        foreach (var evt in events)
        {
            WriteMember("E:" + type.FullName + "." + evt.Name, evt.Name);
        }
    }
}

void WriteEnumFields(Type type)
{
    sb.AppendLine("### Values");
    sb.AppendLine();

    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
    {
        WriteMember("F:" + type.FullName + "." + field.Name, field.Name);
    }
}

void WriteMember(string id, string heading, bool returns = false)
{
    sb.AppendLine($"### `{heading}`");
    sb.AppendLine();

    if (members.TryGetValue(id, out var memberDoc))
    {
        WriteDocBody(memberDoc, returns);
    }
    else
    {
        sb.AppendLine();
    }
}

void WriteDocBody(XElement member, bool includeReturns = false)
{
    var summary = member.Element("summary");
    if (summary is not null)
    {
        sb.AppendLine(ConvertToMarkdown(summary));
        sb.AppendLine();
    }

    foreach (var param in member.Elements("param"))
    {
        var name = (string)param.Attribute("name")!;
        sb.AppendLine($"- `{name}`: {ConvertToMarkdown(param).Trim()}");
    }

    if (member.Elements("param").Any())
    {
        sb.AppendLine();
    }

    var example = member.Element("example");
    if (example is not null)
    {
        sb.AppendLine(ConvertToMarkdown(example));
        sb.AppendLine();
    }

    var remarks = member.Element("remarks");
    if (remarks is not null)
    {
        sb.AppendLine(ConvertToMarkdown(remarks));
        sb.AppendLine();
    }

    if (includeReturns)
    {
        var returns = member.Element("returns");
        if (returns is not null)
        {
            sb.AppendLine("#### Returns");
            sb.AppendLine();
            sb.AppendLine(ConvertToMarkdown(returns));
            sb.AppendLine();
        }
    }
}

bool HasReturn(MethodInfo method) => method.ReturnType != typeof(void);

bool IsCompilerGenerated(MethodBase method) =>
    method.GetCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>() is not null;

string MemberId(MethodBase method)
{
    var name = method is ConstructorInfo ? "#ctor" : method.Name;
    var typeName = method.DeclaringType!.FullName;
    var parameters = method.GetParameters();

    if (method is MethodInfo { IsGenericMethodDefinition: true } generic)
    {
        name += "``" + generic.GetGenericArguments().Length;
    }

    if (parameters.Length == 0)
    {
        return $"M:{typeName}.{name}";
    }

    var paramTypes = string.Join(",", parameters.Select(p => XmlDocTypeName(p.ParameterType)));
    return $"M:{typeName}.{name}({paramTypes})";
}

string XmlDocTypeName(Type type)
{
    if (type.IsArray)
    {
        return $"{XmlDocTypeName(type.GetElementType()!)}[]";
    }

    if (type.IsGenericType)
    {
        var baseName = type.GetGenericTypeDefinition().FullName!.Split('`')[0];
        var args = string.Join(",", type.GetGenericArguments().Select(XmlDocTypeName));
        return $"{baseName}{{{args}}}";
    }

    return type.FullName ?? type.Name;
}

string ParamList(ParameterInfo[] parameters) =>
    string.Join(", ", parameters.Select(ParamSignature));

string ParamSignature(ParameterInfo p)
{
    var typeName = FriendlyTypeName(p.ParameterType);

    if (!p.HasDefaultValue)
    {
        return typeName;
    }

    var defaultText = p.DefaultValue switch
    {
        null => "null",
        bool b => b ? "true" : "false",
        string s => $"\"{s}\"",
        Enum e => e.ToString(),
        var v => v.ToString()
    };

    return $"{typeName} = {defaultText}";
}

string FriendlyTypeName(Type type)
{
    if (type.IsArray)
    {
        return $"{FriendlyTypeName(type.GetElementType()!)}[]";
    }

    if (type.IsGenericType)
    {
        var name = type.Name.Split('`')[0];
        var args = string.Join(", ", type.GetGenericArguments().Select(FriendlyTypeName));
        return $"{name}<{args}>";
    }

    return type.Name;
}

string ConvertToMarkdown(XElement element)
{
    var codeBlocks = new List<string>();

    string RenderTopLevel(XNode node)
    {
        if (node is XElement el && el.Name.LocalName == "code")
        {
            codeBlocks.Add(NodeToString(el).Trim('\n'));
            return $"\n\n~~~CODEBLOCK{codeBlocks.Count - 1}~~~\n\n";
        }

        return NodeToString(node);
    }

    var raw = string.Concat(element.Nodes().Select(RenderTopLevel));
    var dedented = Dedent(raw);

    dedented = Regex.Replace(dedented, @"~~~CODEBLOCK(\d+)~~~", m => codeBlocks[int.Parse(m.Groups[1].Value)]);

    return dedented;
}

string Dedent(string raw)
{
    var lines = raw.Split('\n').Select(l => l.TrimEnd()).ToList();

    while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[0])) lines.RemoveAt(0);
    while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[^1])) lines.RemoveAt(lines.Count - 1);

    var minIndent = lines.Where(l => !string.IsNullOrWhiteSpace(l) && !Regex.IsMatch(l.Trim(), @"^~~~CODEBLOCK\d+~~~$"))
        .Select(l => l.Length - l.TrimStart().Length)
        .DefaultIfEmpty(0)
        .Min();

    var trimmed = lines.Select(l =>
    {
        var actualIndent = Math.Min(minIndent, l.Length - l.TrimStart().Length);
        return l[actualIndent..];
    });

    return string.Join("\n", trimmed);
}

string NodeToString(XNode node) => node switch
{
    XText text => text.Value,
    XElement element => ElementToString(element),
    _ => string.Empty
};

string ElementToString(XElement element)
{
    var inner = string.Concat(element.Nodes().Select(NodeToString));
    return element.Name.LocalName switch
    {
        "c" => $"`{inner.Trim()}`",
        "code" => $"\n```c#\n{Dedent(inner)}\n```\n",
        "see" => element.Attribute("href") is { } href
            ? $"[{inner.Trim()}]({href.Value})"
            : element.Attribute("cref") is { } cref
                ? $"`{CrefToText(cref.Value)}`"
                : inner,
        "paramref" => $"`{(string?)element.Attribute("name")}`",
        "para" => $"\n\n{inner}\n\n",
        _ => inner
    };
}

string CrefToText(string cref)
{
    var name = cref.Contains(':') ? cref[(cref.IndexOf(':') + 1)..] : cref;
    var parenIndex = name.IndexOf('(');
    var nameOnly = parenIndex >= 0 ? name[..parenIndex] : name;
    var lastDot = nameOnly.LastIndexOf('.');

    return lastDot >= 0 ? nameOnly[(lastDot + 1)..] : nameOnly;
}
