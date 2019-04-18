using zipkin4net.Tracers.Zipkin;

namespace thrift
{
    public class DefaultStatistics : IStatistics
    {
        public void UpdateRecordProcessed()
        {
        }

        public void UpdateSpanSent()
        {

        }

        public void UpdateSpanFlushed()
        {

        }

        public void UpdateSpanSentBytes(int bytesSent)
        {

        }

        public long RecordProcessed { get; }
        public long SpanSent { get; }
        public long SpanSentTotalBytes { get; }
        public long SpanFlushed { get; }
    }
}
