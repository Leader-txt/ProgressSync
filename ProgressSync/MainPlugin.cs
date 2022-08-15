using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace ProgressSync
{
    [ApiVersion(2, 1)]
    public class MainPlugin : TerrariaPlugin
    {
        public override string Name => "ProgressingSync";
        public override string Description => "进度同步";
        public override string Author => "Leader";
        public override Version Version => new Version(1, 0, 1, 0);
        public Progress lastProgress { get; set; } = null;
        public MainPlugin(Main game) : base(game)
        {
            Data.Init();
            Commands.ChatCommands.Add(new Command("ProgressingSync.admin", sync, "ProgressSync", "ps","进度同步"));
            ServerApi.Hooks.NpcKilled.Register(this, OnNPCKIlled);
            ServerApi.Hooks.GamePostInitialize.Register(this, OnGamepostInit);
        }

        private void OnGamepostInit(EventArgs args)
        {
            Utils.Sync(Progress.Get()[0]);
        }

        private void OnNPCKIlled(NpcKilledEventArgs args)
        {
            if (lastProgress == null)
            {
                lastProgress = Progress.GetProgress();
            }
            else
            {
                var now=Progress.GetProgress();
                if(now.progress.ToList().FindAll(x=>lastProgress.progress[x.Key]!=x.Value).Any())
                {
                    lastProgress = now;
                    bool updated = false;
                    while (Progress.Get()[0].progress.ToList().FindAll(x => now.progress[x.Key] != x.Value).Any())
                    {
                        updated = true;
                        now.Update(null, "progress");
                    }
                    if (updated)
                        foreach (var s in Config.GetConfig().Servers)
                        {
                            try
                            {
                                Utils.RawCmd(s, "/ps");
                            }
                            catch { }
                        }
                }
            }
        }

        private void sync(CommandArgs args)
        {
            if (args.Parameters.Count == 0)
                Utils.Sync(Progress.Get()[0]);
            else if (args.Parameters[0] == "reset")
            {
                Progress.Delete();
            }
            else if(args.Parameters[0] == "rps")
            {
                Main.hardMode = false;
                foreach (var yield in typeof(NPC).GetFields())
                {
                    if (yield.Name.StartsWith("downed") || yield.Name.StartsWith("saved"))
                    {
                        yield.SetValue(null, false);
                    }
                }
                TSPlayer.All.SendData(PacketTypes.WorldInfo);
                TShock.Utils.Broadcast("进度已重置！", Color.Green);
            }
        }

        public override void Initialize()
        {

        }
    }
}
