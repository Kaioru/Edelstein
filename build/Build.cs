using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tools.DotNet;

[GitHubActions(
    "build",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(Compile) }
)]
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

    Target Clean => d => d
        .Before(Restore)
        .Executes(() => DotNetTasks.DotNetClean());

    Target Restore => d => d
        .Executes(() => DotNetTasks.DotNetRestore());

    Target Compile => d => d
        .DependsOn(Restore)
        .Executes(() => DotNetTasks
            .DotNetBuild(s => s
            .SetConfiguration(Configuration)));
}
