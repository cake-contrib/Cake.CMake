using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.CMake
{
  /// <summary>
  /// Implementation of the CMake build mode runner.
  /// </summary>
  public class CMakeBuildRunner : CMakeRunnerBase<CMakeBuildSettings>
  {
    private readonly ICakeEnvironment _environment;

    /// <summary>
    /// Initializes a new instance of the <see cref="CMakeBuildRunner"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="toolLocator">The tool locator.</param>
    public CMakeBuildRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator toolLocator)
      : base(fileSystem, environment, processRunner, toolLocator)
    {
      _environment = environment;
    }

    /// <summary>
    /// Runs the tool using the specified settings.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public override void Run(CMakeBuildSettings settings)
    {
      if (settings == null)
      {
        throw new ArgumentNullException(nameof(settings));
      }

      if (settings.BinaryPath == null)
      {
        throw new ArgumentNullException(nameof(settings.BinaryPath));
      }

      var binaryPath = settings.BinaryPath.MakeAbsolute(_environment);

      // Create the process settings.
      var processSettings = new ProcessSettings
      {
        WorkingDirectory = binaryPath
      };

      this.Run(settings, this.GetArguments(settings), processSettings, null);
    }

    private ProcessArgumentBuilder GetArguments(CMakeBuildSettings settings)
    {
      var builder = new ProcessArgumentBuilder();

      var binaryPath = settings.BinaryPath.MakeAbsolute(_environment);
      builder.AppendSwitchQuoted("--build", binaryPath.FullPath);

      // Generator
      if (settings.CleanFirst)
      {
        builder.Append("--clean-first");
      }

      // Toolset
      if (settings.Targets != null)
      {
        builder.AppendSwitch("--target", string.Join(",", settings.Targets));
      }

      //Configuration
      if (settings.Configuration != null)
      {
          builder.AppendSwitch("--config", settings.Configuration);
      }

      // Options
      if (settings.Options != null)
      {
        foreach (string option in settings.Options)
        {
          builder.AppendQuoted(option);
        }
      }

      return builder;
    }
  }
}