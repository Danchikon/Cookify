using System.Net;
using AngleSharp;
using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Favorite;
using Cookify.Domain.Ingredient;
using Cookify.Domain.IngredientUser;
using Cookify.Domain.Like;
using Cookify.Domain.MealCategory;
using Cookify.Domain.ProductMarket;
using Cookify.Domain.Recipe;
using Cookify.Domain.RecipeCategory;
using Cookify.Domain.Session;
using Cookify.Domain.User;
using Cookify.Infrastructure.Common.Helpers;
using Cookify.Infrastructure.Common.Seeders;
using Cookify.Infrastructure.Options;
using Cookify.Infrastructure.Persistence;
using Cookify.Infrastructure.Persistence.Interceptors;
using Cookify.Infrastructure.Persistence.Seeders;
using Cookify.Infrastructure.Repositories;
using Cookify.Infrastructure.Scheduling.Jobs;
using Cookify.Infrastructure.Services;
using Cookify.Infrastructure.Services.FileDownloader;
using Cookify.Infrastructure.Services.FileStorages;
using Cookify.Infrastructure.Services.ProductMarkets;
using Cookify.Infrastructure.Services.RestApis;
using Cookify.Infrastructure.Services.Translation;
using Cookify.Infrastructure.UnitOfWork;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Google.Cloud.Translation.V2;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using IronPdf;
using IronPdf.Rendering.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Quartz;
using Refit;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Cookify.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddCurrentUserService();
        services.AddInternetFileDownloader();
        services.AddGoogleImageSearcher();
        services.AddTheMealDbApi();
        services.AddSilpoProductMarketService();
        services.AddIronPdf();
        services.AddQuartzScheduling();
        services.AddEfPostgres();
        services.AddSeeders();
        services.AddRepositories();

        if (AspNetCoreEnvironment.IsProduction)
        {
            services.AddGoogleTextTranslation();
            services.AddGoogleFileStorage();
        }
        else
        {
            services.AddMinioFileStorage();
            services.AddScoped<ITextTranslationService, FakeTextTranslationService>();
        }
        
        return services;
    }

    #region Current User Service

    public static IServiceCollection AddCurrentUserService(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, HttpCurrentUserService>();
        services.AddScoped<Lazy<ICurrentUserService>>(provider => new Lazy<ICurrentUserService>(provider.GetRequiredService<ICurrentUserService>));
        
        return services;
    }

    #endregion

    #region IronPdf

    public static IServiceCollection AddIronPdf(this IServiceCollection services)
    {
        Installation.LinuxAndDockerDependenciesAutoConfig = false;
        Installation.ChromeGpuMode = IronPdf.Engines.Chrome.ChromeGpuModes.Disabled;
        Installation.Initialize();

        services.AddScoped<BasePdfRenderer, ChromePdfRenderer>();
        
        return services;
    }
    
    #endregion

    #region Internet File Downloader

    public static IServiceCollection AddInternetFileDownloader(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(InternetFileDownloaderOptions.SectionName);
        services.Configure<InternetFileDownloaderOptions>(section);
        
        services.AddScoped<WebClient, FileWebClient>();
        services.AddScoped<IInternetFileDownloaderService, InternetFileDownloaderService>();

        return services;
    }
    
    #endregion

    #region Seeders

    public static IServiceCollection AddSeeders(this IServiceCollection services)
    {
        services.AddScoped<SeederBase, ProductMarketsSeeder>();

        return services;
    }
    
    #endregion

    #region Google Image Searcher

    public static IServiceCollection AddGoogleImageSearcher(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(GoogleImageSearcherOptions.SectionName);
        services.Configure<GoogleImageSearcherOptions>(section);
        
        services.AddScoped<IBrowsingContext>(_ => BrowsingContext.New(Configuration.Default.WithDefaultLoader()));
        services.AddScoped<IImageSearcherService, GoogleImageSearcherService>();

        return services;
    }

    #endregion

    #region TheMealDBApi

    public static IServiceCollection AddTheMealDbApi(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();

        TheMealDbApiOptions options = new();
        
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(TheMealDbApiOptions.SectionName);
        services.Configure<TheMealDbApiOptions>(section);
        section.Bind(options);
        
        services
            .AddRefitClient<ITheMealDbApi>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.Timeout = TimeSpan.FromSeconds(options.TimeoutInSeconds);
                httpClient.BaseAddress = new Uri(options.Url);
            });

        return services;
    }
    
    #endregion

    #region Product Market Services

    public static IServiceCollection AddSilpoProductMarketService(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();

        SilpoProductMarketOptions options = new();
        
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(SilpoProductMarketOptions.SectionName);
        services.Configure<SilpoProductMarketOptions>(section);
        section.Bind(options);

        services.AddScoped<IGraphQLClient>(_ => new GraphQLHttpClient(options.ApiUrl, new SystemTextJsonSerializer()));
        services.AddScoped<SilpoProductMarketService>();

        return services;
    }
    
    #endregion

    #region Repositories

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, EfUnitOfWork<CookifyDbContext>>();
        
        services.AddScoped<IRecipeCategoriesRepository, RecipeCategoriesRepository>();
        services.AddScoped<IIngredientsRepository, IngredientsRepository>();
        services.AddScoped<IProductMarketsRepository, ProductMarketsRepository>();
        services.AddScoped<IRecipesRepository, RecipesRepository>();
        services.AddScoped<ISessionsRepository, SessionsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IFavoritesRepository, FavoritesRepository>();
        services.AddScoped<IFavoritesRepository, FavoritesRepository>();
        services.AddScoped<ILikesRepository, LikesRepository>();
        services.AddScoped<IIngredientUsersRepository, IngredientUsersRepository>();
        
        return services;
    }
    
    #endregion

    #region Google Translation 

    public static IServiceCollection AddGoogleTextTranslation(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        
        GoogleTranslationOptions options = new();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(GoogleTranslationOptions.SectionName);
        services.Configure<GoogleTranslationOptions>(section);
        section.Bind(options);

        services.AddScoped(_ =>
        {
            var client = TranslationClient.CreateFromApiKey(options.ApiKey);
            client.Service.HttpClient.Timeout = TimeSpan.FromMinutes(10);
            
            return client;
        });
        
        services.AddScoped<ITextTranslationService, GoogleTextTranslationService>();

        return services;
    }

    #endregion

    #region Minio File Storage

    public static IServiceCollection AddMinioFileStorage(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        
        MinioStorageOptions options = new();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(MinioStorageOptions.SectionName);
        services.Configure<MinioStorageOptions>(section);
        section.Bind(options);
        
        services.AddScoped<IMinioClient>(_ => new MinioClient()
            .WithEndpoint(options.Endpoint, options.Port)
            .WithCredentials(options.Login, options.Password)
            .Build()
        );
        
        services.AddScoped<IFileStorageService, MinioFileStorageService>();

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
        services.Configure<GoogleStorageOptions>(section);
        section.Bind(options);

        var credentials = GoogleCredential.FromFile(options.CredentialFileJson);
        
        services.AddScoped(_ => new Lazy<StorageClient>(() => StorageClient.Create(credentials)));
        
        services.AddScoped<IFileStorageService, GoogleFileStorageService>();

        return services;
    }
    
    #endregion
    
    #region Quartz Scheduling
    
    public static IServiceCollection AddQuartzScheduling(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("Postgres");

        services.AddQuartz(quartzConfigurator =>
        {
            quartzConfigurator.ScheduleJob<RecipePdfGeneratorJob>(triggerConfigurator =>
            {
                triggerConfigurator.ForJob(nameof(RecipePdfGeneratorJob));
                triggerConfigurator.WithIdentity(nameof(RecipePdfGeneratorJob));
                triggerConfigurator.StartAt(DateTimeOffset.Now.AddMinutes(10));
                triggerConfigurator.WithSimpleSchedule(scheduleBuilder =>
                {
                    scheduleBuilder.RepeatForever();
                    scheduleBuilder.WithIntervalInMinutes(10);
                });
            });
            
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
                triggerConfigurator.StartAt(DateTimeOffset.Now.AddMinutes(5));
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

    #region Entity Framework Postgres

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
            if (!AspNetCoreEnvironment.IsProduction)
            {
                dbContextOptionsBuilder.EnableSensitiveDataLogging();
            }
            dbContextOptionsBuilder.AddInterceptors(new EntitySaveChangesInterceptor());
            dbContextOptionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(CookifyDbContext).Assembly.FullName);
            });
        });

        return services;
    }

    #endregion
}