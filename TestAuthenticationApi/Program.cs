using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;

namespace TestAuthenticationApi
    {
    public class Program
        {
        public static void Main(string[] args)
            {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://accounts.google.com/";
        options.TokenValidationParameters = new TokenValidationParameters
            {
            ValidateIssuer = true,
            ValidIssuer = "https://accounts.google.com/",
            ValidateAudience = true,
            ValidAudiences = new List<string>
            {
                "283580482176-v7o7a3vs9sd269i8qtknjua8kddmine1.apps.googleusercontent.com", // Replace with your actual Google client ID
                // Add more audiences as needed (e.g., Facebook client ID)
            },
            ValidateLifetime = true
            };

        /* Facebook configuration
        options.Authority = "https://graph.facebook.com/";
        options.TokenValidationParameters = new TokenValidationParameters
            {
            ValidateIssuer = true,
            ValidIssuer = "https://graph.facebook.com/",
            ValidateAudience = true,
            ValidAudiences = new List<string>
            {
                "YOUR_FACEBOOK_APP_ID", // Replace with your actual Facebook app ID of client wasm
                // Add more audiences as needed
            },
            ValidateLifetime = true
            };*/
    });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                {
                app.UseSwagger();
                app.UseSwaggerUI();
                }

            app.UseHttpsRedirection();

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            }
        }
    }
