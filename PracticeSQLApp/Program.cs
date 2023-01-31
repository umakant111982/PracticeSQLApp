using Microsoft.FeatureManagement;
using PracticeSQLApp.Service;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Endpoint=https://azureappconfiguration31.azconfig.io;Id=SICc-lh-s0:geJC+cTrn8HuPKF2YFJo;Secret=LY0D3wH4kJ0CPG7bkaILMyiWaJoekY8Ai7adwV4lpGM=";

builder.Host.ConfigureAppConfiguration(builder =>
{
    builder.AddAzureAppConfiguration(options =>
    {
        options.Connect("connectionString").UseFeatureFlags();
    }
    );
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddFeatureManagement();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
