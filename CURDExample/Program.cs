using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContract;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//add services into IoC container
builder.Services.AddScoped<ICountryService, CountriesService>();
builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddDbContext<PersonsDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();


app.Run();

