using ProjProcessOrders.UseCase.Enums;

namespace ProjProcessOrders.WebAPI.Infrastructure.Messaging.DTOs
{
    public class ChunkMessage
    {
        public byte[] Payload { get; set; }
        public int TotalChunks { get; set; }
        public int CurrentChunk { get; set; }
        public RequestTypeEnum RequestType { get; set; }
        public int StatusCode { get; set; }
    }
}
