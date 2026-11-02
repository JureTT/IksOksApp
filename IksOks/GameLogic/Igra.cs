using System;
using System.Collections.Generic;
using System.Text;

namespace IksOks.GameLogic
{
    class Igra
    {
        private string[,] Mreza;      //ovdje smo mogli napraviti i niz objekaza klase Znak, ali nismo htjeli komplicirati i zbunjivati
        private string Apscisa;
        private int Ordinata;
        public void PokreniPartiju()
        {
            Mreza = new string[3, 3];      // igra iksoks sa više redova i stupaca je besmislena jer je jako teško povezati 4 u nizu (a 3 u nizu na tablicu 4x4 je prelagano)
        }
        public bool UnosZnaka(Znak simbol)
        {
            Apscisa = simbol.Polje[0].ToString();
            Ordinata = int.Parse(simbol.Polje[1].ToString());  //char se koverta u string pa se onda parsira u int
            int x = Konverzijica(Apscisa);
            int y = Ordinata - 1;
            bool provjera = string.IsNullOrEmpty(Mreza[y, x]);       //može i naredba '.IsNullOrWhiteSpace()'

            if (provjera == true)
            {
                if (simbol.Vrsta == "X")
                {
                    Mreza[y, x] = "X";
                }
                else if (simbol.Vrsta == "O")
                {
                    Mreza[y, x] = "O";
                }
                else     // dodano radi provjere
                {
                    Console.WriteLine("Nešto si zeznuo, objekt je klase Znak najvjerojatnije.");
                }
            }
            //else { Console.WriteLine("NEŠTO NEVALJA"); }

            return provjera;
        }
        private int Konverzijica(string apsa)
        {
            string[] nizApscise = new string[3] { "A", "B", "C" };
            int index = Array.IndexOf(nizApscise, apsa);
            return index;
        }
        public void PrikazTablice()
        {
            string tablica = "X/O  A   B   C\n";
            for (int i = 0; i < 3; i++)
            {
                tablica = tablica + " " + (i + 1).ToString();
                for (int j = 0; j < 3; j++)
                {
                    if (Mreza[i, j] == null)
                    {
                        tablica = tablica + "   _";
                    }
                    else
                    {
                        tablica = tablica + "   " + Mreza[i, j];
                    }
                }
                tablica = tablica + "\n";
            }
            Console.WriteLine(tablica);
        }
        public string ProvjeraTablice()
        {
            string linija1 = "";
            string linija2 = "";
            string slovo = "";
            string polje = "";
            for (int k = 0; k < 3; k++)
            {
                for (int l = 0; l < 3; l++)
                {
                    linija1 += (Mreza[k, l] == null) ? ("#" + k.ToString() + l.ToString()) : Mreza[k, l];
                    //linija1 += Mreza[k, l];
                }
                slovo = (slovo.Length == 1) ? slovo : ProvjeraPobijede(linija1.Trim());     //provjera uvjeta ako su 3 u nizu
                polje += ProvjeraLinije(linija1.Trim());       //provjera linije dali su 2 ista znaka u nizu
                linija1 = "";
            }

            for (int i = 0; i < 3; i++)
            {
                linija1 += (Mreza[i, i] == null) ? ("#" + i.ToString() + i.ToString()) : Mreza[i, i];
                linija2 += (Mreza[i, (2 - i)] == null) ? ("#" + i.ToString() + (2 - i).ToString()) : Mreza[i, (2 - i)];
                //linija1 += Mreza[i, i];
                //linija2 += Mreza[i, (2 - i)];
            }
            slovo = (slovo.Length == 1) ? slovo : ProvjeraPobijede(linija1.Trim());
            polje += ProvjeraLinije(linija1.Trim());       //provjera linije dali su 2 ista znaka u nizu
            linija1 = "";
            slovo = (slovo.Length == 1) ? slovo : ProvjeraPobijede(linija2.Trim());
            polje += ProvjeraLinije(linija2.Trim());       //provjera linije dali su 2 ista znaka u nizu
            linija2 = "";

            for (int l = 0; l < 3; l++)
            {
                for (int k = 0; k < 3; k++)
                {
                    linija1 += (Mreza[k, l] == null) ? ("#" + k.ToString() + l.ToString()) : Mreza[k, l];
                    //linija1 += Mreza[k, l];
                }
                slovo = (slovo.Length == 1) ? slovo : ProvjeraPobijede(linija1.Trim());
                polje += ProvjeraLinije(linija1.Trim());       //provjera linije dali su 2 ista znaka u nizu
                linija1 = "";
            }
            if (polje != "" && slovo == "")
            {
                return polje;
            }
            else
            {
                return slovo;
            }
        }
        public string ProvjeraPobijede(string linija)
        {
            string slovo = "";      //zato što se metoda pozove svaki put prazno je "slovo" jer ga vraća svaki put kao prazan string (isprazniš string 'slovo' svaki put)
            if (linija == "XXX" || linija == "OOO")
            {
                //Console.WriteLine("Igra je završila, imamo pobijednika!");
                slovo = linija[0].ToString();
            }
            return slovo;
        }
        public string ProvjeraLinije(string linija)
        {
            string polje = "";
            if (linija.Trim().Length == 5)
            {
                int udica = linija.IndexOf("#");
                string ostatak = linija.Remove(udica, 3);
                if (ostatak == "XX" || ostatak == "OO")
                {
                    polje = linija[udica + 1].ToString() + linija[udica + 2].ToString() + ostatak[1].ToString();
                }
            }
            return polje;
        }
        public string DohvatiPolje(int red, int stupac)
        {
            return Mreza[red, stupac];
        }
    }
}