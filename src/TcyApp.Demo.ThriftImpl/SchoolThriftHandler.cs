using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcyApp.Demo.Thrift;

namespace TcyApp.Demo.ThriftImpl
{
    public class SchoolThriftHandler : MarshalByRefObject, SchoolThrift.Iface
    {
        public GetTeachersResult GetTeachers()
        {
       //     System.Threading.Thread.Sleep(10);
            return new GetTeachersResult() { Code = 0, Message = "", Data = new List<Teacher>() { } };
        }

        public GetSchoolNameResult GetSchoolName()
        {
          //  System.Threading.Thread.Sleep(10);
            return new GetSchoolNameResult() { Code = 0, Message = "", Data = $"schoolname" };
        }
    }
}
