#tool nuget:?package=xunit.runner.console
#tool nuget:?package=OpenCover
#tool nuget:?package=Codecov
#addin nuget:?package=Cake.Codecov

// Consts.
const string Version = "1.0.0";
const string ProjectName = "Demian";

// Arguments.
var configuration = Argument("configuration", "Release");

// Variables.
var target =  EnvironmentVariable("target") ?? Argument("target", "Default");
var version = EnvironmentVariable("version") ?? Version;
var nugetApiKey = EnvironmentVariable("nugetApiKey") ?? Argument("nugetApiKey", "");
var codecovApiKey = EnvironmentVariable("codecovApiKey") ?? Argument("codecovApiKey", "");

// Configured paths.
var solutionRootPath = "./src";
var solutionPath = $"{solutionRootPath}/{ProjectName}.sln";
var testsPath = GetFiles($"{solutionRootPath}/Tests/bin/{configuration}/netcoreapp2.1/*.Tests.dll");

// Tasks.
Task("Clean")
    .Does(() =>
    {
        CleanDirectory("./artifacts");
    });

Task("Restore-NuGet-Packages")
    .Does(() =>
    {
        NuGetRestore(solutionPath);
    });

Task("Build")
    .Does(() =>
    {
        MSBuild(solutionPath, new MSBuildSettings
        {
            Configuration = configuration,
            ArgumentCustomization = arg => arg.AppendSwitch("/p:DebugType", "=", "Full")
        });
    });

Task("Run-Tests")
    .Does(() =>
    {
        var coverageOutput = File("./artifacts/tests-coverage.xml");
        var openCoverSettings = new OpenCoverSettings
        { 
            OldStyle = true,
            Register = "user",
            ReturnTargetCodeOffset = 0
        }
        .WithFilter("+[*]* -[*Tests]*");

        OpenCover(context => context.XUnit2(testsPath), coverageOutput, openCoverSettings);
    });

Task("Upload-Coverage")
    .Does(() =>
    {
        Codecov("./artifacts/tests-coverage.xml", codecovApiKey);
    });

// Executable tasks.
Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Tests");

Task("Default-CI")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Tests")
    .IsDependentOn("Upload-Coverage");

RunTarget(target);