using ForumThreads.Model;
using ForumThreads.Services;
using ForumThreads.Data;
using MongoDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForumThreads
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var mongoDBSettings = builder.Configuration.GetSection("ForumDatabase").Get<ForumDatabaseSettings>();

            builder.Services.Configure<ForumDatabaseSettings>(
                builder.Configuration.GetSection("ForumDatabase"));

            // Add services to the container.
            builder.Services.AddSingleton<ThreadsService>();
            builder.Services.AddSingleton<CommentsService>();
            builder.Services.AddControllers();

            builder.Services.AddDbContext<ForumDbContext>(options =>
                options.UseMongoDB(mongoDBSettings.ConnectionString ?? "", mongoDBSettings.DatabaseName ?? ""));

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
