using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceCrude
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    //[ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SuperHeroService : ISuperHero
        {
        private string connStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
            public List<SuperHero> GetAllHeroes()
            {
                List<SuperHero> superHeroes = new List<SuperHero>();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "SELECT * FROM SuperHeroes";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            SuperHero hero = new SuperHero
                            {
                                Id = (int)reader["ID"],
                                FirstName = reader["FirstName"].ToString().Trim(),
                                LastName = reader["LastName"].ToString().Trim(),
                                HeroName = reader["HeroName"].ToString().Trim(),
                                PlaceOfBirth = reader["PlaceOfBirth"].ToString().Trim(),
                                Combat = (int)reader["Combat"],
                                DateBirth = ((DateTime)reader["DateBirth"]).ToString("yyyy-MM-dd HH:mm:ss"),
                            };
                            superHeroes.Add(hero);
                        }
                    }
                }
                return superHeroes;
            }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "AddHero", Method = "POST")]
        public string AddHero(SuperHero hero)
        {
            string Message;
           
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO SuperHeroes(FirstName, LastName, HeroName, PlaceOfBirth, Combat, DateBirth) values(@FirstName, @LastName, @HeroName, @PlaceOfBirth, @Combat, @DateBirth)");
                //string query = "INSERT INTO SuperHeroes(FirstName, LastName, HeroName, PlaceOfBirth, Combat) values(@FirstName, @LastName, @HeroName, @PlaceOfBirth, @Combat)";
                string query = sb.ToString();
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", hero.FirstName);
                cmd.Parameters.AddWithValue("@LastName", hero.LastName);
                cmd.Parameters.AddWithValue("@HeroName", hero.HeroName);
                cmd.Parameters.AddWithValue("@PlaceOfBirth", hero.PlaceOfBirth);
                cmd.Parameters.AddWithValue("@Combat", hero.Combat);
                cmd.Parameters.AddWithValue("@DateBirth", DateTime.ParseExact(hero.DateBirth, "yyyy-MM-dd HH:mm:ss", null));
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    Message = $"{hero.HeroName} Details inserted succsessfully";
                }
                else
                {
                    Message = $"{hero.HeroName} Details not inserted succsessfully";
                }
                return Message;
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UpdateHero/{id}", Method = "PUT")]
        public void UpdateHero(SuperHero updatedHero, string id)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = $"UPDATE SuperHeroes SET FirstName=@FirstName, LastName=@LastName, HeroName=@HeroName, PlaceOfBirth=@PlaceOfBirth, DateBirth=@DateBirth, Combat=@Combat WHERE ID={id}";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", updatedHero.FirstName);
                cmd.Parameters.AddWithValue("@LastName", updatedHero.LastName);
                cmd.Parameters.AddWithValue("@HeroName", updatedHero.HeroName);
                cmd.Parameters.AddWithValue("@PlaceOfBirth", updatedHero.PlaceOfBirth);
                cmd.Parameters.AddWithValue("@Combat", updatedHero.Combat);
                cmd.Parameters.AddWithValue("@DateBirth", updatedHero.DateBirth);
                cmd.ExecuteNonQuery();
            }
        }
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "DeleteHero/{id}", Method = "DELETE")]
        public bool DeleteHero(string id)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = $"DELETE FROM SuperHeroes WHERE ID={id}";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            //Data.SuperHeroes = Data.SuperHeroes.Where(sh => sh.Id != int.Parse(id)).ToList();
            //return Data.SuperHeroes;
        }

        //[OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetHero/{id}")]
        //public SuperHero GetHero(string id)
        //{
        //    return Data.SuperHeroes.Find(sh => sh.Id == int.Parse(id));
        //}
        //[OperationContract]
        //[WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
        //   UriTemplate = "UpdateHero/{id}", Method = "PUT")]
        //public SuperHero UpdateHero(SuperHero updatedHero, string id)
        //{
        //    SuperHero hero = Data.SuperHeroes.Where(sh => sh.Id == int.Parse(id)).FirstOrDefault();

        //    hero.FirstName = updatedHero.FirstName;
        //    hero.LastName = updatedHero.LastName;
        //    hero.HeroName = updatedHero.HeroName;
        //    hero.PlaceOfBirth = updatedHero.PlaceOfBirth;
        //    hero.Combat = updatedHero.Combat;
        //    return hero;
        //}



    }
}
