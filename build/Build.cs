using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] 
    readonly Solution Solution;

    AbsolutePath OutputDirectory => RootDirectory / "artifacts";

    Target Clean => d => d
        .Before(Restore)
        .Executes(() => DotNetTasks.DotNetClean());
    
    Target CleanArtifacts => d => d
        .Before(Restore)
        .Executes(() => OutputDirectory.CreateOrCleanDirectory());

    Target Restore => d => d
        .Executes(() => DotNetTasks.DotNetRestore());

    Target Compile => d => d
        .DependsOn(Restore)
        .Executes(() => DotNetTasks
            .DotNetBuild(s => s
            .SetConfiguration(Configuration)));
}
