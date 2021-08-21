using BepInEx; // requires BepInEx.dll and BepInEx.Harmony.dll
using UnboundLib; // requires UnboundLib.dll
using UnboundLib.Cards; // " "
using UnityEngine; // requires UnityEngine.dll, UnityEngine.CoreModule.dll, and UnityEngine.AssetBundleModule.dll
using HarmonyLib; // requires 0Harmony.dll
using System.Collections;
using Photon.Pun;
using Jotunn.Utils;
using UnboundLib.GameModes;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using CardChoiceSpawnUniqueCardPatch;
// requires Assembly-CSharp.dll
// requires MMHOOK-Assembly-CSharp.dll

namespace ModdingUtils
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)] // necessary for most modding stuff here
    [BepInDependency("pykess.rounds.plugins.playerjumppatch", BepInDependency.DependencyFlags.HardDependency)] // fixes multiple jumps
    [BepInDependency("pykess.rounds.plugins.legraycasterspatch", BepInDependency.DependencyFlags.HardDependency)] // fixes physics for small players
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)] // fixes allowMultiple and blacklistedCategories
    [BepInDependency("pykess.rounds.plugins.gununblockablepatch", BepInDependency.DependencyFlags.HardDependency)] // fixes gun.unblockable
    [BepInDependency("pykess.rounds.plugins.temporarystatspatch", BepInDependency.DependencyFlags.HardDependency)] // fixes Taste Of Blood, Pristine Perserverence, and Chase when combined with cards from PCE
    [BepInPlugin(ModId, ModName, "0.0.0.5")]
    [BepInProcess("Rounds.exe")]
    public class ModdingUtils : BaseUnityPlugin
    {
        private void Awake()
        {
            new Harmony(ModId).PatchAll();
        }
        private void Start()
        {
            // register credits with unbound
            Unbound.RegisterCredits(ModName, new string[] { "Pykess" }, new string[] { "github", "Buy me a coffee" }, new string[] { "https://github.com/Rounds-Modding/ModdingUtils", "https://www.buymeacoffee.com/Pykess" });

            GameModeManager.AddHook(GameModeHooks.HookPickEnd, (gm) => EndPickPhaseShow());
        }

        private IEnumerator EndPickPhaseShow()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            yield return Utils.CardBarUtils.instance.EndPickPhaseShow();
            yield break;
        }
        private const string ModId = "pykess.rounds.plugins.moddingutils";

        private const string ModName = "Modding Utilities";
    }
}
