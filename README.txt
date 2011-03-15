NET Source Project Builder
==========================

The published .NET 4.0 source code is distributed as an .msi installer that extracts the .NET source to a complex folder hierarchy. There are no csproj files, so it's hard to browse the code using Visual Studio.

This utility takes the .NET 4.0 .cs files, copies them to a file hierarchy based on their namespace and adds them to csproj files.

How to use it
-------------

1. Go here: http://referencesource.microsoft.com/netframework.aspx
2. Download the .Net 4 msi (3rd from the bottom of the list)
3. When asked, choose a directory to extract the .Net 4 source code into. e.g. C:\Source\RefSrc
4. Create a directory for the new projects. e.g. C:\Source\Net40
4. Run this utility like this:

NetSourceProjectBuilder.exe C:\Source\RefSrc\Source\.Net\4.0\DEVDIV_TFS\Dev10\Releases\RTMRel C:\Source\Net40

Note, the first path sould be the path under the directory where you extracted the .Net source code that ends with 'RTMRel'.

You will see output like this when the tool completes processing the cs files (it can take a few minutes):

Completed
Number of files processed: 11859

5. Open C:\Source\Net40\Net40\Net40.csproj in Visual Studio to browse the source code.




