using MediatR;
using Microsoft.AspNetCore.Http;

namespace MediatREndPoints.Contracts;

public interface IRequestEndPointHandler<in TEndPointRequestModel>:IRequestHandler<TEndPointRequestModel,IResult>
where TEndPointRequestModel:IRequestEndPoint
{
    
}