using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NetCorePlayground.Tests.Mocks
{
    static class MockDbSetExtensions
    {
        public static void SetFromSqlRaw<T>(this Mock<DbSet<T>> dbSet, IList<T> entities) where T : class
        {
            var queryProvider = new Mock<IQueryProvider>();
            queryProvider
                .Setup(p => p.CreateQuery<T>(It.IsAny<Expression>()))
                .Returns<Expression>(x => entities.AsQueryable());

            dbSet.As<IQueryable<T>>()
                .SetupGet(q => q.Provider)
                .Returns(() => queryProvider.Object);

            dbSet.As<IQueryable<T>>()
                .Setup(q => q.Expression)
                .Returns(Expression.Constant(dbSet.Object));
        }
    }
}