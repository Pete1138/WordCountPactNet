﻿using PactNet;
using PactNet.Mocks.MockHttpService;
using System;

namespace WordCount.Service.Tests
{
    public class WordCountWebServiceApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }
        public int MockServerPort { get { return 1234; } }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

        public WordCountWebServiceApiPact()
        {
            PactBuilder = new PactBuilder()
                .ServiceConsumer("WordCountWebService")
                .HasPactWith("WordCountServiceApi");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}
