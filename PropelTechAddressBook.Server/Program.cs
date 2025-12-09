using PropelTechAddressBook.Server.Core;
using PropelTechAddressBook.Server.Services;
using PropelTechAddressBook.Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// CORS so React dev server can call API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactDev", //"AllowAll"
        policy =>
        {
            policy.WithOrigins("https://localhost:52752") // React dev server URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
     options.JsonSerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
 });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IAddressBookService, AddressBookService>();

// Configuration
builder.Services.Configure<Configuration>(
    builder.Configuration.GetSection("Configuration"));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Enable CORS
app.UseCors("AllowReactDev");

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
