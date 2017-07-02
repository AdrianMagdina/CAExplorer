// programmed by Adrian Magdina in 2013
// in this file is the implementation for application specific CAExplorer exception

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CAExplorerNamespace
{
    //this is specific exception for this Application
    [Serializable()]
    public class CAExplorerException : Exception
    {
        public CAExplorerException()
            : base()
        {
        }

        public CAExplorerException(string msgIn)
            : base(msgIn)
        {
        }

        public CAExplorerException(string msgIn, Exception exIn)
            : base(msgIn, exIn)
        {
        }

        protected CAExplorerException(SerializationInfo infoIn, StreamingContext contextIn)
            : base(infoIn, contextIn)
        {
        }
    }
}
