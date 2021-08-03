using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Common.Interop
{
    public class ServerRegistryService : IServerRegistryService
    {
        private readonly IDictionary<string, ServerRegistryEntry> _entries;
        private readonly ISessionRegistryService _sessionRegistryService;

        public ServerRegistryService(ISessionRegistryService sessionRegistryService)
        {
            _entries = new Dictionary<string, ServerRegistryEntry>();
            _sessionRegistryService = sessionRegistryService;
        }

        public Task<ServiceRegistryResponse> RegisterServer(RegisterServerRequest request) { throw new NotImplementedException(); }
        public Task<ServiceRegistryResponse> DeregisterServer(DeregisterServerRequest request) { throw new NotImplementedException(); }
        public Task<ServiceRegistryResponse> UpdateServer(UpdateServerRequest request) { throw new NotImplementedException(); }
        public Task<DescribeServerResponse> DescribeServer(DescribeServerRequest request) { throw new NotImplementedException(); }
        public Task<DescribeServersResponse> DescribeServers(DescribeServersRequest request) { throw new NotImplementedException(); }

        public async Task<DispatchResponse> Dispatch(DispatchRequest request)
        {
            var response = new DispatchResponse() { Result = DispatchResult.Ok };
            var dispatch = new DispatchObject() { Packet = request.Packet };

            await Task.WhenAll(_entries.Values.Select(e => e.Dispatch.Writer.WriteAsync(dispatch).AsTask()));

            return response;
        }

        public async Task<DispatchResponse> DispatchToServer(DispatchToServerRequest request)
        {
            var response = new DispatchResponse() { Result = DispatchResult.Ok };
            var dispatch = new DispatchObject() { Packet = request.Packet };

            if (_entries.ContainsKey(request.Server))
                await _entries[request.Server].Dispatch.Writer.WriteAsync(dispatch);
            else response.Result = DispatchResult.Failed;

            return response;
        }
        public async Task<DispatchResponse> DispatchToServers(DispatchToServersRequest request)
        {
            var response = new DispatchResponse() { Result = DispatchResult.Ok };
            var dispatch = new DispatchObject() { Packet = request.Packet };
            var targets = _entries.Values
                .Where(e => request.Tags.All(t => e.Server.Tags.ContainsKey(t.Key) && e.Server.Tags[t.Key] == t.Value))
                .ToList();

            await Task.WhenAll(targets.Select(e => e.Dispatch.Writer.WriteAsync(dispatch).AsTask()));

            if (targets.Count == 0) response.Result = DispatchResult.Failed;

            return response;
        }

        public async Task<DispatchResponse> DispatchToAlliance(DispatchToAllianceRequest request)
        {
            var response = new DispatchResponse() { Result = DispatchResult.Ok };
            var dispatch = new DispatchObject() { Packet = request.Packet, Alliance = request.Alliance };

            await Task.WhenAll(_entries.Values.Select(e => e.Dispatch.Writer.WriteAsync(dispatch).AsTask()));

            return response;
        }

        public async Task<DispatchResponse> DispatchToGuild(DispatchToGuildRequest request)
        {
            var response = new DispatchResponse() { Result = DispatchResult.Ok };
            var dispatch = new DispatchObject() { Packet = request.Packet, Guild = request.Guild };

            await Task.WhenAll(_entries.Values.Select(e => e.Dispatch.Writer.WriteAsync(dispatch).AsTask()));

            return response;
        }

        public async Task<DispatchResponse> DispatchToParty(DispatchToPartyRequest request)
        {
            var response = new DispatchResponse() { Result = DispatchResult.Ok };
            var dispatch = new DispatchObject() { Packet = request.Packet, Party = request.Party };

            await Task.WhenAll(_entries.Values.Select(e => e.Dispatch.Writer.WriteAsync(dispatch).AsTask()));

            return response;
        }

        public async Task<DispatchResponse> DispatchToCharacter(DispatchToCharacterRequest request)
        {
            var response = new DispatchResponse() { Result = DispatchResult.Ok };
            var dispatch = new DispatchObject() { Packet = request.Packet, Character = request.Character };
            var description = await _sessionRegistryService.DescribeSessionByCharacter(new DescribeSessionByCharacterRequest { Character = request.Character });

            if (description.Result == SessionRegistryResult.Ok)
            {
                var server = description.Session.Server;

                if (_entries.ContainsKey(server)) await _entries[server].Dispatch.Writer.WriteAsync(dispatch);
                else response.Result = DispatchResult.Failed;
            }
            else response.Result = DispatchResult.Failed;

            return response;
        }

        public async IAsyncEnumerable<DispatchObject> SubscribeDispatch(DispatchSubscription request)
        {
            if (_entries.ContainsKey(request.Server))
            {
                await foreach (var dispatch in _entries[request.Server].Dispatch.Reader.ReadAllAsync())
                    yield return dispatch;
            }

            yield break;
        }
    }
}
