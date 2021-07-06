using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetCorePlayground.Tests.Mocks
{
    public class MockHttpMessageHandler : DelegatingHandler
    {
        private readonly HttpResponseMessage _httpResponse;
        private readonly Func<HttpRequestMessage, bool> _validator;

        public MockHttpMessageHandler(HttpResponseMessage httpResponse)
        {
            _httpResponse = httpResponse;
        }

        public MockHttpMessageHandler(HttpResponseMessage httpResponse, Func<HttpRequestMessage, bool> validator)
            : this(httpResponse)
        {
            _validator = validator;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken)
        {
            if (_validator != null && !_validator(httpRequest))
                throw new ValidationException("The request validation faild");

            return await Task.FromResult(_httpResponse);
        }
    }
}
