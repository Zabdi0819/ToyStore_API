using ToyStore.API.Interfaces;
using ToyStore.API.Models;
using ToyStore.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddControllers();

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ToyStore API", Version = "v1" });
    c.OperationFilter<RemoveIdFromRequestExamplesFilter>();

    c.SchemaFilter<SwaggerIgnoreIdSchemaFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowFrontend");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

SeedData(app.Services);

app.Run();

static void SeedData(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var repo = scope.ServiceProvider.GetRequiredService<IProductRepository>();

    if (!repo.GetAll().Any())
    {
        repo.Add(new Product
        {
            Name = "Lego Classic",
            Description = "Caja de bloques de colores",
            AgeRestriction = 6,
            Company = "LEGO",
            Price = 499.99m
        });

        repo.Add(new Product
        {
            Name = "Barbie Dreamhouse",
            Description = "Casa de muñecas",
            AgeRestriction = 3,
            Company = "Mattel",
            Price = 899.50m
        });
    }
}