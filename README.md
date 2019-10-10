# Cake.CMake

Addin that extends Cake with CMake support.

| Stable | Pre-release |
|:--:|:--:|
|[![Nuget](https://img.shields.io/nuget/v/Cake.CMake.svg)](https://www.nuget.org/packages/Cake.CMake)|[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Cake.CMake)](https://www.nuget.org/packages/Cake.CMake)|

## Build status

| develop | master |
|:--:|:--:|
|[![Build status](https://ci.appveyor.com/api/projects/status/e5k60e6yfk57i0jm/branch/develop?svg=true)](https://ci.appveyor.com/project/cakecontrib/cake-cmake/branch/develop)|[![Build status](https://ci.appveyor.com/api/projects/status/e5k60e6yfk57i0jm/branch/master?svg=true)](https://ci.appveyor.com/project/cakecontrib/cake-cmake/branch/master)|

## How to use

### For to generate

```csharp
#addin nuget:?package=Cake.CMake

Task("CMake")
    .Does(() =>
{
    var settings = new CMakeSettings
    {
        OutputPath = "path/to/build",
        SourcePath = "path/to/source"
    };

    CMake(settings);
});
```

### For to build

```csharp
#addin nuget:?package=Cake.CMake

Task("CMake")
    .Does(() =>
{
    var settings = new CMakeBuildSettings
    {
        BinaryPath = "path/to/build"
    };

    CMakeBuild(settings);
});
```
