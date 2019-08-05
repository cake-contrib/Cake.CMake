using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.CMake
{
  /// <summary>
  /// Contains settings for CMake build mode.
  /// </summary>
  /// <remarks>cmake --build &lt;dir&gt; [&lt;options&gt;] [-- &lt;build-tool-options&gt;]</remarks>
  public class CMakeBuildSettings : ToolSettings
  {
    /// <summary>
    /// Gets or sets the project binary directory to be built.
    /// </summary>
    /// <value>
    /// The binary path.
    /// </value>
    public DirectoryPath BinaryPath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether build target clean first, then build. (To clean only, use --target clean.)
    /// </summary>
    /// <remarks>--clean-first</remarks>
    /// <value>
    ///   <c>true</c> if clean first; otherwise, <c>false</c>.
    /// </value>
    public bool CleanFirst { get; set; }

    /// <summary>
    /// Gets or sets the targets. Build passed target instead of the default target.
    /// </summary>
    /// <remarks>--target &lt;tgt&gt;..., -t &lt;tgt&gt;...</remarks>
    /// <value>
    /// The targets.
    /// </value>
    public ICollection<string> Targets { get; set; }

    /// <summary>
    /// Gets or sets CMake build options.
    /// </summary>
    /// <value>CMake build options.</value>
    public ICollection<string> Options { get; set; }
  }
}