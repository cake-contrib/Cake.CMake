using System;
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.CMake.Tests
{
  public sealed class CMakeRunnerTests
  {
    public sealed class TheConstructor
    {
      [Fact]
      public void ShouldThrowIfFileSystemIsNull()
      {
        // Given
        var fixture = new CMakeRunnerFixture
        {
          FileSystem = null
        };

        // When
        var result = Record.Exception(() => fixture.Run());

        // Then
        Assert.NotNull(result);
        Assert.IsType<ArgumentNullException>(result);
        Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
      }

      [Fact]
      public void ShouldThrowIfEnvironmentIsNull()
      {
        // Given
        var fixture = new CMakeRunnerFixture
        {
          Environment = null
        };

        // When
        var result = Record.Exception(() => fixture.Run());

        // Then
        Assert.NotNull(result);
        Assert.IsType<ArgumentNullException>(result);
        Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
      }

      [Fact]
      public void ShouldThrowIfProcessRunnerIsNull()
      {
        // Given
        var fixture = new CMakeRunnerFixture
        {
          ProcessRunner = null
        };

        // When
        var result = Record.Exception(() => fixture.Run());

        // Then
        Assert.NotNull(result);
        Assert.IsType<ArgumentNullException>(result);
        Assert.Equal("processRunner", ((ArgumentNullException)result).ParamName);
      }

      [Fact]
      public void ShouldThrowIfGlobberIsNull()
      {
        // Given
        var fixture = new CMakeRunnerFixture();
        fixture.Tools = null;

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
      public void ShouldThrowIfSourcePathIsNull()
      {
        // Given
        var fixture = new CMakeRunnerFixture
        {
          SourcePath = null
        };

        // When
        var result = Record.Exception(() => fixture.Run());

        // Then
        Assert.NotNull(result);
        Assert.IsType<ArgumentNullException>(result);
        Assert.Equal("sourcePath", ((ArgumentNullException)result).ParamName);
      }

      [Fact]
      public void ShouldThrowIfSettingsAreNull()
      {
        // Given
        var fixture = new CMakeRunnerFixture
        {
          Settings = null
        };

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
      public void ShouldUseCMakeExecutableFromToolPathIfProvided(string toolPath, string expected)
      {
        // Given
        var fixture = new CMakeRunnerFixture(toolPath: expected);
        fixture.Settings.ToolPath = toolPath;

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(expected, result.Path.FullPath);
      }

      [Fact]
      public void ShouldLookInProgramFiles()
      {
        // Given
        var fixture = new CMakeRunnerFixture(defaultToolExist: false);
        const string Expected = "/ProgramFilesX86/cmake/bin/cmake.exe";
        fixture.FileSystem.CreateFile(Expected);
        fixture.Environment.SetSpecialPath(SpecialPath.ProgramFilesX86, "/ProgramFilesX86");

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Path.FullPath);
      }

      [Fact]
      public void ShouldAppendSourceDirectoryToArguments()
      {
        // Given
        const string Expected = "-S \"/Working/source\"";
        var fixture = new CMakeRunnerFixture();

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Args);
      }

      [Fact]
      public void ShouldSetWorkingDirectoryToSourcePathIfNoOutputDirectorySpecified()
      {
        // Given
        const string Expected = "/Working/source";
        var fixture = new CMakeRunnerFixture();

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Process.WorkingDirectory.FullPath);
      }

      [Fact]
      public void ShouldSetWorkingDirectoryToOutputPathIfSet()
      {
        // Given
        const string Expected = "/Working/build";
        var fixture = new CMakeRunnerFixture();
        fixture.Settings.OutputPath = "./build";

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Process.WorkingDirectory.FullPath);
      }

      [Fact]
      public void ShouldAppendGeneratorToArguments()
      {
        // Given
        const string Expected = "-S \"/Working/source\" -G \"cool_generator\"";
        var fixture = new CMakeRunnerFixture();
        fixture.Settings.Generator = "cool_generator";

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Args);
      }

      [Fact]
      public void ShouldAppendToolsetToArguments()
      {
        // Given
        const string Expected = "-S \"/Working/source\" -T \"cool_toolset\"";
        var fixture = new CMakeRunnerFixture();
        fixture.Settings.Toolset = "cool_toolset";

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Args);
      }

      [Fact]
      public void ShouldAppendPlatformToArguments()
      {
        // Given
        const string Expected = "-S \"/Working/source\" -A \"x64\"";
        var fixture = new CMakeRunnerFixture();
        fixture.Settings.Platform = "x64";

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Args);
      }

      [Fact]
      public void ShouldAppendOptionsToArguments()
      {
        // Given
        const string Expected = "-S \"/Working/source\" \"-DCMAKE_IS_COOL\" \"-DCAKE_IS_COOL\"";
        var fixture = new CMakeRunnerFixture();
        fixture.Settings.Options = new List<string> { "-DCMAKE_IS_COOL", "-DCAKE_IS_COOL" };

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Args);
      }
    }
  }
}
