using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressSync
{
    public class Server
    {
        [JsonProperty("服务器IP")]
        public string IP { get; set; } = "127.0.0.1";
        [JsonProperty("Rest端口")]
        public string RestPort { get; set; } = "7878";
        [JsonProperty("Rest令牌")]
        public string Token { get; set; } = "";
    }
    public class Config
    {
        public const string path = "tshock/ProgressSync.json";
        [JsonProperty("Mysql IP地址")]
        public string Host { get; set; } = "localhost";
        [JsonProperty("Mysql 端口")]
        public string Port { get; set; } = "3306";
        [JsonProperty("库名")]
        public string Db { get; set; } = "";
        [JsonProperty("Mysql用户名")]
        public string User { get; set; } = "root";
        [JsonProperty("Mysql密码")]
        public string Pass { get; set; } = "";
        [JsonProperty("需要通知的服务器")]
        public Server[] Servers { get; set; } = new Server[] { new Server() };
        public void Save()
        {
            using (StreamWriter wr = new StreamWriter(path))
            {
                wr.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }
        public static Config GetConfig()
        {
            var config = new Config();
            if (!File.Exists(path))
            {
                config.Save();
                return config;
            }
            else
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    config = JsonConvert.DeserializeObject<Config>(sr.ReadToEnd());
                }
                return config;
            }
        }
    }
}
