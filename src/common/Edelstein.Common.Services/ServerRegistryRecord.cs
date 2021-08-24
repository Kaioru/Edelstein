using System;
using System.Collections.Generic;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Services.Contracts;

namespace Edelstein.Common.Services
{
    public record ServerRegistryRecord : IDataDocument
    {
        public int ID { get; set; }

        public string ServerID { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public IDictionary<string, string> Metadata { get; set; }

        public DateTime DateExpire { get; set; }

        public DateTime DateDocumentCreated { get; set; }
        public DateTime DateDocumentUpdated { get; set; }

        public ServerRegistryRecord() { }
        public ServerRegistryRecord(ServerContract server) => Populate(server);

        public void Populate(ServerContract server)
        {
            ServerID = server.Id;
            Host = server.Host;
            Port = server.Port;
            Metadata = server.Metadata;
        }

        public ServerContract AsContract()
        {
            var contract = new ServerContract
            {
                Id = ServerID,
                Host = Host,
                Port = Port
            };

            contract.Metadata.Add(Metadata);

            return contract;
        }
    }
}
