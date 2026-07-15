using HarmonyLib;
using Verse;
using RimWorld;

namespace RimSynapse.WorldNews.Patches
{
    [HarmonyPatch(typeof(LetterStack), "ReceiveLetter")]
    [HarmonyPatch(new[] { typeof(Letter), typeof(string), typeof(int), typeof(bool) })]
    public static class LetterStack_ReceiveLetter_Patch
    {
        public static void Postfix(Letter let)
        {
            if (let == null) return;
            
            // Skip letters that are just minor notifications if desired, 
            // but for now let's capture everything and let the WorldComponent filter or queue them.
            
            var worldComp = Find.World?.GetComponent<SynapseWorldNewsWorldComponent>();
            if (worldComp != null)
            {
                worldComp.RecordEventFromLetter(let);
            }
        }
    }
}
