using Application.Contracts;
using Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;

namespace Application.Common.BehavioursPipe
{
    public class CachedQueryBehaviours<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICacheQuery, IRequest<TResponse>
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CachedQueryBehaviours(IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;
            var Key = GenerateKey();
            var cachedResponse = await _cache.GetAsync(Key, cancellationToken);
            if (cachedResponse!=null)
            {
                response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
            }
            else
            {
                response = await next();//go to get the response
                var serialized = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
                await CreateNewCache(request, Key, cancellationToken, serialized);
            }
            return response;
        }
        private Task CreateNewCache(TRequest request,string Key, CancellationToken cancellationToken, byte[] serialized)
        {
            return _cache.SetAsync(
                Key, serialized, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeToLive(request)
                }
                , cancellationToken);
        }
        private static TimeSpan TimeToLive(TRequest request)
        {
            return new TimeSpan(request.HoursSaveData, 0, 0, 0);
        }
        private string GenerateKey() 
        {
            return IdGenerator.GenerateCacheKeyFromRequest(_httpContextAccessor.HttpContext.Request);
        }
    }
}
