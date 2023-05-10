using MediatR;
using MediatREndPoints.Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace MediatREndPoints.Contracts;

/// <summary>
/// Sets up MediatR requests to use minimal APIs and REPR pattern
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IRequestEndPoint:IRequest<IResult>
{
    /// <summary>
    /// Sets up the endpoint model to use in minimal API configuration
    /// </summary>
    /// <returns>an instance of EndPointModel</returns>
    EndPointModel SetUpEndPointModel();
}