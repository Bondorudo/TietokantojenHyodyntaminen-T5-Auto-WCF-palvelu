using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;

namespace T5_Auto_WCF_palvelu
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        #region Yhteys Tietokantaan
        // Testaa Yhteys tietokantaan.
        [OperationContract]
        bool TestDatabaseConnection(bool result);

        // Disconnect from database
        [OperationContract]
        void Disconnect();
        #endregion

        #region Tallenna Auto Tietokantaan
        [OperationContract]
        bool SaveAuto(Auto newAuto);
        #endregion

        #region Hakee Auton Tiedot
        // Hakee listan auton merkeistä.
        [OperationContract]
        List<Auto> GetAllAutoMakers();

        // Hakee listan auton malleista auton merkki id:n perusteella.
        [OperationContract]
        List<Auto> GetAutoModels(int makerId);

        // Hakee listan auton polttoaineista.
        [OperationContract]
        List<Auto> Polttoaine();

        // Hakee listan auton väreistä.
        [OperationContract]
        List<Auto> Vari();
        #endregion

        #region Hakee Autot tietokannasta Id:n mukaan
        // Edellinen rivi tietokannasta.
        [OperationContract]
        void Edellinen();

        // Seuraava rivi tietokannasta.
        [OperationContract]
        void Seuraava();

        // Selaus objekti jolla pitäisi olla muutuujien arvot.
        [OperationContract]
        void Selaus(Auto newSelaus);
        #endregion
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Auto
    {
        int id;
        int merkkiId;
        string merkkiNimi;
        string malliNimi;
        string polttoaineen_nimi;
        string varin_nimi;
        DateTime rekisteriPVM;
        string hinta;
        string moottoriTilavuus;
        string mittariLukema;
        string malliId;
        string polttoaineId;
        string variId;

        [DataMember]
        public int Id { get => id; set => id = value; }
        [DataMember]
        public int MerkkiId { get => merkkiId; set => merkkiId = value; }
        [DataMember]
        public string MerkkiNimi { get => merkkiNimi; set => merkkiNimi = value; }
        [DataMember]
        public string MalliNimi { get => malliNimi; set => malliNimi = value; }
        [DataMember]
        public string Polttoaineen_nimi { get => polttoaineen_nimi; set => polttoaineen_nimi = value; }
        [DataMember]
        public string Varin_nimi { get => varin_nimi; set => varin_nimi = value; }
        [DataMember]
        public DateTime RekisteriPVM { get => rekisteriPVM; set => rekisteriPVM = value; }
        [DataMember]
        public string Hinta { get => hinta; set => hinta = value; }
        [DataMember]
        public string MoottoriTilavuus { get => moottoriTilavuus; set => moottoriTilavuus = value; }
        [DataMember]
        public string MittariLukema { get => mittariLukema; set => mittariLukema = value; }
        [DataMember]
        public string MalliId { get => malliId; set => malliId = value; }
        [DataMember]
        public string PolttoaineId { get => polttoaineId; set => polttoaineId = value; }
        [DataMember]
        public string VariId { get => variId; set => variId = value; }
    }
}
