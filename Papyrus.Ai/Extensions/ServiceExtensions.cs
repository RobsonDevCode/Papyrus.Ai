using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Papyrus.Ai.Validators;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Domain.Services.Interfaces.Notes;
using Papyrus.Domain.Services.Notes;
using Papyrus.Persistence.MongoDb;
using Papyrus.Persistence.MongoDb.Reader;
using Papyrus.Persistence.MongoDb.Writer;
using Papyrus.Perstistance.Interfaces.Mongo;
using Papyrus.Perstistance.Interfaces.Reader;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Ai.Extensions;

public static class ServiceExtensions
{
    public static void AddPersistence(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDocumentWriter, DocumentWriter>();
        serviceCollection.AddScoped<IDocumentReader, DocumentReader>();
        serviceCollection.AddScoped<INoteReader, NoteReader>();
        serviceCollection.AddScoped<INoteWriter, NoteWriter>();
        serviceCollection.AddSingleton<IMongoBookDbConnector, MongoBookDbConnector>();
    }

    public static void AddDomain(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDocumentWriterService, DocumentWriterService>();
        serviceCollection.AddScoped<IDocumentReaderService, DocumentReaderService>();
        serviceCollection.AddScoped<INoteWriterService, NoteWriterService>();
        serviceCollection.AddScoped<INoteReaderService, NoteReaderService>();
        serviceCollection.AddSingleton<IPapyrusAiClient, PapyrusAiClient>();
        serviceCollection.AddSingleton<IMapper, Mapper>();
        serviceCollection.AddMemoryCache();
    }

    public static void AddPapyrusSwagger(this IServiceCollection serviceCollection)
    {
        const string version = "v1";
        serviceCollection.AddSwaggerGen(s =>
        {
            s.SwaggerDoc(version, new OpenApiInfo
            {
                Title = "Papyrus API",
                Version = version
            });
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            s.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, [] }
            });
        });
    }

    public static void AddValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IValidator<FormFile>, FormFileValidator>();
        serviceCollection.AddScoped<IValidator<WriteNoteRequest>, WriteNotesValidator>();
        serviceCollection.AddScoped<IValidator<WriteImageNoteRequest>, WriteImageNoteValidator>();
        serviceCollection.AddScoped<IValidator<AddToNoteRequest>, AddToNoteRequestValidator>();
    }
}