namespace Metalsharp.Tests
{
    public class TestPlugin : IMetalsharpPlugin
    {
        public void Execute(MetalsharpDirectory directory) =>
            directory.Meta("test", true);
    }
}
