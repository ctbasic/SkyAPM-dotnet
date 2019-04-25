/*
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
using System.IO;
using System.Web;
using System.Xml;
using SkyApm.Common;
using SkyApm.Config;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;
using SpanLayer = SkyApm.Tracing.Segments.SpanLayer;
using SkyApm.Agent.AspNet.CtCustom;

namespace SkyApm.Agent.AspNet
{
    internal class InstrumentRequestCallback
    {
        private readonly InstrumentConfig _config;
        private readonly ITracingContext _tracingContext;
        private readonly IEntrySegmentContextAccessor _contextAccessor;

        public InstrumentRequestCallback(IConfigAccessor configAccessor, ITracingContext tracingContext,
            IEntrySegmentContextAccessor contextAccessor)
        {
            _config = configAccessor.Get<InstrumentConfig>();
            _tracingContext = tracingContext;
            _contextAccessor = contextAccessor;
        }

        public void ApplicationOnBeginRequest(object sender, EventArgs e)
        {
            HttpApplication httpApplication = sender as HttpApplication;
            if (httpApplication == null)
            {
                return;
            }
            HttpContext httpContext = httpApplication.Context;

            if (httpContext.Request.HttpMethod == "OPTIONS")
            {
                //asp.net Exclude OPTIONS request
                return;
            }
            //using (Stream inputStream= httpContext.Request.GetBufferedInputStream())
            //{
            //    TextReader reader = new StreamReader(inputStream);
            //    string inputContet = reader.ReadToEnd();
            //}


                var context = _tracingContext.CreateEntrySegmentContext(httpContext.Request.Path,
                    new HttpRequestCarrierHeaderCollection(httpContext.Request));
            context.Span.SpanLayer = SpanLayer.HTTP;
            context.Span.Peer = new StringOrIntValue(httpContext.Request.UserHostAddress);
            context.Span.Component = Common.Components.ASPNET;
            context.Span.AddTag(Tags.URL, httpContext.Request.Url.OriginalString);
            context.Span.AddTag(Tags.PATH, httpContext.Request.Path);
            context.Span.AddTag(Tags.HTTP_METHOD, httpContext.Request.HttpMethod);
            context.Span.AddLog(LogEvent.Event("AspNet BeginRequest"),
                LogEvent.Message(
                    $"Request starting {httpContext.Request.Url.Scheme} {httpContext.Request.HttpMethod} {httpContext.Request.Url.OriginalString}"));
            AspNetWebUtils.SegmentContext = context;
        }

        public void ApplicationOnEndRequest(object sender, EventArgs e)
        {
            var context = _contextAccessor.Context ?? AspNetWebUtils.SegmentContext;
            if (context == null)
            {
                return;
            }
            
            var httpApplication = sender as HttpApplication;
            var httpContext = httpApplication.Context;
            if (httpContext.Request.HttpMethod == "OPTIONS")
            {
                //asp.net Exclude OPTIONS request
                return;
            }
            
            var statusCode = httpContext.Response.StatusCode;
            if (statusCode >= 400)
            {
                context.Span.ErrorOccurred();
            }
            
            var exception = httpContext.Error;
            if (exception != null)
            {
                context.Span.ErrorOccurred(exception);
            }

            context.Span.AddLog(LogEvent.Event("AspNet EndRequest"),
                LogEvent.Message(
                    $"Request finished {httpContext.Response.StatusCode} {httpContext.Response.ContentType}"));
            
            _tracingContext.Release(context);
        }
    }
}