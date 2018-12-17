using System;
using Edelstein.Core.Services.Info;
using Foundatio.Serializer;

namespace Edelstein.Core.Services.Peers
{
    [Serializable]
    public class PeerServiceStatusMessage
    {
        public PeerServiceStatus Status { get; set; }

        public string DataType { get; set; }
        public byte[] Data { get; set; }

        public static PeerServiceStatusMessage Create(PeerServiceStatus status, ServiceInfo info)
        {
            return new PeerServiceStatusMessage
            {
                Status = status,
                DataType = info.GetType().FullName,
                Data = DefaultSerializer.Instance.SerializeToBytes(info)
            };
        }

        public ServiceInfo GetInfo()
            => DefaultSerializer.Instance.Deserialize(Data, Type.GetType(DataType)) as ServiceInfo;
    }
}