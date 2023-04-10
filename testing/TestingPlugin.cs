using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using PX.Stats;
using TearsOfSerene.RPG;


[BepInPlugin("devopsdinosaur.scarlet_tower.testing", "Testing", "0.0.1")]
public class ActionSpeedPlugin : BaseUnityPlugin {

	private Harmony m_harmony = new Harmony("devopsdinosaur.scarlet_tower.testing");
	public static ManualLogSource logger;

	private static ConfigEntry<bool> m_enabled;
	
	private void Awake() {
		logger = this.Logger;
		try {
			m_enabled = this.Config.Bind<bool>("General", "Enabled", true, "Set to false to disable this mod.");
			if (m_enabled.Value) {
				this.m_harmony.PatchAll();
			}
			logger.LogInfo("devopsdinosaur.scarlet_tower.testing v0.0.1 loaded.");
		} catch (Exception e) {
			logger.LogError("** Awake FATAL - " + e);
		}
	}

	/*
	[HarmonyPatch(typeof(Stats), "Awake")]
	class HarmonyPatch_Stats_Awake {

		private static void Postfix(Dictionary<string, FloatStat> ____floatStats) {
			try {
				if (!m_enabled.Value) {
					return;
				}
				logger.LogInfo("Stats.Awake_Postfix");
				if (!____floatStats.ContainsKey("Gathering")) {
					return;
				}
				string output = "\n";
				foreach (string key in ____floatStats.Keys) {
					FloatStat stat = ____floatStats[key];
					output += "new StatInfo(\"" + key + "\", " + stat.Base + ", " + stat.minValue + ", " + stat.maxValue + "),\n";
				}
				logger.LogInfo(output);
			} catch (Exception e) {
				logger.LogError("Stats.Awake_Postfix ERROR - " + e);
			}
		}
	}
	*/
}