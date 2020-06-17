using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace WcfServiceCrude
{
    [ServiceContract]
    public interface ISuperHero
    {
        [OperationContract]
        List<SuperHero> GetAllHeroes();
        [OperationContract]
        string AddHero(SuperHero hero);
        [OperationContract]
        void UpdateHero(SuperHero updatedHero, string id);
        [OperationContract]
        bool DeleteHero(string id);
    }
    // Use a data contract as illustrated in the sample below to add composite types to service operations.  
    [DataContract]
    public class SuperHero
    {
        int id;
        string firstName;
        string lastName;
        string heroName;
        string placeOfBirth;
        int combat;
        string dateBirth;
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        [DataMember]
        public string HeroName
        {
            get { return heroName; }
            set { heroName = value; }
        }
        [DataMember]
        public string PlaceOfBirth
        {
            get { return placeOfBirth; }
            set { placeOfBirth = value; }
        }
        [DataMember]
        public int Combat
        {
            get { return combat; }
            set { combat = value; }
        }
        [DataMember]
        public string DateBirth
        {
            get { return dateBirth; }
            set { dateBirth = value; }
        }



        //    [DataContract]
        //public class SuperHero
        //{
        //    [DataMember]
        //    public int Id { get; set; }
        //    [DataMember]
        //    public string FirstName { get; set; }
        //    [DataMember]
        //    public string LastName { get; set; }
        //    [DataMember]
        //    public string HeroName { get; set; }
        //    [DataMember]
        //    public string PlaceOfBirth { get; set; }
        //    [DataMember]
        //    public int Combat { get; set; }
    }
}