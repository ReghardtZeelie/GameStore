﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using Serilog;
using System.Collections.Generic;
using System.Data;


namespace DAL
{
    public class ItemsDAL
    {
        private readonly IConfiguration _configuration;

        public ItemsDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ImageModel QImage_Item(int ItemCode,ref string log)
        {
            ImageModel image = new ImageModel();
            Byte[] b= null;
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = connection;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "QImageItem";

            SqlParameter p1 = new SqlParameter("@ItemCode", SqlDbType.Int, 4);
            p1.Value = ItemCode;

            sqlCmd.Parameters.Add(p1);

            SqlDataReader sqlReader = null;

            try
            {
                connection.Open();
                sqlReader = sqlCmd.ExecuteReader();
                if (sqlReader.HasRows)
                {
                    sqlReader.Read();
                    b = new Byte[sqlReader.GetBytes(0, 0, null, 0, int.MaxValue)];
                    sqlReader.GetBytes(0, 0, b, 0, b.Length);
                    System.IO.MemoryStream strm = new System.IO.MemoryStream(b);
                    strm.Write(b, 0, b.Length);

                    image.ImageFile = b;
                    image.FileName = sqlReader["FileName"].ToString();
                    image.fileType = sqlReader["extention"].ToString();

                }
            }
            catch (Exception ex)
            {
                image = null;
                    log = "An exception has uncured while retrieving the image. Error:  " + ex.Message.ToString();
            }
            finally
            {
                if (!sqlReader.IsClosed)
                {
                    sqlReader.Close();
                }

                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            
            return image;
        }
        public bool DItem(int itemcode, ref string log)
        {
            SqlTransaction sqlTransaction = null;
           
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));
            try
            {

                connection.Open();
                sqlTransaction = connection.BeginTransaction();
                SqlCommand cmd = connection.CreateCommand();
                SqlCommand cmd1 = connection.CreateCommand();
                cmd.Parameters.AddWithValue("@itemCode", itemcode);
                cmd.CommandText = "DItem";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = sqlTransaction;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {

                        if (Convert.ToBoolean(dr["status"].ToString()) != true)
                        {
                            log = "Item with item code: " + itemcode.ToString() + " does not exist.";
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
                log = "An exception has uncured while adding the item to the database. Error:  " + ex.Message.ToString();
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                return false;
            }
            return true;
        }
        public List<ItemsModel> QAllItems(string ItemName, ref string log)
        {
            SqlTransaction sqlTransaction = null;
            List<ItemsModel> itemlist = new List<ItemsModel>(); 
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));
            try
            {

                connection.Open();
                sqlTransaction = connection.BeginTransaction();
                SqlCommand cmd = connection.CreateCommand();
                SqlCommand cmd1 = connection.CreateCommand();
                cmd.Parameters.AddWithValue("@Itemname", ItemName);
                cmd.CommandText = "QAllItems";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = sqlTransaction;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ItemsModel item = new ItemsModel
                        {
                            ItemName = dr["ItemName"].ToString(),
                            ItemDescription = dr["Description"].ToString(),
                            itemCost = Convert.ToDecimal(dr["Cost"].ToString()),
                            ItemWholeSale = Convert.ToDecimal(dr["wholesale"].ToString()),
                            ItemRetail = Convert.ToDecimal(dr["Retail"].ToString()),
                            fileType = dr["Extention"].ToString(),
                            FileName = dr["ImageFileName"].ToString(),
                            Id = Convert.ToInt32(dr["ItemCode"].ToString()),
                            ImageFile = null,
                            Make = dr["Make"].ToString(),
                            Model = dr["Model"].ToString()

                        };

                        itemlist.Add(item);

                    }
                }
                dr.Close();
                sqlTransaction.Commit();
                connection.Close();
                connection.Dispose();

            }
            catch (SqlException ex)
            {
                itemlist = null;
                connection.Close();
                connection.Dispose();
                log = "An exception has uncured while reading the items from the database. Error:  " + ex.Message.ToString();
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }
                return itemlist;
            }
            return itemlist;
        }
        public int IItem(ItemsModel newItem, ref string log)
        {
            SqlTransaction sqlTransaction = null;
            int newItemID = 0;
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("GameStoreSQL"));
            try
            {
                
                connection.Open();
                sqlTransaction = connection.BeginTransaction();
                SqlCommand cmd = connection.CreateCommand();
               
                cmd.Parameters.AddWithValue("@Name", newItem.ItemName);
                cmd.Parameters.AddWithValue("@Description", newItem.ItemDescription);
                cmd.Parameters.AddWithValue("@CostPrice", newItem.itemCost);
                cmd.Parameters.AddWithValue("@Wholesale", newItem.ItemWholeSale);
                cmd.Parameters.AddWithValue("@Retail", newItem.ItemRetail);
                cmd.Parameters.AddWithValue("@Filetype", newItem.fileType);
                cmd.Parameters.AddWithValue("@Filename", newItem.FileName);
                cmd.Parameters.AddWithValue("@ImageFile", newItem.ImageFile);
                cmd.Parameters.AddWithValue("@Make", newItem.Make);
                cmd.Parameters.AddWithValue("@Model", newItem.Model);
                cmd.CommandText = "IItem";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = sqlTransaction;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {

                        if (dr["NewItemCode"].ToString() != "ExistingItem")
                        {
                            newItemID = Convert.ToInt32(dr["NewItemCode"].ToString());
                        }
                        else
                        {
                            log = "Item with name: "+ newItem.ItemName + " already exist in the database. Please try use another name.";
                        }
                    }
                }
                dr.Close();
                sqlTransaction.Commit();
                connection.Close();
                connection.Dispose();
                return newItemID;
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
                return newItemID;
            }
        }
    }
}
