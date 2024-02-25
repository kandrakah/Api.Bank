using Api.Bank.Domain.Interfaces;
using Api.Bank.Infra.Data;
using Api.Bank.Infra.Data.Extensions;
using Api.Bank.Infra.Shared.Extensions;
using Api.Bank.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Api.Bank.Application
{
    /// <summary>
    /// Classe de inicialização.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Método de inicialização.
        /// </summary>
        /// <param name="args">Argumentos de inicialização.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Formatting = Formatting.None;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            builder.WebHost.SuppressStatusMessages(true);
            builder.Logging.AddSerilog(builder.Configuration);

            builder.Services.AddDbContext<DataContext>(o => o.UsePostgres(builder.Configuration.GetConnectionString("default")));
            builder.Services.AddUnitOfWork();

            builder.Services.AddScoped<DbContext, DataContext>();

            builder.Services.AddScoped<IBankService, BankService>();
            builder.Services.AddScoped<IBankSlipService, BankSlipService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(o =>
            {
                foreach (var name in Directory.GetFiles(AppContext.BaseDirectory, "Api.Bank.*.xml", SearchOption.AllDirectories))
                {
                    o.IncludeXmlComments(filePath: name);
                }

                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Por favor, informe o token de autenticação.",
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                          new OpenApiSecurityScheme()
                            {
                                Reference = new OpenApiReference()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHsts();

            app.UseHttpsRedirection();           
            app.UseAuthorization();
            app.MapControllers();

            app.ApplyMigrations<DataContext>();

            app.Run();
        }
    }
}
