using FakeItEasy;
using FakeItEasy.Core;
using Microsoft.Owin.Testing;
using PactNet;
using PactNet.Reporters.Outputters;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Xunit;

namespace WordCount.Service.Tests
{
    public interface IFoo
    {
        string GetFoo(string input, int number, bool isValid);
    }

    public class WordCountServiceApiTests : IDisposable
    {
        private TestServer _server;


        [Fact]
        public async Task EnsureWordCountServiceApiHonoursPactWithConsumer()
        {
            //Arrange
            var outputter = new CustomOutputter();
            var config = new PactVerifierConfig();
            config.ReportOutputters.Add(outputter);
            IPactVerifier pactVerifier = new PactVerifier(() => { }, () => { }, config);

            pactVerifier
                .ProviderState(
                    "there is a count of 1 for words beginning with 'A'",
                    setUp: IncrementWordCountForLetterA);

            var configuration = new HttpConfiguration();

            _server = TestServer.Create(app =>
            {
                var apiStartup = new Startup();
                apiStartup.Configuration(app, configuration);
            });

            //var result = await _server.HttpClient.PutAsync("AddWord/AardVark", null);
            using (var client = new HttpClient(_server.Handler))
            {
                // requires routing setup in Startup
                //var response = await client.PutAsync("http://localhost/api/default/AddWord/Ardvark", null);
                var response = await client.GetAsync("http://localhost/api/default/Count");
                var result = await response.Content.ReadAsAsync<string>();
            }

            //Act / Assert
            pactVerifier
                   .ServiceProvider("WordCountServiceApi", _server.HttpClient)
                   .HonoursPactWith("WordCountWebService")
                   .PactUri("../../../../WordCount.WebService.Tests/bin/pacts/wordcountwebservice-wordcountserviceapi.json")
                   .Verify();

            // Verify that verifaction log is also sent to additional reporters defined in the config
            Assert.Contains("Verifying a Pact between WordCountWebService and WordCountService API", outputter.Output);
        }

        private void IncrementWordCountForLetterA()
        {

        }

        public virtual void Dispose()
        {
            if (_server != null)
            {
                _server.Dispose();
            }
        }

        private class CustomOutputter : IReportOutputter
        {
            public string Output { get; private set; }

            public void Write(string report)
            {
                Output += report;
            }
        }
    }
}
