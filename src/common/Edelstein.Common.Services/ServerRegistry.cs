using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Contracts;
using Foundatio.Caching;

namespace Edelstein.Common.Services
{
    public class ServerRegistry : IServerRegistry
    {
        private static readonly TimeSpan ServerRegistryTimeoutDuration = TimeSpan.FromMinutes(2);
        private readonly ServerRegistryRepository _repository;

        public ServerRegistry(ICacheClient cache, IDataStore store)
            => _repository = new ServerRegistryRepository(cache, store);

        public async Task<RegisterServerResponse> Register(RegisterServerRequest request)
        {
            var result = ServerRegistryResult.Ok;
            var record = await _repository.RetrieveByServerID(request.Server.Id);

            Console.WriteLine(DateTime.UtcNow);
            Console.WriteLine(record?.DateExpire.ToUniversalTime());

            if (record != null && DateTime.UtcNow < record.DateExpire.ToUniversalTime())
                result = ServerRegistryResult.FailedAlreadyRegistered;

            if (result == ServerRegistryResult.Ok)
            {
                var newRecord = new ServerRegistryRecord(request.Server)
                {
                    DateExpire = DateTime.UtcNow.Add(ServerRegistryTimeoutDuration)
                };

                if (record != null)
                    await _repository.Delete(record);
                await _repository.Insert(newRecord);
            }

            return new RegisterServerResponse { Result = result };
        }

        public async Task<DeregisterServerResponse> Deregister(DeregisterServerRequest request)
        {
            var result = ServerRegistryResult.Ok;
            var record = await _repository.RetrieveByServerID(request.Id);

            if (record == null) result = ServerRegistryResult.FailedNotRegistered;

            if (result == ServerRegistryResult.Ok)
                await _repository.Delete(record);

            return new DeregisterServerResponse { Result = result };
        }

        public async Task<UpdateServerResponse> Update(UpdateServerRequest request)
        {
            var result = ServerRegistryResult.Ok;
            var record = await _repository.RetrieveByServerID(request.Server.Id);

            if (record == null || DateTime.UtcNow > record.DateExpire.ToUniversalTime()) result = ServerRegistryResult.FailedNotRegistered;

            if (result == ServerRegistryResult.Ok)
            {
                record.FromContract(request.Server);
                record.DateExpire = DateTime.UtcNow.Add(ServerRegistryTimeoutDuration);

                await _repository.Update(record);
            }

            return new UpdateServerResponse { Result = result };
        }

        public async Task<DescribeServerByIDResponse> DescribeByID(DescribeServerByIDRequest request)
        {
            var result = ServerRegistryResult.Ok;
            var record = await _repository.RetrieveByServerID(request.Id);

            if (record == null || DateTime.UtcNow > record.DateExpire.ToUniversalTime())
            {
                result = ServerRegistryResult.FailedNotRegistered;
                record = null;
            }

            return new DescribeServerByIDResponse { Result = result, Server = record?.ToContract() };
        }

        public async Task<DescribeServerByMetadataResponse> DescribeByMetadata(DescribeServerByMetadataRequest request)
        {
            var response = new DescribeServerByMetadataResponse();
            var records = await _repository.RetrieveAllByMetadata(request.Metadata);
            var available = records.Where(r => DateTime.UtcNow < r.DateExpire.ToUniversalTime()).Select(r => r.ToContract()).ToList();

            response.Servers.Add(available);

            return response;
        }
    }
}
