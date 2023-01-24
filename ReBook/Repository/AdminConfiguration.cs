namespace ReBook.Repository
{
    public class AdminConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        private const string ADMIN_ID = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7";

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var admin = new ApplicationUser
            {
                Id = ADMIN_ID,
                Email = "admin@mvc.com",
                NormalizedEmail = "ADMIN@MVC.COM",
                EmailConfirmed = true,
                UserName = "admin@mvc.com",
                NormalizedUserName = "ADMIN@MVC.COM"
            };

            admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "Admin1234");

            builder.HasData(admin);
        }
    }
}
