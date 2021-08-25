using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnboundLib;
using Photon.Pun;
using UnboundLib.Cards;
using System.Reflection;
using HarmonyLib;
using UnboundLib.Networking;
using UnityEngine;
using TMPro;
using ModdingUtils.Extensions;
using UnityEngine.UI;

namespace ModdingUtils.Utils
{
    public sealed class CardBarUtils
    {
        // singleton design
        public static readonly CardBarUtils instance = new CardBarUtils();

        private static readonly float displayDuration = 1.5f;
        public static readonly Vector3 localShift = new Vector3(-50f, 0f, 0f);
        public static readonly float barlocalScaleMult = 1.1f;
        public static readonly float cardLocalScaleMult = 1f;

        private DictionaryOfLists<Player, CardInfo> cardsToShow = new DictionaryOfLists<Player, CardInfo>();

        private CardBar[] CardBars
        {
            get
            {
                return (CardBar[])Traverse.Create(CardBarHandler.instance).Field("cardBars").GetValue();
            }
        }

        public CardBar PlayersCardBar(int teamID)
        {
            return CardBars[teamID];
        }
        public CardBar PlayersCardBar(Player player)
        {
            return CardBars[player.teamID];
        }

        private CardBarUtils()
        {
            CardBarUtils instance = this;
        }

        private void Reset()
        {
            cardsToShow = new DictionaryOfLists<Player, CardInfo>();
        }
        private void Reset(Player player)
        {
            cardsToShow[player] = new List<CardInfo>();
        }

        public void ShowAtEndOfPhase(Player player, CardInfo card)
        {
            if (card == null)
            {
                return;
            }

            List<CardInfo> newList = new List<CardInfo>(cardsToShow[player]);
            newList.Add(card);
            cardsToShow[player] = newList;
        }

