using System;
using System.Collections.Generic;
using System.Linq;

namespace SchnellmeldungSeptember
{
    public class Relationsgruppen : List<Relationsgruppe>
    {
        public Relationsgruppen()
        {
            Console.Write("Relationsgruppe ..");
            // Dokumentation siehe schips.schule.nrw.de/

            this.Add(new Relationsgruppe("BK BS Fachklasse EQ TZ", new List<string>() { "A01" }, new List<string>() { "01", "02", "03" }, Enumerable.Range(10000, 79999 - 10000).ToList()));
            this.Add(new Relationsgruppe("BK BS Fachklasse EQ TZ (hj. endend)", new List<string>() { "A01" }, new List<string>() { "04" }, null));
            this.Add(new Relationsgruppe("BK BS Ausbildungsvorbereitung VZ", new List<string>() { "A12" }, new List<string>() { "01" }, null));
            this.Add(new Relationsgruppe("BK BS Ausbildungsvorbereitung TZ", new List<string>() { "A13" }, new List<string>() { "01" }, null));
            this.Add(new Relationsgruppe("BK BF ber. Kennt. (Vor. HSA) 1-jährig", new List<string>() { "B06" }, new List<string>() { "01" }, null));
            this.Add(new Relationsgruppe("BK BF ber. Kennt. (Vor. HSA 10) 1-jährig", new List<string>() { "B02", "B07" }, new List<string>() { "01" }, null));
            this.Add(new Relationsgruppe("BK BF ber. Kennt. und FHS 2-jährig", new List<string>() { "C03", "C13" }, new List<string>() { "01", "02" }, null));
            this.Add(new Relationsgruppe("BK BF BA-LRecht und FOS 2-jährig", new List<string>() { "B01", "B04", "B08" }, new List<string>() { "01", "02" }, null));
            this.Add(new Relationsgruppe("BK BF ber. Kennt. und AHR 3-jährig", new List<string>() { "D02" }, new List<string>() { "01", "02", "03" }, null));
            this.Add(new Relationsgruppe("BK FO 12B 1-jährig VZ", new List<string>() { "C08" }, new List<string>() { "01" }, null));
            this.Add(new Relationsgruppe("BK FO Klasse 11 1-jährig TZ", new List<string>() { "C05" }, new List<string>() { "01" }, null));
            this.Add(new Relationsgruppe("BK FO Klasse 12 1-jährig VZ", new List<string>() { "C06" }, new List<string>() { "01" }, null));
            Console.WriteLine(".".PadRight(30, '.') + " " + (this.Count - 1));
        }        
    }
}