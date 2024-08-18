using Nuke.Common.CI.GitHubActions;

[GitHubActions(
    "Build",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(Compile) }
)]
partial class Build;