        public void ShowCard(Player player, CardInfo card)
        {
            ShowCard(player.teamID, card.name);
        }
        public void ShowCard(int teamID, CardInfo card)
        {
            ShowCard(teamID, card.name);
        }
        public void ShowCard(Player player, int cardID)
        {
            ShowCard(player.teamID, Cards.instance.GetCardWithID(cardID).name);
        }
        public void ShowCard(int teamID, int cardID)
        {
            ShowCard(teamID, Cards.instance.GetCardWithID(cardID).name);
        }
        public void ShowCard(int teamID, string cardName)
        {
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(CardBarUtils), nameof(RPCA_ShowCard), new object[] { teamID, cardName });
            }
        }

        [UnboundRPC]
        private static void RPCA_ShowCard(int teamID, string cardName)
        {
            int cardID = Cards.instance.GetCardID(cardName);

            try
            {
                if (Cards.instance.GetCardWithID(cardID) == null) { return; }
            }
            catch
            {
                return;
            }
            instance.PlayersCardBar(teamID).OnHover(Cards.instance.GetCardWithID(cardID), Vector3.zero);
            ((GameObject)Traverse.Create(instance.PlayersCardBar(teamID)).Field("currentCard").GetValue()).gameObject.transform.localScale = Vector3.one * cardLocalScaleMult;

        }

        public void HideCard(Player player)
        {
            HideCard(player.teamID);
        }
        public void HideCard(int teamID)
        {
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(CardBarUtils), nameof(RPCA_HideCard), new object[] { teamID });
            }
        }
        [UnboundRPC]
        private static void RPCA_HideCard(int teamID)
        {
            instance.PlayersCardBar(teamID).StopHover();
        }
        [UnboundRPC]
        private static void RPCA_HighlightCardBar(int teamID)
        {
            instance.PlayersCardBar(teamID).gameObject.transform.localScale = Vector3.one * barlocalScaleMult;
            instance.PlayersCardBar(teamID).gameObject.transform.localPosition += localShift;
            instance.ChangePlayersLineColor(teamID, Color.white);
            Color.RGBToHSV(instance.GetPlayersBarColor(teamID), out float h, out float s, out float v);
            instance.ChangePlayersBarColor(teamID, Color.HSVToRGB(h, s + 0.1f, v + 0.1f));
        }
        [UnboundRPC]
        private static void RPCA_UnhighlightCardBar(int teamID, float r, float g, float b, float a)
        {
            instance.PlayersCardBar(teamID).gameObject.transform.localScale = Vector3.one * 1f;
            instance.PlayersCardBar(teamID).gameObject.transform.localPosition -= localShift;
            instance.ResetPlayersLineColor(teamID);
            instance.ChangePlayersBarColor(teamID, new Color(r,g,b,a));
        }
        public Color HighlightCardBar(int teamID)
        {
            Color orig = GetPlayersBarColor(teamID);
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(CardBarUtils), nameof(RPCA_HighlightCardBar), new object[] { teamID });
            }
            return orig;
        }
        public void UnhighlightCardBar(int teamID, Color original_color)
        {
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(CardBarUtils), nameof(RPCA_UnhighlightCardBar), new object[] { teamID, original_color.r, original_color.g, original_color.b, original_color.a });
            }
        }
        public GameObject GetCardBarSquare(int teamID, int idx)
        {
            return GetCardBarSquares(teamID)[idx + 1];
        }
        public GameObject GetCardBarSquare(Player player, int idx)
        {
            return GetCardBarSquare(player.teamID, idx);
        }

        public GameObject[] GetCardBarSquares(int teamID)
        {
            List<GameObject> children = new List<GameObject>();

            foreach (Transform child in PlayersCardBar(teamID).transform)
            {
                children.Add(child.gameObject);
            }

            return children.ToArray();
        }

        public GameObject[] GetCardBarSquares(Player player)
        {
            return GetCardBarSquares(player.teamID);
        }
        public void ResetPlayersLineColor(int teamID)
        {
            List<Graphic> graphics = PlayersCardBar(teamID).gameObject.GetComponentsInChildren<Graphic>().Where(gr => !gr.gameObject.name.Contains("CarrdOrange")).ToList();

            foreach (Graphic graphic in graphics)
            {
                if (graphic.gameObject.name.Contains("Card"))
                {
                    graphic.color = new Color(0.462f, 0.462f, 0.462f, 1f);
                }
                else if (graphic.gameObject.name.Contains("Text"))
                {
                    graphic.color = new Color(0.6509f, 0.6509f, 0.6509f, 1f);
                }
            }
        }
        public void ResetPlayersLineColor(Player player)
        {
            ResetPlayersLineColor(player.teamID);
        }
        public void ChangePlayersLineColor(int teamID, Color color)
        {
            List<Graphic> graphics = PlayersCardBar(teamID).gameObject.GetComponentsInChildren<Graphic>().Where(gr => !gr.gameObject.name.Contains("CarrdOrange")).ToList();

            foreach (Graphic graphic in graphics)
            {
                graphic.color = color;
            }
        }
        public void ChangePlayersLineColor(Player player, Color color)
        {
            ChangePlayersLineColor(player.teamID, color);
        }

        public Color GetPlayersBarColor(int teamID)
        {
            List<Graphic> graphics = PlayersCardBar(teamID).gameObject.GetComponentsInChildren<Graphic>().Where(gr => gr.gameObject.name.Contains("CarrdOrange")).ToList();

            return graphics[0].color;
        }
        public Color GetPlayersBarColor(Player player)
        {
            return GetPlayersBarColor(player.teamID);
        }

        public Color ChangePlayersBarColor(int teamID, Color color)
        {
            List<Graphic> graphics = PlayersCardBar(teamID).gameObject.GetComponentsInChildren<Graphic>().Where(gr => gr.gameObject.name.Contains("CarrdOrange")).ToList();

            Color orig = graphics[0].color;

            foreach (Graphic graphic in graphics)
            {
                graphic.color = color;
            }

            return orig;
        }

        public Color ChangePlayersBarColor(Player player, Color color)
        {
            return ChangePlayersBarColor(player.teamID, color);
        }

        public Color GetCardSquareColor(GameObject cardSquare)
        {
            List<Graphic> graphics = cardSquare.GetComponentsInChildren<Graphic>().ToList();
            return graphics[0].color;
        }
        public Color ChangeCardSquareColor(GameObject cardSquare, Color color)
        {
            List<Graphic> graphics = cardSquare.GetComponentsInChildren<Graphic>().ToList();
            Color orig = graphics[0].color;

            foreach(Graphic graphic in graphics)
            {
                graphic.color = color;
            }
            return orig;
        }

        public static void SilentAddToCardBar(int teamID, CardInfo card, string twoLetterCode = "")
        {
            Traverse.Create(instance.PlayersCardBar(teamID)).Field("ci").SetValue(card);
            GameObject source = (GameObject)Traverse.Create(instance.PlayersCardBar(teamID)).Field("source").GetValue();
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(source, source.transform.position, source.transform.rotation, source.transform.parent);
            gameObject.transform.localScale = Vector3.one;
            string text = card.cardName;
            if (twoLetterCode != "") { text = twoLetterCode; }
            while (text.Length < 2)
            {
                text += " ";
            }
            text = text.Substring(0, 2);
            string text2 = text[0].ToString().ToUpper();
            if (text.Length > 1)
            {
                string str = text[1].ToString().ToLower();
                text = text2 + str;
            }
            else
            {
                text = text2;
            }
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
            Traverse.Create(gameObject.GetComponent<CardBarButton>()).Field("card").SetValue(card);
            gameObject.gameObject.SetActive(true);
        }
        public static void SilentAddToCardBar(Player player, CardInfo card, string twoLetterCode = "")
        {
            SilentAddToCardBar(player.teamID, card, twoLetterCode);
        }

        internal System.Collections.IEnumerator EndPickPhaseShow()
        {
            foreach (Player player in PlayerManager.instance.players)
            {
                Color orig = Color.clear;
                try
                {
                    orig = GetPlayersBarColor(player);
                }
                catch
                {
                    continue;
                }

                if (cardsToShow[player].Count > 0)
                {
                    orig = HighlightCardBar(player.teamID);
                }
                foreach (CardInfo card in cardsToShow[player].Where(card => player.data.currentCards.Select(card => card.name).Contains(card.name)))
                {

                    ShowCard(player, card);
                    yield return new WaitForSecondsRealtime(displayDuration);
                    HideCard(player);
                }
                if (cardsToShow[player].Count > 0)
                {
                    UnhighlightCardBar(player.teamID, orig);
                }
            }
            Reset();
            yield break;
        }
        public System.Collections.IEnumerator ShowImmediate(int teamID, int cardID, float? duration = null)
        {
            float displayDuration = duration ?? CardBarUtils.displayDuration;

            Color orig = HighlightCardBar(teamID);


            ShowCard(teamID, cardID);
            yield return new WaitForSecondsRealtime(displayDuration);
            HideCard(teamID);


            UnhighlightCardBar(teamID, orig);


            yield break;
        }

        public System.Collections.IEnumerator ShowImmediate(Player player, int cardID, float? duration = null)
        {
            return ShowImmediate(player.teamID, cardID, duration);
        }
        public System.Collections.IEnumerator ShowImmediate(Player player, CardInfo card, float? duration = null)
        {
            return ShowImmediate(player.teamID, Cards.instance.GetCardID(card), duration);
        }
        public System.Collections.IEnumerator ShowImmediate(int teamID, CardInfo card, float? duration = null)
        {
            return ShowImmediate(teamID, Cards.instance.GetCardID(card), duration);
        }
        public System.Collections.IEnumerator ShowImmediate(Player player, int[] cardIDs, float? duration = null)
        {
            return ShowImmediate(player.teamID, cardIDs, duration);
        }
        public System.Collections.IEnumerator ShowImmediate(int teamID, CardInfo[] cards, float? duration = null)
        {
            List<int> cardIDs = new List<int>();
            foreach (CardInfo card in cards)
            {
                cardIDs.Add(Cards.instance.GetCardID(card));
            }

            return ShowImmediate(teamID, cardIDs.ToArray(), duration);
        }
        public System.Collections.IEnumerator ShowImmediate(Player player, CardInfo[] cards, float? duration = null)
        {
            List<int> cardIDs = new List<int>();
            foreach (CardInfo card in cards)
            {
                cardIDs.Add(Cards.instance.GetCardID(card));
            }

            return ShowImmediate(player.teamID, cardIDs.ToArray(), duration);
        }
        public System.Collections.IEnumerator ShowImmediate(Player player, int cardID)
        {
            return ShowImmediate(player, cardID, null);
        }
        public System.Collections.IEnumerator ShowImmediate(Player player, CardInfo card)
        {
            return ShowImmediate(player, card, null);
        }
        public System.Collections.IEnumerator ShowImmediate(int teamID, CardInfo card)
        {
            return ShowImmediate(teamID, card, null);
        }
        public System.Collections.IEnumerator ShowImmediate(Player player, int[] cardIDs)
        {
            return ShowImmediate(player, cardIDs, null);
        }
        public System.Collections.IEnumerator ShowImmediate(int teamID, CardInfo[] cards)
        {
            return ShowImmediate(teamID, cards, null);
        }
        public System.Collections.IEnumerator ShowImmediate(Player player, CardInfo[] cards)
        {
            return ShowImmediate(player, cards, null);
        }
        public System.Collections.IEnumerator ShowImmediate(int teamID, int[] cardIDs, float? duration = null)
        {
            float displayDuration = duration ?? CardBarUtils.displayDuration;

            Color orig = HighlightCardBar(teamID);

            foreach (int cardID in cardIDs)
            {
                ShowCard(teamID, cardID);
                yield return new WaitForSecondsRealtime(displayDuration);
                HideCard(teamID);
            }

            UnhighlightCardBar(teamID, orig);


            yield break;
        }
        
        public void ClearCardBar(Player player)
        {
            if (PhotonNetwork.OfflineMode)
            {
                PlayersCardBar(player).ClearBar();
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(CardBarUtils), "RPCA_ClearCardBar", new object[] { player.data.view.ControllerActorNr });
            }
        }

        [UnboundRPC]
        private static void RPCA_ClearCardBar(int actorID)
        {
            Player playerToReset = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithActorID",
            BindingFlags.Instance | BindingFlags.InvokeMethod |
            BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { actorID });

            instance.PlayersCardBar(playerToReset.teamID).ClearBar();
        }
    }
    public class DictionaryOfLists<TKey, TListValue> : Dictionary<TKey, List<TListValue>>
    {
        public DictionaryOfLists() : base() { }
        public new List<TListValue> this[TKey key]
        {
            get
            {
                return TryGetValue(key, out List<TListValue> t) ? t : new List<TListValue>();
            }
            set { base[key] = value; }
        }
    }
}
