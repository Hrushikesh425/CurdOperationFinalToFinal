﻿using CurdOperationFinalToFinal.Models;
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
                    userList.Gender = dr["Gender"].ToString();

                    userFinal.Add(userList);
                }
                con.Close();

            }
            return userFinal;
        }

        public bool Insert(UserVM model, List<userAddress> Address)
        {
            string addressListJson = JsonConvert.SerializeObject(Address);
            int id = 0;
            using (con = new SqlConnection(GetConnectionString()))
            {
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[SaveEmployee]";
                cmd.Parameters.AddWithValue("@firstname", model.userData.firstName);
                cmd.Parameters.AddWithValue("@lastname", model.userData.lastName);
                cmd.Parameters.AddWithValue("@dob", model.userData.dob.Date);
                cmd.Parameters.AddWithValue("@phonenumber", model.userData.phoneNumber);
                cmd.Parameters.AddWithValue("@isActive", model.userData.isActive);
                cmd.Parameters.AddWithValue("@email", model.userData.Email);
                cmd.Parameters.AddWithValue("genderId", model.userData.Gender);
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

        public UserVM GetById(int id)
        {
            try
            {
                UserVM employee = new UserVM()
                {
                    userData = new userData(),
                    AddressList = new List<userAddress>()
                };

                using (con = new SqlConnection(GetConnectionString()))
                {
                    userAddress address = new userAddress();
                    cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spGet_AllDetailsById]";
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        
                        employee.userData.id = Convert.ToInt32(dr["id"]);
                        employee.userData.firstName = Convert.ToString(dr["firstName"]);
                        employee.userData.lastName = Convert.ToString(dr["lastName"]);
                        employee.userData.Gender = Convert.ToString(dr["GenderId"]);
                        employee.userData.dob = Convert.ToDateTime(dr["dob"]).Date;
                        employee.userData.Email = Convert.ToString(dr["Email"]);
                        employee.userData.phoneNumber = Convert.ToString(dr["phoneNumber"]);
                        employee.userData.isActive = Convert.ToBoolean(dr["isActive"]);


                        address.countryId = Convert.ToInt32(dr["CountryId"]);
                        address.stateId = Convert.ToInt32(dr["StateId"]);
                        address.address = Convert.ToString(dr["address"]);
                        address.city = Convert.ToString(dr["City"]);


                        // Add the address to the address list
                        employee.AddressList.Add(address);
                    }
                    con.Close();
                    return employee;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public bool Update(userData model)
        {
            int id = 0;
            using (con = new SqlConnection(GetConnectionString()))
            {
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[sp_update_user]";
                cmd.Parameters.AddWithValue("@id", model.id);
                cmd.Parameters.AddWithValue("@firstname", model.firstName);
                cmd.Parameters.AddWithValue("@lastname", model.lastName);
                cmd.Parameters.AddWithValue("@dob", model.dob.Date);
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
