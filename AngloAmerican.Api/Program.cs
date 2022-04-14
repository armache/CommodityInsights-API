using AngloAmerican.Api;
using AngloAmerican.Api.DbContexts;
using AngloAmerican.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CommodityInsightsCors", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IDataVendorService, DataVendorService>();
builder.Services.AddScoped<IPnlService, PnlService>();
builder.Services.AddScoped<ITradeRepository, TradeRepository>();

builder.Services.AddDbContext<CommodityDbContext>(DbContextOptions =>
    DbContextOptions.UseSqlServer(builder.Configuration["ConnectionStrings:CommodityDbConnectionString"],
        builder => builder.EnableRetryOnFailure()));

builder.Services.Configure<DataConfig>(builder.Configuration.GetSection("Data"));

var app = builder.Build();

//uncomment if you need to recreate the database each time the app is restarted
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<CommodityDbContext>();
//    dbContext.Database.EnsureDeleted();
//    dbContext.Database.Migrate();
//}

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors("CommodityInsightsCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
