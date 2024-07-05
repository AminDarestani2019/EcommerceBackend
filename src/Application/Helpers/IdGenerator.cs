using Microsoft.AspNetCore.Http;
using System.Text;

namespace Application.Helpers
{
    public class IdGenerator
    {
        public static string GenerateCacheKeyFromRequest(HttpRequest request)
        { 
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");//save the path
            foreach ( var (key,value) in request.Query.OrderBy(x=>x.Key)) 
                keyBuilder.Append($"|{key}-{value}");//save query
            return keyBuilder.ToString();
        }
    }
}
