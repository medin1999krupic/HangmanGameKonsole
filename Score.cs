using System;

namespace HangMan
{
    public class Score
    {


        public Score()
        {
            Matches = 0;
            Wins = 0;
            Losts = 0;
            Points = 0;
            KD = 0;
        }
        public int Matches { get; set; }
        public int Wins { get; set; }

        public int Losts { get; set; }

        public int Points { get; set; }

        public int KD { get; set; }

        public string Liga { get
            {
                string result = "";

                    if (this.Points >= 0 && this.Points <= 99)
                    {
                     //   Console.WriteLine(100 - this.Points + " bis Liga Bronze");
                    result= "keine Liga";
                    }

                    else if (this.Points >= 100 && this.Points <= 249)
                    {
                       // Console.WriteLine(250 - this.Points + " bis Liga Silber");
                    result = "Bronze";
                    }
                    else if (this.Points >= 250 && this.Points <= 499)
                    {
                      //  Console.WriteLine(500 - this.Points + " bis Liga Gold");
                    result = "Silber";
                    }
                    else if (this.Points >= 500 && this.Points <= 749)
                    {
                      //  Console.WriteLine(750 - this.Points + " bis Liga Platin");
                    result = "Gold";
                    }
                    else if (this.Points >= 750 && this.Points <= 999)
                    {
                       // Console.WriteLine(1000 - this.Points + " bis Liga Diamant");
                    result = "Platin";
                    }
                    else if (this.Points >= 1000)
                    {
                        //Console.WriteLine("Du hast die höhste Liga erreicht.");
                    result = "Diamant";
                    }

                return result;
            }
        }

    }
}