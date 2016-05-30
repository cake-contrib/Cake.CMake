using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.CMake
{
    /// <summary>
    /// Contains settings for CMake.
    /// </summary>
    public sealed class CMakeSettings : ToolSettings
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
        /// Gets or sets the platform name.
        /// </summary>
        /// <value>The platform name.</value>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        /// <value>The output path.</value>
        public DirectoryPath OutputPath { get; set; }
    }
}