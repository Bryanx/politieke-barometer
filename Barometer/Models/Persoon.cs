using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Barometer.Models {
    public class Persoon {
        public int Id  { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalMentions { get; set; }
        public int Positive { get; set; }
        public int Negative { get; set; }
        public int Trend { get; set; }

        public Persoon(int id, string firstName, string lastName, int totalMentions, int positive, int negative, int trend) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            TotalMentions = totalMentions;
            Positive = positive;
            Negative = negative;
            Trend = trend;
        }

        //temp data
        public static IEnumerable<Persoon> getPersonen() {
            List<Persoon> list = new List<Persoon>();
            Random rnd = new Random();
            list.Add(new Persoon(1,"Bryan", "Knight", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(2,"Thom", "Verstraten", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(3,"Jarne", "Van Aerde", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(4,"James", "Hetfield", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(5,"Anthony", "Torfs", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(6,"Robbe", "Dillen", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(1,"Bryan", "Knight", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(7,"Thom", "Verstraten", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(8,"Jarne", "Van Aerde", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(9,"James", "Hetfield", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(10,"Anthony", "Torfs", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(11,"Robbe", "Dillen", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(1,"Bryan", "Knight", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(12,"Thom", "Verstraten", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(13,"Jarne", "Van Aerde", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(14,"James", "Hetfield", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(15,"Anthony", "Torfs", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(16,"Robbe", "Dillen", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(1,"Bryan", "Knight", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(17,"Thom", "Verstraten", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(18,"Jarne", "Van Aerde", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(19,"James", "Hetfield", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(20,"Anthony", "Torfs", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(21,"Robbe", "Dillen", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(1,"Bryan", "Knight", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(22,"Thom", "Verstraten", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(23,"Jarne", "Van Aerde", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(24,"James", "Hetfield", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(25,"Anthony", "Torfs", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            list.Add(new Persoon(26,"Robbe", "Dillen", rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930), rnd.Next(1, 34930)));
            
            return list.AsEnumerable();
        }
    }
}