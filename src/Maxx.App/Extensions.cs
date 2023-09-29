using Carter;

using FluentValidation;

using Maxx.Application.Behaviors;
using Maxx.Infrastructure.BackgroundJobs;
using Maxx.Infrastructure.Idempotence;
using Maxx.Persistence;
using Maxx.Persistence.Interceptors;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Quartz;

namespace Maxx.App;

public static class Extensions
{
    public static IServiceCollection ConfigureCarterEndpoints(this IServiceCollection services)
    {
        services.AddCarter(new(Maxx.Application.AssemblyReference.Assembly));

        return services;
    }
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Maxx.Application.AssemblyReference.Assembly);
        });

        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        return services;
    }
    public static IServiceCollection ConfigureValidators(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services.AddValidatorsFromAssembly(
            Maxx.Application.AssemblyReference.Assembly,
            includeInternalTypes: true);

        return services;
    }
    public static IServiceCollection ConfigureScrutor(this IServiceCollection services)
    {
        services
            .Scan(
                selector => selector
                    .FromAssemblies(
                        Infrastructure.AssemblyReference.Assembly,
                        AssemblyReference.Assembly)
                    .AddClasses(false)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //todo: add xml doc files

        return services;
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddDbContext<ApplicationDbContext>(
            (sp, optionsBuilder) =>
            {
                var outboxInterceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()!;
                var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>()!;

                optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"))
                    .AddInterceptors(
                        outboxInterceptor,
                        auditableInterceptor);
            });

        return services;
    }

    public static IServiceCollection ConfigureQuartz(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(100)
                                        .RepeatForever()));
        });

        services.AddQuartzHostedService();

        return services;
    }
}
