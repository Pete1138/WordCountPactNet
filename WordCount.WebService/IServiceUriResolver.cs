using System;

namespace WordCount.WebService
{
    public interface IServiceUriResolver
    {
        Uri GetServiceUri();
    }
}
