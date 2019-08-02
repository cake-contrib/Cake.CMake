#load "nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&version=1.0.0"

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context, 
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.CMake",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.CMake",
                            appVeyorAccountName: "cakecontrib",
                            shouldRunDotNetCorePack: true,
                            shouldRunDupFinder: false,
                            shouldRunInspectCode: false);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] { 
                                BuildParameters.RootDirectoryPath + "/src/Cake.CMake.Tests/**/*.cs" },
                            testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* ",
                            testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");
Build.RunDotNetCore();