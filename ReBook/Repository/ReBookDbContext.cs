namespace ReBook.Repository
{
    public class ReBookDbContext : IdentityDbContext<ApplicationUser>
    {
        public ReBookDbContext(DbContextOptions<ReBookDbContext> options) : base(options)
        {
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Copy> Copies { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Wish> Wishes { get; set; }
        public DbSet<Pretender> Pretenders { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //If you have alot of data configurations you can use this (works from ASP.Net core 2.2):

            //This will pick up all configurations that are defined in the assembly
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Instead of this:
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new UsersWithRolesConfig());
        }
    }
}
