using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Tea_Coffe
{
    
    internal class DataBase
    {
        public string connectionString = "host='localhost';database='tea_coffe';uid='root';pwd='root';charset=utf8;";
        public DataTable LoadData(string str)
        {

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query1 = "SELECT * FROM " + str + ";";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }

        public DataTable LoadProducts()
        {

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query1 = "SELECT * FROM tea_coffe.product join products_unit on unit = idProducts_unit join product_category on category = idProduct_category;";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }
    }

    
}
