namespace NFChawk.Models.Interfaces
{
    public interface IAuctionFinalizationService
    {
        Task FinalizeExpiredAuctionsAsync();
    }
}
