using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {
            
        }

        //now create DbSets --> property of dbcontext class that represent a collection of entities in database

    }
}
