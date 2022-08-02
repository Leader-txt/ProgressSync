using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;

namespace ProgressSync
{
	internal class Utils
	{
		public static JObject GetHttp(string uri)
		{
			JObject result;
			using (HttpClient httpClient = new HttpClient())
			{
				result = JObject.Parse(httpClient.GetAsync(uri).Result.Content.ReadAsStringAsync().Result);
			}
			return result;
		}
		public static string RawCmd(Server server, string cmd)
		{
			var info = GetHttp("http://" + server.IP + ":" + server.RestPort + "/v3/server/rawcmd?token=" + server.Token + "&cmd=" + cmd);
			return info["response"].ToString();
		}
		public static void Sync(Progress progress)
        {
            if (progress.progress["HardMode"])
            {
                if (!Main.hardMode)
                {
					Main.hardMode = true;
					WorldGen.StartHardmode();
                }
            }
            else
            {
				Main.hardMode=false;
            }
            foreach (var yield in typeof(NPC).GetFields())
            {
                if (progress.progress.ContainsKey(yield.Name))
                {
                    yield.SetValue(null, progress.progress[yield.Name]);
                }
            }
			/*foreach (var item in progress.progress)
            {
				Console.WriteLine($"{item.Key} {item.Value}");
            }*/
			TSPlayer.All.SendData(PacketTypes.WorldInfo);
			TShock.Utils.Broadcast("进度已同步！", Color.Green);
        }
	}
}
