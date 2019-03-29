using DataSync;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SampleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var provider = new SyncProvider();
            var manager = new SyncManager(provider);
            provider.Init();

            var sql = provider.Connection;

            //var contact = new Contact
            //{
            //    Name = "Yan",
            //    Info = "SomeInfo"
            //};
            //sql.Insert(contact);

            var contact = sql.Table<Contact>().First();
            contact.Name = "YansCorp";
            contact.Status = EntityStatus.Modified;
            sql.Update(contact);

            await manager.SyncAsync<Contact, EntityBase>(SyncMode.RemoteWins);

        }
    }
}
