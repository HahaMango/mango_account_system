using MangoAccountSystem.Component;
using MangoAccountSystem.Component.Imp;
using MangoAccountSystem.Dao;
using MangoAccountSystem.Models;
using MangoAccountSystem.Service;
using MangoAccountSystem.Service.Imp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MangoAccountSystem.Test
{
	public class TestSetup
	{
		public TestSetup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
            //配置用户数据库
            services.AddDbContext<UserDbContext>(op =>
			{
                op.UseInMemoryDatabase(databaseName: "mangousersystem");
                op.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);               
            });
            
			//配置identity
			services.AddIdentity<MangoUser, MangoUserRole>()
				.AddDefaultTokenProviders();

			services.AddTransient<IUserStore<MangoUser>, MangoUserStore>();
			services.AddTransient<IRoleStore<MangoUserRole>, MangoUserRoleStore>();

			services.AddSingleton<IEmailComponent, EmailComponent>();
			services.AddSingleton<IEmailService, EmailService>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
		}
	}
}
