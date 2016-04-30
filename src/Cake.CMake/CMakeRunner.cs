using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.CMake
{
    /// <summary>
    /// Implementation of the CMake tool runner.
    /// </summary>
    public sealed class CMakeRunner : Tool<CMakeSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="CMakeRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public CMakeRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs CMake using the specified path and settings.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(DirectoryPath sourcePath, CMakeSettings settings)
        {
            if (sourcePath == null)
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            // Get the output path.
            var outputPath = settings.OutputPath;
            outputPath = outputPath ?? sourcePath;
            outputPath = outputPath.MakeAbsolute(_environment);

            // Create the process settings.
            var processSettings = new ProcessSettings();
            processSettings.WorkingDirectory = outputPath;

            // Run the tool using the specified settings.
            Run(settings, GetArguments(sourcePath, settings), processSettings, null);
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns></returns>
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
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(CMakeSettings settings)
        {
            if (!_environment.IsUnix())
            {
                var programFiles = _environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
                var cmakePath = programFiles.Combine("cmake/bin").CombineWithFilePath("cmake.exe");
                return new[] { cmakePath };
            }
            return new FilePath[] { };
        }

        private ProcessArgumentBuilder GetArguments(DirectoryPath sourcePath, CMakeSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Source path
            var sourcepath = sourcePath.MakeAbsolute(_environment);
            builder.AppendQuoted(sourcepath.FullPath);

            // Generator
            if (!string.IsNullOrWhiteSpace(settings.Generator))
            {
                builder.Append("-G");
                builder.AppendQuoted(settings.Generator);
            }

            // Toolset
            if (!string.IsNullOrWhiteSpace(settings.Toolset))
            {
                builder.Append("-T");
                builder.AppendQuoted(settings.Toolset);
            }

            return builder;
        }
    }
}
