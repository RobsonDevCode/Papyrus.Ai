using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Papyrus.Ai.Validators;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services;
using Papyrus.Domain.Services.Bookmark;
using Papyrus.Domain.Services.Images;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Domain.Services.Interfaces.Bookmark;
using Papyrus.Domain.Services.Interfaces.Images;
using Papyrus.Domain.Services.Interfaces.Notes;
using Papyrus.Domain.Services.Notes;
using Papyrus.Domain.Services.Pdf;
using Papyrus.Mappers;
using Papyrus.Persistence.MongoDb;
using Papyrus.Persistence.MongoDb.Reader;
using Papyrus.Persistence.MongoDb.Writer;
using Papyrus.Persistance.Interfaces.Mongo;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistance.Interfaces.Writer;
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
        serviceCollection.AddScoped<IPromptHistoryReader, PromptHistoryReader>();
        serviceCollection.AddScoped<IPromptHistoryWriter, PromptHistoryWriter>();
        serviceCollection.AddScoped<IImageReader, ImageReader>();
        serviceCollection.AddScoped<IBookmarkWriter, BookmarkWriter>();
        serviceCollection.AddScoped<IBookmarkReader, BookmarkReader>();
        
        serviceCollection.AddSingleton<IMongoPromptDbConnector, MongoPromptDbConnector>();
        serviceCollection.AddSingleton<IMongoBookDbConnector, MongoBookDbConnector>();
    }

    public static void AddDomain(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDocumentWriterService, DocumentWriterService>();
        serviceCollection.AddScoped<IDocumentReaderService, DocumentReaderService>();
        serviceCollection.AddScoped<INoteWriterService, NoteWriterService>();
        serviceCollection.AddScoped<INoteReaderService, NoteReaderService>();
        serviceCollection.AddScoped<IImageWriterService, ImageWriterService>();
        serviceCollection.AddScoped<IImageReaderService, ImageReaderService>();
        serviceCollection.AddScoped<IPdfWriterService, PdfWriterService>();
        serviceCollection.AddScoped<IPdfReaderService, PdfReaderService>();
        serviceCollection.AddScoped<IBookmarkWriterService, BookmarkWriterService>();
        serviceCollection.AddScoped<IBookmarkReaderService, BookmarkReaderService>();
        
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
        serviceCollection.AddScoped<IValidator<EditNoteRequest>, EditNoteRequestValidator>();
        serviceCollection.AddScoped<IValidator<CreateBookmarkRequest>, CreateBookmarkRequestValidator>();
    }
}