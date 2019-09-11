using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace SchnellmeldungSeptember
{
    public class Klasses : List<Klasse>
    {
        public string AktSjAtlantis { get; private set; }

        public Klasses(string ConnectionStringAtlantis)
        {
            AktSjAtlantis = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Year + 1 - 2000);

            using (OdbcConnection connection = new OdbcConnection(ConnectionStringAtlantis))
            {
                DataSet dataSet = new DataSet();
                OdbcDataAdapter schuelerAdapter = new OdbcDataAdapter(@"SELECT DBA.klasse.kl_id,
DBA.klasse.klasse,
DBA.klasse.schul_jahr,
DBA.klasse.jahrgang,
DBA.klasse.s_klasse_art AS Klassenart,
DBA.klasse.s_uorg,
DBA.klasse.s_gliederungsplan_kl,
DBA.klasse.s_bildungsgang
FROM DBA.klasse
WHERE schul_jahr = '" + AktSjAtlantis + "' ORDER BY DBA.klasse.klasse ASC", connection);

                connection.Open();
                schuelerAdapter.Fill(dataSet, "DBA.klasse");

                foreach (DataRow theRow in dataSet.Tables["DBA.klasse"].Rows)
                {

                    var klasseAtlantis = new Klasse();

                    if (klasseAtlantis != null)
                    {
                        klasseAtlantis.IdAtlantis = theRow["kl_id"] == null ? -99 : Convert.ToInt32(theRow["kl_id"]);
                        klasseAtlantis.NameAtlantis = theRow["klasse"] == null ? "" : theRow["klasse"].ToString();
                        klasseAtlantis.Gliederung = theRow["s_uorg"] == null ? "" : theRow["s_uorg"].ToString();
                        klasseAtlantis.OrgForm = theRow["Klassenart"] == null ? "" : theRow["Klassenart"].ToString();
                        if (klasseAtlantis.Gliederung != "")
                        {
                            string jahrgang = theRow["jahrgang"].ToString();

                            klasseAtlantis.Jahrgang = theRow["jahrgang"] == null || klasseAtlantis.Gliederung == "" ? "" : "0" + theRow["jahrgang"].ToString().Replace(klasseAtlantis.Gliederung, "");
                            if (klasseAtlantis.Jahrgang == "")

                                klasseAtlantis.Anlage = theRow["s_uorg"] == null ? "" : theRow["s_uorg"].ToString().Substring(0, Math.Min(1, klasseAtlantis.Jahrgang.Length));
                            if (klasseAtlantis.Anlage == "")

                                klasseAtlantis.Gliederungsplan = theRow["s_gliederungsplan_kl"] == null ? "" : theRow["s_gliederungsplan_kl"].ToString();
                            if (klasseAtlantis.Gliederungsplan != "" && klasseAtlantis.NameAtlantis != "Z")
                            {
                                string fk = theRow["s_gliederungsplan_kl"].ToString().Replace(theRow["s_uorg"].ToString(), "");
                                fk = fk.Length > 5 ? fk.Substring(0, 5) : fk;
                                klasseAtlantis.Fachklassenschlüssel = theRow["s_gliederungsplan_kl"] == null ? "" : fk;
                                                                
                                this.Add(klasseAtlantis);

                                connection.Close();
                            }
                        }
                    }
                }
                Console.WriteLine("Klassen " + ".".PadRight(30, '.') + " " + (this.Count - 1));
            }
        }
    }
}