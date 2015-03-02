using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
	//Marker interface used for serialization purposes. 
	//Kind of workaround for using attributes of any particular serialization library e.g. protobuf
	public interface ISerializable {
	}
}
