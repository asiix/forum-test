using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ForumUsers.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ForumUsers.Data;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ForumUsers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                        "CorsPolicy",
                        builder => builder
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                    );
            });

            // Add services to the container.
            builder.Services.AddDbContext<ForumUsersContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Forum")));
            builder.Services.AddControllers().AddJsonOptions(
                    options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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

            app.UseCors("CorsPolicy");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
