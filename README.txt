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

You will see output like this as the tool processes the cs files:

    0 
  214 NetFx35
 1078 NetFx40
   83 System.Runtime.DurableInstancing
  444 WCF
  452 WF
  831 BCL
   84 ManagedLibraries
   48 AddIn
    1 BID
  120 CommonUI
  341 CompMod
  143 Configuration
  348 Core
  382 Data
  627 DataEntity
   55 DataEntityDesign
   11 DataSet
  225 DataWeb
   23 DataWebControls
   29 DataWebControlsDesign
   88 DLinq
   29 Misc
  262 MIT
  300 Net
   23 Regex
   24 security
  249 Services
   72 Sys
   29 SystemNet
   45 tx
  546 WinForms
   36 Wmi
    4 XLinq
  354 Xml
  187 XmlUtils
 1390 xsp
    4 .Net
  249 Base
   20 BuildTasks
 1057 Core
 1098 Framework
    5 Graphics
   91 Shared
   20 Themes
Completed
Number of files processed: 11859

5. Goto C:\Source\Net40 (the path you specified as the second argument), you should see a list of directories that match the output reported by the tool.
6. Open any of the directories and double click the csproj file to load with Visual Studio.



