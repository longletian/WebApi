using WebApi.Models;

namespace WebApi.Services.IService
{
    public interface IApiService
    {
        ResponseData AddApiService();

        ResponseData UploadApiService();

        ResponseData DeleteApiService(long id);

        ResponseData GetApiList();
    }
}
