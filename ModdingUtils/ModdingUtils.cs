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
using ModdingUtils.Extensions;
using ModdingUtils.MonoBehaviours;

// requires Assembly-CSharp.dll
// requires MMHOOK-Assembly-CSharp.dll

namespace ModdingUtils
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)] // necessary for most modding stuff here
    [BepInPlugin(ModId, ModName, "0.0.0.7")]
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
            Unbound.RegisterCredits(ModName, new string[] { "Pykess", "Boss sloth" }, new string[] { "github", "Buy me a coffee" }, new string[] { "https://github.com/Rounds-Modding/ModdingUtils", "https://www.buymeacoffee.com/Pykess" });

            GameModeManager.AddHook(GameModeHooks.HookPickEnd, (gm) => EndPickPhaseShow());

            // reset player blacklisted categories on game start
            GameModeManager.AddHook(GameModeHooks.HookGameStart, CharacterStatModifiersExtension.Reset);
            
            GameModeManager.AddHook(GameModeHooks.HookBattleStart, (gm) => ResetEffectsBetweenBattles());
            GameModeManager.AddHook(GameModeHooks.HookBattleStart, (gm) => ResetTimers()); // I sure hope this doesn't have unintended side effects...
            
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, (gm) => ResetEffectsBetweenBattles());
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, (gm) => ResetTimers());
        }

        private IEnumerator EndPickPhaseShow()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            yield return Utils.CardBarUtils.instance.EndPickPhaseShow();
            yield break;
        }
        
        private IEnumerator ResetEffectsBetweenBattles()
        {
            Player[] players = PlayerManager.instance.players.ToArray();
            for (int j = 0; j < players.Length; j++)
            {
                CustomEffects.ClearAllReversibleEffects(players[j].gameObject);
                foreach (InConeEffect effect in players[j].GetComponents<InConeEffect>())
                {
                    effect.RemoveAllEffects();
                }
            }
            foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (gameObject.name == "LaserTrail(Clone)")
                {
                    Destroy(gameObject);
                }
            }
            yield break;
        }

        private IEnumerator ResetTimers()
        {
            Player[] players = PlayerManager.instance.players.ToArray();
            for (int j = 0; j < players.Length; j++)
            {
                CustomEffects.ResetAllTimers(players[j].gameObject);
            }
            yield break;
        }
        
        private const string ModId = "pykess.rounds.plugins.moddingutils";

        private const string ModName = "Modding Utilities";
    }
}
