using Microsoft.EntityFrameworkCore;
using QuickPay.Application.Common;
using QuickPay.Application.Repositories;
using QuickPay.Application.Services;
using QuickPay.Infrastructure.Contexts;
using QuickPay.Infrastructure.Randomization;
using QuickPay.Infrastructure.Repositories;

namespace QuickPay.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext
            builder.Services.AddDbContext<PaymentDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("QuicPayConnection")));

            // Add services to the container.
            // Application services DI
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            // Repository DI
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

            builder.Services.AddScoped<IRandom, SystemRandom>();

            builder.Services.AddControllers();

            // Services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            // CORS policy 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigins",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost") 
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();
            app.Urls.Add("http://0.0.0.0:8080");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuickPay API V1");
                    c.RoutePrefix = string.Empty;
                });

            }
            
            app.UseHttpsRedirection();

            app.UseAuthorization();

            // CORS middleware
            app.UseCors("AllowOrigins");

            app.MapControllers();

            app.Run();
        }
    }
}
