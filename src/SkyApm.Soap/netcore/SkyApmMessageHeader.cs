#if NETSTANDARD
using System;
using System.ServiceModel.Channels;
using System.Xml;

namespace SkyApm.Soap.netcore
{
    [Obsolete]
    internal class SkyApmMessageHeader: MessageHeader
    {
        public override string Name
        {
            get { return "SkyApmHeaderString"; }
        }

        private string header;
        public SkyApmMessageHeader(string header)
        {
            this.header = header;
        }

        public override string Namespace { get; }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteElementString(Name, header);
        }
    }
}
#endif