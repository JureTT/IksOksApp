using System;

namespace IksOks.GameLogic
{
    class Igrac
    {
        public string Ime;
        public Znak BiraniZnak;

        public virtual void PovuciPotez(Igra partija) { }
    }

    class SlabiKomp : Igrac
    {
        public override void PovuciPotez(Igra partija)
        {
            Random broj = new Random();

            void SlucajnoPolje()
            {
                int proizvoljno1 = broj.Next(0, 3);
                int proizvoljno2 = broj.Next(0, 3);
                string kordinate = Znak.Konverzija(proizvoljno1, proizvoljno2);
                BiraniZnak.Upisi(kordinate);
            }

            SlucajnoPolje();
            bool provjeraPoteza = partija.UnosZnaka(BiraniZnak);
            while (provjeraPoteza == false)
            {
                SlucajnoPolje();
                provjeraPoteza = partija.UnosZnaka(BiraniZnak);
            }
        }
    }

    class NormalniKomp : SlabiKomp
    {
        public override void PovuciPotez(Igra partija)
        {
            string provjera = partija.ProvjeraTablice();
            int i = provjera.Length;
            if (i > 0)
            {
                int aps, ord;
                int kuka = provjera.IndexOf(BiraniZnak.Vrsta);
                if (kuka > 0)
                {
                    ord = int.Parse(provjera[kuka - 2].ToString());
                    aps = int.Parse(provjera[kuka - 1].ToString());
                }
                else
                {
                    ord = int.Parse(provjera[0].ToString());
                    aps = int.Parse(provjera[1].ToString());
                }
                string kordinate = Znak.Konverzija(aps, ord);
                BiraniZnak.Upisi(kordinate);
                partija.UnosZnaka(BiraniZnak);
            }
            else
            {
                base.PovuciPotez(partija);
            }
        }
    }

    class JakiKomp : NormalniKomp
    {
        public override void PovuciPotez(Igra partija)
        {
            string provjera = partija.ProvjeraTablice();
            if (provjera.Length > 0) { }
            else
            {
                base.PovuciPotez(partija);
            }
        }
    }
}