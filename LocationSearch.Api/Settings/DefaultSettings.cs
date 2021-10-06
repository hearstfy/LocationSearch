namespace LocationSearch.Api.Settings
{
    public class DefaultSettings
    {
        public int MaximumNumberOfResults { get; set; }
        public int ChunkSize { get; set; }
        public double SlidingExpiration { get; set; }
        public double AbsoluteExpiration { get; set; }
    }
}