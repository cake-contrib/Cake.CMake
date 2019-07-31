# Cake.CMake
[![Nuget](https://img.shields.io/nuget/v/Cake.CMake.svg)](https://www.nuget.org/packages/Cake.CMake)

Addin that extends Cake with CMake support.

# How to use
```csharp
#addin nuget:?package=Cake.CMake

Task("CMake")
    .Does(() =>
{
    var settings = new CMakeSettings
    {
        OutputPath = "path/to/build"
    };
    
    CMake("path/to/source", settings);
});

```
