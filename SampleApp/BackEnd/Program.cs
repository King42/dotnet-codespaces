var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.MapGet("/advent2023/day/{day}/part/{part}", (int day, int part, bool useTestData = false) => Advent.Advent2023.Solver.GetAnswer(day, part, useTestData))
.WithName("GetAdventResult")
.WithOpenApi();

app.Run();
