using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.EntityFramework;

partial class Build
{
    AbsolutePath OutputExeDirectory => OutputDirectory / "dist";

    [CanBeNull] SolutionFolder SourceSolutionFolder => Solution?.GetSolutionFolder("src");
    [CanBeNull] SolutionFolder SourceAppSolutionFolder => SourceSolutionFolder?.GetSolutionFolder("app");
    [CanBeNull] SolutionFolder SourcePluginSolutionFolder => SourceSolutionFolder?.GetSolutionFolder("plugin");
    [CanBeNull] SolutionFolder SourceCommonSolutionFolder => SourceSolutionFolder?.GetSolutionFolder("common");

    IEnumerable<string> Runtimes => new[]
    {
        "win-x64",
        "win-arm64",
        "linux-x64",
        "linux-arm64",
        "osx-arm64"
    };
    
    Target Publish => d => d
        .Produces(OutputExeDirectory / "*.zip")
        .Executes(() =>
        {
            OutputExeDirectory.CreateOrCleanDirectory();
            
            if (SourceAppSolutionFolder?.Projects == null) return;

            foreach (var runtime in Runtimes)
            {
                var outputRuntimeDirectory = OutputExeDirectory / $"{runtime}";
                var outputRuntimePluginsDirectory = outputRuntimeDirectory / "plugins";

                outputRuntimeDirectory.CreateOrCleanDirectory();
                
                foreach (var project in SourceAppSolutionFolder.Projects)
                {
                    DotNetTasks.DotNetPublish(s => s
                        .SetProject(project)
                        .SetConfiguration(Configuration)
                        .SetRuntime(runtime)
                        .SetPublishSingleFile(true)
                        .SetSelfContained(true)
                        .SetOutput(outputRuntimeDirectory));
                }

                if (SourceCommonSolutionFolder?.Projects != null)
                    foreach (var project in SourceCommonSolutionFolder.Projects)
                    {
                        if (project.Name.StartsWith("Edelstein.Common.Database."))
                        {
                            var provider = project.Name.Split(".").Last().ToLower();

                            EntityFrameworkTasks.EntityFrameworkMigrationsScript(s => s
                                .SetProject(project)
                                .SetConfiguration(Configuration)
                                .SetOutput(outputRuntimeDirectory / $"db-migrate-{provider}.sql"));
                        }
                    }

                if (SourcePluginSolutionFolder?.Projects != null)
                    foreach (var project in SourcePluginSolutionFolder.Projects)
                    {
                        DotNetTasks.DotNetPublish(s => s
                            .SetProject(project)
                            .SetConfiguration(Configuration)
                            .SetOutput(outputRuntimePluginsDirectory / project.Name));
                    }
                
                outputRuntimeDirectory.ZipTo(OutputExeDirectory / $"{outputRuntimeDirectory.Name}.zip");
            }
        });
}
