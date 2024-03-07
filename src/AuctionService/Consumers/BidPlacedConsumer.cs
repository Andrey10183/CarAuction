using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly AuctionDbContext _dbContext;

    public BidPlacedConsumer(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine("--> Consuming bid placed");
        var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);

        if (auction.CurrentBid == null || 
            context.Message.BidStatus.Contains("Accepted") &&
            context.Message.Amount > auction.CurrentBid)
        {
            auction.CurrentBid = context.Message.Amount;
            await _dbContext.SaveChangesAsync();
        }
    }
}
