using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

namespace MediatREndPoints.Contracts.Models;

/// <summary>
/// Sets up API endpoint settings
/// </summary>
public class EndPointBuilder
{
    private EndPointTypes _endPointType; 
    private string _endPointName;
    private string _endPointGroup;
    private string _endPointAddress;
    private string _endPointTag;
    private Func<OpenApiOperation, OpenApiOperation>? _openApiOperation;
    private Action<AuthorizationPolicyBuilder>? _authorizationPolicyBuilder;
    private IEndpointFilter _endpointFilter;
    /// <summary>
    /// Sets up the end point action type
    /// </summary>
    /// <param name="endPointType"></param>
    /// <returns></returns>
    public EndPointBuilder WithEndPointType(EndPointTypes endPointType)
    {
        this._endPointType = endPointType;
        return this;
    }


    /// <summary>
    /// Sets up end point name to use in minimal API
    /// </summary>
    /// <param name="endPointName"></param>
    /// <returns></returns>
    public EndPointBuilder WithEndPointName(string endPointName)
    {
        ArgumentNullException.ThrowIfNull(endPointName);

        this._endPointName = endPointName;
        return this;
    }

    /// <summary>
    /// Sets up end point group name to use in minimal Api
    /// </summary>
    /// <param name="endPointGroup"></param>
    /// <returns></returns>
    public EndPointBuilder WithEndPointGroup(string endPointGroup)
    {
        ArgumentNullException.ThrowIfNull(endPointGroup);
        this._endPointGroup = endPointGroup;
        return this;
    }

    /// <summary>
    /// Sets up end point address to use in minimal Api
    /// </summary>
    /// <param name="endPointAddress"></param>
    /// <returns></returns>
    public EndPointBuilder WithEndPointAddress(string endPointAddress)
    {
        ArgumentNullException.ThrowIfNull(endPointAddress);
        this._endPointAddress = endPointAddress;
        return this;
    }

    /// <summary>
    /// Sets up end point tag to use in minimal Api
    /// </summary>
    /// <param name="endPointTag"></param>
    /// <returns></returns>
    public EndPointBuilder WithEndPointTag(string endPointTag)
    {
        ArgumentNullException.ThrowIfNull(endPointTag);

        this._endPointTag = endPointTag;
        return this;
    }

    /// <summary>
    /// Sets up open api configurations for using in swagger panel
    /// </summary>
    /// <param name="openApiOperation"></param>
    /// <returns></returns>
    public EndPointBuilder WithOpenApiOperation(Func<OpenApiOperation, OpenApiOperation> openApiOperation)
    {
        ArgumentNullException.ThrowIfNull(openApiOperation);

        this._openApiOperation = openApiOperation;
        return this;
    }


    /// <summary>
    /// Sets up authorization policy for specific end point
    /// </summary>
    /// <param name="authorizationPolicyBuilder"></param>
    /// <returns></returns>
    public EndPointBuilder WithAuthorizationPolicy(Action<AuthorizationPolicyBuilder> authorizationPolicyBuilder)
    {
        ArgumentNullException.ThrowIfNull(authorizationPolicyBuilder);

        this._authorizationPolicyBuilder = authorizationPolicyBuilder;

        return this;
    }

    /// <summary>
    /// Specifies an EndPoint Filter To Be Used For This EndPoint
    /// </summary>
    /// <param name="endpointFilter"></param>
    /// <returns></returns>
    public EndPointBuilder WithEndPointFilter(IEndpointFilter endpointFilter)
    {
        this._endpointFilter= endpointFilter;
        return this;
    }

    /// <summary>
    /// Builds the EndPointModel Class to use for minimal API configuration
    /// </summary>
    /// <returns></returns>
    public EndPointModel Build()
    {
        ArgumentNullException.ThrowIfNull(this._endPointName);
        ArgumentNullException.ThrowIfNull(this._endPointAddress);

        return new EndPointModel(this._endPointType
            , this._endPointName
            , this._endPointGroup
            , this._endPointAddress
            , this._endPointTag
            , this._openApiOperation
            , this._authorizationPolicyBuilder
            , _endpointFilter);
    }

}