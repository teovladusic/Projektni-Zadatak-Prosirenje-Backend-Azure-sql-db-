using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Common;
using Service;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.Controllers;
using Newtonsoft.Json;

namespace ProjektniZadatakProsirenje
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        readonly string MyAllowSpecificOrigins = "  ";


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });


            services.AddOptions();
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjektniZadatakProsirenje", Version = "v1" });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<VehicleMakesController>().PropertiesAutowired();

            builder.RegisterType<Repository<VehicleMake>>().As<IRepository<VehicleMake>>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleMakesService>().As<IVehicleMakesService>().InstancePerLifetimeScope();
            builder.RegisterType<SortHelper<VehicleMake>>().As<ISortHelper<VehicleMake>>().InstancePerLifetimeScope();


            builder.RegisterType<VehicleModelsService>().As<IVehicleModelsService>().InstancePerLifetimeScope();
            builder.RegisterType<SortHelper<VehicleModel>>().As<ISortHelper<VehicleModel>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<VehicleModel>>().As<IRepository<VehicleModel>>().InstancePerLifetimeScope();

            builder.RegisterInstance(AutoMapperConfig.Initialize());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();

                var opt = new DbContextOptionsBuilder<ApplicationDbContext>();
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                opt.EnableSensitiveDataLogging();

                return new ApplicationDbContext(opt.Options);
            }).AsSelf();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjektniZadatakProsirenje v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
