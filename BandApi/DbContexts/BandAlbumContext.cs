using System;
using BandApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BandApi.DbContexts
{

public class BandAlbumContext : DbContext
    {
        public BandAlbumContext(DbContextOptions<BandAlbumContext> options):
            base(options)
        {
        }

        public DbSet<Band> Bands { get; set; }

        public DbSet<Album> Albums { get; set; }

    }
}
