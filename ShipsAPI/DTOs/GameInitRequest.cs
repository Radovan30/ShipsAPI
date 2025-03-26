namespace ShipsAPI.DTOs
{
    public class GameInitRequest
    {
        public string Player1 { get; set; } = null!;
        public string Player2 { get; set; } = null!;
        public int BoardSize { get; set; }
    }
}
