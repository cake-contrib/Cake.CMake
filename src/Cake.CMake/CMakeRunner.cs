﻿using System;
using System.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.CMake
{
  /// <summary>
  /// Implementation of the CMake tool runner.
  /// </summary>
  public sealed class CMakeRunner : CMakeRunnerBase<CMakeSettings>
  {
    private readonly ICakeEnvironment _environment;
    private readonly ICakeLog _log;

    /// <summary>
    /// Initializes a new instance of the <see cref="CMakeRunner" /> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    /// <param name="log">The log.</param>
    public CMakeRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, ICakeLog log)
        : base(fileSystem, environment, processRunner, tools)
    {
      _environment = environment;
      _log = log;
    }

    /// <summary>
    /// Runs CMake using the specified path and settings.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public override void Run(CMakeSettings settings)
    {
      if (settings == null)
      {
        throw new ArgumentNullException(nameof(settings));
      }

      if (settings.OutputPath == null && settings.SourcePath == null)
      {
        throw new ArgumentException("The settings properties OutputPath or SourcePath should not be null.");
      }

      // Get the output path.
      var outputPath = settings.OutputPath;
      var workingDirectory = (outputPath ?? settings.SourcePath).MakeAbsolute(_environment);

      if (!Directory.Exists(workingDirectory.FullPath))
      {
        _log.Information("The working directory {0} does not found and will create.", workingDirectory.FullPath);

        Directory.CreateDirectory(workingDirectory.FullPath);

        _log.Information("The working directory {0} was created.", workingDirectory.FullPath);
      }

      // Create the process settings.
      var processSettings = new ProcessSettings
      {
        WorkingDirectory = workingDirectory
      };

      // Run the tool using the specified settings.
      this.Run(settings, this.GetArguments(settings), processSettings, null);
    }

    private ProcessArgumentBuilder GetArguments(CMakeSettings settings)
    {
      var builder = new ProcessArgumentBuilder();

      if (settings.SourcePath != null)
      {
        var sourcePath = settings.SourcePath.MakeAbsolute(_environment);

        builder.AppendSwitchQuoted("-S", sourcePath.FullPath);
      }

      // Generator
      if (!string.IsNullOrWhiteSpace(settings.Generator))
      {
        builder.AppendSwitchQuoted("-G", settings.Generator);
      }

      // Toolset
      if (!string.IsNullOrWhiteSpace(settings.Toolset))
      {
        builder.AppendSwitchQuoted("-T", settings.Toolset);
      }

      // Platform
      if (!string.IsNullOrWhiteSpace(settings.Platform))
      {
        builder.AppendSwitchQuoted("-A", settings.Platform);
      }

      // Options
      if (settings.Options != null)
      {
        foreach (var option in settings.Options)
        {
          builder.AppendQuoted(option);
        }
      }

      return builder;
    }
  }
}
