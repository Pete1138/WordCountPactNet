using System;
using System.Fabric;

namespace WordCount.WebService
{
    public class ServiceUriResolver : IServiceUriResolver
    {
        private readonly ICodePackageActivationContext _activationContext;

        public ServiceUriResolver(ICodePackageActivationContext activationContext)
        {
            _activationContext = activationContext;
        }

        public Uri GetServiceUri()
        {
            return new Uri(_activationContext.ApplicationName + "/WordCountService");
        }
    }
}
