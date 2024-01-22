using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LR10_C121_GornakDmitri
{
    public class Teams
    {
        public string Teamfirst { get; set; }
        public string Teamsecond { get; set; }
        public DateTime? DateMatch { get; set; }
        public string TypeMatch { get; set; }
        public string Stadion { get; set; }
        public string TypeStadion { get; set; }

        public int TeamfirstPoints { get; set; }
        public int TeamsecondPoints { get; set; }

        public int Id { get; set; }
        //public double Result => SetOutcome();

        public Teams(string teamfirst, string teamsecond, DateTime? dateMatch, string typematch, string stadion, string typeStadion, string proverka)
        {
            Teamfirst = teamfirst;
            Teamsecond = teamsecond;
            DateMatch = dateMatch;
            TypeMatch = typematch;
            Stadion = stadion;
            TypeStadion = typeStadion;
            if (proverka == "Победа")
            {
                TeamfirstPoints = 2;
                TeamsecondPoints = 0;
            }
            if (proverka == "Поражение")
            {
                TeamfirstPoints = 0;
                TeamsecondPoints = 2;
            }
            if (proverka == "Ничья")
            {
                TeamfirstPoints = 1;
                TeamsecondPoints = 1;
            }
        }
    }

}

