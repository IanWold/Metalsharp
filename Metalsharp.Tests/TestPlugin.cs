namespace Metalsharp.Tests
{
    public class TestPlugin : IMetalsharpPlugin
    {
        public void Execute(MetalsharpProject project) =>
            project.Meta("test", true);
    }
}
