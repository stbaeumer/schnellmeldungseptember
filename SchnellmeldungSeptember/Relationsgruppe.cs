using System;
using System.Collections.Generic;

namespace SchnellmeldungSeptember
{
    public class Relationsgruppe
    {
        public string BeschreibungSchulministerium { get; private set; }

        public List<string> Jahrgänge { get; private set; }
        public List<string> Fachklassenschlüssel { get; private set; }
        public List<string> Gliederungen { get; private set; }
        public double Relation { get; private set; }

        public Relationsgruppe(string BeschreibungSchulministerium, List<string> Gliederungen, List<string> Jahrgänge, List<int> Fachklassenschlüssel, double relation)
        {
            this.Relation = relation;
            this.Fachklassenschlüssel = new List<string>();
            this.BeschreibungSchulministerium = BeschreibungSchulministerium;
            this.Gliederungen = Gliederungen;
            this.Jahrgänge = Jahrgänge;
            try
            {
                if (Fachklassenschlüssel != null)
                {
                    foreach (var item in Fachklassenschlüssel)
                    {
                        this.Fachklassenschlüssel.Add(item.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}