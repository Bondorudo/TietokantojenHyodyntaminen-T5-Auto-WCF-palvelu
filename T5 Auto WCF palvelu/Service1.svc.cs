using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace T5_Auto_WCF_palvelu
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        string databaseYhteys;
        public string DatabaseYhteys { get => databaseYhteys; set => databaseYhteys = value; }

        SqlConnection dbYhteys;
        
        #region connect / disconnect
        public void DatabaseHallinta()
        {
            DatabaseYhteys = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        // Connect to database.
        public bool connectDatabase()
        {
            dbYhteys = new SqlConnection();
            dbYhteys.ConnectionString = DatabaseYhteys;

            try
            {
                dbYhteys.Open();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Virheilmoitukset:" + e);
                dbYhteys.Close();
                return false;
            }
        }

        // Disconnect from database.
        public void disconnectDatabase()
        {
            dbYhteys.Close();
        }
        #endregion

        #region Tallennus
        // Tallentaa tietokantaan
        public bool saveAutoIntoDatabase(Auto auto)
        {
            string comString = "INSERT INTO auto (Hinta, Rekisteri_paivamaara, Moottorin_tilavuus, Mittarilukema, AutonMerkkiID, AutonMalliID, VaritID, PolttoaineID) VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8)";
            bool palaute = false;

            using (SqlCommand com = new SqlCommand(comString, dbYhteys))
            {
                com.Parameters.AddWithValue("@val1", decimal.Parse(auto.Hinta));
                com.Parameters.AddWithValue("@val2", (DateTime)auto.RekisteriPVM);
                com.Parameters.AddWithValue("@val3", decimal.Parse(auto.MoottoriTilavuus));
                com.Parameters.AddWithValue("@val4", int.Parse(auto.MittariLukema));
                com.Parameters.AddWithValue("@val5", auto.MerkkiId);
                com.Parameters.AddWithValue("@val6", int.Parse(auto.MalliId));
                com.Parameters.AddWithValue("@val7", int.Parse(auto.VariId));
                com.Parameters.AddWithValue("@val8", int.Parse(auto.PolttoaineId));

                try
                {
                    Console.WriteLine("Hinta: " + auto.Hinta.ToString() + " | RekisteriPVM:  " + auto.RekisteriPVM.ToString() + " | Moottorin Tilavuus: " + auto.MoottoriTilavuus.ToString() + " | Mittarilukema: " + auto.MittariLukema + " | Merkki: " + auto.MerkkiId + " | Malli: " + auto.MalliId + " | Väri: " + auto.VariId + " | Polttoaine: " + auto.PolttoaineId);
                    com.ExecuteNonQuery();
                    palaute = true;

                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    palaute = false;
                }
            }
            return palaute;
        }
        #endregion

        #region AutonMerkki
        // Hakee auton merkit tietokannasta.
        public List<Auto> GetAllAutoMakersFromDatabase()
        {
            List<Auto> aMerkit = new List<Auto>();
            SqlDataReader myReader;
            SqlCommand com = new SqlCommand("SELECT * FROM AutonMerkki", dbYhteys);
            try
            {
                using (myReader = com.ExecuteReader())
                {
                    while (myReader.Read())
                    {
                        Auto auto = new Auto();
                        auto.Id = Convert.ToInt32(myReader["ID"]);
                        auto.MerkkiNimi = myReader["Merkki"].ToString();

                        aMerkit.Add(auto);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return aMerkit;
        }
        #endregion

        #region AutonMalli
        // Hakee auton mallit tietokannasta.
        public List<Auto> GetAutoModelsByMakerId(int makerId)
        {
            // Luo uusi lista johon lisätään kaikki auton mallit joiden merkkiId on sama kuin auton merkin id.
            List<Auto> aMalli = new List<Auto>();

            SqlDataReader myReader;
            SqlCommand com = new SqlCommand("SELECT * FROM AutonMallit", dbYhteys);
            try
            {
                using (myReader = com.ExecuteReader())
                {
                    while (myReader.Read())
                    {
                        Auto autonMalli = new Auto();
                        autonMalli.Id = Convert.ToInt32(myReader["ID"]);
                        autonMalli.MalliNimi = myReader["Auton_mallin_nimi"].ToString();
                        autonMalli.MerkkiId = Convert.ToInt32(myReader["AutonMerkkiID"]);

                        aMalli.Add(autonMalli);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            List<Auto> uusiAutonMalliLista = new List<Auto>();

            var autoMallit = from aM in aMalli
                             where aM.MerkkiId == makerId
                             select aM;

            foreach (var autojenMallit in autoMallit)
            {
                uusiAutonMalliLista.Add(autojenMallit);
            }
            return uusiAutonMalliLista;
        }
        #endregion

        #region AutonPolttoaine
        public List<Auto> Polttoaineet()
        {
            List<Auto> polttoaine = new List<Auto>();

            SqlDataReader myReader;
            SqlCommand com = new SqlCommand("SELECT * FROM Polttoaine", dbYhteys);

            try
            {
                using (myReader = com.ExecuteReader())
                {
                    while (myReader.Read())
                    {
                        Auto pAine = new Auto();
                        pAine.Id = Convert.ToInt32(myReader["ID"]);
                        pAine.Polttoaineen_nimi = myReader["Polttoaineen_nimi"].ToString();

                        polttoaine.Add(pAine);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return polttoaine;
        }
        #endregion

        #region autonVari
        public List<Auto> autonVarit()
        {
            List<Auto> variList = new List<Auto>();

            SqlDataReader myReader;
            SqlCommand com = new SqlCommand("SELECT * FROM Varit", dbYhteys);

            try
            {
                using (myReader = com.ExecuteReader())
                {
                    while (myReader.Read())
                    {
                        Auto autonVari = new Auto();
                        autonVari.Id = Convert.ToInt32(myReader["ID"]);
                        autonVari.Varin_nimi = myReader["Varin_nimi"].ToString();

                        variList.Add(autonVari);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return variList;
        }
        #endregion

        #region Selaus
        public int top = 1;

        public void Selaaminen(Auto uusiSelaus)
        {
            string comString = "SELECT TOP " + top + " * FROM auto ORDER BY ID ASC";

            using (SqlCommand com = new SqlCommand(comString, dbYhteys))
            {
                SqlDataReader dReader = com.ExecuteReader();

                while (dReader.Read())
                {
                    uusiSelaus.Id = (int)dReader["ID"];
                    uusiSelaus.Hinta = dReader["Hinta"].ToString();
                    uusiSelaus.RekisteriPVM = (DateTime)dReader["Rekisteri_paivamaara"];
                    uusiSelaus.MoottoriTilavuus = dReader["Moottorin_tilavuus"].ToString();
                    uusiSelaus.MittariLukema = dReader["Mittarilukema"].ToString();
                    uusiSelaus.MerkkiId = (int)dReader["AutonMerkkiID"];
                    uusiSelaus.MalliId = dReader["AutonMalliID"].ToString();
                    uusiSelaus.VariId = dReader["VaritID"].ToString();
                    uusiSelaus.PolttoaineId = dReader["PolttoaineID"].ToString();
                }
                dReader.Close();
            }
        }

        public bool TestDatabaseConnection(bool result)
        {
            return connectDatabase();
        }

        public void Disconnect()
        {
            disconnectDatabase();
        }

        public bool SaveAuto(Auto newAuto)
        {
            return saveAutoIntoDatabase(newAuto);
        }

        public List<Auto> GetAllAutoMakers()
        {
            return GetAllAutoMakersFromDatabase();
        }

        public List<Auto> GetAutoModels(int makerId)
        {
            return GetAutoModelsByMakerId(makerId);
        }

        public List<Auto> Polttoaine()
        {
            return Polttoaineet();
        }

        public List<Auto> Vari()
        {
            return autonVarit();
        }

        public void Edellinen()
        {
            top--;
            Console.WriteLine("Top: " + top);
        }

        public void Seuraava()
        {
            top++;
            Console.WriteLine("Top: " + top);
        }

        public void Selaus(Auto newSelaus)
        {
            Selaaminen(newSelaus);
        }
        #endregion
    }
}
