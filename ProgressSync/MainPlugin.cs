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
        public override Version Version => new Version(1, 0, 0, 0);
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
            /*Console.WriteLine(NPC.downedSlimeKing);
            var prog = Progress.GetProgress();
            Console.WriteLine(prog.progress==null);
            foreach (var pair in prog.progress)
            {
                Console.WriteLine($"{pair.Key} {pair.Value}");
            }*/
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
                if(now != lastProgress)
                {
                    lastProgress = now;
                    now.Update(null, "progress");
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
        }

        public override void Initialize()
        {

        }
    }
}
