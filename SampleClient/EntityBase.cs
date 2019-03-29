using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleClient
{
    public enum EntityStatus
    {
        Modified,
        Synchronized
    }
    public class EntityBase
    {
        public int? Id { get; set; }

        [PrimaryKey, AutoIncrement]
        public int LocalId { get; set; }

        public DateTimeOffset? Modified { get; set; }

        public EntityStatus Status { get; set; }


    }
}
