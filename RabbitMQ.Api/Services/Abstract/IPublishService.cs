using RabbitMQ.Shared.Dto;
using System.Threading.Tasks;

namespace RabbitMQ.Api.Services.Abstract
{
    public interface IPublishService
    {
        Task PublishQuee(EmailSendRequestDto dto);

    }
}
