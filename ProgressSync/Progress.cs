using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ProgressSync
{
    public class Progress : Table<Progress>
    {
        public Dictionary<string, bool> progress { get; set; } = new Dictionary<string, bool>();
        public static Progress GetProgress()
        {
            var progress = new Progress();
            progress.progress.Add("HardMode", Main.hardMode);
            foreach (var yield in typeof(NPC).GetFields())
            {
                if (yield.Name.StartsWith("downed"))
                {
                    progress.progress.Add(yield.Name, (bool)yield.GetValue(null));
                }
            }
            return progress;
        }
    }
}
