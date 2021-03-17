
using AutoMapper;
using WebApi.Models;
using WebApi.Repository;
using WebApi.Services.IService;

namespace WebApi.Services.Service
{
    public class ApiService :BaseService<ApiModel>, IApiService
    {
        private readonly IFreeSql freeSql;
        private readonly IMapper mapper;
        private readonly IBaseEntityRepository<ApiModel,long> baseApiRepository;

        public ApiService(
            IFreeSql freeSql,
            IMapper mapper,
            IBaseEntityRepository<ApiModel, long> baseApiRepository)
        {
            this.freeSql = freeSql;
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
            throw new System.NotImplementedException();
        }

        public ResponseData UploadApiService()
        {
            throw new System.NotImplementedException();
        }
    }
}
