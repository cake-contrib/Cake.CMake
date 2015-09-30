var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////
// PREPARATION
///////////////////////////////////////////////////////////////

// Parse the releae notes.
var releaseNotes = ParseReleaseNotes("./ReleaseNotes.md");

// Initialize build numbers.
var local = BuildSystem.IsLocalBuild;
var buildNumber = AppVeyor.Environment.Build.Number;
var version = releaseNotes.Version.ToString();
var semVersion = local ? version : (version + string.Concat("-build-", buildNumber));

///////////////////////////////////////////////////////////////
// DIRECTORIES
///////////////////////////////////////////////////////////////

var build = Directory("./src/Cake.CMake/bin") + Directory(configuration);
var artifacts = Directory("./artifacts");
var bin = artifacts + Directory("bin");
var nuget = artifacts + Directory("nuget");

///////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////

Task("Clean")
  .Does(() =>
{
  CleanDirectories(new DirectoryPath[] { artifacts, bin, nuget });
});

Task("Restore-NuGet-Packages")
  .IsDependentOn("Clean")
  .Does(() =>
{
  NuGetRestore("./src/Cake.CMake.sln");
});

Task("Patch-Assembly-Info")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    var file = "./src/SolutionInfo.cs";
    CreateAssemblyInfo(file, new AssemblyInfoSettings {
        Product = "Cake.CMake",
        Version = version,
        FileVersion = version,
        InformationalVersion = semVersion,
        Copyright = "Copyright (c) Patrik Svensson"
    });
});

Task("Build")
  .IsDependentOn("Patch-Assembly-Info")
  .Does(() =>
{
  MSBuild("./src/Cake.CMake.sln", new MSBuildSettings()
    .SetConfiguration(configuration)
    .SetPlatformTarget(PlatformTarget.MSIL));
});

Task("Run-Unit-Tests")
  .IsDependentOn("Build")
  .Does(() =>
{
  XUnit2("./**/bin/" + configuration + "/*.Tests.dll");
});

Task("Copy-Files")
  .IsDependentOn("Run-Unit-Tests")
  .Does(() =>
{
  CopyFileToDirectory(build + File("Cake.CMake.dll"), bin);
  CopyFileToDirectory(build + File("Cake.CMake.xml"), bin);
  CopyFileToDirectory(build + File("Cake.CMake.pdb"), bin);
  CopyFiles(new FilePath[] { "LICENSE", "README.md", "ReleaseNotes.md"}, bin);
});

Task("Zip-Files")
  .IsDependentOn("Copy-Files");

Task("Create-NuGet-Package")
  .IsDependentOn("Copy-Files")
  .Does(() =>
{
  NuGetPack("./Cake.CMake.nuspec", new NuGetPackSettings {
      Version = semVersion,
      ReleaseNotes = releaseNotes.Notes.ToArray(),
      BasePath = bin,
      OutputDirectory = nuget,
      Symbols = false
  });
});

Task("Publish-NuGet-Package")
  .IsDependentOn("Create-NuGet-Package")
  .WithCriteria(() => BuildSystem.IsLocalBuild)
  .Does(() =>
{
  // Get the package path.
  var package = nuget + File("Cake.CMake." + version + ".nupkg");

  // Get the API key.
  var apiKey = EnvironmentVariable("NUGET_API_KEY");
  if(string.IsNullOrWhiteSpace(apiKey))
  {
    throw new CakeException("Could not resolve NuGet API key.");
  }

  // Push the package.
  NuGetPush(package, new NuGetPushSettings {
      ApiKey = apiKey
  });
});

///////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////

Task("Default")
  .IsDependentOn("Package");

Task("Package")
  .IsDependentOn("Zip-Files")
  .IsDependentOn("Create-NuGet-Package");

Task("Publish")
  .IsDependentOn("Publish-NuGet-Package");

RunTarget(target);
