using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWork.Customer;
using UnitOfWork.Repositories;

namespace UnitOfWork.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            //These are two services available at constructor
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //This is the only service available at ConfigureServices

            //使用内存数据库
            //var connection = new SqliteConnection(Configuration.GetConnectionString("InMemoryConnection"));
            //connection.Open();
            //services.AddDbContext<UnitOfWorkDbContext>(options =>
            //    options.UseSqlite(connection));

            //services.AddDbContext<UnitOfWorkDbContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            //使用扩展方法注入Uow依赖
            //services.AddUnitOfWork<UnitOfWorkDbContext>();
            //使用默认方法注入Uow依赖
            services.AddUnitOfWork();
            //services.AddScoped<IUnitOfWork, UnitOfWork<UnitOfWorkDbContext>>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IDbContextProvider<>), typeof(DbContextProvider<>));
            //注入泛型仓储
            //services.AddTransient(typeof(IRepository<>), typeof(EfCoreRepository<>));
            //services.AddTransient(typeof(IRepository<,>), typeof(EfCoreRepository<,>));
            var typeFinder = new TypeFinder();
            var dbContextTypes = typeFinder.FindClassesOfType<DbContext>();
            var contextTypes = dbContextTypes as Type[] ?? dbContextTypes.ToArray();
            if (!contextTypes.Any())
            {
                throw new Exception("没有找到任何数据库访问上下文");
            }

            foreach (var dbContextType in contextTypes)
            {
                //注入dbcontext
                var uowconn = Configuration["ConnectionStrings:DefaultConnection"];
                var uowOptions = new DbContextOptionsBuilder<UnitOfWorkDbContext>()
                    .UseSqlServer(uowconn)
                    .Options;
                services.AddSingleton(uowOptions).AddTransient(typeof(UnitOfWorkDbContext));
                //注入每个实体仓库
                var entities = from property in dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where
                        (ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) ||
                         ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbQuery<>))) &&
                        ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0],
                            typeof(IEntity<>))
                    select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);
                foreach (var entity in entities)
                {
                    var primaryKeyType = ReflectionHelper.GetPrimaryKeyType(entity.EntityType);
                    var protype = typeof(IRepository<>).MakeGenericType(entity.EntityType);
                    var eFprotype = typeof(EfCoreRepository<,>).MakeGenericType(entity.DeclaringType,entity.EntityType);
                    var protypekey= typeof(IRepository<,>).MakeGenericType(entity.EntityType,primaryKeyType);
                    var eFprotypekey= typeof(EfCoreRepository<,,>).MakeGenericType(entity.DeclaringType,entity.EntityType, primaryKeyType);
                    services.AddTransient(protype, eFprotype);
                    services.AddTransient(protypekey, eFprotypekey);
                }
            }

            services.AddTransient<ICustomerAppService, CustomerAppService>();

            var uowconn1 = Configuration["ConnectionStrings:DefaultConnection"];
            var uowOptions1 = new DbContextOptionsBuilder<UnitOfWorkDbContext>()
                .UseSqlServer(uowconn1)
                .Options;
            //var tt = new ServiceCollection()
            //    .AddSingleton(uowOptions1)
            //    .AddScoped<UnitOfWorkDbContext>();
            services.BuildServiceProvider();
            //注入MVC
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //使得webroot（默认为wwwroot）下的文件可以被访问
            app.UseStaticFiles();

            //配置MVC路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Customer}/{action=Index}/{id?}");
            });

            //配置默认请求响应
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!" );
            //});
        }
    }
}
