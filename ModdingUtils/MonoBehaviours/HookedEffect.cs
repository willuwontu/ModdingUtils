using System;
using System.Collections.Generic;
using System.Collections;
using Photon.Pun;
using UnboundLib.GameModes;

namespace ModdingUtils.MonoBehaviours
{
    /// <summary>
    /// A MonobehaviorPun that has events for each of the GameModeHooks.
    /// </summary>
    public class HookedEffect : MonoBehaviourPun
    {
        /// <summary>
        /// If true, the effect will automatically be destroyed when a new game is called.
        /// </summary>
        public bool removeOnRematch = false;

        /// <summary>
        /// Use OnStart() for anything that needs to be run in Start().
        /// </summary>
        public void Start()
        {
            HookedEffectManager.instance.hookedEffects.Add(this);

            OnStart();
        }

        /// <summary>
        /// Run in Start().
        /// </summary>
        public virtual void OnStart()
        {

        }

        /// <summary>
        /// Run at the start of a round.
        /// </summary>
        public virtual void OnRoundStart()
        {

        }

        /// <summary>
        /// Run at the end of a round.
        /// </summary>
        public virtual void OnRoundEnd()
        {

        }

        /// <summary>
        /// Run at the start of a match point.
        /// </summary>
        public virtual void OnPointStart()
        {

        }

        /// <summary>
        /// Run at the end of a match point.
        /// </summary>
        public virtual void OnPointEnd()
        {

        }

        /// <summary>
        /// Run at the start of a game.
        /// </summary>
        public virtual void OnGameStart()
        {

        }

        /// <summary>
        /// Run at the end of a game.
        /// </summary>
        public virtual void OnGameEnd()
        {

        }

        /// <summary>
        /// Run at the start of a battle, that is, when guns are able to be fired.
        /// </summary>
        public virtual void OnBattleStart()
        {

        }

        /// <summary>
        /// Run at the start of the pick phase.
        /// </summary>
        public virtual void OnPickStart()
        {
            
        }

        /// <summary>
        /// Run after the pick phase.
        /// </summary>
        public virtual void OnPickEnd()
        {

        }

        /// <summary>
        /// Run before a player is presented with cards to choose from.
        /// </summary>
        public virtual void OnPlayerPickStart()
        {

        }

        /// <summary>
        /// Run after a player has selected their card.
        /// </summary>
        public virtual void OnPlayerPickEnd()
        {

        }

        /// <summary>
        /// Use OnOnDestroy() for anything that needs to be run in OnDestroy().
        /// </summary>
        public void OnDestroy()
        {
            HookedEffectManager.instance.hookedEffects.Remove(this);
            OnOnDestroy();
        }

        /// <summary>
        /// Runs when the mono is destroyed.
        /// </summary>
        public virtual void OnOnDestroy()
        {

        }
    }

    internal sealed class HookedEffectManager
    {
        internal List<HookedEffect> hookedEffects = new List<HookedEffect>();

        public static readonly HookedEffectManager instance = new HookedEffectManager();

        internal static IEnumerator GameStart(IGameModeHandler gm)
        {
            var hookedEffects = instance.hookedEffects.ToArray();

            foreach (var hookedEffect in hookedEffects)
            {
                try
                {
                    hookedEffect.OnGameStart();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }

                if (hookedEffect.removeOnRematch)
                {
                    UnityEngine.GameObject.Destroy(hookedEffect);
                }
            }

            yield break;
        }

        internal static IEnumerator GameEnd(IGameModeHandler gm)
        {
            var hookedEffects = instance.hookedEffects.ToArray();

            foreach (var hookedEffect in hookedEffects)
            {
                try
                {
                    hookedEffect.OnGameEnd();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            yield break;
        }

        internal static IEnumerator RoundStart(IGameModeHandler gm)
        {
            var hookedEffects = instance.hookedEffects.ToArray();

            foreach (var hookedEffect in hookedEffects)
            {
                try
                {
                    hookedEffect.OnRoundStart();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            yield break;
        }

        internal static IEnumerator RoundEnd(IGameModeHandler gm)
        {
            var hookedEffects = instance.hookedEffects.ToArray();

            foreach (var hookedEffect in hookedEffects)
            {
                try
                {
                    hookedEffect.OnRoundEnd();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            yield break;
        }

        internal static IEnumerator PointStart(IGameModeHandler gm)
        {
            var hookedEffects = instance.hookedEffects.ToArray();

            foreach (var hookedEffect in hookedEffects)
            {
                try
                {
                    hookedEffect.OnPointStart();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            yield break;
        }

        internal static IEnumerator PointEnd(IGameModeHandler gm)
        {
            var hookedEffects = instance.hookedEffects.ToArray();

            foreach (var hookedEffect in hookedEffects)
            {
                try
                {
                    hookedEffect.OnPointEnd();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            yield break;
        }

        internal static IEnumerator BattleStart(IGameModeHandler gm)
        {
            var hookedEffects = instance.hookedEffects.ToArray();

            foreach (var hookedEffect in hookedEffects)
            {
                try
                {
                    hookedEffect.OnBattleStart();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            yield break;
        }

        internal static IEnumerator PickStart(IGameModeHandler gm)
        {
            var hookedEffects = instance.hookedEffects.ToArray();

            foreach (var hookedEffect in hookedEffects)
            {
                try
                {
                    hookedEffect.OnPickStart();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            yield break;
        }

        internal static IEnumerator PickEnd(IGameModeHandler gm)
        {
            var hookedEffects = instance.hookedEffects.ToArray();

            foreach (var hookedEffect in hookedEffects)
            {
                try
                {
                    hookedEffect.OnPickEnd();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            yield break;
        }

        internal static IEnumerator PlayerPickStart(IGameModeHandler gm)
        {
            var hookedEffects = instance.hookedEffects.ToArray();

            foreach (var hookedEffect in hookedEffects)
            {
                try
                {
                    hookedEffect.OnPlayerPickStart();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            yield break;
        }

        internal static IEnumerator PlayerPickEnd(IGameModeHandler gm)
        {
            var hookedEffects = instance.hookedEffects.ToArray();

            foreach (var hookedEffect in hookedEffects)
            {
                try
                {
                    hookedEffect.OnPlayerPickEnd();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            yield break;
        }
    }
}
