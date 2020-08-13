using Inlog.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;


namespace InlogTest
{
    public static class InMemoryContextFactory
    {
        public static InlogDbContext Create()
        {
            var options = new DbContextOptionsBuilder<InlogDbContext>()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new InlogDbContext(options);
        }
    }
}
