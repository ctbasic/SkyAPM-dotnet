﻿/*
 * Licensed to the SkyAPM under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The SkyAPM licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;
using SpanLayer = SkyApm.Tracing.Segments.SpanLayer;

namespace SkyApm.Agent.AspNet
{
    public class HttpTracingHandler : DelegatingHandler
    {
        public HttpTracingHandler()
            : this(new HttpClientHandler())
        {
        }

        public HttpTracingHandler(HttpMessageHandler innerHandler)
        {
            InnerHandler = innerHandler;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var tracingContext = ServiceLocator.Current.GetInstance<ITracingContext>();
            var operationName = request.RequestUri.ToString();
            var networkAddress = $"{request.RequestUri.Host}:{request.RequestUri.Port}";
            var context = tracingContext.CreateExitSegmentContext(operationName, networkAddress,
                new CarrierHeaderCollection(request.Headers));
            try
            {
                context.Span.SpanLayer = SpanLayer.HTTP;
                context.Span.Component = Common.Components.HTTPCLIENT;
                context.Span.AddTag(Common.Tags.URL, request.RequestUri.ToString());
                context.Span.AddTag(Common.Tags.PATH, request.RequestUri.PathAndQuery);
                context.Span.AddTag(Common.Tags.HTTP_METHOD, request.Method.ToString());
                var response = base.SendAsync(request, cancellationToken).GetAwaiter().GetResult();

                var statusCode = (int)response.StatusCode;
                if (statusCode >= 400)
                {
                    context.Span.ErrorOccurred();
                }

                context.Span.AddTag(Common.Tags.STATUS_CODE, statusCode);
                return Task.FromResult(response);
            }
            catch (Exception exception)
            {
                context.Span.ErrorOccurred(exception);
                throw;
            }
            finally
            {
                tracingContext.Release(context);
            }
        }

        private class CarrierHeaderCollection : ICarrierHeaderCollection
        {
            private readonly HttpRequestHeaders _headers;

            public CarrierHeaderCollection(HttpRequestHeaders headers)
            {
                _headers = headers;
            }

            public void Add(string key, string value)
            {
                _headers.Add(key, value);
            }

            public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}