using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {
            //  var connectionString = "Server=localhost;Port=3306;Database=dbapi;Uid=root;pwd=1234";
            var connectionString = "Server=DESKTOP-FTROGCJ\\BANCOVALERIA;Database=bancoApi2;User id=sa;Password =1234;MultipleActiveResultsets=true;Encrypt=YES;TrustServerCertificate=YES";

            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            //   optionsBuilder.UseMySql(connectionString);
            optionsBuilder.UseSqlServer(connectionString);
            return new MyContext(optionsBuilder.Options);
        }
    }
}
