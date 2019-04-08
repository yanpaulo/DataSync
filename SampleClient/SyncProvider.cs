using DataSync;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Yansoft.Rest;

namespace SampleClient
{
    public class SyncProvider : ISyncProvider<EntityBase>
    {
        private readonly RestHttpClient _client = new RestHttpClient
        {
            BaseAddress = new Uri("http://localhost:65476/api/"),
            Converter = new JsonRestConverter
            {
                JsonSerializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            }
        };

        public SyncProvider()
        {
            
        }

        public SQLiteConnection Connection { get; private set; }

        public void Init()
        {
            Connection = new SQLiteConnection("data.db");
            Connection.CreateTable<Contact>();
        }
        
        public async Task PullAsync<T>() where T : EntityBase, new()
        {
            var modified = Connection.Table<T>().OrderByDescending(e => e.Modified).FirstOrDefault()?.Modified;
            var itens = await _client.GetAsync<List<T>>($"contacts?modified={modified?.ToString("o")}");

            foreach (var remote in itens)
            {
                var local = Connection.Table<T>().SingleOrDefault(e => e.Id == remote.Id);
                remote.LocalId = local?.LocalId ?? 0;
                remote.Status = EntityStatus.Synchronized;

                Connection.InsertOrReplace(remote);
            }

        }

        public async Task PushAsync<T>() where T : EntityBase, new()
        {
            var items = Connection.Table<T>().Where(e => e.Id == null).ToList();
            var remoteItems = await _client.PostAsync<List<T>>("contacts", items);
            for (int i = 0; i < remoteItems.Count; i++)
            {
                var local = items[i];
                var remote = remoteItems[i];

                remote.Status = EntityStatus.Synchronized;
                remote.LocalId = local.LocalId;
            }

            Connection.UpdateAll(remoteItems);
        }

        public async Task PutAsync<T>() where T : EntityBase, new()
        {
            var items = Connection.Table<T>().Where(e => e.Status == EntityStatus.Modified && e.Id != null).ToList();
            await _client.PutAsync<string>("contacts", items);
        }
    }
}
