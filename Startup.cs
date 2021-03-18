using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using Messanger.Models.ProgramModels;
using Messanger.DataBase;
using System.Collections.Generic;
using System.Linq;
using System;
using Messanger.Domain.Core;
using Messanger.Domain.Interfaces;
using Messanger.Services.Interfaces;
using Messanger.Infrastructure.Data;
using Messanger.Infrastructure.Business;


namespace Messanger
{
    public class Startup
    {       
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public AuthorizationOptions authoption { get; private set; }        
              
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<DataBase.UsersContext>(options => options.UseSqlServer(connection));
            authoption = Configuration.GetSection("Option").Get<AuthorizationOptions>();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authoption.Key));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Data", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
             });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {                            
                            ValidateIssuer = true,

                            ValidIssuer = authoption.Issuer,

                            ValidateAudience = true,

                            ValidAudience = authoption.Audience,

                            ValidateLifetime = true,

                            IssuerSigningKey = key,

                            ValidateIssuerSigningKey = true,
                        };
                    });
            services.AddControllersWithViews();
            services.AddSession();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAddUser, UserAdd>();
            services.AddScoped<IMessageRepository,MessageRepository >();
            services.AddScoped<IAddSms,SmsAdd>();
            services.AddDbContext<Infrastructure.Data.UsersContext>(options =>options.UseSqlServer(connection));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            //app.UseSwagger();
            //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.OAuthClientId("swagger-ui");
                c.OAuthClientSecret("swagger-ui-secret");
                c.OAuthRealm("swagger-ui-realm");
                c.OAuthAppName("Swagger UI");


            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
