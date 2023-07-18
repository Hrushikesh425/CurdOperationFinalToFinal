using CurdOperationFinalToFinal.Models;
using CurdOperationFinalToFinal.Models.UserVM;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Windows.Input;

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
                    userList.userExcel = dr["userExcel"].ToString();
                    userList.Gender = dr["Gender"].ToString();

                    userFinal.Add(userList);
                }
                con.Close();

            }
            return userFinal;
        }

        public bool Insert(userData model, List<userAddress> Address)
        {
            string addressListJson = JsonConvert.SerializeObject(Address);
            int id = 0;
            string demo = "\\uploadFile\\"+model.uploadFile.FileName;
            using (con = new SqlConnection(GetConnectionString()))
            {
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[SaveEmployee]";
                cmd.Parameters.AddWithValue("@firstname", model.firstName);
                cmd.Parameters.AddWithValue("@lastname", model.lastName);
                cmd.Parameters.AddWithValue("@dob", model.dob.Date);
                cmd.Parameters.AddWithValue("@phonenumber", model.phoneNumber);
                cmd.Parameters.AddWithValue("@isActive", model.isActive);
                cmd.Parameters.AddWithValue("@email", model.Email);
                cmd.Parameters.AddWithValue("genderId", model.Gender);
                cmd.Parameters.AddWithValue("@userExcel", model.userExcel);
                cmd.Parameters.AddWithValue("@AddressList", addressListJson);

                // Set the SQLDbType to NVarChar for the @AddressList parameter
                cmd.Parameters["@AddressList"].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters["@AddressList"].Size = -1;
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
        public string GetImage(int id)
        {
            string imagePath = null;
            using (con = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT userExcel FROM userData WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", id);

                con.Open();

                imagePath = (string)command.ExecuteScalar();
            }

            return imagePath;
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





        public userData GetById(int id)
            {
            List<country> countries = GetCountries();
            try
            {


                userData employee = new userData()
                {
                   
                    AddressList = new List<userAddress>(),
                    
                };
                using (con = new SqlConnection(GetConnectionString()))
                {
                    userData data = new userData();
                    //userAddress address = new userAddress();
                    cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spGet_DetailsById]";
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        
                       employee.id = Convert.ToInt32(dr["id"]);
                        employee.firstName = Convert.ToString(dr["firstName"]);
                       employee.lastName = Convert.ToString(dr["lastName"]);
                      employee.Gender = Convert.ToString(dr["GenderId"]);
                        employee.dob = Convert.ToDateTime(dr["dob"]).Date;
                        employee.Email = Convert.ToString(dr["Email"]);
                        employee.phoneNumber = Convert.ToString(dr["phoneNumber"]);
                        employee.isActive = Convert.ToBoolean(dr["isActive"]);
                        employee.userExcel = Convert.ToString(dr["userExcel"]);
                    }
                    employee.AddressList.Clear();
                    dr.NextResult();
                    while (dr.Read())
                    {

                        userAddress add = new userAddress();
                        add.addressId = Convert.ToInt32(dr["addressId"]);
                        add.countryId = Convert.ToInt32(dr["CountryId"]);
                        add.stateId = Convert.ToInt32(dr["StateId"]);
                        add.address = Convert.ToString(dr["address"]);
                        add.city = Convert.ToString(dr["City"]);

                        employee.AddressList.Add(add);
                    }

                        // Add the address to the address list
                    con.Close();
                    return employee;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public bool Update(userData model, List<userAddress> Address)
        {
            string addressListJson = JsonConvert.SerializeObject(Address);
            int id = 0;
            using (con = new SqlConnection(GetConnectionString()))
            {
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spUpdateAll_Employee]";
                cmd.Parameters.AddWithValue("@UserId", model.id);
                cmd.Parameters.AddWithValue("@firstname", model.firstName);
                cmd.Parameters.AddWithValue("@lastname", model.lastName);
                cmd.Parameters.AddWithValue("@dob", model.dob.Date);
                cmd.Parameters.AddWithValue("@phonenumber", model.phoneNumber);
                cmd.Parameters.AddWithValue("@isActive", model.isActive);
                cmd.Parameters.AddWithValue("@email", model.Email);
                cmd.Parameters.AddWithValue("genderId", model.Gender);
                cmd.Parameters.AddWithValue("userExcel", model.userExcel);
                cmd.Parameters.AddWithValue("@AddressesJson", addressListJson);
                con.Open();

                id = cmd.ExecuteNonQuery();
                con.Close();
            }
            return id > 0 ? true : false;
        }


        public bool UpdateAll(userData model, List<userAddress> AddressList)
        {
            int id = 0;
            using (con = new SqlConnection(GetConnectionString()))
            {
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "finalUpdate";
                cmd.Parameters.AddWithValue("@UserId", model.id);
                cmd.Parameters.AddWithValue("@firstName", model.firstName);
                cmd.Parameters.AddWithValue("@lastName", model.lastName);
                cmd.Parameters.AddWithValue("@genderId", model.Gender);
                cmd.Parameters.AddWithValue("@dob", model.dob);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@phoneNumber", model.phoneNumber);
                cmd.Parameters.AddWithValue("@isActive", model.isActive);
                cmd.Parameters.AddWithValue("userExcel", model.userExcel);
                cmd.Parameters.AddWithValue("@AddressesJson", JsonConvert.SerializeObject(AddressList));

                con.Open();
                id = cmd.ExecuteNonQuery();
                con.Close();
            }

            return id > 0 ? true : false;
        }






        public bool Delete(int id)
        {
            int i = 0;
            using (con = new SqlConnection(GetConnectionString()))
            {
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[sp_delete_user]";
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();
            }
            return i > 0 ? true : false;
        }




    }
}
