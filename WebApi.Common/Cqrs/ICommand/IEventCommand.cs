using MediatR;

namespace WebApi.Common
{
    /// <summary>
    /// 具有返回值
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface IEventCommand<out TResponse> : IRequest<TResponse>
    {}

    /// <summary>
    ///  为了实现cqrs
    ///  IRequest 单播
    /// </summary>
    public interface IEventCommand : IRequest
    {}
}
