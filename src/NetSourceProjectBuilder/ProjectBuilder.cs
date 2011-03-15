using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NetSourceProjectBuilder
{
    public class ProjectBuilder
    {
        private readonly Regex namespaceRegex = new Regex(@"\s*namespace\s+([a-zA-Z_0-9\.]+)\s*");

        public void BuildProjectTest()
        {
            const string baseDirectory = @"C:\Source\RefSrc\Source\.Net\4.0\DEVDIV_TFS\Dev10\Releases\RTMRel";
            const string targetDirectory = @"C:\Source\Net40";

            BuildProject(baseDirectory, targetDirectory);
        }

        public string GetNamespace(string filePath)
        {
            var fileText = File.ReadAllText(filePath);
            var match = namespaceRegex.Match(fileText);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return "NoNamespace";
        }

        public IEnumerable<string> GetSourceFiles(DirectoryInfo directory)
        {
            if (directory == null) throw new ArgumentNullException("directory");

            foreach (var fileInfo in directory.GetFiles("*.cs"))
            {
                yield return fileInfo.FullName;
            }

            foreach (var fileName in directory.GetDirectories().SelectMany(GetSourceFiles))
            {
                yield return fileName;
            }
        }

        public void BuildProject(string baseDirectory, string targetDirectory)
        {
            var count = 0;
            const string topLevel = "Net40";

            var projectFileBuilder = new StringBuilder(projectTemplate);
            projectFileBuilder.Replace("{{projectname}}", topLevel);
            var projectDirectory = Path.Combine(targetDirectory, topLevel);

            foreach (var sourceFile in GetSourceFiles(new DirectoryInfo(baseDirectory)))
            {
                count++;

                var pathComponents = sourceFile.Split('\\');
                var pathEnd = pathComponents.Length - 1;
                var fileName = pathComponents[pathEnd];

                var namespacePath = GetNamespace(sourceFile).Replace('.', '\\');
                var newRelativePath = Path.Combine(namespacePath, fileName);
                var newFilePath = Path.Combine(projectDirectory, newRelativePath);

                if(!Directory.Exists(Path.GetDirectoryName(newFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(newFilePath));
                }

                CopyToNewLocation(sourceFile, newFilePath);
                projectFileBuilder.AppendLine(string.Format("    <Compile Include=\"{0}\" />", newRelativePath));
            }

            projectFileBuilder.Append(endProjectTemplate);
            if (!Directory.Exists(projectDirectory))
            {
                Directory.CreateDirectory(projectDirectory);
            }
            var projectFilePath = Path.Combine(projectDirectory, topLevel + ".csproj");
            File.WriteAllText(projectFilePath, projectFileBuilder.ToString());

            Console.Out.WriteLine("Completed");
            Console.Out.WriteLine("Number of files processed: {0}", count);
        }


        public void TestCopy()
        {
            const string oldFile = @"C:\Source\Array.cs";
            const string newFile = @"C:\Source\Array_new.cs";

            CopyToNewLocation(oldFile, newFile);
        }

        private static void CopyToNewLocation(string sourceFile, string newFilePath)
        {
            using(var inputStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            using(var reader = new StreamReader(inputStream))
            using(var outputStream = new FileStream(newFilePath, FileMode.Create, FileAccess.Write))
            using(var writer = new StreamWriter(outputStream))
            {
                string line = null;
                while(!Eof(line = reader.ReadLine()))
                {
                    writer.WriteLine(line);
                } 
            }
        }

        private static bool Eof(string line)
        {
            return line == null || line.Trim().StartsWith(eofNotification);
        }

        private const string eofNotification = @"// File provided for Reference Use Only by Microsoft Corporation";

        private const string projectTemplate =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProductVersion>8.0.30703</ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>{A304473B-4182-47B2-95F8-689F2F0189BF}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>{{projectname}}</RootNamespace>
    <AssemblyName>{{projectname}}</AssemblyName>
    <TargetFrameworkVersion>v4.0</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>";

        const string endProjectTemplate =
@"  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name=""BeforeBuild"">
  </Target>
  <Target Name=""AfterBuild"">
  </Target>
  -->
</Project>"
        ;
    }
}