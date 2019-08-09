using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.CMake.Tests.Fixtures
{
  public class CMakeBuildRunnerFixture : ToolFixture<CMakeBuildSettings>
  {
    public CMakeBuildRunnerFixture(FilePath toolPath = null, bool defaultToolExist = true)
      : base("/Working/tools/cmake.exe")
    {
      if (defaultToolExist)
      {
        this.FileSystem.CreateFile("/Working/tools/cmake.exe");
      }

      if (toolPath != null)
      {
        this.FileSystem.CreateFile(toolPath);
      }

      this.Settings.BinaryPath = "./bin";
      this.Environment = new FakeEnvironment(PlatformFamily.Windows)
      {
        WorkingDirectory = "/Working"
      };
    }

    protected override void RunTool()
    {
      var runner = new CMakeBuildRunner(this.FileSystem, this.Environment, this.ProcessRunner, this.Tools);
      runner.Run(this.Settings);
    }
  }
}