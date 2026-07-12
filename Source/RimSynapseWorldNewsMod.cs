using Verse;
using HarmonyLib;

namespace RimSynapse.WorldNews
{
    public class RimSynapseWorldNewsMod : Mod
    {
        public static SynapseModHandle ModHandle;

        public RimSynapseWorldNewsMod(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("rimsynapse.worldnews");
            harmony.PatchAll();

            LongEventHandler.ExecuteWhenFinished(() =>
            {
                ModHandle = SynapseCore.Register(
                    "rimsynapse.worldnews",
                    "RimSynapse WorldNews"
                );
            });
        }
    }
}
