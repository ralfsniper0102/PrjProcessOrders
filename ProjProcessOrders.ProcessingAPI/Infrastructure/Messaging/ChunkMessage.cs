using ProjProcessOrders.UseCase.Enums;

namespace ProjProcessOrders.ProcessingAPI.Infrastructure.Messaging
{
    public class ChunkMessage
    {
        public byte[] Payload { get; set; }
        public int TotalChunks { get; set; }
        public int CurrentChunk { get; set; }
        public RequestTypeEnum RequestType { get; set; }
    }
}
