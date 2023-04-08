using HighScoreWebAPI2;
using HighScoreWebAPI2.Services;
using ProfanityFilter.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDatastoreClient()
    .AddScoped<IUsernameValidationService, UsernameValidationService>()
    .AddScoped<IProfanityFilter, ProfanityFilter.ProfanityFilter>()
    .AddScoped<IHashService, HashService>();
AddDatabaseService(builder);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void AddDatabaseService(WebApplicationBuilder builder)
{
    string databaseNamespace;
    if (builder.Environment.IsDevelopment())
        databaseNamespace = "Development";
    else
        databaseNamespace = "leaderboard-api";
    builder.Services.AddSingleton<IDatabaseService, GcpDatastore>(sp => new GcpDatastore(Constants.GcpProjectName, databaseNamespace));
}
