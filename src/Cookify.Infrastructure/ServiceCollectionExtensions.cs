using System.Net;
using AngleSharp;
using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Ingredient;
using Cookify.Domain.MealCategory;
using Cookify.Domain.ProductMarket;
using Cookify.Domain.Recipe;
using Cookify.Domain.RecipeCategory;
using Cookify.Infrastructure.Common.Seeders;
using Cookify.Infrastructure.Options;
using Cookify.Infrastructure.Persistence;
using Cookify.Infrastructure.Persistence.Interceptors;
using Cookify.Infrastructure.Persistence.Seeders;
using Cookify.Infrastructure.Repositories;
using Cookify.Infrastructure.Scheduling.Jobs;
using Cookify.Infrastructure.Services;
using Cookify.Infrastructure.Services.RestApis;
using Cookify.Infrastructure.UnitOfWork;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Google.Cloud.Translation.V2;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Refit;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Cookify.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICurrentUserService, HttpCurrentUserService>();
        services.AddScoped<IBrowsingContext>(_ => BrowsingContext.New(Configuration.Default.WithDefaultLoader()));
        services.AddScoped<IImageSearcherService, GoogleImageSearcherService>();
        services.AddInternetFileDownloader();
        services.AddTheMealDbApi();
        services.AddSilpoProductMarket();
        services.AddGoogleFileStorage();
        services.AddGoogleTextTranslation();
        services.AddQuartzScheduling();
        services.AddEfPostgres();
        services.AddSeeders();
        services.AddRepositories();
        
        return services;
    }
    
    public static IServiceCollection AddTheMealDbApi(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();

        TheMealDbApiOptions options = new();
        
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(TheMealDbApiOptions.SectionName);
        services.Configure<TheMealDbApiOptions>(section, binderOptions => binderOptions.ErrorOnUnknownConfiguration = true);
        section.Bind(options);
        
        services
            .AddRefitClient<ITheMealDbApi>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(options.Url);
            });

        return services;
    }
    
    public static IServiceCollection AddSilpoProductMarket(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();

        SilpoProductMarketOptions options = new();
        
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(SilpoProductMarketOptions.SectionName);
        services.Configure<SilpoProductMarketOptions>(section, binderOptions => binderOptions.ErrorOnUnknownConfiguration = true);
        section.Bind(options);

        services.AddScoped<IGraphQLClient>(_ => new GraphQLHttpClient(options.ApiUrl, new SystemTextJsonSerializer()));
        services.AddScoped<SilpoProductMarketService>();

        return services;
    }
    
    public static IServiceCollection AddInternetFileDownloader(this IServiceCollection services)
    {
        services.AddScoped<WebClient>();
        services.AddScoped<IInternetFileDownloaderService, InternetFileDownloaderService>();

        return services;
    }
    
    public static IServiceCollection AddSeeders(this IServiceCollection services)
    {
        services.AddScoped<SeederBase, ProductMarketsSeeder>();

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRecipeCategoriesRepository, RecipeCategoriesRepository>();
        services.AddScoped<IIngredientsRepository, IngredientsRepository>();
        services.AddScoped<IProductMarketsRepository, ProductMarketsRepository>();
        services.AddScoped<IRecipesRepository, RecipesRepository>();
        
        return services;
    }

    #region Google Translation 

    public static IServiceCollection AddGoogleTextTranslation(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        
        GoogleTranslationOptions options = new();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(GoogleTranslationOptions.SectionName);
        services.Configure<GoogleTranslationOptions>(section, binderOptions => binderOptions.ErrorOnUnknownConfiguration = true);
        section.Bind(options);

        services.AddScoped(_ => TranslationClient.CreateFromApiKey(options.ApiKey));
        
        services.AddScoped<ITextTranslationService, GoogleTextTranslationService>();

        return services;
    }

    #endregion

    #region Google File Storage
    
    public static IServiceCollection AddGoogleFileStorage(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        
        GoogleStorageOptions options = new();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(GoogleStorageOptions.SectionName);
        services.Configure<GoogleStorageOptions>(section, binderOptions => binderOptions.ErrorOnUnknownConfiguration = true);
        section.Bind(options);

        var credentials = GoogleCredential.FromFile(options.CredentialFileJson);
        
        services.AddScoped(_ => new Lazy<StorageClient>(() => StorageClient.Create(credentials)));
        
        services.AddScoped<IFileStorageService, GoogleFileStorageService>();

        return services;
    }
    
    #endregion
    
    #region Quartz
    
    public static IServiceCollection AddQuartzScheduling(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("Postgres");

        services.AddQuartz(quartzConfigurator =>
        {
            quartzConfigurator.ScheduleJob<TheMealDbRecipeCategoriesCachingJob>(triggerConfigurator =>
            {
                triggerConfigurator.ForJob(nameof(TheMealDbRecipeCategoriesCachingJob));
                triggerConfigurator.WithIdentity(nameof(TheMealDbRecipeCategoriesCachingJob));
                triggerConfigurator.StartNow();
                triggerConfigurator.WithSimpleSchedule(scheduleBuilder =>
                {
                    scheduleBuilder.RepeatForever();
                    scheduleBuilder.WithIntervalInHours(12);
                });
            });
            
            quartzConfigurator.ScheduleJob<TheMealDbRecipesCachingJob>(triggerConfigurator =>
            {
                triggerConfigurator.ForJob(nameof(TheMealDbRecipesCachingJob));
                triggerConfigurator.WithIdentity(nameof(TheMealDbRecipesCachingJob));
                triggerConfigurator.StartNow();
                triggerConfigurator.WithSimpleSchedule(scheduleBuilder =>
                {
                    scheduleBuilder.RepeatForever();
                    scheduleBuilder.WithIntervalInHours(12);
                });
            });

            quartzConfigurator.ScheduleJob<TheMealDbIngredientsCachingJob>(triggerConfigurator =>
            {
                triggerConfigurator.ForJob(nameof(TheMealDbIngredientsCachingJob));
                triggerConfigurator.WithIdentity(nameof(TheMealDbIngredientsCachingJob));
                triggerConfigurator.StartNow();
                triggerConfigurator.WithSimpleSchedule(scheduleBuilder =>
                {
                    scheduleBuilder.RepeatForever();
                    scheduleBuilder.WithIntervalInHours(12);
                });
            });
            
            quartzConfigurator.ScheduleJob<IngredientsSearcherJob>(triggerConfigurator =>
            {
                triggerConfigurator.ForJob(nameof(IngredientsSearcherJob));
                triggerConfigurator.WithIdentity(nameof(IngredientsSearcherJob));
                triggerConfigurator.StartNow();
                triggerConfigurator.WithSimpleSchedule(scheduleBuilder =>
                {
                    scheduleBuilder.RepeatForever();
                    scheduleBuilder.WithIntervalInHours(12);
                });
            });

            quartzConfigurator.UseDefaultThreadPool(threadPoolOptions =>
            {
                threadPoolOptions.MaxConcurrency = Environment.ProcessorCount;
            });

            quartzConfigurator.UseMicrosoftDependencyInjectionJobFactory();

            quartzConfigurator.UsePersistentStore(persistentStoreOptions =>
            {

                persistentStoreOptions.UseProperties = true;
                persistentStoreOptions.UsePostgres(connectionString);
                persistentStoreOptions.UseJsonSerializer();
            });
        });
            
        services.AddQuartzServer(options =>
        {
            options.AwaitApplicationStarted = true;
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
    
    #endregion

    #region Postgres

    public static IServiceCollection AddEfPostgres(this IServiceCollection services) 
    {
        using var serviceProvider = services.BuildServiceProvider();
        
        EfDatabaseOptions dbOptions = new();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("Postgres");
        var section = configuration.GetSection(EfDatabaseOptions.SectionName);
        services.Configure<EfDatabaseOptions>(section);
        section.Bind(dbOptions);
        
        services.AddDbContextPool<CookifyDbContext>(dbContextOptionsBuilder =>
        {
            dbContextOptionsBuilder.EnableSensitiveDataLogging();
            dbContextOptionsBuilder.AddInterceptors(new EntitySaveChangesInterceptor());
            dbContextOptionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(CookifyDbContext).Assembly.FullName);
            });
        });

        services.AddScoped<IUnitOfWork, EfUnitOfWork<CookifyDbContext>>();

        return services;
    }

    #endregion
}