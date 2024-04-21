# xunit.AssertMessages.Analyzers
[![NuGet version (Xunit.AssertMessages.Analyzers)](https://img.shields.io/nuget/v/Xunit.AssertMessages.Analyzers.svg?style=flat-square)](https://www.nuget.org/packages/Xunit.AssertMessages.Analyzers/)

Adds code analysis rules for the `Xunit.AssertMessages` package in C#. This package is still in beta status.

## Usage
For now you need to add the nuget package explicitly. Once we are out of beta it will be added automatically when you add the `Xunit.AssertMessages` package.

Get the nuget package [here](https://www.nuget.org/packages/Xunit.AssertMessages.Analyzers).

The analyzer will wrap the original analyzers from xunit and thus uses the xunit 2xxx rules for asserts. (https://xunit.net/xunit.analyzers/rules/)