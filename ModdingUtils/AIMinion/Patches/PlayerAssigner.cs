using System;
using System.Linq;
using HarmonyLib;
using UnboundLib;
using UnityEngine;
using InControl;

namespace ModdingUtils.AIMinion.Patches
{
	[HarmonyPatch(typeof(PlayerAssigner), "LateUpdate")]
	class PlayerAssignerPatchLateUpdate
	{
		// patch to fix lag when space is pressed
		private static bool Prefix(PlayerAssigner __instance)
		{
			if (!(bool)__instance.GetFieldValue("playersCanJoin"))
			{
				return false;
			}
			if (__instance.players.Count >= __instance.maxPlayers)
			{
				return false;
			}
			if (DevConsole.isTyping)
			{
				return false;
			}
			if (Input.GetKeyDown(KeyCode.B) && !GameManager.lockInput)
			{
				__instance.StartCoroutine(__instance.CreatePlayer(null, true));
			}
			if (Input.GetKey(KeyCode.Space))
			{
				bool flag = true;
				for (int i = 0; i < __instance.players.Count; i++)
				{
					if (__instance.players[i].playerActions != null && __instance.players[i].playerActions.Device == null)
					{
						flag = false;
					}
				}
				if (flag)
				{
					__instance.StartCoroutine(__instance.CreatePlayer(null, false));
				}
			}
			for (int j = 0; j < InputManager.ActiveDevices.Count; j++)
			{
				InputDevice inputDevice = InputManager.ActiveDevices[j];
				if ((bool)__instance.InvokeMethod("JoinButtonWasPressedOnDevice", inputDevice) && (bool)__instance.InvokeMethod("ThereIsNoPlayerUsingDevice",inputDevice))
				{
					__instance.StartCoroutine(__instance.CreatePlayer(inputDevice, false));
				}
			}
			return false;
		}
	}
}
