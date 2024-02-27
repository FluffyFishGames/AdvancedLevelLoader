using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace LethalLevelLoader.General
{
    internal class LethalLibPatches
    {

        [HarmonyPriority(Patches.harmonyPriority)]
        [HarmonyPatch(typeof(LethalLib.Modules.Dungeon), "RoundManager_Start")]
        [HarmonyPrefix]
        internal static bool Dungeon_Start_Prefix(On.RoundManager.orig_Start orig, RoundManager self)
        {
            DebugHelper.LogWarning("Disabling LethalLib Dungeon.RoundManager_Start() Function To Prevent Conflicts");
            orig(self);
            return (false);
        }

        [HarmonyPriority(Patches.harmonyPriority)]
        [HarmonyPatch(typeof(LethalLib.Modules.Dungeon), "RoundManager_GenerateNewFloor")]
        [HarmonyPrefix]
        internal static bool Dungeon_GenerateNewFloor_Prefix(On.RoundManager.orig_GenerateNewFloor orig, RoundManager self)
        {
            DebugHelper.LogWarning("Disabling LethalLib Dungeon.RoundManager_GenerateNewFloor() Function To Prevent Conflicts");
            orig(self);
            return (false);
        }
    }
}
