using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchnellmeldungSeptember
{
    public class Schueler
    {
        public int IdAtlantis { get; internal set; }
        public string Vorname { get; internal set; }
        public string Klasse { get; internal set; }
        public string Nachname { get; internal set; }
        public string Status { get; internal set; }
        public string Gliederung { get; internal set; }
        public string Fachklasse { get; internal set; }
        public string OrgForm { get; internal set; }
        public string Jahrgang { get; internal set; }
        public int Bezugsjahr { get; internal set; }
    }
}
