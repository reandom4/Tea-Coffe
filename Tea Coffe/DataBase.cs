using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using static Tea_Coffe.Window1;
using System.Transactions;
using System.Collections;
using MySqlX.XDevAPI.Common;
using System.Security.Cryptography;

namespace Tea_Coffe
{
    
    internal class DataBase
    {
        public string connectionString = "Server='localhost';database='tea_coffe';uid='root';pwd='root';";

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

        public DataTable SearchProducts(string search,string sort)
        {
            if(sort == "Популярные")
            {
                sort = "total_quantity DESC";
            }
            if(sort == "Сначала дешёвые")
            {
                sort = "cost";
            }
            if(sort == "Сначала дорогие")
            {
                sort = "cost DESC";
            }
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query1 = $"SELECT p.*,u.*,c.*,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(quantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category Where name like '%{search}%' Order by {sort};";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }

        public DataTable FiltrProducts(string Filtr, string sort)
        {
            if (sort == "Популярные")
            {
                sort = "total_quantity DESC";
            }
            if (sort == "Сначала дешёвые")
            {
                sort = "cost";
            }
            if (sort == "Сначала дорогие")
            {
                sort = "cost DESC";
            }
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query1 = $"SELECT p.*,u.*,c.*,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(quantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category Where Product_categoryname = '{Filtr}' Order by {sort};";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }
        public DataTable SearchFiltrProducts(string search,string Filtr, string sort)
        {
            if (sort == "Популярные")
            {
                sort = "total_quantity DESC";
            }
            if (sort == "Сначала дешёвые")
            {
                sort = "cost";
            }
            if (sort == "Сначала дорогие")
            {
                sort = "cost DESC";
            }
            if(search != "")
            {
                search = $"And p.name ='{search}'";
            }
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query1 = $"SELECT p.*,u.*,c.*,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(quantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category Where Product_categoryname = '{Filtr}' {search} Order by {sort};";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }
        public DataTable BigFiltrProducts(string Filtr, string sort)
        {
            if (sort == "Популярные")
            {
                sort = "total_quantity DESC";
            }
            if (sort == "Сначала дешёвые")
            {
                sort = "cost";
            }
            if (sort == "Сначала дорогие")
            {
                sort = "cost DESC";
            }
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query1 = $"SELECT p.*,u.*,c.*,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(quantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category Where Product_categoryname like '%{Filtr}%' Order by {sort};";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }
        public DataTable CacaoFiltrProducts(string Filtr1,string Filtr2, string sort)
        {
            if (sort == "Популярные")
            {
                sort = "total_quantity DESC";
            }
            if (sort == "Сначала дешёвые")
            {
                sort = "cost";
            }
            if (sort == "Сначала дорогие")
            {
                sort = "cost DESC";
            }
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query1 = $"SELECT p.*,u.*,c.*,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(quantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category Where Product_categoryname like '%{Filtr1}%' or Product_categoryname like '%{Filtr2}%' Order by {sort};";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }

        public bool AddOrder(List<ProductItem> items,int employeid,DateTime lastDate)
        {
            bool result = false;
            using (TransactionScope transaction = new TransactionScope())
            {
                MySqlConnection connection = new MySqlConnection(connectionString);

                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT MAX(idOrder_Items) FROM order_items", connection);
                int lastId = Convert.ToInt32(command.ExecuteScalar()) + 1;
                MySqlCommand command2;
                MySqlCommand command21;
                int fulprice = 0;
                foreach (ProductItem item in items)
                {
                    command2 = new MySqlCommand($"INSERT INTO `tea_coffe`.`order_items` (`idOrder_Items`, `product_id`, `quantity`) VALUES('{lastId}', '{item.Id}', '{item.BasketQuantity}');", connection);
                    command2.ExecuteNonQuery();
                    command21 = new MySqlCommand($"UPDATE `tea_coffe`.`product` SET `quantity` = `quantity`- {item.BasketQuantity} WHERE (`idProducts` = '{item.Id}');", connection);
                    command21.ExecuteNonQuery();
                    fulprice += item.BasketCost;
                }

                
                string date = lastDate.ToString("yyyy.MM.dd HH:mm:ss");
                // Запрос 2
                MySqlCommand command3 = new MySqlCommand($"INSERT INTO `tea_coffe`.`order` (`OrderProducts`, `OrderPrice`, `employeid`, `date`) VALUES ('{lastId}', '{fulprice}', '{employeid}','{date}');", connection);
                command3.ExecuteNonQuery();

                // Запрос 3 и так далее

                // Если все запросы выполнены успешно, коммитим транзакцию
                transaction.Complete();
                result = true;
                connection.Close();


            }
            return result;
        }

        public string ValidateLogin(string login, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                connection.Open();

                string query = "SELECT password, salt, User_roleName FROM user inner join user_role on role = idUser_role WHERE login = @Login";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader.GetString(0);
                            string storedSalt = reader.GetString(1);
                            string role = reader.GetString(2);
                            string hashedPassword = HashPassword(password, storedSalt);
                            if( storedHash == hashedPassword)
                            {
                                return role;
                            }
                        }
                    }
                }

            }

            return null;
        }
        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                byte[] hash = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hash);
            }
        }
    }

    
}
