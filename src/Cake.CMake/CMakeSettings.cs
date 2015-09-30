using Cake.Core.IO;

namespace Cake.CMake
{
    /// <summary>
    /// Contains settings for CMake.
    /// </summary>
    public sealed class CMakeSettings
    {
        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        public string Generator { get; set; }

        /// <summary>
        /// Gets or sets the toolset.
        /// </summary>
        /// <value>The toolset.</value>
        public string Toolset { get; set; }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        /// <value>The output path.</value>
        public DirectoryPath OutputPath { get; set; }

        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }
    }
}