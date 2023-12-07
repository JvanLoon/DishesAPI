using AutoMapper;
using DishesAPI.DbContexts;
using DishesAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//register the DbContext on the container, getting the connection string from appsettings.json
builder.Services.AddDbContext<DishesDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DishesDBConnectionString"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapGet("/dishes",async (DishesDbContext dishesDbContext, IMapper mapper) =>
{
    return mapper.Map<IEnumerable<DishDTO>>(await dishesDbContext.Dishes.ToListAsync());
});

app.MapGet("/dishes/{id:guid}", async (DishesDbContext dishesDbContext, IMapper mapper, Guid id) =>
{
    return mapper.Map<DishDTO>(await dishesDbContext.Dishes.FindAsync(id));
});


app.MapGet("/dishes/{id}/ingredients", async (DishesDbContext dishesDbContext, IMapper mapper, Guid id) =>
{
    return mapper.Map<IEnumerable<IngredientDTO>>((await dishesDbContext.Dishes.FindAsync(id))?.Ingredients);
});

//get dish by name
app.MapGet("/dishes/{name}", async (DishesDbContext dishesDbContext, IMapper mapper, string name) =>
{
    return mapper.Map<DishDTO>(await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Name == name));
});

//recreate and migrate the database on each run
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<DishesDbContext>();
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}

app.Run();
