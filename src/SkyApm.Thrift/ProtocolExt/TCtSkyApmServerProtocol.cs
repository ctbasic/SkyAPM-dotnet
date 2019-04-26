/**
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements. See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership. The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations
 * under the License.
 *
 * Contains some contributions under the Thrift Software License.
 * Please see doc/old-thrift-license.txt in the Thrift distribution for
 * details.
 */

#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CommonServiceLocator;
using SkyApm.Agent.Thrift;
using SkyApm.Agent.Thrift.ProtocolExt;
using SkyApm.Common;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;
using Thrift.Transport;

#endregion

namespace Thrift.Protocol
{
    public class TCtSkyApmServerProtocol : TProtocol
    {
        protected const uint VERSION_MASK = 0xffff0000;
        protected const uint VERSION_1 = 0x80010000;

        protected bool strictRead_;
        protected bool strictWrite_ = true;

        private static SkyApm.Logging.ILogger logger = ServiceLocator.Current.GetInstance<SkyApm.Logging.ILoggerFactory>().CreateLogger(typeof(TCtSkyApmServerProtocol));
        private readonly ITracingContext tracingContext;
        private readonly IEntrySegmentContextAccessor segmentContextAccessor;

        #region BinaryProtocol Factory

        /**
         * Factory
         */
        public class Factory : TProtocolFactory
        {
            protected bool strictRead_;
            protected bool strictWrite_ = true;

            public Factory()
                : this(false, true)
            {
            }

            public Factory(bool strictRead, bool strictWrite)
            {
                strictRead_ = strictRead;
                strictWrite_ = strictWrite;
            }

            public TProtocol GetProtocol(TTransport trans)
            {
                return new TCtSkyApmServerProtocol(trans, strictRead_, strictWrite_);
            }
        }

        #endregion

        public TCtSkyApmServerProtocol(TTransport trans)
            : this(trans, false, true)
        {
        }

        public TCtSkyApmServerProtocol(TTransport trans, bool strictRead, bool strictWrite)
            : base(trans)
        {
            strictRead_ = strictRead;
            strictWrite_ = strictWrite;

            tracingContext = ServiceLocator.Current.GetInstance<ITracingContext>();
            segmentContextAccessor = ServiceLocator.Current.GetInstance<IEntrySegmentContextAccessor>();
        }

        #region Write Methods

        public override void WriteMessageBegin(TMessage message)
        {
            var wrapName = ProtocolUtils.WrapMessageName(message.Name);
            message.Name = wrapName;
            if (strictWrite_)
            {
                var version = VERSION_1 | (uint) (message.Type);
                WriteI32((int) version);
                WriteString(message.Name);
                WriteI32(message.SeqID);
            }
            else
            {
                WriteString(message.Name);
                WriteByte((sbyte) message.Type);
                WriteI32(message.SeqID);
            }

            ThriftServerSend();
        }

        private void ThriftServerSend()
        {
            try
            {
                if (segmentContextAccessor.Context == null)
                {
                    logger.Warning("ThriftServerSend Context is null");
                }

                tracingContext.Release(segmentContextAccessor.Context);
            }
            catch (Exception e)
            {
                logger.Error("ThriftServerSend error", e);
            }
        }

        public override void WriteMessageEnd()
        {
        }

        public override void WriteStructBegin(TStruct struc)
        {
        }

        public override void WriteStructEnd()
        {
        }

        public override void WriteFieldBegin(TField field)
        {
            WriteByte((sbyte) field.Type);
            WriteI16(field.ID);
        }

        public override void WriteFieldEnd()
        {
        }

        public override void WriteFieldStop()
        {
            WriteByte((sbyte) TType.Stop);
        }

        public override void WriteMapBegin(TMap map)
        {
            WriteByte((sbyte) map.KeyType);
            WriteByte((sbyte) map.ValueType);
            WriteI32(map.Count);
        }

        public override void WriteMapEnd()
        {
        }

        public override void WriteListBegin(TList list)
        {
            WriteByte((sbyte) list.ElementType);
            WriteI32(list.Count);
        }

