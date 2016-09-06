using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.CMake
{
    /// <summary>
    /// Contains functionality to running CMake.
    /// </summary>
    [CakeAliasCategory("CMake")]
    public static class CMakeAliases
    {
        /// <summary>
        /// Runs CMake with the specified source path and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void CMake(this ICakeContext context, DirectoryPath sourcePath, CMakeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new CMakeRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(sourcePath, settings);
        }
    }
}
