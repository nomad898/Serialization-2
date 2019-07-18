using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task
{
    public class SerializationContext
    {
        public IObjectContextAdapter ObjectContextAdapter { get; set; }
        public Type SerializingType { get; set; }
    }
}
