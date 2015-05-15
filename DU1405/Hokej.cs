using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU1405
{
    class ScoreBoard
    {
        public Team Domaci { get; set; }
        public Team Hostia { get; set; }

        private int golyDomacich;
        private int golyHosti;

        public ScoreBoard(Team domaci, Team hostia)
        {
            Domaci = domaci;
            Hostia = hostia;
        }

        public void ShowCore()
        {
            Console.WriteLine(golyDomacich + ":" + golyHosti);
        }

        public void Goal(Team team)
        {
            if (team == Domaci)
            {
                golyDomacich++;
            }
            else
            {
                golyHosti++;
            }
            ShowCore();
        }

    }
    class Team
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
        
    }

    class Fans
    {
        public string Name { get; set; }
        public Team FansOf { get; set; }

        

        public void Applause()
        {
            Console.WriteLine(FansOf + " do toho!");
        }

        public void Whistle()
        {
            Console.WriteLine("Fuuuuj!");
        }

        public void Goal(Team team)
        {
            if (team == FansOf)
            {
                Applause();
            }
            else
            {
                Whistle();
            }
        }

    }
    class HockeyMatch
    {
        public delegate void OnGoal(Team team);

        public OnGoal goalScored;

        public Team Domaci { get; set; }
        public Team Hostia { get; set; }

        public HockeyMatch(Team domaci, Team hostia)
        {
            Domaci = domaci;
            Hostia = hostia;
        }

        public void GoalDomaci()
        {
            Goal(Domaci);
        }
        public void GoalHostia()
        {
            Goal(Hostia);
        }
        public void Goal(Team team)
        {
            Console.WriteLine("Goal scored");
            goalScored(team);
        }

    }

    class Program2
    {
        static int Main(string[] args)
        {
            Team kosice = new Team() { Name = "HC Kosice" };
            Team bratislava = new Team() { Name = "Slovan Bratislava" };

            Fans kosiceFans = new Fans() { Name = "Saleny vranare", FansOf = kosice };
            Fans bratislavaFans = new Fans() { Name = "Pastekari", FansOf = bratislava };

            ScoreBoard scoreBoard = new ScoreBoard(kosice, bratislava);

            HockeyMatch match = new HockeyMatch(kosice, bratislava);

            match.goalScored += scoreBoard.Goal;
            match.goalScored += kosiceFans.Goal;
            match.goalScored += bratislavaFans.Goal;

            match.GoalDomaci();
            match.GoalDomaci();
            match.GoalDomaci();
            match.GoalHostia();
            match.GoalHostia();


            Console.ReadLine();
            return 0;
        }
    }
}
