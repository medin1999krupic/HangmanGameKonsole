using HangMan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HangManConsole
{
    class Program
    {

        public static string[] HANGMANPICS = new string[] { @"
      
      
      
      
      
      
=========", @"
      
      |
      |
      |
      |
      |
=========", @"
  +---+
      |
      |
      |
      |
      |
=========", @"
  +---+
  |   |
      |
      |
      |
      |
=========", @"
  +---+
  |   |
  O   |
      |
      |
      |
=========", @"
  +---+
  |   |
  O   |
  |   |
      |
      |
=========", @"
  +---+
  |   |
  O   |
 /|   |
      |
      |
=========", @"
  +---+
  |   |
  O   |
 /|\  |
      |
      |
=========", @"
  +---+
  |   |
  O   |
 /|\  |
 /    |
      |
=========", @"
  +---+
  |   |
  O   |
 /|\  |
 / \  |
      |
=========" };

        public static string HANGMANPICSWonn = @"
  +---+
  |   |
      |
  O   |
 /|\  |
 / \  |
=========";


        public static int Versuche { get; set; }
        public static bool GameStarted { get; set; }
        //public static bool Gewonnen { get; set; }

        public static string gesuchtesWort = string.Empty;

        private static User currentUser;

        public static List<string> falscheBuchstaben { get; set; }

        public static char[] gesuchtesWortarray;
        public static char[] gesuchtesWortarrayToShow;
        public static Users Users { get; set; }
        public static bool GuestUser { get; set; }


        public static int Fehler = 0;

        public static int NochOffen = 0;

        public static List<string> wordsdictionary = new List<string>();

        static void Main(string[] args)
        {
            ProjektStart();
            GuestUser = true;

            string frage = "";
            while ((frage = Console.ReadLine().ToUpper()) != "EXIT")
            {
               

                if (frage.ToUpper() == "SHOWUSERS")
                {
                    
                    foreach(var user in Users.User)
                    {
                        Console.WriteLine($"Name: {user.Name}, Punkte: {user.Score.Points}, Liga: {user.Score.Liga}");
                    }
                }

                if (frage.ToUpper() == "GAST")
                {
                    PlayAsGuestUser();
                }

                if (frage.ToUpper() == "ANMELDEN")
                {
                    if(currentUser == null)
                    {
                        Login();
                    }
                    else
                    {
                        Console.WriteLine("Du bist bereits Angemeldet.");
                    }
                }

                if (frage.ToUpper() == "REGISTRIEREN")
                {
                    Register();
                }

                if (frage.ToUpper() == "ABMELDEN")
                {

                    if(currentUser != null)
                    {
                        currentUser = null;
                        Console.WriteLine("Du wurdest erfolgreich Abgemeldet.");
                        GuestUser = true;
                    }
                    else
                    {
                        Console.WriteLine("Du kannst dich als Gast nicht Abmelden.");
                    }
                }

                if (frage.ToUpper() == "START")
                {
                    if (!GameStarted)
                        StartGame();

                    string getesteterBuchstabe = string.Empty;
                    while (GameStarted)
                    {
                        Console.WriteLine("Gebe einen Buchstaben ein deiner Wahl, um einen buchstaben vom generiertem Wort zu erraten.");
                        getesteterBuchstabe = Console.ReadLine().ToUpper();

                        //--------------
                       if(getesteterBuchstabe.Length == 1)
                        {

                      

                        if (NochOffen == 0)
                        {
                            Gewonnen();

                        }
                        else if (Fehler == 10)
                        {
                            Verloren();
                        }

                        bool istEnthalten = false;

                        istEnthalten = gesuchtesWort.Contains(getesteterBuchstabe);

                        if (istEnthalten)
                        {
                            for (int i = 0; i < gesuchtesWortarray.Length; i++)
                            {
                                if (gesuchtesWortarray[i].ToString() == getesteterBuchstabe)
                                {
                                    if (gesuchtesWortarrayToShow[i] == '_')
                                    {
                                        gesuchtesWortarrayToShow[i] = Char.Parse(getesteterBuchstabe);
                                        NochOffen--;
                                    }
                                    else
                                    {
                                        Console.WriteLine(HANGMANPICS[Fehler]);
                                        Fehler++;

                                        if (!falscheBuchstaben.Contains(getesteterBuchstabe))
                                            falscheBuchstaben.Add(getesteterBuchstabe);

                                        ShowFehlerListe();
                                    }
                                }

                            }
                        }
                        else
                        {
                            //Panels[Fehler].Visible = true;
                            Console.WriteLine(HANGMANPICS[Fehler]);
                            Fehler++;

                            if (!falscheBuchstaben.Contains(getesteterBuchstabe))
                                falscheBuchstaben.Add(getesteterBuchstabe);

                            ShowFehlerListe();

                        }
                        if (NochOffen == 0)
                        {
                            Gewonnen();
                            //Console.WriteLine(HANGMANPICSWonn);
                        }
                        else if (Fehler == 10)
                        {
                            Verloren();
                        }
                        ShowWord();
                        }
                       else
                        {
                            //Wort
                            if(getesteterBuchstabe =="EXIT")
                            {
                                ExitGame();
                                GameStarted = false;
                            }
                            else
                            {
                                //ganzes Wort eingetippt
                                if (getesteterBuchstabe == gesuchtesWort)
                                {
                                    Gewonnen();
                                }
                                else
                                {
                                    Verloren();
                                    Console.WriteLine(HANGMANPICS[HANGMANPICS.Length-1]);
                                }
                                    
                                ShowWord();
                            }
                        }

                    }


                }

            }
        }

        private static void Register()
        {
            Console.WriteLine("Gebe deinen Benutzernamen ein.");
            string name = Console.ReadLine();
            Console.WriteLine("Gebe dein Passwort ein.");

            string pwd = Console.ReadLine();

            if (name == "" || pwd == "")
            {
                Console.WriteLine("Bitte gültigen text eingeben.");
                return;
            }

            Console.WriteLine("Daten sind gespeichert. Du bist Regestriert.");
            Console.WriteLine("Merke dir deine Daten.");

            User newuser = new User();
            newuser.Name = name;
            newuser.Password = pwd;

            Users.User.Add(newuser);

            Save();
        }

        private static void ShowFehlerListe()
        {
            Console.WriteLine("Folgende Fehler wurden bereits eingegeben:");
            foreach (string fehler in falscheBuchstaben)
            {
                Console.WriteLine(fehler);
            }
        }

        private static void Login()
        {

            if (currentUser != null)
            {
                Console.WriteLine("Du bist bereits angemeldet " + currentUser.Name + ".");
                return;
            }

            Console.WriteLine("Gebe deinen Benutzernamen ein.");
            string name = Console.ReadLine();

            Console.WriteLine("Gebe dein Passwort ein.");
            string pwd = Console.ReadLine();


            Console.WriteLine("Anmeldung wird mit den Regestrierten Daten verglichen.");


            if (name == "" || pwd == "")
            {
                Console.WriteLine("Bitte gültigen text eingeben.");
                return;
            }

            User foundUser = Users.User.FirstOrDefault(ee => ee.Name == name && ee.Password == pwd);

            if (foundUser != null)
            {
                GuestUser = false;
                currentUser = foundUser;
            }
            else
            {
                Console.WriteLine("Benutzername oder Passwort sind falsch.");
                Console.WriteLine("Bitte Registriere dich oder spiele als Gast.");
            }

            ShowUser();
            Console.WriteLine("Gebe 'Start' ein um das Spiel zu Starten.");
        }

        private static void ProjektStart()
        {
            Console.WriteLine("Hang Man-Projekt wird geöffnet.");
            Console.WriteLine("Wenn du dich Registrieren willst, gebe 'Registrieren' ein.");//Registrieren
            Console.WriteLine("Wenn du dich anmelden willst, gebe 'Anmelden' ein.");//Anmelden
            Console.WriteLine("Gebe 'Gast' ein, um als Gast fortzufahren.");//Als Gast
            
            Console.WriteLine("Gebe 'Start' ein um das Spiel zu Starten.");
            Console.WriteLine("Falls du das spiel beenden willst, gebe 'Exit' ein.");


            LoadUsers();
            LoadDictionary();
        }

        private static void LoadDictionary()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\medin.krupic\Desktop\Dictionary.xml");

            XmlNodeList words = doc.SelectNodes("//Wort");

            foreach (XmlNode xmlnode in words)
            {
                wordsdictionary.Add(xmlnode.InnerText);
            }
        }

        private static void LoadUsers()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Users));
            TextReader reader = new StreamReader(@"C:\Users\medin.krupic\Desktop\UserDB");
            object obj = deserializer.Deserialize(reader);
            Users = (Users)obj;
            reader.Close();
        }

        private static void ExitGame()
        {
            Console.WriteLine("Das Programm wird beendet!!!");
            Console.ReadLine();
        }

        //private static void ErgebnisAuswerten(int zahl)
        //{
        //    if (zahl == GeneratedNumber)
        //    {
        //        Console.WriteLine("Gewonnen");
        //        GameStarted = false;
        //        Gewonnen = true;
        //    }
        //    else if (zahl > GeneratedNumber)
        //    {
        //        Console.WriteLine("Zahl ist zu groß");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Zahl ist zu klein");
        //    }
        //}

        private static void StartGame()
        {
            Console.WriteLine("Hang Man-Spiel wird gestartet.");

            //generiertes wort
            Versuche = 10;
            Fehler = 0;
            GameStarted = true;
            falscheBuchstaben = new List<string>();
            GenerateWord();

            Schwirigkeitsgrad();


            ShowWord();
        }

        private static void ShowWord()
        {
            string word = string.Empty;
            foreach (var s in gesuchtesWortarrayToShow)
            {
                word += " " + s;
            }
            Console.WriteLine(word);
        }

        private static void GenerateWord()
        {
            int wordcount = wordsdictionary.Count;
            int selected = new Random().Next(0, wordcount);
            gesuchtesWort = wordsdictionary[selected];


            //Fehler = 0;
            // NochOffen = currentword.Length;
            gesuchtesWort = gesuchtesWort.ToUpper();
            gesuchtesWortarray = gesuchtesWort.ToCharArray();
            gesuchtesWortarrayToShow = new char[gesuchtesWort.Length];
            for (int i = 0; i < gesuchtesWort.Length; i++)
            {
                gesuchtesWortarrayToShow[i] = '_';
            }
            NochOffen = gesuchtesWort.Length;

        }

        private static void PlayAsGuestUser()
        {
            currentUser = null;
            GuestUser = true;
            //MessageBox.Show("Du Spielst als Gast.");
            ShowUser();
        }

        private static void ShowUser()
        {
            if (GuestUser)
            {
                Console.WriteLine("Du bist als Gast Angemeldet.");
            }
            else if (currentUser != null)
            {
                Console.WriteLine($"Name: {currentUser.Name}, Punkte: {currentUser.Score.Points}, Liga: {currentUser.Score.Liga}");
            }
            else
            {
                Console.WriteLine("Fehler bei der Anmeldung.");
            }
        }



        private static void AbmeldenUser()
        {
            Console.WriteLine("Du kannst dich auch Abmelden, indem du 'Abmelden' eingibst.");

            Console.WriteLine("Du bis Abgemeldet.");

            //currentUser = null;
            //UpdateUI();
        }



        private static void Schwirigkeitsgrad()
        {


            Console.WriteLine("Gebe 'Leicht' ein um im leichten modus zu spielen.");

            Console.WriteLine("Dir werden 2 Buchstaben vorgezeigt als hilfe.");

            Console.WriteLine("Gebe 'Mittel' ein um im mittlerem modus zu spielen.");

            Console.WriteLine("Dir wird 1 Buchstabe vorgezeigt als hilfe.");

            Console.WriteLine("Gebe 'Schwer' ein um im schweren modus zu spielen.");

            Console.WriteLine("Dir werden 0 Buchstaben vorgezeigt als hilfe.");

            string schwierigkeitsgrad = Console.ReadLine().ToUpper();





            //-------------------------------------------------------------------


            Console.WriteLine("Wähle einen Schwirigkeitsgrad.");

            switch (schwierigkeitsgrad)
            {
                case "LEICHT":
                    Console.WriteLine("Leicht");
                    {
                        int lenght = gesuchtesWort.Length;

                        //1
                        int random = new Random().Next(0, lenght);


                        string gesuchterBuchstabe = gesuchtesWortarray[random].ToString();
                        FillCharacter(Char.Parse(gesuchterBuchstabe));

                        int random2;
                        while ((random2 = new Random().Next(0, lenght)) == random || gesuchtesWortarray[random].ToString() == gesuchtesWortarray[random2].ToString())
                        {

                        }
                        //2

                        random = random2;

                        gesuchterBuchstabe = gesuchtesWortarray[random].ToString();
                        FillCharacter(Char.Parse(gesuchterBuchstabe));

                        if (NochOffen == 0)
                        {
                            Gewonnen();
                            ShowWord();
                            return;
                        }
                        break;
                    }

                case "MITTEL":
                    {
                        Console.WriteLine("Mittel");
                        int lenght = gesuchtesWort.Length;
                        int random = new Random().Next(0, lenght);
                        string gesuchterBuchstabe = gesuchtesWortarray[random].ToString();
                        FillCharacter(Char.Parse(gesuchterBuchstabe));


                        break;
                    }

                case "SCHWER":
                    Console.WriteLine("Schwer");
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
//-------------------------------------------------------------------------
    }

        public  static void FillCharacter(char character)
        {
            for (int i = 0; i < gesuchtesWortarray.Length; i++)
            {
                if (gesuchtesWortarray[i] == character)
                {
                    if (gesuchtesWortarrayToShow[i] == '_')
                    {
                        gesuchtesWortarrayToShow[i] = character;
                        NochOffen--;
                    }
                }
            }
        }


        private static void Gewonnen()
        {
            for (int i = 0; i < gesuchtesWortarray.Length; i++)
            {
                gesuchtesWortarrayToShow[i] = gesuchtesWortarray[i];
            }


            Console.WriteLine("Du hast Gewonnen");

            if (currentUser != null)
            {
                currentUser.Score.Matches++;
                currentUser.Score.Wins++;
                currentUser.Score.Points += 10;
                Save();
            }
            GameStarted = false;
        
            Console.WriteLine(HANGMANPICSWonn);
        }

        private static void Verloren()
        {

            Console.WriteLine("Du hast Verloren");


            for(int i = 0; i<gesuchtesWortarray.Length; i++)
            {
                gesuchtesWortarrayToShow[i] = gesuchtesWortarray[i];
            }

            if (currentUser != null)
            {
                currentUser.Score.Matches++;
                currentUser.Score.Losts++;
                currentUser.Score.Points -= 15;
                Save();
            }
            GameStarted = false;
        }


    private static void Generator()
        {
            Console.WriteLine("Gebe einen Buchstaben ein deiner Wahl, um einen buchstaben vom generiertem Wort zu erraten.");

     
        int verbleibendeFehler = 0;
            Console.WriteLine("Du hast noch " + verbleibendeFehler + " Versuche.");
            //HangMan wird gezeichnet

            //Richtig
            Console.WriteLine("Du hast noch " + verbleibendeFehler + " Versuche.");
            //HangMan wird gezeichnet





            Console.WriteLine("Gebe das ganze Wort ein welches dem generiertem Wort entspricht, falls du es weist. Bedenke das diese Aktion zwischen einem Verlieren und einem Gewinnenen entscheidet.");
            //Fehler
            //Verloren
            //HangMan wird gezeichnet mit X-Augen

            //Richtig
            //Gewonnen
            //HangMan wird gezeichnet
        }

        private static void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Users));
            using (TextWriter writer = new StreamWriter(@"C:\Users\medin.krupic\Desktop\UserDB"))
            {
                serializer.Serialize(writer, Users);
            }

            //UpdateUI();
        }

        private void Liga()
        {

            if (currentUser.Score.Points >= 0 && currentUser.Score.Points <= 99)
            {
                Console.WriteLine(100 - currentUser.Score.Points + " bis Liga Bronze");
                Console.WriteLine("keine Liga");
            }

            else if (currentUser.Score.Points >= 100 && currentUser.Score.Points <= 249)
            {
                Console.WriteLine(250 - currentUser.Score.Points + " bis Liga Silber");
                Console.WriteLine("Bronze");
            }
            else if (currentUser.Score.Points >= 250 && currentUser.Score.Points <= 499)
            {
                Console.WriteLine(500 - currentUser.Score.Points + " bis Liga Gold");
                Console.WriteLine("Silber");
            }
            else if (currentUser.Score.Points >= 500 && currentUser.Score.Points <= 749)
            {
                Console.WriteLine(750 - currentUser.Score.Points + " bis Liga Platin");
                Console.WriteLine("Gold");
            }
            else if (currentUser.Score.Points >= 750 && currentUser.Score.Points <= 999)
            {
                Console.WriteLine(1000 - currentUser.Score.Points + " bis Liga Diamant");
                Console.WriteLine("Platin");
            }
            else if (currentUser.Score.Points >= 1000)
            {
                Console.WriteLine("Du hast die höhste Liga erreicht.");
                Console.WriteLine("Diamant");
            }
        }

    }
}