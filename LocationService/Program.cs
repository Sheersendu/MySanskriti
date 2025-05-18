using LocationService.Application.Interfaces;
using LocationService.Application.Mappings;
using LocationService.Application.UseCases;
using LocationService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(LocationRequestToLocationMapping));

builder.Services.AddSingleton<LocationDBContext>();
builder.Services.AddSingleton<ILocationRepository, LocationRepository>();
builder.Services.AddSingleton<GetLocationHandler>();
builder.Services.AddSingleton<AddLocationHandler>();
builder.Services.AddSingleton<UpdateLocationHandler>();

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

app.Run();