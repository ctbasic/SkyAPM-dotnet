// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: language-agent-v2/JVMMetric.proto
// </auto-generated>
// Original file comments:
//
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreements.  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the License.  You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
//
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace SkyWalking.NetworkProtocol {
  public static partial class JVMMetricReportService
  {
    static readonly string __ServiceName = "JVMMetricReportService";

    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.JVMMetricCollection> __Marshaller_JVMMetricCollection = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.JVMMetricCollection.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.Commands> __Marshaller_Commands = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.Commands.Parser.ParseFrom);

    static readonly grpc::Method<global::SkyWalking.NetworkProtocol.JVMMetricCollection, global::SkyWalking.NetworkProtocol.Commands> __Method_collect = new grpc::Method<global::SkyWalking.NetworkProtocol.JVMMetricCollection, global::SkyWalking.NetworkProtocol.Commands>(
        grpc::MethodType.Unary,
        __ServiceName,
        "collect",
        __Marshaller_JVMMetricCollection,
        __Marshaller_Commands);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::SkyWalking.NetworkProtocol.JVMMetricReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of JVMMetricReportService</summary>
    public abstract partial class JVMMetricReportServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::SkyWalking.NetworkProtocol.Commands> collect(global::SkyWalking.NetworkProtocol.JVMMetricCollection request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for JVMMetricReportService</summary>
    public partial class JVMMetricReportServiceClient : grpc::ClientBase<JVMMetricReportServiceClient>
    {
      /// <summary>Creates a new client for JVMMetricReportService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public JVMMetricReportServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for JVMMetricReportService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public JVMMetricReportServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected JVMMetricReportServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected JVMMetricReportServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::SkyWalking.NetworkProtocol.Commands collect(global::SkyWalking.NetworkProtocol.JVMMetricCollection request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return collect(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::SkyWalking.NetworkProtocol.Commands collect(global::SkyWalking.NetworkProtocol.JVMMetricCollection request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_collect, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.Commands> collectAsync(global::SkyWalking.NetworkProtocol.JVMMetricCollection request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return collectAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.Commands> collectAsync(global::SkyWalking.NetworkProtocol.JVMMetricCollection request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_collect, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override JVMMetricReportServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new JVMMetricReportServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(JVMMetricReportServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_collect, serviceImpl.collect).Build();
    }

    /// <summary>Register service method implementations with a service binder. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, JVMMetricReportServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_collect, serviceImpl.collect);
    }

  }
}
#endregion
