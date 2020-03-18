using MangoAccountSystem.Component;
using MangoAccountSystem.Component.Imp;
using MangoAccountSystem.Dao;
using MangoAccountSystem.Models;
using MangoAccountSystem.Service;
using MangoAccountSystem.Service.Imp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;

namespace MangoAccountSystem
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => false;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.Configure<ForwardedHeadersOptions>(options =>
			{
				//options.KnownProxies.Add(IPAddress.Parse("192.168.99.100"));
			});

			//配置用户数据库
			services.AddDbContext<UserDbContext>(op =>
			{
				op.UseMySql(Configuration.GetConnectionString("UserDatabase"));
				op.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
			});

			//配置identity
			services.AddIdentity<MangoUser, MangoUserRole>()
				.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 1;
			});

			services.AddScoped<IUserStore<MangoUser>, MangoUserStore>();
			services.AddScoped<IRoleStore<MangoUserRole>, MangoUserRoleStore>();
			services.AddScoped<Transaction>();

			services.AddSingleton<IEmailComponent, EmailComponent>();
			services.AddSingleton<IEmailService, EmailService>();

			//配置identityserver4
			services.AddIdentityServer()
				.AddAspNetIdentity<MangoUser>()
				.AddSigningCredential(new X509Certificate2(Configuration["IdentityServerPfx:Path"], Configuration["IdentityServerPfx:Password"]))
				.AddInMemoryApiResources(Config.GetApiResources())
				.AddInMemoryClients(Config.GetClients(Configuration))
				.AddInMemoryIdentityResources(Config.GetIdentityResources());


			services.AddAuthentication()
				.AddGitHub("github", options =>
				  {
					  options.ClientId = Configuration["OAuth_key:Github_Client_Id"];
					  options.ClientSecret = Configuration["OAuth_key:Github_Client_Secret"];

					  options.Scope.Add("user");

					  options.ClaimActions.MapJsonKey("mangouser:email", "email");
					  options.ClaimActions.MapJsonKey("mangouser:name", "login");
					  options.ClaimActions.MapJsonKey("mangouser:nickname", "name");
				  })
				.AddGitee("gitee", options =>
				   {
					   options.ClientId = Configuration["OAuth_key:Gitee_Client_Id"];
					   options.ClientSecret = Configuration["OAuth_key:Gitee_Client_Secret"];
				   });


			//配置跨域
			services.AddCors(config =>
			{
				config.AddPolicy("all", p =>
				{
					p.SetIsOriginAllowed(op => true)
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials();
				});
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Center/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				//app.UseHsts();
			}
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			app.UseCors("all");
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();
			app.UseIdentityServer();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Center}/{action=Home}/{id?}");
			});
		}
	}
}
