using SkyApm.Tracing.Segments;

namespace SkyApm.CtCustom
{
    public interface ICustomSegmentContextAccessor
    {
        /// <summary>
        /// 当前入链路上下文
        /// </summary>
        SegmentContext CurrentEntrySegmentContext { get;}

        /// <summary>
        /// 当前出链路上下文
        /// </summary>
        SegmentContext CurrentExitSegmentContext { get;}

        /// <summary>
        /// 当前本地链路上下文
        /// </summary>
        SegmentContext CurrentLocalSegmentContext { get; }

        /// <summary>
        /// 当前tranceId
        /// </summary>
        string CurrentTraceId { get; }
    }
}
