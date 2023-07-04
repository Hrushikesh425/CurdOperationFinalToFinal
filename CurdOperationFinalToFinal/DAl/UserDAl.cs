using CurdOperationFinalToFinal.Models;
using System.Data;
using System.Data.SqlClient;
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

        public bool Insert(userData model)
		{
			int id = 0;
			using (con = new SqlConnection(GetConnectionString()))
			{
				cmd = con.CreateCommand();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "new_sp_write";
				cmd.Parameters.AddWithValue("@firstname", model.firstName);
				cmd.Parameters.AddWithValue("@lastname", model.lastName);
				cmd.Parameters.AddWithValue("@dob", model.dob);
				cmd.Parameters.AddWithValue("@phonenumber", model.phoneNumber);
				cmd.Parameters.AddWithValue("@isActive", model.isActive);
				cmd.Parameters.AddWithValue("@email", model.Email);
                cmd.Parameters.AddWithValue("genderId", model.Gender);
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



    }
}
