using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using HarmonyLib;

namespace Crazy_Cat_Lady
{
	[StaticConstructorOnStartup]
	public static class CatIdentifier
	{
		public static HashSet<ThingDef> extraCatDefs = new();
		static CatIdentifier()
		{
			try
			{
				Log.Message($"CrazyCatLady Trying for ErinsCatOverhaulSettings");

				Type erinModType = AccessTools.TypeByName("ErinsCatOverhaulMod");
				if (erinModType == null) return;

				FieldInfo ErinsCatSettingsInfo = AccessTools.Field(erinModType, "settings");
				MethodInfo LoadedPresentAnimalsSetsInfo = AccessTools.PropertyGetter("ErinsCatOverhaulSettings:LoadedPresentAnimalsSets");
				Dictionary<ThingDef, bool> erinsCatDefs = (Dictionary<ThingDef, bool>)LoadedPresentAnimalsSetsInfo.Invoke(ErinsCatSettingsInfo.GetValue(null), new object[] { });

				extraCatDefs.AddRange(from kvp in erinsCatDefs where kvp.Value select kvp.Key);

				Log.Message($"CrazyCatLady Got ErinsCatOverhaulSettings: {extraCatDefs.ToStringSafeEnumerable()}");
			}
			catch (Exception e)
			{
				Verse.Log.Warning($"CrazyCatLady failed to get ErinsCatOverhaulSettings: {e}");
				//Sallright.
			}
		}

		public static bool IsCat(this Pawn pawn) =>
			pawn.def == CatDefOf.Cat ||
			extraCatDefs.Contains(pawn.def) || 
			pawn.RaceProps?.useMeatFrom == CatDefOf.Cat ||
			pawn.RaceProps?.leatherDef == CatDefOf.Leather_Panthera || //morbid way to find what big cats are
			(pawn.def?.defName?.ToLower().Contains("cat") ?? false) ||
			(pawn.def?.defName?.ToLower().Contains("tiger") ?? false) ||//because it's hard to determine what is a cat
			(pawn.kindDef?.defName?.ToLower().Contains("cat") ?? false);
		
	}
	public class ThoughtWorker_CrazyCatLady : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn pawn)
		{
			int catCount = pawn.Map?.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer)
				.Count(CatIdentifier.IsCat) ?? -1;

			if (catCount < 0)
				return ThoughtState.Inactive;
			if (catCount == 0)
				return ThoughtState.ActiveAtStage(0);
			if (catCount == 1)
				return ThoughtState.ActiveAtStage(1);
			if (catCount == 2)
				return ThoughtState.ActiveAtStage(2);
			if (catCount < 5)
				return ThoughtState.ActiveAtStage(3);
			if (catCount < 10)
				return ThoughtState.ActiveAtStage(4);
			return ThoughtState.ActiveAtStage(5);
		}
	}
}
