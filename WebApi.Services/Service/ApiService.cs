
using AutoMapper;
using WebApi.Models;
using WebApi.Repository;
using WebApi.Services.IService;

namespace WebApi.Services.Service
{
    public class ApiService :BaseService<ApiModel>, IApiService
    {
        private readonly IMapper mapper;
        private readonly IBaseEntityRepository<ApiModel> baseApiRepository;

        public ApiService(
            IMapper mapper,
            IBaseEntityRepository<ApiModel> baseApiRepository)
        {
            this.mapper = mapper;
            base.baseRepository = baseApiRepository;
            this.baseApiRepository = baseApiRepository;
        }

        public ResponseData AddApiService()
        {
            throw new System.NotImplementedException();
        }

        public ResponseData DeleteApiService(long id)
        {
            throw new System.NotImplementedException();
        }

        public ResponseData GetApiList()
        {
            var result = this.baseApiRepository.Orm.Select<ApiModel>().Page(1, 10).ToList();
            return new ResponseData { MsgCode = 200, Message = "请求成功", Data = new { dataNum = result.Count, dataList = result }         }

        public ResponseData UploadApiService()
        {
            throw new System.NotImplementedException();
        }
    }
}
