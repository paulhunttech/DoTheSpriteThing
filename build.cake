const string baseVersionNumber = "5.0.0";
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

	DotNetCoreBuild("src/DoTheSpriteThing/DoTheSpriteThing.csproj", buildSettings);
	DotNetCoreBuild("test/DoTheSpriteThing.Tests/DoTheSpriteThing.Tests.csproj", buildSettings);
	DotNetCoreBuild("test/DoTheSpriteThing.Testbed/DoTheSpriteThing.Testbed.csproj", buildSettings);
});

Task("Test").IsDependentOn("Build").Does(() => {
	var testSettings = new DotNetCoreTestSettings
	{
		Configuration = buildConfiguration,
		NoBuild = true
	};

	DotNetCoreTest("test/DoTheSpriteThing.Tests/DoTheSpriteThing.Tests.csproj", testSettings);
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