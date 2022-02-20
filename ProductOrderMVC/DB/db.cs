using System;
using System.IO;
using System.Collections.Generic;
using Npgsql;

namespace ProductOrderWebApp
{
    /* Analyze the CSV files and create a model that can represent 
    the order information contained in the files */
    public class DbProductOrders
    {
        private NpgsqlConnection connection;

        public DbProductOrders()
        {
            var cs = "Server=localhost;Port=5432;UserId=dbuser;Password=My-$3cr37-P@55w0rD;Database=productorders"; // To Github to fake pw
            
            connection = new NpgsqlConnection(cs);
            OpenConn(connection);
            DbCreateTable(connection);
        }

        private static void OpenConn(NpgsqlConnection connection)
        {
            try
            {   
                connection.Open();  
                Console.WriteLine("Database is : " + connection.FullState);             
            }
            catch (Exception exp)
            {                
                Console.WriteLine("Error open datbase connection: " + exp );
            }
        }

        private void DbCreateTable(NpgsqlConnection connection)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();

            cmd.Connection = connection;

            cmd.CommandText = "DROP TABLE IF EXISTS orders";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"CREATE TABLE orders(id SERIAL PRIMARY KEY, 
                                                    ordernumber VARCHAR(255),
                                                    orderlinenumber INT,
                                                    productnumber VARCHAR(255),
                                                    quantity VARCHAR(255),
                                                    name VARCHAR(255),
                                                    description VARCHAR(255),
                                                    price VARCHAR(255),
                                                    productgroup VARCHAR(255),
                                                    orderdate DATE,
                                                    customername VARCHAR(255),
                                                    customernumber VARCHAR(255))";
            cmd.ExecuteNonQuery();

            Console.WriteLine("Table orders created");
        }

        public void FetchDataFromCSV()
        {
            Console.WriteLine("FETCH DATA");
            string filePath = String.Empty;
            filePath = Environment.CurrentDirectory + "/App_Data/";
            string[] AllFiles = Directory.GetFiles(@filePath);

            for (int i = 0; i < AllFiles.Length; i++)
            {
                /*Read the file as one string.*/
                using StreamReader sr = new StreamReader(AllFiles[i]);
                var customerOrderList = new List<List<string>>();

                /* Jumping first line since it is the header */
                string headerLine = sr.ReadLine();

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string csvData = line;

                    /* Split by row / ordered item */
                    foreach (string row in csvData.Split("\n"))
                    {
                        var rowList = new List<string>();
                        /* Split by pipe / order description */
                        foreach (string word in row.Split('|'))
                        {
                            rowList.Add(word);
                        }

                        customerOrderList.Add(rowList);
                    }
                }
                PopulateOrderDB(customerOrderList);
            }
        }

        private void PopulateOrderDB(List<List<string>> customerOrderList)
        {
            Console.WriteLine("POP!");
            /*
            Prepared statements are faster and guard against SQL injection attacks. 
            The @name and @price are placeholders, which are going to be filled later.
            Values are bound to the placeholders with the AddWithValue method.
            */
            var sql = "INSERT INTO orders(ordernumber, orderlinenumber, productnumber, quantity, name, description, price, productgroup, orderdate, customername, customernumber) VALUES(@ordernumber, @orderlinenumber, @productnumber, @quantity, @name, @description, @price, @productgroup, @orderdate, @customername, @customernumber)";
            using var cmd = new NpgsqlCommand(sql, connection);
            
            
            foreach (var list in customerOrderList)
            {
                cmd.Parameters.AddWithValue("ordernumber", list[1]);
                cmd.Parameters.AddWithValue("orderlinenumber", Convert.ToInt32(list[2]));
                cmd.Parameters.AddWithValue("productnumber", list[3]);
                cmd.Parameters.AddWithValue("quantity", list[4]);
                cmd.Parameters.AddWithValue("name", list[5]);
                cmd.Parameters.AddWithValue("description", list[6]);
                cmd.Parameters.AddWithValue("price", list[7]);
                cmd.Parameters.AddWithValue("productgroup", list[8]);
                cmd.Parameters.AddWithValue("orderdate", Convert.ToDateTime(list[9]).Date);
                cmd.Parameters.AddWithValue("customername", list[10]);
                cmd.Parameters.AddWithValue("customernumber", list[11]);
                
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Row inserted");
                DbPrintTableColumnHeaders(connection);
            }
        } 

        /* Laboration */
        private void DbPrintTableColumnHeaders(NpgsqlConnection connection)
        {
            var sql = "SELECT * FROM orders";

            using var cmd = new NpgsqlCommand(sql, connection);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();


            while (rdr.Read())
            {
                Console.WriteLine($"{rdr.GetName(0),0} {rdr.GetName(1),4} {rdr.GetName(2),13} {rdr.GetName(3),0}");
                Console.WriteLine($"{rdr.GetInt32(0),0} {rdr.GetString(1), 5} {rdr.GetInt32(2),12} {rdr.GetString(3),14}");
                Console.WriteLine();
                Console.WriteLine($"{rdr.GetName(4),0} {rdr.GetName(5),4} {rdr.GetName(6),13} {rdr.GetName(7),0}");
                Console.WriteLine($"{rdr.GetString(4),0} {rdr.GetString(5), 5} {rdr.GetString(6),12} {rdr.GetString(7),14}");
                Console.WriteLine();
                Console.WriteLine($"{rdr.GetName(8),0} {rdr.GetName(9),4} {rdr.GetName(10),13}");
                Console.WriteLine($"{rdr.GetString(8),0} {rdr.GetString(8), 5} {rdr.GetDateTime(9),12} {rdr.GetString(10),14}");
                Console.WriteLine();
            }
        }
        
        private void DbSqlVersion(NpgsqlConnection connection)
        {
            var sql = "SELECT version()";
            using var cmd = new NpgsqlCommand(sql, connection);

            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"PostgreSQL version: {version}");
        }

        private void SqlCommand(string cmd, NpgsqlConnection connection)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            
            command.CommandText = cmd;
            command.ExecuteNonQuery();
        }          
    }
}