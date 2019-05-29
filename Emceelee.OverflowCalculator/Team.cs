using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Emceelee.OverflowCalculator
{
    public class Team
    {
        private List<Participant> Participants { get; set; } = new List<Participant>();

        public void AddParticipant(string name, decimal target, decimal raised)
        {
            Participants.Add(new Participant(name, target, raised));
        }

        public void ResolveOverflows()
        {
            Console.Out.WriteLine("Original");
            PrintTeamStatus();

            decimal teamSurplus = Participants.Sum(p => p.CollectSurplus());
            var originalTeamSurplus = teamSurplus;

            while (teamSurplus > 0 && !TeamIsFullyFundraised())
            {
                var targets = GetUnfundedParticipants();
                var splits = ProgressiveRounder.GetSplits(teamSurplus, targets.Count);

                Debug.Assert(targets.Count == splits.Count);

                for (int i = 0; i < targets.Count; ++i)
                {
                    var amount = splits[i];
                    var participant = targets[i];
                    teamSurplus -= amount;
                    teamSurplus += participant.AllocateOverflow(amount);
                }
            }

            Console.Out.WriteLine("Final");
            PrintTeamStatus();

            decimal teamOverflow = Participants.Sum(p => p.AmountFromOverflow);

            Console.Out.WriteLine($"Original Surplus: {originalTeamSurplus}");
            Console.Out.WriteLine($"Amount From Overflow: {teamOverflow}");
        }

        public bool TeamIsFullyFundraised()
        {
            foreach(var p in Participants)
            {
                if (!p.IsFullyFundraised)
                    return false;
            }
            return true;
        }

        public List<Participant> GetUnfundedParticipants()
        {
            return Participants.Where(p => !p.IsFullyFundraised).ToList();
        }

        public string GetParticipantStatus(Participant participant)
        {
            return String.Format(
@"Name: {0}
IsFullyFundraised: {1}
AmountTarget: {2}
AmountRaisedOriginal: {3}
AmountRaised: {4}
AmountFromOverflow: {5}
AmountSurplus: {6}
",
            participant.Name,
            participant.IsFullyFundraised,
            participant.AmountTarget,
            participant.AmountRaisedOriginal,
            participant.AmountRaised,
            participant.AmountFromOverflow,
            participant.AmountSurplus);
        }

        public void PrintTeamStatus()
        {
            foreach(var p in Participants)
            {
                Console.Out.WriteLine(GetParticipantStatus(p));
            }
        }

        public class Participant
        {
            public Participant(string name, decimal target, decimal raised)
            {
                Name = name;
                AmountTarget = target;
                AmountRaisedOriginal = raised;

                if(raised > target)
                {
                    AmountRaised = target;
                    AmountSurplus = raised - target;
                }
                else
                {
                    AmountRaised = raised;
                }
            }
            public string Name { get; }
            public decimal AmountTarget { get; }
            public decimal AmountRaisedOriginal { get; }
            public decimal AmountRaised { get; private set; } = 0;
            public decimal AmountFromOverflow { get; private set; } = 0;
            public decimal AmountSurplus { get; private set; } = 0;
            public bool IsFullyFundraised
            {
                get { return AmountRaised == AmountTarget; }
            }

            public decimal CollectSurplus()
            {
                var surplus = AmountSurplus;
                AmountSurplus = 0;
                return surplus;
            }

            public decimal AllocateOverflow(decimal amount)
            {
                var remainder = AmountTarget - AmountRaised;
                decimal surplus = 0;
                decimal amountToAdd = 0;

                if (amount > remainder)
                {
                    amountToAdd = remainder;
                    surplus = amount - amountToAdd;
                }
                else
                {
                    amountToAdd = amount;
                }

                AmountRaised += amountToAdd;
                AmountFromOverflow += amountToAdd;

                return surplus;
            }
        }
    }
}
