﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchnellmeldungSeptember
{
    class Program
    {
        public const string ConnectionStringAtlantis = @"Dsn=Atlantis17u;uid=DBA";

        static void Main(string[] args)
        {
            Console.WriteLine("Schnellmeldung September");
            Console.WriteLine("========================");
            Console.WriteLine("");

            Schuelers schuelers = new Schuelers(ConnectionStringAtlantis);

            Klasses klasses = new Klasses(ConnectionStringAtlantis);

            Relationsgruppen relationsgruppen = new Relationsgruppen();

            foreach (var klasse in klasses)
            {
                if (klasse.NameAtlantis == "HBT19A")
                {
                    string a = "";
                }
                foreach (var r in relationsgruppen)
                {
                    if (r.Gliederungen.Contains(klasse.Gliederung))
                    {
                        if ((r.Fachklassenschlüssel.Count == 0 || r.Fachklassenschlüssel.Contains(klasse.Fachklassenschlüssel)))
                        {
                            if (r.Jahrgänge.Contains(klasse.Jahrgang) && klasse.NameAtlantis != "Z" && klasse.NameAtlantis != "Abgang")
                            {
                                klasse.Relationsgruppe = r.BeschreibungSchulministerium;
                            }
                        }
                    }
                }
            }

            foreach (var klasse in (from k in klasses where k.Relationsgruppe == null select k).ToList())
            {
                Console.Write("Die Klasse " + klasse.NameAtlantis + " kann keiner Relationsgruppe zugeordnet werden.");
                if ((from s in schuelers where s.Klasse == klasse.NameAtlantis select s).Any())
                {
                    Console.Write("Die Klasse hat aber Schüler. Das muss unbedingt gefixt werden!");
                    Console.ReadKey();
                }
                Console.WriteLine();
            }
            List<Schueler> schuelerSchnellmeldung = new List<Schueler>();
            Console.WriteLine("");
            List<string> datei = new List<string>
                {
                    "^  Relationsgruppe  ^  ".PadRight(43) + "1.Jg  ^".PadRight(6) + "2.Jg  ^".PadRight(6) + "3.Jg  ^".PadRight(6) + "4.Jg  ^".PadRight(5) + "Summe  ^  Relation  ^  StellenBS  ^  StellenVZ  ^"
                };
            Console.WriteLine("Relationsgruppe".PadRight(43) + "1.Jg".PadRight(6) + "2.Jg".PadRight(6) + "3.Jg".PadRight(6) + "4.Jg".PadRight(6) + "Summe Relation StellenBS StellenVZ");

            int summe = 0;
            
            double stellenBs = 0;
            double stellenVz = 0;

            foreach (var relationsgruppe in relationsgruppen)
            {
                string zeile = "|" + (relationsgruppe.BeschreibungSchulministerium + ":").PadRight(41) + "  |  ";

                var kl = (from k in klasses where k.Relationsgruppe == relationsgruppe.BeschreibungSchulministerium select k).ToList();

                foreach (var jg in new List<string>() { "01", "02", "03", "04" })
                {
                    string z = "";
                    if (relationsgruppe.Jahrgänge.Contains(jg))
                    {
                        int x = (from s in schuelers
                                 where (from k in kl
                                        where k.NameAtlantis == s.Klasse
                                        where s.Jahrgang == jg
                                        select k).Any()
                                 select s).Count();

                        schuelerSchnellmeldung.AddRange((from s in schuelers
                                                         where (from k in kl where k.NameAtlantis == s.Klasse where s.Jahrgang == jg select k).Any()
                                                         select s));
                        summe += x;
                        z = x.ToString();
                    }
                    zeile = zeile + z.PadLeft(6) + "|";
                }

                int t = (from s in schuelers
                         where (from k in kl where k.NameAtlantis == s.Klasse select k).Any()
                         select s).Count();

                zeile = zeile + t.ToString().PadLeft(6) + "|";

                // Relation:

                zeile = zeile + relationsgruppe.Relation.ToString().PadLeft(9) + "|";

                // Stellen:
                
                if (relationsgruppe.BeschreibungSchulministerium.StartsWith("BK BS"))
                {
                    zeile = zeile + (t / relationsgruppe.Relation).ToString("0.0000").PadLeft(10) + "|             |";
                    stellenBs = stellenBs + (t / relationsgruppe.Relation);
                }
                else
                {
                    zeile = zeile + "          |" + (t / relationsgruppe.Relation).ToString("0.0000").PadLeft(10) + "|";
                    stellenVz = stellenVz + (t / relationsgruppe.Relation);
                }

                datei.Add(zeile);
                Console.WriteLine(zeile);
            }

            //datei.Add("----------------------------------------------------------------------------------------------------");

            datei.Add("|Summe:".PadRight(67) + summe + "|||||||    " + stellenBs.ToString("0.0000").PadLeft(10) + "|" + stellenVz.ToString("0.0000").PadLeft(10) + "|");
            datei.Add("");
            datei.Add("   Leitungszeit: " + (9 + 50 * 0.7 + ((stellenBs+stellenVz) - 50) * 0.3).ToString("0.00")  + "   (= 9 + 50 * 0,7 + (" + stellenBs.ToString("0.0000") + "+" + stellenVz.ToString("0.0000") + " - 50) * 0,3)  Verordnung zur Ausführung des § 93 Abs. 2 Schulgesetz (VO zu § 93 Abs. 2 SchulG) vom 18.03.2005");
            datei.Add("   Anrechnungen: " + (stellenBs * 0.5 + stellenVz * 1.2).ToString("00.00") + "   (= " + stellenBs.ToString("0.0000") + " * 0,5 + " + stellenVz.ToString("0.0000") + " * 1,2)");
            datei.Add("");
            Console.WriteLine("Summe: " + summe);

            foreach (var item in schuelers)
            {
                if (!(from s in schuelerSchnellmeldung where s.IdAtlantis == item.IdAtlantis select s).Any())
                {
                    datei.Add("Der Schüler " + item.Nachname + ", " + item.Vorname + " " + item.Klasse + " ist nicht in der Schnellmeldung erfasst. Prüfen!");
                }
            }

            datei.Add("   Schüler in Atlantis insgesamt: " + schuelers.Count());
            Console.WriteLine("Schüler in Atlantis insgesamt: " + schuelers.Count());
            datei.Add("");

            datei.Add("Details");
            datei.Add("=======");
            datei.Add("");

            foreach (var relationsgruppe in relationsgruppen)
            {
                int sum = 0;
                datei.Add(relationsgruppe.BeschreibungSchulministerium);
                int i = 1;
                foreach (var klasse in (from k in klasses where k.Relationsgruppe == relationsgruppe.BeschreibungSchulministerium select k).ToList())
                {
                    int d = (from s in schuelers where s.Klasse == klasse.NameAtlantis select s).Count();
                    sum += d;
                    datei.Add(i.ToString().PadLeft(2) + ". " + klasse.NameAtlantis.PadRight(20) + d.ToString().PadLeft(2));
                    i++;
                }
                datei.Add("--------------------------");
                datei.Add("".PadRight(15) + "Summe: " + sum.ToString().PadLeft(4));
                datei.Add("");
            }

            datei.Add(System.Environment.UserName + " | " + DateTime.Now);

            string pfad = DateTime.Now.ToString("yyyyMMdd") + ".txt";

            using (StreamWriter outputFile = new StreamWriter(pfad))
            {
                foreach (string line in datei)
                    outputFile.WriteLine(line);
            }
            EditorOeffnen(pfad);
            
            System.Diagnostics.Process.Start("http://www.schips.schule.nrw.de/");            
        }

        public static void EditorOeffnen(string pfad)
        {
            try
            {
                System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Notepad++\Notepad++.exe", pfad);
            }
            catch (Exception)
            {
                System.Diagnostics.Process.Start("Notepad.exe", pfad);
            }
        }


        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}