using CurdOperationFinalToFinal.Models;
using CurdOperationFinalToFinal.Models.UserVM;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace CurdOperationFinalToFinal.DAl
{
	public class UserDAl
	{
		SqlConnection con = null;
		SqlCommand cmd = null;

		public static IConfiguration configuration { get; set; }

		private string GetConnectionString()
		{
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
			configuration = builder.Build();
			return configuration.GetConnectionString("DefaultString");
		}
        public List<userData> GetAll()
        {
            List<userData> userFinal = new List<userData>();
            using (con = new SqlConnection(GetConnectionString()))
            {
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "get_user";
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    userData userList = new userData();
                    userList.id = Convert.ToInt32(dr["id"]);
                    userList.firstName = dr["firstName"].ToString();
                    userList.lastName = dr["lastName"].ToString();
					userList.dob = Convert.ToDateTime(dr["dob"]).Date;
                    userList.phoneNumber = dr["phoneNumber"].ToString();
					userList.Email = dr["Email"].ToString();
					userList.isActive = Convert.ToBoolean(dr["isActive"]);
                    userList.Gender = dr["Gender"].ToString();
                    
                    userFinal.Add(userList);
                }
                con.Close();

            }
            return userFinal;
        }

            public bool Insert(UserVM model)
		    {
			    int id = 0;
			    using (con = new SqlConnection(GetConnectionString()))
			    {
				    cmd = con.CreateCommand();
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.CommandText = "new_sp_write";
				    cmd.Parameters.AddWithValue("@firstname", model.userData.firstName);
				    cmd.Parameters.AddWithValue("@lastname", model.userData.lastName);
				    cmd.Parameters.AddWithValue("@dob", model.userData.dob.Date);
				    cmd.Parameters.AddWithValue("@phonenumber", model.userData.phoneNumber);
				    cmd.Parameters.AddWithValue("@isActive", model.userData.isActive);
				    cmd.Parameters.AddWithValue("@email", model.userData.Email);
                    cmd.Parameters.AddWithValue("genderId", model.userData.Gender);
				    con.Open();

				    id = cmd.ExecuteNonQuery();
				    con.Close();
			    }
			    return id > 0 ? true : false;
		    }

		//public List<gender> GetCombinedData()
		//{
		//    List<gender> combinedData = new List<gender>();

		//    {
		//        con.Open();

		//        string query = "SELECT id, name FROM gender";
		//        SqlCommand cmd = new SqlCommand(query, con);

		//        using (SqlDataReader reader = cmd.ExecuteReader())
		//        {
		//            while (reader.Read())
		//            {
		//                string id = reader["id"].ToString();
		//                string name = reader["name"].ToString();


		//            }
		//        }
		//    }

		//    // Add predefined data

		//    return combinedData;
		//}


		//this function for country dropdown 

		public List<country> GetCountries()
		{
			List<country> countries = new List<country>();

			using (con = new SqlConnection(GetConnectionString()))
			{
				string query = "SELECT id, [name] FROM Country";
				SqlCommand command = new SqlCommand(query, con);

				con.Open();

				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						country country = new country
						{
							id = reader.GetInt32(0),
							name = reader.GetString(1)
						};

						countries.Add(country);
					}
				}
			}

			return countries;
		}

		//this function for state display using country id
		public List<state> GetStatesByCountry(int countryId)
		{
			List<state> states = new List<state>();

			using (con = new SqlConnection(GetConnectionString()))
			{
				string query = "SELECT Id, Name FROM State WHERE CountryId = @CountryId";
				SqlCommand command = new SqlCommand(query, con);
				command.Parameters.AddWithValue("@CountryId", countryId);

				con.Open();

				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						state state = new state
						{
							id = reader.GetInt32(0),
							name = reader.GetString(1)
						};

						states.Add(state);
					}
				}
			}

			return states;
		}








	}
}
