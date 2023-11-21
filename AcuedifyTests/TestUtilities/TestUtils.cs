using Acuedify.Data;
using Microsoft.EntityFrameworkCore;

namespace Acuedify.Tests.TestUtilities
{
    internal static class TestUtils
    {
        public static AppDBContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDBContext(options);
        }
    }
}