using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleClient
{
    public class Contact : EntityBase
    {
        public string Name { get; set; }

        public string Info { get; set; }
    }
}
