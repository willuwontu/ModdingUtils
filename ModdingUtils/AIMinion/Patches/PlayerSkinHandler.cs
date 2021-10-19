using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using ModdingUtils.AIMinion.Extensions;
using UnityEngine;
using Photon.Pun;

namespace ModdingUtils.AIMinion.Patches
{
    // patch to set correct skin for minions
    [Serializable]
    [HarmonyPatch(typeof(PlayerSkinHandler), "Init")]
    class PlayerSkinHandlerPatchInit
    {
        private static bool Prefix(PlayerSkinHandler __instance, ref bool ___inited, ref bool ___simpleSkin, ref CharacterData ___data, ref PlayerSkinParticle[] ___skins)
        {
			if (___inited)
			{
				return true;
			}
			___inited = true;
			__instance.ToggleSimpleSkin(___simpleSkin);
			___data = __instance.GetComponentInParent<CharacterData>();
			if (!___simpleSkin)
			{
				GameObject gameObject;
				if (___data.GetAdditionalData().isAIMinion)
                {
					gameObject = UnityEngine.GameObject.Instantiate<GameObject>(PlayerSkinBank.GetPlayerSkinColors(___data.GetAdditionalData().spawner.playerID).gameObject, __instance.transform.position, __instance.transform.rotation, __instance.transform);
				}
				else
                {
					gameObject = UnityEngine.GameObject.Instantiate<GameObject>(PlayerSkinBank.GetPlayerSkinColors(___data.player.playerID).gameObject, __instance.transform.position, __instance.transform.rotation, __instance.transform);
				}

				___skins = gameObject.GetComponentsInChildren<PlayerSkinParticle>();

			}
			return false;
		}
    }
}
