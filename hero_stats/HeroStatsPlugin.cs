using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using PX.Stats;
using TearsOfSerene.RPG;


[BepInPlugin("devopsdinosaur.scarlet_tower.hero_stats", "Hero Stats", "0.0.1")]
public class HeroStatsPlugin : BaseUnityPlugin {

	private Harmony m_harmony = new Harmony("devopsdinosaur.scarlet_tower.hero_stats");
	public static ManualLogSource logger;

	public class StatInfo {
		
		public string m_name;
		public float m_base;
		public float m_min;
		public float m_max;
		public ConfigEntry<float> m_config;
		
		public StatInfo(string name, float _base, float min, float max) {
			this.m_name = name;
			this.m_base = _base;
			this.m_min = min;
			this.m_max = max;
		}
	}

	private static StatInfo[] STAT_INFOS = {
		new StatInfo("MoveSpeed", 350, 0, 999999),
		new StatInfo("AttackSpeed", 1, 0, 90),
		new StatInfo("Damage", 15, 0, 999999),
		new StatInfo("Armor", 0, 0, 80),
		new StatInfo("CriticalChance", 0, 0, 100),
		new StatInfo("HPRegen", 0.3f, 0, 999999),
		new StatInfo("CriticalDamage", 50, 0, 999),
		new StatInfo("CDR", 0, 0, 90),
		new StatInfo("Luck", 0, 0, 999999),
		new StatInfo("Greed", 0, 0, 999999),
		new StatInfo("Gathering", 2, 0, 999999),
		new StatInfo("DestructionBonus", 0, 0, 100),
		new StatInfo("ReasonBonus", 0, 0, 100),
		new StatInfo("SerenityBonus", 0, 0, 100),
		new StatInfo("HarmonyBonus", 0, 0, 100),
		new StatInfo("LustBonus", 0, 0, 100),
		new StatInfo("AgonyBonus", 0, 0, 100)
	};

	private static ConfigEntry<bool> m_enabled;
	
	private void Awake() {
		logger = this.Logger;
		try {
			m_enabled = this.Config.Bind<bool>("General", "Enabled", true, "Set to false to disable this mod.");
			foreach (StatInfo info in STAT_INFOS) {
				info.m_config = this.Config.Bind<float>("General", info.m_name + " Delta", 0f, "Float amount to inc/dec the '" + info.m_name + "' stat (base stat info - starting val: " + info.m_base + ", min val: " + info.m_min + ", max val: " + info.m_max + ")");
			}
			if (m_enabled.Value) {
				this.m_harmony.PatchAll();
			}
			logger.LogInfo("devopsdinosaur.scarlet_tower.hero_stats v0.0.1 loaded.");
		} catch (Exception e) {
			logger.LogError("** Awake FATAL - " + e);
		}
	}
	
	[HarmonyPatch(typeof(Hero), "Start")]
	class HarmonyPatch_Hero_Start {

		private static void Postfix(Hero __instance) {
			try {
				if (!m_enabled.Value) {
					return;
				}
				foreach (StatInfo info in STAT_INFOS) {
					__instance.Stats.GetFloatStat(info.m_name).AddModifier(new AttributeModifier(info.m_config.Value, "hero_stats_mod", 0, AttributeType.Simple));
				}
			} catch (Exception e) {
				logger.LogError("Hero.Start_Postfix ERROR - " + e);
			}
		}
	}
}