using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CentiroHomeAssignment.Models;

/* MVC - The Controller handles user's requests and returns a respons */
namespace CentiroHomeAssignment.Controllers
{
    public class OrdersController : Controller
    {
        public List<OrdersModel> _orderlist = new List<OrdersModel>();

        public ActionResult Index()
        {
            GetAll();
            ViewData["OrderList"] = _orderlist;

            return View();
        }

        
        public IActionResult GetAll()
        {
            // TODO: Return all orders to a view
            StreamReadCSV();
            return Ok();
        }

        public IActionResult GetByOrderNumber(string orderNumber)
        {
            // TODO: Return the specific order to a view

            throw new NotImplementedException();
        }

        public void StreamReadCSV()
        {
            string filePath = String.Empty;
            filePath = Environment.CurrentDirectory + "/App_Data/";
            string[] AllFiles = Directory.GetFiles(@filePath);

            for (int i = 0; i < AllFiles.Length; i++)
            {
                /*Read the file as one string.*/
                using (StreamReader sr = new StreamReader(AllFiles[i]))
                {
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
        }

        private void PopulateOrderList(List<List<string>> customerOrderList)
        {
            /* Repeated often */
            var order = new OrdersModel();
            order.OrderNumber = customerOrderList[0][1];
            order.OrderDate = Convert.ToDateTime(customerOrderList[0][9]).Date;
            order.CustomerName = customerOrderList[0][10];
            order.CustomerNumber = customerOrderList[0][11];
            order.OrderItems = new List<OrderItem>();

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
    }
}