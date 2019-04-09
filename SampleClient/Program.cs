using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Yansoft.DataSync;

namespace SampleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var provider = new SyncProvider();
            provider.Init();

            var builder = new SyncManagerBuilder();
            builder
                .Type<EntityBase>()
                .SyncWith(provider);

            var manager = builder.Build();

            var sql = provider.Connection;

            //var contact = new Contact
            //{
            //    Name = "Yan",
            //    Info = "SomeInfo"
            //};
            //sql.Insert(contact);

            //var contact = sql.Table<Contact>().First();
            //contact.Name = "YansCorp";
            //contact.Status = EntityStatus.Modified;
            //sql.Update(contact);

            await manager.SyncAsync<Contact>(SyncMode.RemoteWins);

        }
    }
}
