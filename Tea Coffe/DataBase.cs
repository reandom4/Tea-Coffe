using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using static Tea_Coffe.Window1;

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
        public DataTable LoadUsers()
        {

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query1 = "SELECT * FROM tea_coffe.user join user_role where role = idUser_role;";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }
        public DataTable LoadUsersSearch(string search)
        {

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query1 = $"SELECT * FROM tea_coffe.user join user_role where role = idUser_role and CONCAT(name, ' ', patronymic) like '%{search}%' ;";
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

        public DataTable LoadProductsStorage()
        {

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query1 = "SELECT * FROM tea_coffe.product join products_unit on unit = idProducts_unit join product_category on category = idProduct_category order by (quantity / products_unitcol);";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }

        public DataTable SearchProducts(string search, string sort)
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
            string query1 = $"SELECT p.*,u.*,c.*,IFNULL(o.total_Position, 0) AS total_Position,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(quantity) AS total_Position, Sum(unitquantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category Where name like '%{search}%' Order by {sort};";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }

        public DataTable SearchProducts(string search, string sort, int curtab, int itemontab)
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
            string query1 = $"SELECT p.*,u.*,c.*,IFNULL(o.total_Position, 0) AS total_Position,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(quantity) AS total_Position, Sum(unitquantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category Where name like '%{search}%' Order by {sort} Limit {itemontab} Offset {curtab * itemontab}; ";
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
            string query1 = $"SELECT p.*,u.*,c.*,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(unitquantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category Where Product_categoryname = '{Filtr}' Order by {sort};";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }
        public DataTable SearchFiltrProducts(string search, string Filtr, string sort)
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
            if (search != "")
            {
                search = $"And p.name ='{search}'";
            }
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query1 = $"SELECT p.*,u.*,c.*,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(unitquantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category Where Product_categoryname = '{Filtr}' {search} Order by {sort};";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }
        public DataTable BigFiltr(string Filtr, string sort, string search, int limit, int curtab)
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

            if (Filtr.Split(' ')[0] == "Результаты" || Filtr.Split(' ')[0] == "Все")
            {
                Filtr = "";
            }
            else if (Filtr == "Чай" || Filtr == "ЧАЙ")
            {
                Filtr = "and  (Product_categoryname like '%Улун%' or Product_categoryname like  '%Чай%'  or Product_categoryname like '%Ройбуш%') ";
            }
            else if (Filtr == "Кофе" || Filtr == "КОФЕ")
            {
                Filtr = "and  (Product_categoryname like '%кофе%') ";
            }
            else if (Filtr == "Какао" || Filtr == "КАКАО")
            {
                Filtr = "and  (Product_categoryname like '%Горячий шоколад%' or Product_categoryname like  '%Какао%') ";
            }
            else
            {
                Filtr = $"and (Product_categoryname like '%{Filtr}%') ";
            }
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string bigquery = $"SELECT p.*,u.*,c.*,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(unitquantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category " +
                $"Where  p.name like '%{search}%' " +
                $"{Filtr} " +
                $"Order by {sort} " +
                $"limit {limit} offset {curtab * limit};";
            MySqlCommand mySqlCommand = new MySqlCommand(bigquery, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }

        public int GetCount(string Filtr, string sort, string search)
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

            if (Filtr.Split(' ')[0] == "Результаты" || Filtr.Split(' ')[0] == "Все")
            {
                Filtr = "";
            }
            else if (Filtr == "Чай" || Filtr == "ЧАЙ")
            {
                Filtr = "and  (Product_categoryname like '%Улун%' or Product_categoryname like  '%Чай%'  or Product_categoryname like '%Ройбуш%') ";
            }
            else if (Filtr == "Кофе" || Filtr == "КОФЕ")
            {
                Filtr = "and  (Product_categoryname like '%кофе%') ";
            }
            else if (Filtr == "Какао" || Filtr == "КАКАО")
            {
                Filtr = "and  (Product_categoryname like '%Горячий шоколад%' or Product_categoryname like  '%Какао%') ";
            }
            else
            {
                Filtr = $"and (Product_categoryname like '%{Filtr}%') ";
            }
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string bigquery = $"SELECT  COUNT(*) AS total_rows FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(unitquantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category " +
                $"Where  p.name like '%{search}%' " +
                $"{Filtr} " +
                $"Order by {sort} ; ";

            MySqlCommand mySqlCommand = new MySqlCommand(bigquery, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return Convert.ToInt32(dataTable.Rows[0][0].ToString());

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
            string query1 = $"SELECT p.*,u.*,c.*,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(unitquantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category Where Product_categoryname like '%{Filtr}%' Order by {sort};";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }
        public DataTable CacaoFiltrProducts(string Filtr1, string Filtr2, string sort)
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
            string query1 = $"SELECT p.*,u.*,c.*,IFNULL(o.total_quantity, 0) AS total_quantity FROM tea_coffe.product p LEFT JOIN (SELECT product_id, SUM(unitquantity) AS total_quantity FROM tea_coffe.order_items GROUP BY product_id) o ON p.idProducts = o.product_id JOIN products_unit u ON p.unit = u.idProducts_unit JOIN product_category c ON p.category = c.idProduct_category Where Product_categoryname like '%{Filtr1}%' or Product_categoryname like '%{Filtr2}%' Order by {sort};";
            MySqlCommand mySqlCommand = new MySqlCommand(query1, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;

        }

        public bool AddOrder(List<ProductItem> items, int employeid, DateTime lastDate)
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
                    command2 = new MySqlCommand($"INSERT INTO `tea_coffe`.`order_items` (`idOrder_Items`, `product_id`, `quantity` , `unitquantity`) VALUES('{lastId}', '{item.Id}', '{item.BasketQuantity}','{item.BasketQuantity / item.MinUnit}');", connection);
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
                            if (storedHash == hashedPassword)
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

        public static string GenerateSalt(int length)
        {
            // Создаем массив байтов для хранения случайных значений
            byte[] saltBytes = new byte[length];

            // Используем RNGCryptoServiceProvider для заполнения массива случайными байтами
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            // Преобразуем байты в строку в формате Base64
            string salt = Convert.ToBase64String(saltBytes);

            // Обрезаем строку до нужной длины, если она длиннее
            return salt.Substring(0, length);
        }

        public void AddProduct(ProductItem item)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string com = $"INSERT INTO `tea_coffe`.`product` (`name`, `description`, `cost`, `photo`, `quantity`, `unit`, `category`, `cooking_method`, `taste_and_aroma`) VALUES ('{item.Name}', '{item.Description}', '{item.Cost}', '{item.ImageData}', '0', (SELECT idProducts_unit FROM tea_coffe.products_unit where Products_unitname = '{item.Unit}'), (SELECT idProduct_category FROM tea_coffe.product_category where Product_categoryname = '{item.Category}'), '{item.Cooking_method}', '{item.Taste_and_aroma}');";
            MySqlCommand command = new MySqlCommand(com, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void RemoveProduct(string id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string com = $"DELETE FROM `tea_coffe`.`product` WHERE (`idProducts` = '{id}');";
            MySqlCommand command = new MySqlCommand(com, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void ChangeProduct(ProductItem item)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string com = $"UPDATE `tea_coffe`.`product` SET `name` = '{item.Name}', `description` = '{item.Description}', `cost` = '{item.Cost}', `photo` = '{item.ImageData}', `unit` = (SELECT idProducts_unit FROM tea_coffe.products_unit where Products_unitname = '{item.Unit}'), `category` = (SELECT idProduct_category FROM tea_coffe.product_category where Product_categoryname = '{item.Category}'), `cooking_method` = '{item.Cooking_method}', `taste_and_aroma` = '{item.Taste_and_aroma}' WHERE (`idProducts` = '{item.Id}');";
            MySqlCommand command = new MySqlCommand(com, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void ChangeQuantity(ProductItem item)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();
            string com = $"UPDATE `tea_coffe`.`product` SET `quantity` = '{item.QuantityInStock}' WHERE (`idProducts` = '{item.Id}');";
            MySqlCommand command = new MySqlCommand(com, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void CreateUser(User user)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            string salt = GenerateSalt(16);
            string password = HashPassword(user.Password, salt);
            connection.Open();
            string com = $"INSERT INTO `tea_coffe`.`user` (`login`, `password`, `salt`, `surname`, `name`, `patronymic`, `role`) VALUES ('{user.Login}', '{password}', '{salt}', '{user.Surname}', '{user.Name}', '{user.Patronymic}', (SELECT idUser_role from user_role where User_roleName = '{user.Role}'));";
            MySqlCommand command = new MySqlCommand(com, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void ChangeUser(User user)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            string com;
            if (user.Password == "" || user.Password == null)
            {
                com = $"UPDATE `tea_coffe`.`user` SET `login` = '{user.Login}', `surname` = '{user.Surname}', `name` = '{user.Name}', `patronymic` = '{user.Patronymic}', `role` = (SELECT idUser_role from user_role where User_roleName = '{user.Role}') WHERE (`idUser` = '{user.IdUser}');";
            }
            else
            {
                string salt = GenerateSalt(16);
                string password = HashPassword(user.Password, salt);
                com = $"UPDATE `tea_coffe`.`user` SET `login` = '{user.Login}', `password` = '{password}', `salt` = '{salt}', `surname` = '{user.Surname}', `name` = '{user.Name}', `patronymic` = '{user.Patronymic}', `role` = (SELECT idUser_role from user_role where User_roleName = '{user.Role}') WHERE (`idUser` = '{user.IdUser}');";
            }
            connection.Open();
            MySqlCommand command = new MySqlCommand(com, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void DeleteUser(User user)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string com = $"DELETE FROM `tea_coffe`.`user` WHERE (`idUser` = '{user.IdUser}');";
            MySqlCommand command = new MySqlCommand(com, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public double AverageBill(string datestart, string dateend)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string com = $"SELECT AVG(OrderPrice) AS AverageOrderPrice FROM `order` WHERE `date` BETWEEN '{datestart}' AND DATE_ADD('{dateend}', INTERVAL 1 DAY);";
            MySqlCommand mySqlCommand = new MySqlCommand(com, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            string avg = dataTable.Rows[0][0].ToString();

            connection.Close();
            return (Convert.ToDouble(avg));
        }

        public DataTable BillProduct(string datestart, string dateend)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string com = $"SELECT p.*, u.*, c.*, " +
                $"IFNULL(o.total_Position, 0) AS total_Position, " +
                $"IFNULL(o.total_quantity, 0) AS total_quantity " +
                $"FROM tea_coffe.product p " +
                $"LEFT JOIN ( " +
                $"SELECT oi.product_id, " +
                $"SUM(oi.quantity) AS total_Position, " +
                $"SUM(oi.unitquantity) AS total_quantity " +
                $"FROM tea_coffe.order_items oi " +
                $"JOIN tea_coffe.order o ON oi.idOrder_Items = o.OrderProducts " +
                $"WHERE o.date BETWEEN '{datestart}' AND '{dateend}' " +
                $"GROUP BY oi.product_id " +
                $") " +
                $"o ON p.idProducts = o.product_id " +
                $"JOIN products_unit u ON p.unit = u.idProducts_unit " +
                $"JOIN product_category c ON p.category = c.idProduct_category " +
                $"WHERE p.name LIKE '%%' " +
                $"AND total_Position > 0 " +
                $"ORDER BY total_quantity DESC ";
            MySqlCommand mySqlCommand = new MySqlCommand(com, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);

            connection.Close();
            return dataTable;
        }

        public void Backup()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string programPath = Path.GetDirectoryName(exePath);
            string file = Path.Combine(programPath, "Backup", $"backup{DateTime.Now:dd.MM.yyyy HH.mm.ss}.sql");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(file);
                        conn.Close();
                    }
                }
            }
        }
        public void Restore(string file)
        {

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromFile(file);
                        conn.Close();
                    }
                }
            }
        }

        public DataTable GetOrders()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string com = $"SELECT idOrder,OrderProducts,OrderPrice,date,surname,name,patronymic FROM tea_coffe.order Join user on employeid = idUser;";
            MySqlCommand mySqlCommand = new MySqlCommand(com, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);

            connection.Close();
            return dataTable;
        }

        public DataTable GetOrderItem(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string com = $"SELECT idOrder_Items,product_id,order_items.quantity,product.name,photo,Products_unitname FROM tea_coffe.order_items join product on product_id = idProducts join products_unit on unit = idProducts_unit where idOrder_Items = {id}";
            MySqlCommand mySqlCommand = new MySqlCommand(com, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);

            connection.Close();
            return dataTable;
        }

        public int GetUserId(string login)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string com = $"SELECT idUser FROM tea_coffe.user where login = '{login}'";
            MySqlCommand mySqlCommand = new MySqlCommand(com, connection);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);

            connection.Close();
            return Convert.ToInt32(dataTable.Rows[0][0].ToString());
        }
    }

}
