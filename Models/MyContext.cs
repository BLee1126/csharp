using Microsoft.EntityFrameworkCore;

namespace belt1.Models
{ 
    // the MyContext class representing a session with our MySQL 
    // database allowing us to query for or save data
    public class MyContext : DbContext 
    { 
        public MyContext(DbContextOptions options) : base(options) { }
        // the "Monsters" table name will come from the DbSet variable name
        public DbSet<User> Users { get; set; }
        public DbSet<Rsvp> Rsvps { get; set; }
        public DbSet<Stuff> Stuffs { get; set; }

    }
}