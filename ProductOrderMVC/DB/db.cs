using System;
using System.IO;
using System.Collections.Generic;
using Npgsql;

namespace ProductOrderWebApp.Database
{
    public class DbProductOrders
    {
        public NpgsqlConnection Connection
        {
            get; set;
        }

        public void OpenConn()
        {
            try
            {   
                var cs = "Server=localhost;Port=5432;Database=productorders;UserId=dbuser;Password=My-$3cr37-P@55w0rD"; // To Github with fake pw
                Connection = new NpgsqlConnection(cs);
                Connection.Open();
                Console.WriteLine("Connection is: " + Connection.State);
            }
            catch (Exception exp)
            {                
                Console.WriteLine("Error open datbase connection: " + exp );
            }
        }
        
        public void CloseConn()
        {
            try
            {   
                Connection.Close();
                Console.WriteLine("Connection is: " + Connection.State);
            }
            catch (Exception exp)
            {                
                Console.WriteLine("Error closing database connection: " + exp );
            }
        }

        public void DbCreateTable()
        {
            Console.WriteLine("Drop and create table...");

            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = Connection;
            cmd.CommandText = "DROP TABLE IF EXISTS orders";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE orders(id SERIAL PRIMARY KEY, 
                                                    ordernumber VARCHAR(255),
                                                    orderlinenumber VARCHAR(255),
                                                    productnumber VARCHAR(255),
                                                    quantity VARCHAR(255),
                                                    name VARCHAR(255),
                                                    description VARCHAR(255),
                                                    price VARCHAR(255),
                                                    productgroup VARCHAR(255),
                                                    orderdate VARCHAR(255),
                                                    customername VARCHAR(255),
                                                    customernumber VARCHAR(255))";
            cmd.ExecuteNonQuery();
        }

        public void CsvFileHandler()
        {
            string filePath = String.Empty;
            filePath = Environment.CurrentDirectory + "/App_Data/";
            string[] AllFiles = Directory.GetFiles(@filePath);

            var customerOrderList = new List<List<string>>();

            for (int i = 0; i < AllFiles.Length; i++)
            {
                /*Read the file as one string.*/
                using StreamReader sr = new StreamReader(AllFiles[i]);

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
            }
            PopulateOrderDB(customerOrderList);
        }

        public void PopulateOrderDB(List<List<string>> customerOrderList)
        {
            Console.WriteLine("Populate order to user interface...");
            var sql = "INSERT INTO orders(ordernumber, orderlinenumber, productnumber, quantity, name, description, price, productgroup, orderdate, customername, customernumber) VALUES(@ordernumber, @orderlinenumber, @productnumber, @quantity, @name, @description, @price, @productgroup, @orderdate, @customername, @customernumber)";
            
            foreach (var list in customerOrderList)
            {
                using var cmd = new NpgsqlCommand(sql, Connection);
            
                cmd.Parameters.AddWithValue("ordernumber", list[1]);
                cmd.Parameters.AddWithValue("orderlinenumber", list[2]);
                cmd.Parameters.AddWithValue("productnumber", list[3]);
                cmd.Parameters.AddWithValue("quantity", list[4]);
                cmd.Parameters.AddWithValue("name", list[5]);
                cmd.Parameters.AddWithValue("description", list[6]);
                cmd.Parameters.AddWithValue("price", list[7]);
                cmd.Parameters.AddWithValue("productgroup", list[8]);
                cmd.Parameters.AddWithValue("orderdate", list[9]);
                cmd.Parameters.AddWithValue("customername", list[10]);
                cmd.Parameters.AddWithValue("customernumber", list[11]);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
    }
}