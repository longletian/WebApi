
using AutoMapper;
using WebApi.Models;
using WebApi.Repository;
using WebApi.Services.IService;

namespace WebApi.Services.Service
{
    public class ApiService : BaseService<ApiEntity>, IApiService
    {
        private readonly IMapper mapper;
        private readonly IBaseEntityRepository<ApiEntity> baseApiRepository;

        public ApiService(
            IMapper mapper,
            IBaseEntityRepository<ApiEntity> baseApiRepository)
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
            var result = this.baseApiRepository.Orm.Select<ApiEntity>().Page(1, 10).ToList();
            return new ResponseData { MsgCode = 200, Message = "请求成功", Data = new { dataNum = result.Count, dataList = result } };

        }

        public ResponseData UploadApiService()
        {
            throw new System.NotImplementedException();
        }
    }
}
