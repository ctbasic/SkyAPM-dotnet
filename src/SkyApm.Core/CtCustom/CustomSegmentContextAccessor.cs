using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyApm.CtCustom;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;

namespace SkyWalking.CtCustom
{
    public class CustomSegmentContextAccessor: ICustomSegmentContextAccessor
    {
        private readonly ILocalSegmentContextAccessor localSegmentContextAccessor;

        private readonly IEntrySegmentContextAccessor entrySegmentContextAccessor;

        private readonly IExitSegmentContextAccessor exitSegmentContextAccessor;

        public CustomSegmentContextAccessor(ILocalSegmentContextAccessor localSegmentContextAccessor, IEntrySegmentContextAccessor entrySegmentContextAccessor, IExitSegmentContextAccessor exitSegmentContextAccessor)
        {
            this.localSegmentContextAccessor = localSegmentContextAccessor;
            this.entrySegmentContextAccessor = entrySegmentContextAccessor;
            this.exitSegmentContextAccessor = exitSegmentContextAccessor;
        }

        /// <summary>
        /// 当前入链路上下文
        /// </summary>
        public SegmentContext CurrentEntrySegmentContext => entrySegmentContextAccessor?.Context;

        /// <summary>
        /// 当前出链路上下文
        /// </summary>
        public SegmentContext CurrentExitSegmentContext => exitSegmentContextAccessor?.Context;

        /// <summary>
        /// 当前本地链路上下文
        /// </summary>
        public SegmentContext CurrentLocalSegmentContext => localSegmentContextAccessor?.Context;

        /// <summary>
        /// 当前tranceId
        /// </summary>
        public string CurrentTraceId
        {
            get
            {
                if (CurrentEntrySegmentContext != null)
                {
                    return CurrentEntrySegmentContext.TraceId.ToString();
                }
                if (CurrentExitSegmentContext != null)
                {
                    return CurrentExitSegmentContext.TraceId.ToString();
                }
                if (CurrentLocalSegmentContext != null)
                {
                    return CurrentLocalSegmentContext.TraceId.ToString();
                }

                return string.Empty;
            }
        }
    }
}
