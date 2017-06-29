IL Support extension
====================

IL Support is an extension for Visual Studio 2010, 2012, 2013 and 2015 that provides syntax highlighting for the IL (Intermediate Language) and project templates for C#, F# and Visual Basic that support embedding and calling IL code. You can download it through the Extension Manager, or on [Visual Studio Gallery](http://visualstudiogallery.msdn.microsoft.com/44034a7b-143d-4b51-b7bc-99aa656ba137).

Syntax Highlighting
-------------------

IL Support includes a simple syntax highlighter for IL files (.il). It adds 3 configurable custom classifications (Instruction, Directive and Directive (Security)), in addition to using the standard classifications.

Project Templates
-----------------

IL Support includes multiple project templates that support embedding and calling IL code in addition to another language (C#, F# and Visual Basic). The generated projects have no dependencies and should build on any Visual Studio 2010/2012 installation, even without this extension installed. They support all the project properties and configurations, build incrementally, and allow breakpoints and debugging step by step in both the main language and the IL.

Every non-empty project contains a simple example showing how to embed IL.

The following projects are included:

  * Visual C#
    * Empty Project with IL Support
    * Class Library with IL Support
    * Portable Class Library with IL Support
    * Console Application with IL Support
    * Windows Forms Application with IL Support
    * WPF Application with IL Support
  * Visual F#
    * F# Application with IL Support
    * F# Library with IL Support
    * F# Portable Library with IL Support
  * Visual Basic
    * Empty Project with IL Support
    * Class Library with IL Support
    * Portable Class Library with IL Support
    * Console Application with IL Support
    * Windows Forms Application with IL Support
    * WPF Application with IL Support

Modifying your own projects to allow IL embedding
-------------------------------------------------

  1. Edit your project file and insert [this](https://raw.github.com/ins0mniaque/ILSupport/master/IL%20Support.ProjectTemplates/IL%20Support.targets) __before__ the following section (which should be at the end)
```xml
<!-- To modify your build process, add your task inside one of the targets below and uncomment it.
     Other similar extension points exist, see Microsoft.Common.targets.
<Target Name="BeforeBuild">
</Target>
<Target Name="AfterBuild">
</Target>
-->
```

  2. You're done! Create a new project with IL Support if you need an example of how to embed IL.

How does it work ?
------------------

The project modification adds 5 simple steps :

  1. Hide IL files from the compiler
  2. Let the project compile
  3. Decompile the resulting assembly into IL
  4. Comment out all methods that were marked with the [forwardref](http://msdn.microsoft.com/en-us/library/system.runtime.compilerservices.methodimploptions%28v=vs.110%29.aspx) method implementation option
  5. Recompile the commented out IL with all the other IL files

License
=======

IL Support is licensed under the MIT License. This means you can do whatever you want as long as you include the original copyright.

See the [LICENSE.md file](https://github.com/ins0mniaque/ILSupport/blob/master/LICENSE.md) for the full license text.
