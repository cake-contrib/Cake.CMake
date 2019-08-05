using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.CMake
{
  /// <summary>
  /// The c make runner base
  /// </summary>
  /// <typeparam name="TSettings">The type of the settings.</typeparam>
  /// <seealso cref="Cake.Core.Tooling.Tool{TSettings}" />
  public abstract class CMakeRunnerBase<TSettings> : Tool<TSettings> where TSettings : ToolSettings
  {
    private readonly ICakeEnvironment _environment;

    /// <summary>
    /// Initializes a new instance of the <see cref="CMakeRunnerBase{TSettings}"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="toolLocator">The tool locator.</param>
    protected CMakeRunnerBase(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator toolLocator)
      : base(fileSystem, environment, processRunner, toolLocator)
    {
      _environment = environment;
    }

    /// <summary>
    /// Runs the tool using the specified settings.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public abstract void Run(TSettings settings);

    /// <summary>
    /// Gets the name of the tool.
    /// </summary>
    /// <returns>The tool name.</returns>
    protected override string GetToolName()
    {
      return "CMake";
    }

    /// <summary>
    /// Gets the tool executable names.
    /// </summary>
    /// <returns>The CMake executable name.</returns>
    protected override IEnumerable<string> GetToolExecutableNames()
    {
      return new[] { "cmake.exe", "cmake" };
    }

    /// <summary>
    /// Gets the alternative tool paths to CMake.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <returns>The alternative tool paths to CMake.</returns>
    protected override IEnumerable<FilePath> GetAlternativeToolPaths(TSettings settings)
    {
      if (!_environment.Platform.IsUnix())
      {
        var programFiles = _environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
        var cmakePath = programFiles.Combine("cmake/bin").CombineWithFilePath("cmake.exe");
        return new[] { cmakePath };
      }

      return new FilePath[] { };
    }
  }
}