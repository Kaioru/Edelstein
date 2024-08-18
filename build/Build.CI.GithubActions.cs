using Nuke.Common.CI.GitHubActions;

[GitHubActions(
    "build",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(Compile) }
)]
[GitHubActions(
    "pack",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.WorkflowDispatch },
    InvokedTargets = new[] { nameof(Pack) },
    FetchDepth = 0
)]
[GitHubActions(
    "publish",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.WorkflowDispatch },
    InvokedTargets = new[] { nameof(Publish) },
    FetchDepth = 0
)]
partial class Build;
