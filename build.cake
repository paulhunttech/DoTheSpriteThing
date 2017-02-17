const string baseVersionNumber = "4.1.0";
const string buildConfiguration = "Release";

Task("Restore").Does(() => {	
	DotNetCoreRestore();	
});

Task("Build").IsDependentOn("Restore").Does(() => {
	var buildSettings = new DotNetCoreBuildSettings
	{
		Configuration = buildConfiguration,
		NoIncremental = true		
	};

	DotNetCoreBuild("src/DoTheSpriteThing/project.json", buildSettings);
	DotNetCoreBuild("test/DoTheSpriteThing.Tests/project.json", buildSettings);
	DotNetCoreBuild("test/DoTheSpriteThing.Testbed/project.json", buildSettings);
});

Task("Test").IsDependentOn("Build").Does(() => {
	var testSettings = new DotNetCoreTestSettings
	{
		NoBuild = true
	};

	DotNetCoreTest("test/DoTheSpriteThing.Tests/project.json", testSettings);
});

Task("Pack").IsDependentOn("Test").Does(() => {
	var nuGetPackSettings = new NuGetPackSettings
	{
		Version = baseVersionNumber
	};

	NuGetPack("src/DoTheSpriteThing/DoTheSpriteThing.nuspec", nuGetPackSettings);
});

Task("Default").IsDependentOn("Pack");

var target = Argument("target", "Default");

RunTarget(target);