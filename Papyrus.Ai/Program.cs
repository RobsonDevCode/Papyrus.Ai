using Papyrus.Ai.Configuration;
using Papyrus.Ai.Extensions;
using Papyrus.Ai.Handlers.Endpoints;
using Papyrus.Ai.Handlers.ErrorHandlers;
using Papyrus.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalErrorHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));

builder.Services.AddDomain();
builder.Services.AddPersistence();
builder.Services.AddPapyrusSwagger();
builder.Services.AddValidators();
builder.Services.AddPapyrusAiClient(builder.Configuration);
builder.Services.AddCors();

//The pdf logic is written in c++ so we need ensure its running
var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map endpoints last
app.MapEndpoints();

app.Run();