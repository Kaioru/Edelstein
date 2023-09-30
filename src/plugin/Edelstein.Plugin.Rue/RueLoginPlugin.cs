﻿using Edelstein.Plugin.Rue.Login;
using Edelstein.Plugin.Rue.Plugs;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Plugin.Login;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue;

public class RueLoginPlugin : ILoginPlugin
{
    public string ID => "RueLogin";
    private ILogger? Logger { get; set; }

    public Task OnInit(IPluginHost<LoginContext> host, LoginContext ctx)
    {
        Logger = host.Logger;
        return Task.CompletedTask;
    }

    public Task OnStart(IPluginHost<LoginContext> host, LoginContext ctx)
    {
        ILoginManagerOptions options = new LoginManagerOptions(
            skipAuthorization: false,
            isAutoRegister: true,
            autoLogin: false,
            ("user123", "pass123"));

        var loginManager = new LoginManager(
            Logger,
            options,
            ctx.Services.Auth,
            ctx.Repositories.Account,
            ctx.Services.Session,
            ctx.Stage,
            ctx.Pipelines);

        if (loginManager.Options is not { AutoLogin: true, IsAutoRegister: true })
            ctx.Pipelines.UserOnPacketCreateSecurityHandle.Add(PipelinePriority.High, new UserOnPacketCreateSecurityHandleCustomPlug(loginManager));

        if (loginManager.Options is not { SkipAuthorization: true, IsAutoRegister: true })
            ctx.Pipelines.UserOnPacketCheckPassword.Add(PipelinePriority.High, new UserOnPacketCheckPasswordCustomPlug(loginManager));

        return Task.CompletedTask;
    }

    public Task OnStop()
        => Task.CompletedTask;
}
