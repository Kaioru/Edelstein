using System;
using System.CommandLine;
using System.IO;
using Edelstein.Application.Server;

var commandRoot = new RootCommand(
    "A mushroom game server emulator"
);
var argumentFile = new Argument<FileInfo>(
    "file or directory path", 
    () => new FileInfo(AppDomain.CurrentDomain.BaseDirectory), 
    "The file or directory path to stage json file(s)"
);

commandRoot.AddArgument(argumentFile);
commandRoot.SetHandler(ProgramHandler.ExecuteRoot, argumentFile);

await commandRoot.InvokeAsync(args);
