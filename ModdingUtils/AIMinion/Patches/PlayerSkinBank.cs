using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using ModdingUtils.AIMinion.Extensions;
using System.Linq;

namespace ModdingUtils.AIMinion.Patches
{
    
    // patch to return correct team colors for AI
    [Serializable]
    [HarmonyPatch(typeof(PlayerSkinBank), "GetPlayerSkinColors")]
    class PlayerSkinBankPatchGetPlayersSkinColors
    {
        private static void Prefix(ref int team)
        {
            // find the player or AI this is referring to. NOTE: despite the method parameter being named "team" the code uses this as a playerID
            List<Player> allPlayers = new List<Player>(PlayerManager.instance.players.Where(p => !p.data.GetAdditionalData().isAIMinion));
            foreach (Player player in PlayerManager.instance.players.Where(p => !p.data.GetAdditionalData().isAIMinion && p.data.GetAdditionalData().minions.Count() > 0))
            {
                allPlayers.AddRange(player.data.GetAdditionalData().minions);
            }

            int ID = (int)team;

            if (!allPlayers.Where(p => p.playerID == ID).Any())
            { return; }

            Player requestedPlayer = AIMinionHandler.GetPlayerOrAIWithID(allPlayers.ToArray(), team);

            if (requestedPlayer.data.GetAdditionalData().isAIMinion && requestedPlayer.data.GetAdditionalData().spawner != null)
            {
                team = requestedPlayer.data.GetAdditionalData().spawner.playerID;
            }

        }
    }
    // patch to return correct team colors for AI
    [Serializable]
    [HarmonyPatch(typeof(PlayerSkinBank), "GetPlayerSkin")]
    class PlayerSkinBankPatchGetPlayerSkin
    {
        private static void Prefix(ref int team)
        {
            // find the player or AI this is referring to. NOTE: despite the method parameter being named "team" the code uses this as a playerID
            List<Player> allPlayers = new List<Player>(PlayerManager.instance.players.Where(p => !p.data.GetAdditionalData().isAIMinion));
            foreach (Player player in PlayerManager.instance.players.Where(p => !p.data.GetAdditionalData().isAIMinion && p.data.GetAdditionalData().minions.Count() > 0))
            {
                allPlayers.AddRange(player.data.GetAdditionalData().minions);
            }
            int ID = (int)team;
            if (!allPlayers.Where(p => p.playerID == ID).Any())
            { return; }
            Player requestedPlayer = AIMinionHandler.GetPlayerOrAIWithID(allPlayers.ToArray(), team);

            if (requestedPlayer.data.GetAdditionalData().isAIMinion && requestedPlayer.data.GetAdditionalData().spawner != null)
            {
                team = requestedPlayer.data.GetAdditionalData().spawner.playerID;
            }
        }
    }
}
