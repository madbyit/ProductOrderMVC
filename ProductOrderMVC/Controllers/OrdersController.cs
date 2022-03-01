using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductOrderWebApp.Models;
using ProductOrderWebApp.Database;
using Npgsql;

/* MVC - The Controller handles user's requests and returns a respons */
namespace ProductOrderWebApp.Controllers
{
    public class OrdersController : Controller
    {
        private DbProductOrders dbpo;
        public List<OrdersModel> _orderlist = new List<OrdersModel>();
        
        public ActionResult Index()
        {
            GetAll();
            ViewData["OrderList"] = _orderlist;
            return View();
        }

       [HttpPost]
        public IActionResult GetAll()
        {
            /// Return all orders to a view
            GetAllData();
            
            ViewData["OrderList"] = _orderlist;
            return View("Index");
        }

        [HttpPost]
        public IActionResult GetByOrderNumber(OrdersModel ord)
        {
            string orderNumber = ord.GetOrderByNr;

            List<OrdersModel> _byordernumber = new List<OrdersModel>();
            _byordernumber.Clear();

        // Return the specific order to a view
            StreamReadCSV();

            foreach (var item in _orderlist)
            {
                if (item.OrderNumber == orderNumber)
                {
                    _byordernumber.Add(item);
                    break;
                }
            }
            
            if (_byordernumber.Count > 0)
                ViewData["OrderList"] = _byordernumber;
            else
                ViewData["OrderList"] = _orderlist;

            return View("Index");
        }

        private void GetAllData()
        {
            dbpo = new DbProductOrders();

            List<string> str_list;
            List<List<string>> customerOrderList = new List<List<string>>();

            var sql = "SELECT * FROM orders";
            using var cmd = new NpgsqlCommand(sql, dbpo.Connection);
            using NpgsqlDataReader nrd = cmd.ExecuteReader();

            while(nrd.Read())
            {
                str_list = new List<string>();
                for (int str_index = 1; str_index <= 11; str_index++)
                {
                   str_list.Add(nrd.GetString(str_index));
                }
                customerOrderList.Add(str_list);
            }
            PublishOrderList(customerOrderList);
        }

        public void StreamReadCSV()
        {
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
                PopulateOrderList(customerOrderList);
            }
        }

        private void PopulateOrderList(List<List<string>> customerOrderList)
        {
            /* 
             * The first [0] is the index 0 in the parent-list.
             * This task included 3 files with different orders.
             * 
             * I want to create the orders seperated from each file.
             * The parent list is the three files, and in each of them
             * was the list with orders.
            */
            var order = new OrdersModel
            {
                OrderNumber = customerOrderList[0][1],
                OrderDate = Convert.ToDateTime(customerOrderList[0][9]).Date,
                CustomerName = customerOrderList[0][10],
                CustomerNumber = customerOrderList[0][11],
                OrderItems = new List<OrderItem>()
            };

            foreach (var list in customerOrderList)
            {
                order.OrderItems.Add(new OrderItem
                {
                    OrderLineNumber = Convert.ToInt32(list[2]),
                    ProductNumber = list[3],
                    Quantity = list[4],
                    Name = list[5],
                    Description = list[6],
                    Price = list[7],
                    ProductGroup = list[8],
                });
            }

            _orderlist.Add(order);
        }

        private void PublishOrderList(List<List<string>> customerOrderList)
        {
            
           OrdersModel order;

            foreach (var list in customerOrderList)
            {
                order = new OrdersModel
                {
                    OrderNumber = list[0],
                    OrderDate = Convert.ToDateTime(list[8]).Date,
                    CustomerName = list[9],
                    CustomerNumber = list[10],
                    OrderItems = new List<OrderItem>()
                 };
                
                order.OrderItems.Add(new OrderItem
                {
                    OrderLineNumber = Convert.ToInt32(list[1]),
                    ProductNumber = list[2],
                    Quantity = list[3],
                    Name = list[4],
                    Description = list[5],
                    Price = list[6],
                    ProductGroup = list[7],
                });
                _orderlist.Add(order);
            }        
            
        }           
    }
}