using GithubLib.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace GithubLib.Gists
{
    public class DatabaseByGist : IDatabase
    {
        public IConnectInfo ConnectInfo { get; set; }

        const string KEY_PRIVATE = "ListIDGistDb";
        const string ID_DbFromGists = "61134ab2692f793d9f3a5563ff814620";

        public DatabaseByGist(GithubConnectInfo info)
        {
            this.ConnectInfo = info;
        }

        public async Task<string> GetDatabaseID()
        {
            try
            {
                //read list
                var dic = await _read<Dictionary<string, string>>(ID_DbFromGists, KEY_PRIVATE) ?? new Dictionary<string, string>(); //fix ID

                //check
                if (dic.ContainsKey(ConnectInfo.DatabaseName.ToUpper()) == true)
                {
                    return dic[ConnectInfo.DatabaseName.ToUpper()];
                }

                //NOT EXIST
                //create new
                var newID = await _gistProtocol().CreateNew(ConnectInfo.DatabaseName, "first content");
                //save ID
                dic.Add(ConnectInfo.DatabaseName.ToUpper(), newID);
                await _write(ID_DbFromGists, name: KEY_PRIVATE, dic, KEY_PRIVATE);

                //return new ID;
                return newID;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        GistHelper _gistProtocol()
        {
            return new GistHelper(ConnectInfo.Token);
        }

        public async Task<T> Read<T>() where T : class
        {
            try
            {
                return ConnectInfo.CheckedID ? await _read<T>(ConnectInfo.ID, ConnectInfo.Key) : null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }

        private async Task<T> _read<T>(string iD, string key) where T : class
        {
            var text = await _gistProtocol().GetById(iD);
            var files = JsonConvert.DeserializeAnonymousType(text, new
            {
                files = new object()
            });
            var o_files = JObject.FromObject(files.files);
            var text_content = JsonConvert.DeserializeAnonymousType(o_files.First.First + "", new
            {
                content = ""
            });
            var content_decrypt = EncryptHelper.Decrypt(text_content.content, key);
            return JsonConvert.DeserializeObject<T>(content_decrypt);
        }

        public async Task<string> Write<T>(T content)
        {
            try
            {
                if (ConnectInfo.CheckedID == false)
                {
                    ConnectInfo.ID = await GetDatabaseID();
                    ConnectInfo.CheckedID = string.IsNullOrWhiteSpace(ConnectInfo.ID) == false;
                }
                if (ConnectInfo.CheckedID)
                {
                    return await _write(ConnectInfo.ID, ConnectInfo.DatabaseName, content, ConnectInfo.Key);
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }

        private async Task<string> _write<T>(string ID, string name, T content, string key)
        {
            var text = JsonConvert.SerializeObject(content);
            var content_crypt = EncryptHelper.Encrypt(text, key);
            var respone_text = await _gistProtocol().Update(ID, name, content_crypt);
            var id = JsonConvert.DeserializeAnonymousType(respone_text, new
            {
                id = ""
            });
            return id.id;
        }
    }


    public interface IDatabase
    {
        IConnectInfo ConnectInfo { get; set; }
        Task<T> Read<T>() where T : class;
        Task<string> Write<T>(T content);
    }

    public interface IConnectInfo
    {
        string Token { get; set; }
        string Key { get; set; }
        string ID { get; set; }
        string DatabaseName { get; set; }
        bool CheckedID { get; set; }
    }

    public class GithubConnectInfo : IConnectInfo
    {
        public string Token { get; set; }
        public string DatabaseName { get; set; }
        public string ID { get; set; }
        public string Key { get; set; }
        public bool CheckedID { get; set; }
    }
}
