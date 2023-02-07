namespace Cookify.Infrastructure.Common.Seeders;

public abstract class SeederBase 
{
    public abstract Task SeedAsync(CancellationToken cancellationToken);
}