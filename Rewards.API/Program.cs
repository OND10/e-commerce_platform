using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Rewards.API.DataBase;
using Rewards.API.Extension;
using Rewards.API.Messaging;
using Rewards.API.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding DI to Application Pipeline
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnectionString"));
});

//In these four lines we made azureServices as singleton life to adapt with the DboptionsBuilder
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnectionString"));
//Here we register the RewardService as singleton to consume the AppDbContext
builder.Services.AddSingleton(new RewardService(optionsBuilder.Options));

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

ApplyMigration();

app.UseAzureServiceBusConsumer();

app.Run();

void ApplyMigration()
{
    using(var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        if(_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}