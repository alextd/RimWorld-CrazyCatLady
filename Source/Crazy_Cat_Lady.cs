using System.Reflection;
using System.Linq;
using Verse;
using RimWorld;
using UnityEngine;
using HarmonyLib;

namespace Crazy_Cat_Lady
{
	[DefOf]
	public static class CatDefOf
	{
		public static ThingDef Cat;
		public static ThingDef Leather_Panthera;
		public static MentalStateDef TD_Wander_FollowCat;
		public static TraitDef TD_CrazyCatLady;
	}

	public class Mod : Verse.Mod
	{
		public Mod(ModContentPack content) : base(content)
		{
			// initialize settings
			// GetSettings<Settings>();
#if DEBUG
			Harmony.DEBUG = true;
#endif

			Harmony harmony = new Harmony("Uuugggg.rimworld.Crazy_Cat_Lady.main");

			harmony.PatchAll();
		}

//		public override void DoSettingsWindowContents(Rect inRect)
//		{
//			base.DoSettingsWindowContents(inRect);
//			GetSettings<Settings>().DoWindowContents(inRect);
//		}
//
//		public override string SettingsCategory()
//		{
//			return "Crazy Cat Lady";
//		}
	}
}