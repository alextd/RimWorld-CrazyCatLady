using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Crazy_Cat_Lady
{
	[DefOf]
	public static class CatDefOf
	{
		public static ThingDef Cat;
		public static ThingDef Leather_Panthera;
	}
	public class ThoughtWorker_CrazyCatLady : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn pawn)
		{
			int catCount = pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer)
				.Count(p => p.def == CatDefOf.Cat ||
				p.RaceProps.leatherDef == CatDefOf.Leather_Panthera);//morbid way to find what big cats are

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
