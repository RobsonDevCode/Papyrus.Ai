using Amazon;
using Amazon.S3;
using Papyrus.Ai.Configuration;
using Papyrus.Ai.Extensions;
using Papyrus.Ai.Handlers.Endpoints;
using Papyrus.Ai.Handlers.ErrorHandlers;
using Papyrus.Domain.Models.Authentication;

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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:5173")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    }); 
});
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.AddPapyrusAuthentication(builder.Configuration);


var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseExceptionHandler();
app.UseHttpsRedirection();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Map endpoints last
app.MapEndpoints();

app.Run();