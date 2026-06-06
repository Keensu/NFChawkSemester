namespace NFChawk.Models.Enums
{
    public enum TransactionType
    {
        Deposit = 0,
        Withdraw = 1,

        NFTPurchase = 2,
        NFTSale = 3,

        AuctionBid = 4,
        AuctionWin = 5,

        PlatformCommission = 6,
    }
}
