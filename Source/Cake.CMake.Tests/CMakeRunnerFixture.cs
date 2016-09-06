using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.CMake.Tests
{
    public sealed class CMakeRunnerFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IGlobber Globber { get; set; }

        public DirectoryPath SourcePath { get; set; }
        public CMakeSettings Settings { get; set; }

        public CMakeRunnerFixture(FilePath toolPath = null, bool defaultToolExist = true)
        {
            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";
            Environment.IsUnix().Returns(false);

            Globber = Substitute.For<IGlobber>();
            FileSystem = Substitute.For<IFileSystem>();

            if (defaultToolExist)
            {
                Globber.Match("./tools/**/cmake.exe").Returns(new[] { (FilePath)"/Working/tools/cmake.exe" });
                FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == "/Working/tools/cmake.exe")).Returns(true);
            }

            if (toolPath != null)
            {
                FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == toolPath.FullPath)).Returns(true);
            }

            SourcePath = "./source";
            Settings = new CMakeSettings();
        }

        public void Run()
        {
            var runner = new CMakeRunner(FileSystem, Environment, ProcessRunner, Globber);
            runner.Run(SourcePath, Settings);
        }
    }
}
