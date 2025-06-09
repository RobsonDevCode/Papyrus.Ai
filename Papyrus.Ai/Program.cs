using Papyrus.Ai.Configuration;
using Papyrus.Ai.Extensions;
using Papyrus.Ai.Handlers.Endpoints;
using Papyrus.Ai.Handlers.ErrorHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalErrorHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));

builder.Services.AddDomain();
builder.Services.AddPersistance();
builder.Services.AddPapyrusSwagger();
builder.Services.AddValidators();

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map endpoints last
app.MapEndpoints();

app.Run();