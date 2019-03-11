using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using RimWorld;
using Harmony;

namespace Crazy_Cat_Lady
{
	[HarmonyPatch(typeof(InteractionWorker_Nuzzle), "AddNuzzledThought")]
	class NuzzleDistraction
	{
		//private void AddNuzzledThought(Pawn initiator, Pawn recipient)
		public static void Postfix(Pawn initiator, Pawn recipient)
		{
			if(initiator.IsCat())
				recipient.mindState.mentalStateHandler.TryStartMentalState(CatDefOf.TD_Wander_FollowCat, transitionSilently: true);
		}
	}


	public class JobGiver_WanderCat : JobGiver_Wander
	{
		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IEnumerable<Thing> cats = pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer)
				.Where(CatIdentifier.IsCat).Cast<Thing>();
			Thing cat = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, cats, PathEndMode.ClosestTouch, TraverseParms.For(pawn));
			if (cat == null)
			{
				return IntVec3.Invalid;
			}
			return cat.Position;
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}
	}

	public class MentalState_TD_Wander_Cat: MentalState
	{
		public override void PostStart(string reason)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn) && GetBeginLetterText() is string text)
				Messages.Message(text, this.pawn, MessageTypeDefOf.NegativeEvent, false);
			base.PostStart(reason);
		}
		
	}

	public class MentalStateWorker_CatDistraction : MentalStateWorker
	{
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			if (!pawn.Spawned)
			{
				return false;
			}
			return pawn.story.traits.HasTrait(CatDefOf.TD_CrazyCatLady);
		}
	}
}
