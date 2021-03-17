using MediatR;

namespace WebApi.Common
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {

    }
}
