using System;
using System.Collections.Generic;
using System.Text;

namespace IksOks.GameLogic
{
    class Znak
    {
        public string Polje;
        public string Vrsta;
        public static string ProvjeraKota(string kote)
        {
            if (string.IsNullOrWhiteSpace(kote) || kote.Length < 2)
            {
                kote = "XO";
            }
            string apscisa = kote[0].ToString();
            string ordinata = kote[1].ToString();

            if (provjerica(apscisa) == false || provjerica(ordinata) == false)
            {
                kote += "x/o";
            }
            return kote;

            bool provjerica(string slovo)     //metoda prima char varijablu i provjerava je dali je broj ili ne
            {
                bool istina = int.TryParse(slovo, out int broj);
                if (istina == true)
                {
                    istina = (broj < 1 || broj > 3) ? false : true;  //ovdje je jednolinijska 'if' izjava, ako je broj manji od 1 ili veći od 3 onda tvrdnja nije istinita, tj. broj nije zadovovljio kriterije
                }
                else
                {
                    istina = (slovo == "A" || slovo == "B" || slovo == "C") ? true : false;

                }

                return istina;
            }
        }
        virtual public void Upisi(string kote)     //vjerovatno je bolje da je metoda "abstract"
        {
            Console.WriteLine("Upisano polje");     //kod overrideanja ove metode nije bio upisan string ordinate, što je bila greška, što znači da metoda koja se overrajda mora primati iste parametre kao i ona koja ju overrajda
        }
        public static string Konverzija(int kota1, int kota2)
        {
            string[] nizApscise = new string[3] { "A", "B", "C" };
            string apscisa = nizApscise[kota1];
            string ordinata = (kota2 + 1).ToString();
            string kordinate = apscisa + ordinata;
            return kordinate;
        }
    }
    class Iks : Znak
    {
        public override void Upisi(string kote)
        {
            //Console.WriteLine("Znak X je upisan u navedeno polje.");
            Polje = kote;
        }
        public Iks()
        {
            Vrsta = "X";
        }
    }
    class Oks : Znak
    {
        public override void Upisi(string kote)
        {
            //Console.WriteLine("Znak O je upisan u navedeno polje.");
            Polje = kote;
        }
        public Oks()
        {
            Vrsta = "O";
        }
    }
}