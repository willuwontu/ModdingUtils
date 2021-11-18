using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using ModdingUtils.AIMinion.Extensions;
using ModdingUtils.AIMinion;
using UnboundLib;
using ModdingUtils.Utils;
using System.Linq;
using UnityEngine;
using BepInEx.Bootstrap;
using BepInEx;

namespace ModdingUtils.AIMinion.Patches
{
    // patch to fix player skins
    [HarmonyPatch(typeof(SetPlayerSpriteLayer), "Start")]
    class SetPlayerSpriteLayerPatchStart
    {
        private static bool Prefix(SetPlayerSpriteLayer __instance, ref bool ___simpleSkin)
        {
            ___simpleSkin = __instance.GetComponent<PlayerSkinHandler>().simpleSkin;
            Player componentInParent = __instance.GetComponentInParent<Player>();

            int num;

            if (componentInParent.data.GetAdditionalData().isAIMinion)
            {
                num = SortingLayer.NameToID("Player" + (componentInParent.data.GetAdditionalData().spawner.playerID + 1).ToString());
            }
            else
            {
                num = SortingLayer.NameToID("Player" + (componentInParent.playerID + 1).ToString());
            }

            __instance.InvokeMethod("setSpriteLayerOfChildren",new object[] { __instance.GetComponentInParent<Holding>().holdable.gameObject, num });
            __instance.InvokeMethod("setSpriteLayerOfChildren", new object[] { __instance.gameObject, num });
            if (!___simpleSkin)
            {
                __instance.GetComponent<PlayerSkinHandler>().InitSpriteMask(num);
            }

            return false;
        }
    }
}
