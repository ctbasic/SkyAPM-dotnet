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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using SkyApm.Common;
using SkyApm.Config;
using SkyApm.Diagnostics;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;

namespace SkyApm.AspNetCore.Diagnostics
{
    public class HostingTracingDiagnosticProcessor : ITracingDiagnosticProcessor
    {
        public string ListenerName { get; } = "Microsoft.AspNetCore";

        private readonly ITracingContext _tracingContext;
        private readonly IEntrySegmentContextAccessor _segmentContextAccessor;

        private readonly EndPointPolicyConfig _endPointPolicyConfig;

        public HostingTracingDiagnosticProcessor(IEntrySegmentContextAccessor segmentContextAccessor,
            ITracingContext tracingContext, IConfigAccessor configAccessor)
        {
            _tracingContext = tracingContext;
            _segmentContextAccessor = segmentContextAccessor;
            _endPointPolicyConfig = configAccessor.Get<EndPointPolicyConfig>();
        }

        [DiagnosticName("Microsoft.AspNetCore.Hosting.BeginRequest")]
        public void BeginRequest([Property] HttpContext httpContext)
        {
            if (IsIgnoreEntry(httpContext.Request))
            {
                return;
            }

            var operateName = httpContext.Request.Path.ToString().ToLower();
            var context = _tracingContext.CreateEntrySegmentContext(operateName,
                new HttpRequestCarrierHeaderCollection(httpContext.Request));
            context.Span.SpanLayer = SpanLayer.HTTP;
            context.Span.Component = Common.Components.ASPNETCORE;
            context.Span.Peer = new StringOrIntValue(httpContext.Connection.RemoteIpAddress.ToString());
            context.Span.AddTag(Tags.URL, httpContext.Request.GetDisplayUrl());
            context.Span.AddTag(Tags.PATH, httpContext.Request.Path);
            context.Span.AddTag(Tags.HTTP_METHOD, httpContext.Request.Method);
            context.Span.AddLog(
                LogEvent.Event("AspNetCore Hosting BeginRequest"),
                LogEvent.Message(
                    $"Request starting {httpContext.Request.Protocol} {httpContext.Request.Method} {httpContext.Request.GetDisplayUrl()}"));
        }

        [DiagnosticName("Microsoft.AspNetCore.Hosting.EndRequest")]
        public void EndRequest([Property] HttpContext httpContext)
        {
            var context = _segmentContextAccessor.Context;
            if (context == null)
            {
                return;
            }
            var statusCode = httpContext.Response.StatusCode;
            if (statusCode >= 400)
            {
                context.Span.ErrorOccurred();
            }

            context.Span.AddTag(Tags.STATUS_CODE, statusCode);
            context.Span.AddLog(
                LogEvent.Event("AspNetCore Hosting EndRequest"),
                LogEvent.Message(
                    $"Request finished {httpContext.Response.StatusCode} {httpContext.Response.ContentType}"));
            
            _tracingContext.Release(context);
        }

        [DiagnosticName("Microsoft.AspNetCore.Diagnostics.UnhandledException")]
        public void DiagnosticUnhandledException([Property] HttpContext httpContext, [Property] Exception exception)
        {
            _segmentContextAccessor.Context?.Span?.ErrorOccurred(exception);
        }

        [DiagnosticName("Microsoft.AspNetCore.Hosting.UnhandledException")]
        public void HostingUnhandledException([Property] HttpContext httpContext, [Property] Exception exception)
        {
            _segmentContextAccessor.Context?.Span?.ErrorOccurred(exception);
        }

        //[DiagnosticName("Microsoft.AspNetCore.Mvc.BeforeAction")]
        public void BeforeAction([Property] ActionDescriptor actionDescriptor, [Property] HttpContext httpContext)
        {
        }

        //[DiagnosticName("Microsoft.AspNetCore.Mvc.AfterAction")]
        public void AfterAction([Property] ActionDescriptor actionDescriptor, [Property] HttpContext httpContext)
        {
        }

        private bool IsIgnoreEntry(HttpRequest request)
        {
            if (!_endPointPolicyConfig.Enable)
            {
                return false;
            }
            string operateName = request.Path;
            var extension = System.IO.Path.GetExtension(operateName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                return false;
            }
            if (_endPointPolicyConfig.Policies.Contains(extension))
            {
                return false;
            }
            return true;
        }
    }
}