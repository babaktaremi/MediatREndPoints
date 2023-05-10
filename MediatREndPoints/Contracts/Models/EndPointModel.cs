using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace MediatREndPoints.Contracts.Models;

/// <summary>
/// Sets up end point settings to use in Minimal API . Use EndPointBuilder to construct end point model
/// </summary>
public class EndPointModel
{
    public EndPointTypes EndPointType { get;  }

    public string EndPointName { get;  }
    public string EndPointGroupName { get; }
    public string EndPointAddress { get; }
    public string EndPointTag { get; }
    public Func<OpenApiOperation, OpenApiOperation>? OpenApiOperation { get; }
    public Action<AuthorizationPolicyBuilder> ? AuthorizationPolicy { get; }

    internal EndPointModel(EndPointTypes endPointType, string endPointName, string endPointGroupName, string endPointAddress, string endPointTag, Func<OpenApiOperation, OpenApiOperation>? openApiOperation, Action<AuthorizationPolicyBuilder>? authorizationPolicy)
    {
        EndPointType = endPointType;
        EndPointName = endPointName;
        EndPointGroupName = endPointGroupName;
        EndPointAddress = endPointAddress;
        EndPointTag = endPointTag;
        OpenApiOperation = openApiOperation;
        AuthorizationPolicy = authorizationPolicy;
    }
}