        public override void WriteListEnd()
        {
        }

        public override void WriteSetBegin(TSet set)
        {
            WriteByte((sbyte) set.ElementType);
            WriteI32(set.Count);
        }

        public override void WriteSetEnd()
        {
        }

        public override void WriteBool(bool b)
        {
            WriteByte(b ? (sbyte) 1 : (sbyte) 0);
        }

        private readonly byte[] bout = new byte[1];

        public override void WriteByte(sbyte b)
        {
            bout[0] = (byte) b;
            trans.Write(bout, 0, 1);
        }

        private readonly byte[] i16out = new byte[2];

        public override void WriteI16(short s)
        {
            i16out[0] = (byte) (0xff & (s >> 8));
            i16out[1] = (byte) (0xff & s);
            trans.Write(i16out, 0, 2);
        }

        private readonly byte[] i32out = new byte[4];

        public override void WriteI32(int i32)
        {
            i32out[0] = (byte) (0xff & (i32 >> 24));
            i32out[1] = (byte) (0xff & (i32 >> 16));
            i32out[2] = (byte) (0xff & (i32 >> 8));
            i32out[3] = (byte) (0xff & i32);
            trans.Write(i32out, 0, 4);
        }

        private readonly byte[] i64out = new byte[8];

        public override void WriteI64(long i64)
        {
            i64out[0] = (byte) (0xff & (i64 >> 56));
            i64out[1] = (byte) (0xff & (i64 >> 48));
            i64out[2] = (byte) (0xff & (i64 >> 40));
            i64out[3] = (byte) (0xff & (i64 >> 32));
            i64out[4] = (byte) (0xff & (i64 >> 24));
            i64out[5] = (byte) (0xff & (i64 >> 16));
            i64out[6] = (byte) (0xff & (i64 >> 8));
            i64out[7] = (byte) (0xff & i64);
            trans.Write(i64out, 0, 8);
        }

        public override void WriteDouble(double d)
        {
#if !SILVERLIGHT
            WriteI64(BitConverter.DoubleToInt64Bits(d));
#else
            var bytes = BitConverter.GetBytes(d);
            WriteI64(BitConverter.ToInt64(bytes, 0));
#endif
        }

        public override void WriteBinary(byte[] b)
        {
            WriteI32(b.Length);
            trans.Write(b, 0, b.Length);
        }

        #endregion

        #region ReadMethods

        public override TMessage ReadMessageBegin()
        {
            var message = new TMessage();
            var size = ReadI32();
            if (size < 0)
            {
                var version = (uint) size & VERSION_MASK;
                if (version != VERSION_1)
                {
                    throw new TProtocolException(TProtocolException.BAD_VERSION, "Bad version in ReadMessageBegin: " + version);
                }
                message.Type = (TMessageType) (size & 0x000000ff);

                var sourceMsgName = ReadString();
                var methodName = "";
                var zipKinHeader = ProtocolUtils.GetBinaryProtocolHeader(sourceMsgName, out methodName);
                ThriftServerReceive(zipKinHeader, methodName);

                message.Name = methodName;
                message.SeqID = ReadI32();
            }
            else
            {
                if (strictRead_)
                {
                    throw new TProtocolException(TProtocolException.BAD_VERSION, "Missing version in readMessageBegin, old client?");
                }

                var sourceMsgName = ReadStringBody(size);
                var methodName = "";
                var zipKinHeader = ProtocolUtils.GetBinaryProtocolHeader(sourceMsgName, out methodName);
                ThriftServerReceive(zipKinHeader, methodName);

                message.Name = methodName;
                message.Type = (TMessageType) ReadByte();
                message.SeqID = ReadI32();
            }
            return message;
        }

