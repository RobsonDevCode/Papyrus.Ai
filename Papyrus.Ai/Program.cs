using Amazon;
using Amazon.S3;
using Papyrus.Ai.Configuration;
using Papyrus.Ai.Extensions;
using Papyrus.Ai.Handlers.Endpoints;
using Papyrus.Ai.Handlers.ErrorHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalErrorHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));

builder.Services.AddScoped<IAmazonS3>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var accessKey = config["AWS:AccessKey"];
    var secretKey = config["AWS:SecretKey"];
    var region = RegionEndpoint.GetBySystemName(config["AWS:Region"]);

    if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
    {
        throw new Exception("AWS credentials not configured. Please set AWS:AccessKey and AWS:SecretKey in appsettings.json");
    }

    return new AmazonS3Client(accessKey, secretKey, region);
});


builder.Services.AddDomain();
builder.Services.AddPersistence();
builder.Services.AddPapyrusSwagger();
builder.Services.AddValidators();
builder.Services.AddExternalHttpClients(builder.Configuration);
builder.Services.AddCors();

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