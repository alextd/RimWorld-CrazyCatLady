using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Crazy_Cat_Lady
{
	public static class CatIdentifier
	{
		public static bool IsCat(this Pawn pawn) =>
			pawn.def == CatDefOf.Cat ||
			pawn.RaceProps?.useMeatFrom == CatDefOf.Cat ||
			pawn.RaceProps?.leatherDef == CatDefOf.Leather_Panthera ||
			(pawn.def?.defName?.ToLower().Contains("cat") ?? false) ||
			(pawn.def?.defName?.ToLower().Contains("tiger") ?? false) ||//because it's hard to determine what is a cat
			(pawn.kindDef?.defName?.ToLower().Contains("cat") ?? false);
		//morbid way to find what big cats are
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
