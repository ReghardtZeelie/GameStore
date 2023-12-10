using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        public bool AddItemToCart(CartModel Cart,int UserID, ref string log)
        {
            SqlTransaction sqlTransaction = null;
            int CartID = 0;
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));
            try
            {

                connection.Open();
                sqlTransaction = connection.BeginTransaction();
                SqlCommand cmd = connection.CreateCommand();

                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandText = "ICart";
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
                dr.Close();
                sqlTransaction.Commit();
                cmd.Dispose();

                sqlTransaction = connection.BeginTransaction();
                 cmd = connection.CreateCommand();
                cmd.Transaction = sqlTransaction;
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
                            log = "Item has been added to cart. Cart Total is: " + dr["Total"] + "";
                        }
                    }
                    else
                    {
                        log = "Item with item code:" + item.ItemCode + " does note exist. Please enter a valid item code.";
                    }
                }

                dr.Close();
                sqlTransaction.Commit();
               
            }
            catch (SqlException ex)
            {
                connection.Close();
                connection.Dispose();
                log = "An exception has occurred while adding the item to the database. Error:  " + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool RemoveItemFromCart(CartModel Cart, UsersModel user, ref string log)
        {
            SqlTransaction sqlTransaction = null;
            
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));
            try
            {
                foreach (var item in Cart.cartItems)
                {
                    connection.Open();
                    sqlTransaction = connection.BeginTransaction();
                    SqlCommand cmd = connection.CreateCommand();
                   
                    cmd.Parameters.AddWithValue("@CartID", user.CartID);
                    cmd.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                    cmd.Parameters.AddWithValue("@Qty", item.Qty);
                    cmd.CommandText = "DCartItems";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = sqlTransaction;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            log = "Item has been removed from cart. Cart Total is: " + dr["Total"] + "";
                        }
                    }
                    else
                    {
                        log = "Item with item code:" + item.ItemCode + " does note exist in cart. Please enter a valid item code.";
                    }
                    dr.Close();
                    sqlTransaction.Commit();
                }

            }
            catch (SqlException ex)
            {
                connection.Close();
                connection.Dispose();
                log = "An exception has occurred while adding the item to the database. Error:  " + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public List<ViewCartModel> QCart(UsersModel user, ref string log)
        {
            SqlTransaction sqlTransaction = null;
            List<ViewCartModel> cart = new List<ViewCartModel>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));
            try
            {

                connection.Open();
                sqlTransaction = connection.BeginTransaction();
                SqlCommand cmd = connection.CreateCommand();
                SqlCommand cmd1 = connection.CreateCommand();
                cmd.Parameters.AddWithValue("@UserID", user.ID);
                cmd.CommandText = "QCart";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = sqlTransaction;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ViewCartModel item = new ViewCartModel
                        {
                            ItemName = dr["ItemName"].ToString(),
                            ItemDescription = dr["Description"].ToString(),
                            ItemRetail = Convert.ToDecimal(dr["Retail"].ToString()),
                            Make = dr["Make"].ToString(),
                            Model = dr["Model"].ToString(),
                            QTy = Convert.ToInt32( dr["Qty"].ToString())

                        };

                        cart.Add(item);

                    }
                }
                else
                {
                    cart = null;
                    log = "The cart for user: " + user.Name + " is empty.";
                }
                dr.Close();
                sqlTransaction.Commit();
                connection.Close();
                connection.Dispose();

            }
            catch (SqlException ex)
            {
                cart = null;
                connection.Close();
                connection.Dispose();
                log = "An exception has occurred while reading the items from the database. Error:  " + ex.Message.ToString();
                return cart;
            }
            return cart;
        }
    }
}