        /// <summary>
        ///     服务端受理
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="methodName"></param>
        private void ThriftServerReceive(ThriftHeaders headers, string methodName)
        {
            try
            {
                var tsocket = (TSocket)trans;
                string address = tsocket.TcpClient.Client.LocalEndPoint.ToString();

                var segmentContext = tracingContext.CreateEntrySegmentContext(methodName, new ThriftICarrierHeaderCollection(headers));
                segmentContext.Span.SpanLayer = SpanLayer.RPC_FRAMEWORK;

                segmentContext.Span.Component = Components.THRIFT;
                segmentContext.Span.Peer = new StringOrIntValue(address);
                segmentContext.Span.AddTag(Tags.RPC_METHOD, methodName);
                segmentContext.Span.AddLog(
                    LogEvent.Event("Thrift Hosting BeginRequest"),
                    LogEvent.Message(
                        $"Request starting thrift {methodName}"));

            }
            catch (Exception e)
            {
                logger.Error("ThriftServerReceive error", e);
            }
        }

        public override void ReadMessageEnd()
        {
        }

        public override TStruct ReadStructBegin()
        {
            return new TStruct();
        }

        public override void ReadStructEnd()
        {
        }

        public override TField ReadFieldBegin()
        {
            var field = new TField();
            field.Type = (TType) ReadByte();

            if (field.Type != TType.Stop)
            {
                field.ID = ReadI16();
            }

            return field;
        }

        public override void ReadFieldEnd()
        {
        }

        public override TMap ReadMapBegin()
        {
            var map = new TMap();
            map.KeyType = (TType) ReadByte();
            map.ValueType = (TType) ReadByte();
            map.Count = ReadI32();

            return map;
        }

        public override void ReadMapEnd()
        {
        }

        public override TList ReadListBegin()
        {
            var list = new TList();
            list.ElementType = (TType) ReadByte();
            list.Count = ReadI32();

            return list;
        }

        public override void ReadListEnd()
        {
        }

        public override TSet ReadSetBegin()
        {
            var set = new TSet();
            set.ElementType = (TType) ReadByte();
            set.Count = ReadI32();

            return set;
        }

        public override void ReadSetEnd()
        {
        }

        public override bool ReadBool()
        {
            return ReadByte() == 1;
        }

        private readonly byte[] bin = new byte[1];

        public override sbyte ReadByte()
        {
            ReadAll(bin, 0, 1);
            return (sbyte) bin[0];
        }

        private readonly byte[] i16in = new byte[2];

        public override short ReadI16()
        {
            ReadAll(i16in, 0, 2);
            return (short) (((i16in[0] & 0xff) << 8) | ((i16in[1] & 0xff)));
        }

        private readonly byte[] i32in = new byte[4];

        public override int ReadI32()
        {
            ReadAll(i32in, 0, 4);
            return ((i32in[0] & 0xff) << 24) | ((i32in[1] & 0xff) << 16) | ((i32in[2] & 0xff) << 8) | ((i32in[3] & 0xff));
        }

#pragma warning disable 675

        private readonly byte[] i64in = new byte[8];

        public override long ReadI64()
        {
            ReadAll(i64in, 0, 8);
            return ((long) (i64in[0] & 0xff) << 56) |
                   ((long) (i64in[1] & 0xff) << 48) |
                   ((long) (i64in[2] & 0xff) << 40) |
                   ((long) (i64in[3] & 0xff) << 32) |
                   ((long) (i64in[4] & 0xff) << 24) |
                   ((long) (i64in[5] & 0xff) << 16) |
                   ((long) (i64in[6] & 0xff) << 8) |
                   i64in[7] & 0xff;
        }

#pragma warning restore 675

        public override double ReadDouble()
        {
#if !SILVERLIGHT
            return BitConverter.Int64BitsToDouble(ReadI64());
#else
            var value = ReadI64();
            var bytes = BitConverter.GetBytes(value);
            return BitConverter.ToDouble(bytes, 0);
#endif
        }

        public override byte[] ReadBinary()
        {
            var size = ReadI32();
            var buf = new byte[size];
            trans.ReadAll(buf, 0, size);
            return buf;
        }

        private string ReadStringBody(int size)
        {
            var buf = new byte[size];
            trans.ReadAll(buf, 0, size);
            return Encoding.UTF8.GetString(buf, 0, buf.Length);
        }

        private int ReadAll(byte[] buf, int off, int len)
        {
            return trans.ReadAll(buf, off, len);
        }

        #endregion
    }
}