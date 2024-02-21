using Nest;

namespace ELK.Playground.Api
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var elasticUri = builder.Configuration.GetSection("Elasticsearch")["Uri"];
            ArgumentNullException.ThrowIfNull(elasticUri);

            var settings = new ConnectionSettings(new Uri(elasticUri)).DefaultIndex("people");
            var client = new ElasticClient(settings);

            builder.Services.AddSingleton<IElasticClient>(client);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
