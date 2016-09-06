using System;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using System.Collections.Generic;

namespace Cake.CMake.Tests
{
    public sealed class CMakeRunnerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new CMakeRunnerFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.NotNull(result);
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new CMakeRunnerFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.NotNull(result);
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_ProcessRunner_Is_Null()
            {
                // Given
                var fixture = new CMakeRunnerFixture();
                fixture.ProcessRunner = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.NotNull(result);
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("processRunner", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Globber_Is_Null()
            {
                // Given
                var fixture = new CMakeRunnerFixture();
                fixture.Globber = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.NotNull(result);
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("globber", ((ArgumentNullException)result).ParamName);
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Source_Path_Is_Null()
            {
                // Given
                var fixture = new CMakeRunnerFixture();
                fixture.SourcePath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.NotNull(result);
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("sourcePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new CMakeRunnerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.NotNull(result);
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
            }

            [Theory]
            [InlineData("C:/cmake/cmake.exe", "C:/cmake/cmake.exe")]
            [InlineData("./tools/cmake/cmake.exe", "/Working/tools/cmake/cmake.exe")]
            public void Should_Use_CMake_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new CMakeRunnerFixture(toolPath: expected);
                fixture.Settings.ToolPath = toolPath;

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Look_In_Program_Files()
            {
                // Given
                var fixture = new CMakeRunnerFixture(defaultToolExist: false);
                fixture.FileSystem.Exist(
                    Arg.Is<FilePath>(p => p.FullPath == "/ProgramFilesX86/cmake/bin/cmake.exe")).Returns(true);
                fixture.Environment.GetSpecialPath(
                    Arg.Is<SpecialPath>(p => p == SpecialPath.ProgramFilesX86)).Returns(info => "/ProgramFilesX86");

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/ProgramFilesX86/cmake/bin/cmake.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Append_Source_Directory_To_Arguments()
            {
                // Given
                var fixture = new CMakeRunnerFixture();

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == "\"/Working/source\""));
            }

            [Fact]
            public void Should_Set_Working_Directory_To_Source_Path_If_No_Output_Directory_Specified()
            {
                // Given
                var fixture = new CMakeRunnerFixture();

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.WorkingDirectory.FullPath == "/Working/source"));
            }

            [Fact]
            public void Should_Set_Working_Directory_To_Output_Path_If_Set()
            {
                // Given
                var fixture = new CMakeRunnerFixture();
                fixture.Settings.OutputPath = "./build";

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.WorkingDirectory.FullPath == "/Working/build"));
            }

            [Fact]
            public void Should_Append_Generator_To_Arguments()
            {
                // Given
                var fixture = new CMakeRunnerFixture();
                fixture.Settings.Generator = "cool_generator";

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == "\"/Working/source\" -G \"cool_generator\""));
            }

            [Fact]
            public void Should_Append_Toolset_To_Arguments()
            {
                // Given
                var fixture = new CMakeRunnerFixture();
                fixture.Settings.Toolset = "cool_toolset";

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == "\"/Working/source\" -T \"cool_toolset\""));
            }

            [Fact]
            public void Should_Append_Platform_To_Arguments()
            {
                // Given
                var fixture = new CMakeRunnerFixture();
                fixture.Settings.Platform = "x64";

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == "\"/Working/source\" -A \"x64\""));
            }

            [Fact]
            public void Should_Append_Options_To_Arguments()
            {
                // Given
                var fixture = new CMakeRunnerFixture();
                fixture.Settings.Options = new List<string> { "-DCMAKE_IS_COOL", "-DCAKE_IS_COOL" };

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == "\"/Working/source\" \"-DCMAKE_IS_COOL\" \"-DCAKE_IS_COOL\""));

            }
        }
    }
}
