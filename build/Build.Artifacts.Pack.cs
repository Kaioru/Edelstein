using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

partial class Build
{
    AbsolutePath OutputPkgDirectory => OutputDirectory / "packages";
    
    Target Pack => d => d
        .Produces(OutputPkgDirectory / "*.zip")
        .Executes(() =>
        {
            OutputPkgDirectory.CreateOrCleanDirectory();
            return DotNetTasks.DotNetPack(s => s
                .SetConfiguration(Configuration)
                .SetOutputDirectory(OutputPkgDirectory));
        });
}
