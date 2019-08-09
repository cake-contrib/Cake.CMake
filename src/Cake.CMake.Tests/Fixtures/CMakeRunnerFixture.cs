using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.CMake.Tests.Fixtures
{
  public sealed class CMakeRunnerFixture : ToolFixture<CMakeSettings>
  {
    public CMakeRunnerFixture(FilePath toolPath = null, bool defaultToolExist = true)
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

      this.Settings.SourcePath = "./source";
      this.Environment = new FakeEnvironment(PlatformFamily.Windows)
      {
        WorkingDirectory = "/Working"
      };
    }

    protected override void RunTool()
    {
      var runner = new CMakeRunner(this.FileSystem, this.Environment, this.ProcessRunner, this.Tools, new NullLog());
      runner.Run(this.Settings);
    }
  }
}
