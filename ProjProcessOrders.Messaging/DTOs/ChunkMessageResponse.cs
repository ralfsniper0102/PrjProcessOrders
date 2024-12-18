namespace ProjProcessOrders.Messaging.DTOs
{
    public class ChunkMessageResponse
    {
        public byte[] Payload { get; set; }
        public int TotalChunks { get; set; }
        public int CurrentChunk { get; set; }
        public int StatusCode { get; set; }
    }
}
