using System;
using Cake.CMake.Tests.Fixtures;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.CMake.Tests
{
  public class CMakeBuildRunnerTests
  {
    public sealed class TheConstructor
    {
      [Fact]
      public void ShouldThrowIfFileSystemIsNull()
      {
        // Given
        var fixture = new CMakeBuildRunnerFixture
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
        var fixture = new CMakeBuildRunnerFixture
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
        var fixture = new CMakeBuildRunnerFixture
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
        var fixture = new CMakeBuildRunnerFixture
        {
          Tools = null
        };

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
      public void ShouldThrowIfBinaryPathIsNull()
      {
        // Given
        var fixture = new CMakeBuildRunnerFixture
        {
          Settings =
          {
            BinaryPath = null
          }
        };

        // When
        var result = Record.Exception(() => fixture.Run());

        // Then
        Assert.NotNull(result);
        Assert.IsType<ArgumentNullException>(result);
        Assert.Equal(nameof(CMakeBuildSettings.BinaryPath), ((ArgumentException)result).ParamName);
      }

      [Fact]
      public void ShouldThrowIfSettingsAreNull()
      {
        // Given
        var fixture = new CMakeBuildRunnerFixture
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
        var fixture = new CMakeBuildRunnerFixture(toolPath: expected)
        {
          Settings =
          {
            ToolPath = toolPath
          }
        };

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(expected, result.Path.FullPath);
      }

      [Fact]
      public void ShouldLookInProgramFiles()
      {
        // Given
        var fixture = new CMakeBuildRunnerFixture(defaultToolExist: false);
        const string Expected = "/ProgramFilesX86/cmake/bin/cmake.exe";
        fixture.FileSystem.CreateFile(Expected);
        fixture.Environment.SetSpecialPath(SpecialPath.ProgramFilesX86, "/ProgramFilesX86");

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Path.FullPath);
      }

      [Fact]
      public void ShouldAppendBinaryDirectoryToArguments()
      {
        // Given
        const string Expected = "--build \"/Working/bin\"";
        var fixture = new CMakeBuildRunnerFixture();

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Args);
      }

      [Fact]
      public void ShouldSetWorkingDirectoryToBinaryPathIfNoOutputDirectorySpecified()
      {
        // Given
        const string Expected = "/Working/bin";
        var fixture = new CMakeBuildRunnerFixture();

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Process.WorkingDirectory.FullPath);
      }

      [Fact]
      public void ShouldSetWorkingDirectoryToBinaryPathIfSet()
      {
        // Given
        const string Expected = "/Working/build";
        var fixture = new CMakeBuildRunnerFixture
        {
          Settings =
          {
            BinaryPath = "./build"
          }
        };

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Process.WorkingDirectory.FullPath);
      }

      [Fact]
      public void ShouldAppendCleanFirstToArguments()
      {
        // Given
        const string Expected = "--build \"/Working/bin\" --clean-first";
        var fixture = new CMakeBuildRunnerFixture
        {
          Settings =
          {
            CleanFirst = true
          }
        };

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Args);
      }

      [Fact]
      public void ShouldAppendTargetsToArguments()
      {
        // Given
        const string Expected = "--build \"/Working/bin\" --target Debug,Release";
        var fixture = new CMakeBuildRunnerFixture
        {
          Settings =
          {
            Targets = new [] { "Debug", "Release" }
          }
        };

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Args);
      }
    }

    [Fact]
    public void ShouldAppendOptionsSeparatedByDash()
    {
        // Given
        const string Expected = "--build \"/Working/bin\" -- \"/m:4\"";
        var fixture = new CMakeBuildRunnerFixture
        {
            Settings =
            {
                Options = new [] {"/m:4"}
            }
        };

        // When
        var result = fixture.Run();

        // Then
        Assert.Equal(Expected, result.Args);
        }
  }
}