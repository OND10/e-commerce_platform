using Email.API.DataBase;
using Email.API.Extension;
using Email.API.Messaging;
using Email.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Adding dependencies to the Application Pipeline
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnectionString"));
});
//Singleton because we need single object for diiferent request 
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

//Here you can not integrate the AppDbContext into the IAzureService because service conflicts
var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnectionString"));
//We need to register the EmailService as singleton to consume AppDbContext on it
builder.Services.AddSingleton(new EmailService(optionsBuilder.Options));

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

app.UseAuthorization();

app.MapControllers();

ApplyMigration();

app.UseAzureServiceBusConsumer();

app.Run();

void ApplyMigration()
{
    //First you need to create your method scope
    using(var scope = app.Services.CreateScope())
    {
        //Using the scope for the related service which is AppDbContext 
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //Check if the migration pendingCount is bigger than 0
        if(_db.Database.GetPendingMigrations().Count() > 0)
        {
            //Migrate
            _db.Database.Migrate();
        }
    }
}
