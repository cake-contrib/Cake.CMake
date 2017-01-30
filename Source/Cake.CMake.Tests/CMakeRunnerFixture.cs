using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.CMake.Tests
{
    public sealed class CMakeRunnerFixture : ToolFixture<CMakeSettings>
    {


        public DirectoryPath SourcePath { get; set; }

        public CMakeRunnerFixture(FilePath toolPath = null, bool defaultToolExist = true) :base("/Working/tools/cmake.exe")
        {
            if (defaultToolExist)
            {
                FileSystem.CreateFile("/Working/tools/cmake.exe");
            }

            if (toolPath != null)
            {
                FileSystem.CreateFile(toolPath);
            }

            SourcePath = "./source";
            Environment = new FakeEnvironment(PlatformFamily.Windows);
            Environment.WorkingDirectory = "/Working";
        }


        protected override void RunTool()
        {
            var runner = new CMakeRunner(FileSystem, Environment, ProcessRunner, Tools);
            runner.Run(SourcePath, Settings);
        }
    }
}
