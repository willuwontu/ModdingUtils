using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using ModdingUtils.AIMinion.Extensions;
using ModdingUtils.AIMinion;
using UnboundLib;
using ModdingUtils.Utils;
using System.Linq;

namespace ModdingUtils.AIMinion.Patches
{
    // patch to return the correct number of TEAMS remaining so that Deathmatch works
    [Serializable]
    [HarmonyPatch(typeof(PlayerManager), "PlayerDied")]
    class PlayerManagerPatchPlayerDied
    {
        private static bool Prefix(PlayerManager __instance, Player player)
        {
            int num = PlayerManager.instance.players.Where(p => !p.data.dead).Select(p => p.teamID).Distinct().Count();

            if ((Action<Player,int>)__instance.GetFieldValue("PlayerDiedAction") != null)
            {
                ((Action<Player, int>)__instance.GetFieldValue("PlayerDiedAction"))(player, num);
            }
            return false;

        }
    }
}
