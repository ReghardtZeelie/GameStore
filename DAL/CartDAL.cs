using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   
    public class CartDAL
    {
        private readonly IConfiguration _configuration;
        public CartDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void IItem(CartModel Cart, ref string log)
        {
            SqlTransaction sqlTransaction = null;
            int CartID = 0;
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));
            try
            {

                connection.Open();
                sqlTransaction = connection.BeginTransaction();
                SqlCommand cmd = connection.CreateCommand();

                cmd.Parameters.AddWithValue("@UserID", Cart.UserID);
                cmd.CommandText = "IItem";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = sqlTransaction;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        CartID = Convert.ToInt32(dr["CartID"].ToString());
                    }
                }



                foreach (var item in Cart.cartItems)
                {
                    cmd.Parameters.AddWithValue("@CartID", CartID);
                    cmd.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                    cmd.Parameters.AddWithValue("@Qty", item.Qty);
                    cmd.CommandText = "ICartItem";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Transaction = sqlTransaction;
                     dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            log = "Item has been added to cart. Cart Total is: " + dr["Total"]+"";
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
                log = "An exception has uncured while adding the item to the database. Error:  " + ex.Message.ToString();
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
            }
        }
    }
}
