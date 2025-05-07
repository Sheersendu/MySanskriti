using TicketService.API.Mappings;
using TicketService.Application.Interfaces;
using TicketService.Application.UseCases;
using TicketService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(TicketResponseMapping));

builder.Services.AddSingleton<TicketDBContext>();
builder.Services.AddSingleton<ITicketRepository, TicketRepository>();
builder.Services.AddSingleton<GetTicketHandler>();
builder.Services.AddSingleton<CreateTicketHandler>();
builder.Services.AddSingleton<CancelTicketHandler>();

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