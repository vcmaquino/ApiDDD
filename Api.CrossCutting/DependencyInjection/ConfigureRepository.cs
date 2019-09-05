using Api.Data.Context;
using Api.Data.Repository;
using Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCutting.DependencyInjection
{
    public class ConfigureRepository
    {
        public static void ConfigureDependenciesService(IServiceCollection serviceCollection)
        {
            //nivel de aplicação não servidor
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            // serviceCollection.AddDbContext<MyContext>(
            //                options => options.UseMySql("Server=localhost:3306;Port=3306;Database=dbapi;Uid=root;pwd=1234")
            //            ); //conexão com banco de dados
            serviceCollection.AddDbContext<MyContext>(
             options => options.UseSqlServer("Server=DESKTOP-FTROGCJ\\BANCOVALERIA;Database=bancoApi2;User id=sa;Password =1234;MultipleActiveResultsets=true;Encrypt=YES;TrustServerCertificate=YES")
         );
        }
    }
}
