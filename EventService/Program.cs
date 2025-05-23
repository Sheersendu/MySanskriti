using EventService.Application.Interfaces;
using EventService.Application.UseCases;
using EventService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<EventDBContext>();
builder.Services.AddSingleton<IEventRepository, EventRepository>();

builder.Services.AddSingleton<CreateEventHandler>();
builder.Services.AddSingleton<UpdateEventHandler>();
builder.Services.AddSingleton<GetEventByEventIdHandler>();
builder.Services.AddSingleton<GetEventByEventTypeOrCity>();
builder.Services.AddHttpClient<ILocationClient, LocationClient>(client =>
{
	client.BaseAddress = new Uri("http://localhost:5173");
});


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