﻿

using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace DAL
{
    public class UsersDAL 
    {

        private readonly IConfiguration _configuration;
      
        public UsersDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public UsersModel QUserLogin(string UserName, string Password,ref string log)
        {
           
            SqlTransaction sqlTransaction = null;
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));

            UsersModel user = new UsersModel();
            try
            {
              
                SqlCommand cmd = connection.CreateCommand();
                connection.Open();
                sqlTransaction = connection.BeginTransaction();
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@UserPassword", Password);
                cmd.CommandText = "QUser";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = sqlTransaction;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        if (Convert.ToBoolean(dr["ULogin"].ToString()) == false)
                        {
                            user.ID = Convert.ToInt32(dr["uId"]);
                            user.Name = dr["UName"].ToString();
                            user.CartID = Convert.ToInt32(dr["cartID"]);
                        }
                        else
                        {
                            log = "User: " + UserName + " is already logged in.";
                            user = null;
                        }
                    }
                }
                else
                {
                    user = null;
                }
                dr.Close();
                sqlTransaction.Commit();
                connection.Close();
                connection.Dispose();
            }
            catch(SqlException ex)
            {
                log = "An exception has occurred while retrieving the login details. Error:  " + ex.Message.ToString();
            }
            return user;
        }

        public UsersModel IUser(AddUserModel newuser, ref string log)
        {
            SqlTransaction sqlTransaction = null;
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));
            try
            {
                UsersModel newUser = new UsersModel();
                
                connection.Open();
                sqlTransaction = connection.BeginTransaction();
                SqlCommand cmd = connection.CreateCommand();
                SqlCommand cmd1 = connection.CreateCommand();
                cmd.Parameters.AddWithValue("@Name", newuser.Name);
                cmd.Parameters.AddWithValue("@Password", newuser.Password);
                cmd.Parameters.AddWithValue("@Age", newuser.DOB);
                cmd.CommandText = "IUser";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = sqlTransaction;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        newUser.Name = dr["UName"].ToString();
                    }
                }
                dr.Close();
                sqlTransaction.Commit();
                connection.Close();
                connection.Dispose();
                return newUser;
            }
            catch(SqlException ex)
            {
                connection.Close();
                connection.Dispose();
                log = "An exception has occurred while adding the user to the database. Error:  " + ex.Message.ToString();
                return null;
            }



           
        }
        public bool LogoutUser(UsersModel newuser, ref string log)
        {
            SqlTransaction sqlTransaction = null;
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));
            try
            {
                UsersModel newUser = new UsersModel();

                connection.Open();
                sqlTransaction = connection.BeginTransaction();
                SqlCommand cmd = connection.CreateCommand();
                SqlCommand cmd1 = connection.CreateCommand();
                cmd.Parameters.AddWithValue("@UserID", newuser.ID);
              
                cmd.CommandText = "LogoutUser";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = sqlTransaction;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr["status"].ToString() == "Loggedout")
                        {
                            dr.Close();
                            sqlTransaction.Commit();
                            connection.Close();
                            connection.Dispose();
                            log = "User logged out.";
                            return true;
                        }
                        else
                        {
                            dr.Close();
                            sqlTransaction.Commit();
                            connection.Close();
                            connection.Dispose();
                            log = "User not logged in.";
                            return false;
                        }
                    }
                }
                dr.Close();
                sqlTransaction.Commit();
                connection.Close();
                connection.Dispose();
                
            }
            catch (SqlException ex)
            {
                connection.Close();
                connection.Dispose();
                log = "An exception has occurred while logging out the user. Error:  " + ex.Message.ToString();
                return false;
            }

            return false;


        }

        public bool DUser(UsersModel newuser,int AdminID, ref string  log)
        {
            SqlTransaction sqlTransaction = null;
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));
            try
            {

               
                connection.Open();
                sqlTransaction = connection.BeginTransaction();
                SqlCommand cmd = connection.CreateCommand();
                cmd.Parameters.AddWithValue("", newuser.Name);
                cmd.Parameters.AddWithValue("", AdminID);
                connection.Open();
                cmd.CommandText = "Duser";
                SqlDataAdapter Adatper = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                connection.Close();
                connection.Dispose();

            }
            catch(SqlException ex)
            {
                connection.Close();
                connection.Dispose();
                log = "An exception has occurred while deleting the user from the database. Error:  " + ex.Message.ToString();
               
                return false;
            }

            return true;
        }


    }
}