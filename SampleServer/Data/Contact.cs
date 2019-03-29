using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleServer.Data
{
    public class Contact
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Info { get; set; }

        public DateTimeOffset Modified { get; set; }
    }
}
