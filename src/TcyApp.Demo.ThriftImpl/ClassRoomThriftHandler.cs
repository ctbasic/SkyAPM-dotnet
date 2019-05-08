using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcyApp.Demo.Thrift;

namespace TcyApp.Demo.ThriftImpl
{
    public class ClassRoomThriftHandler : MarshalByRefObject, ClassRoomThrift.Iface
    {
        public GetClassRoomResult GetClassRoom()
        {
           System.Threading.Thread.Sleep(10);
            return new GetClassRoomResult() { Code = 0, Message = "", Data = new List<ClassRoom>() { new ClassRoom() { Game="game", Name = "name", Name2="name22"} } };
        }

        public GetClassRoomNameResult GetClassRoomName(int Id)
        {
           System.Threading.Thread.Sleep(10);
            return new GetClassRoomNameResult() { Code = 0, Message = "", Data = $"classroomname_{Id}" };
        }
    }
}
