using Carter;

using Maxx.App;

using Serilog;

Log.Logger = LoggingSetup.CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .ConfigureScrutor()
        .ConfigureValidators()
        .ConfigureCarterEndpoints()
        .ConfigureMediatR()
        .ConfigureSwagger()
        .ConfigureDatabase(builder.Configuration)
        .ConfigureQuartz();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapCarter();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
