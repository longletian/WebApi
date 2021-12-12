using MediatR;
namespace WebApi.Common
{
    public interface IEventCommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IEventCommand<TResponse>
    {}
    public interface IEventCommandHandler<in TRequest> : IRequestHandler<TRequest> where TRequest : IEventCommand
    {}
}
