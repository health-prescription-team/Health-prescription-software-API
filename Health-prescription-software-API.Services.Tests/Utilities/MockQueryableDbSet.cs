namespace Health_prescription_software_API.Services.Tests.Utilities
{
    public static class MockQueryableDbSet
    {
        public static Mock<DbSet<TEntity>> MockDbSet<TEntity>(IEnumerable<TEntity> data) where TEntity : class
        {
            var mockSet = new Mock<DbSet<TEntity>>();

            var queryableData = data.AsQueryable();

            mockSet.As<IAsyncEnumerable<TEntity>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<TEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<TEntity>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<TEntity>(queryableData.Provider));

            mockSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            return mockSet;
        }
    }
}
