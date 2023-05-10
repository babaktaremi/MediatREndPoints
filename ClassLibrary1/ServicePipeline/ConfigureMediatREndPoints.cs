using System;
using System.Reflection;
using MediatR;
using MediatREndPoints.Contracts;
using MediatREndPoints.Contracts.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MediatREndPoints.ServicePipeline;

public static class ConfigureMediatREndPoints
{
    /// <summary>
    /// Configures EndPoints For MediatR Requests
    /// </summary>
    /// <param name="services"></param>
    /// <param name="mediatRConfiguration"></param>
    /// <returns></returns>
    public static IServiceCollection AddMediatREndPoints(this IServiceCollection services,
        Action<MediatRServiceConfiguration> mediatRConfiguration)
    {
        services.AddEndpointsApiExplorer();

        services.AddHttpContextAccessor();

        services.AddMediatR(mediatRConfiguration);
        return services;
    }

    /// <summary>
    /// Setups application to use MediatR Requests and Handlers in Minimal APIs
    /// </summary>
    /// <param name="app"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <returns></returns>
    public static WebApplication UseMediatREndPointFor<TRequestEndPointModel>(this WebApplication app)
    where TRequestEndPointModel : IRequestEndPoint
    {

        var type = typeof(TRequestEndPointModel);
        var typeConstructorArgumentLength = type.GetConstructors().First().GetParameters().Length;


        var methodInfo = type.GetMethod(nameof(IRequestEndPoint.SetUpEndPointModel));


        if (Activator.CreateInstance(type, new object[typeConstructorArgumentLength]) is IRequestEndPoint requestModel)
        {
            if (methodInfo?.Invoke(requestModel, null) is EndPointModel endPointModel)
            {
                RouteHandlerBuilder? builder = default;
                switch (endPointModel.EndPointType)
                {
                    case EndPointTypes.Post:
                        builder = app.MapPost(endPointModel.EndPointAddress,
                                async (TRequestEndPointModel model, ISender sender) => await sender.Send(model))
                            .WithName(endPointModel.EndPointName);
                        break;
                    case EndPointTypes.Get:
                        builder = app.MapGet(endPointModel.EndPointAddress,
                                async ([AsParameters] TRequestEndPointModel model, ISender sender) =>
                                    await sender.Send(model))
                            .WithName(endPointModel.EndPointName);
                        break;
                    case EndPointTypes.Put:
                        builder = app.MapPut(endPointModel.EndPointAddress,
                                async (TRequestEndPointModel model, ISender sender) => await sender.Send(model))
                            .WithName(endPointModel.EndPointName);

                        break;
                    case EndPointTypes.Patch:
                        builder = app.MapPatch(endPointModel.EndPointAddress,
                                async (TRequestEndPointModel model, ISender sender) => await sender.Send(model))
                            .WithName(endPointModel.EndPointName);
                        break;
                    case EndPointTypes.Delete:
                        builder = app.MapDelete(endPointModel.EndPointAddress,
                                async (TRequestEndPointModel model, ISender sender) => await sender.Send(model))
                            .WithName(endPointModel.EndPointName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (endPointModel.OpenApiOperation != null)
                    builder.WithOpenApi(endPointModel.OpenApiOperation);
                else
                    builder.WithOpenApi();

                if (!string.IsNullOrEmpty(endPointModel.EndPointGroupName))
                    builder.WithGroupName(endPointModel.EndPointGroupName);

                if (!string.IsNullOrEmpty(endPointModel.EndPointTag))
                    builder.WithTags(endPointModel.EndPointTag);

                if (endPointModel.AuthorizationPolicy != null)
                    builder.RequireAuthorization(endPointModel.AuthorizationPolicy);
            }
        }
        return app;
    }
}