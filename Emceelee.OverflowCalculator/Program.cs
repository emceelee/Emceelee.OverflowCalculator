using System;

namespace Emceelee.OverflowCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var team = new Team();
            team.AddParticipant("Lims", 9000, 7605);
            team.AddParticipant("Matt", 2750, 3570);
            team.AddParticipant("Hong", 2750, 2201);
            team.AddParticipant("Jeremy", 2750, 1003);
            team.AddParticipant("Henry", 2750, 2770);
            team.AddParticipant("Daniel", 2750, 1152);
            team.AddParticipant("Chris", 2750, 2651);
            team.AddParticipant("PD", 2750, 6327);

            team.ResolveOverflows();
            Console.ReadLine();
        }
    }
}
