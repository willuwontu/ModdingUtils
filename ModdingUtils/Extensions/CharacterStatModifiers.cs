using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using UnboundLib.GameModes;


namespace ModdingUtils.Extensions
{
    // ADD FIELDS TO CHARACTERSTATMODIFIERS
    [Serializable]
    public class CharacterStatModifiersAdditionalData
    {
        public List<CardCategory> blacklistedCategories;

        public CharacterStatModifiersAdditionalData()
        {
            blacklistedCategories = new List<CardCategory>() { };
        }
    }
    public static class CharacterStatModifiersExtension
    {
        public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData> data =
            new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData>();

        public static CharacterStatModifiersAdditionalData GetAdditionalData(this CharacterStatModifiers characterstats)
        {
            return data.GetOrCreateValue(characterstats);
        }

        public static void AddData(this CharacterStatModifiers characterstats, CharacterStatModifiersAdditionalData value)
        {
            try
            {
                data.Add(characterstats, value);
            }
            catch (Exception) { }
        }

        internal static System.Collections.IEnumerator Reset(IGameModeHandler gm)
        {
            foreach (Player player in PlayerManager.instance.players)
            {
                player.data.stats.GetAdditionalData().blacklistedCategories = new List<CardCategory>() { };
            }    
            yield break;
        }

    }
}
