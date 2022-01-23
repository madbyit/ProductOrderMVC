using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CentiroHomeAssignment.Models;
using System.Linq;

/* MVC - The Controller handles user's requests and returns a respons */
namespace CentiroHomeAssignment.Controllers
{
    public class OrdersController : Controller
    {
        public List<OrdersModel> _orderlist = new List<OrdersModel>();

        public ActionResult Index()
        {
            OrderStart();
            ViewData["OrderList"] = _orderlist;

            return View();
        }

        public void OrderStart()
        {
            //Console.WriteLine("Welcome to the orders controller.");
            StreamReadCSV();
            //GetAll();
            //PrintAll();
        }

        public IActionResult GetAll()
        {
            // TODO: Return all orders to a view

            throw new NotImplementedException();
            //return Ok();
        }

        public IActionResult GetByOrderNumber(string orderNumber)
        {
            // TODO: Return the specific order to a view

            throw new NotImplementedException();
        }

        //private OrdersModel HandleOrder()
        //{
        //    /* For now this only handle first row for test*/
        //    OrdersModel om = new OrdersModel()
        //    {
        //        OrderNumber = Convert.ToInt32(listitems.ElementAt(1)),
        //        OrderLineNumber = Convert.ToInt32(listitems.ElementAt(2)),
        //        ProductNumber = listitems.ElementAt(3),
        //        Quantity = Convert.ToInt32(listitems.ElementAt(4)),
        //        Name = listitems.ElementAt(5),
        //        Description = listitems.ElementAt(6),
        //        Price = listitems.ElementAt(7),
        //        ProductGroup = listitems.ElementAt(8),
        //        OrderDate = DateTime.Parse(listitems.ElementAt(9)),
        //        CustomerName = listitems.ElementAt(10),
        //        CustomerNumber = Convert.ToInt32(listitems.ElementAt(11)),
        //    };

        //   return om;
        //}

        //private void PeekOrder(OrdersModel om)
        //{
        //    Console.WriteLine("Ordernumber : " + om.OrderNumber);
        //    Console.WriteLine("OrderLineNumber : " + om.OrderLineNumber);
        //    Console.WriteLine("ProductNumber : " + om.ProductNumber);
        //    Console.WriteLine("Quantity : " + om.Quantity);
        //    Console.WriteLine("Name : " + om.Name);
        //    Console.WriteLine("Description : " + om.Description);
        //    Console.WriteLine("Price : " + om.Price);
        //    Console.WriteLine("ProductGroup : " + om.ProductGroup);
        //    Console.WriteLine("OrderDate : " + om.OrderDate);
        //    Console.WriteLine("Ordernumber : " + om.OrderDate);
        //    Console.WriteLine("CustomerNumber : " + om.CustomerNumber);
        //}

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
            var order = new OrdersModel();
            order.OrderNumber = customerOrderList[0][1];
            order.CustomerName = customerOrderList[0][10];
            order.CustomerNumber = customerOrderList[0][11];
            order.OrderDate = Convert.ToDateTime(customerOrderList[0][9]).Date;
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

        //private void PrintAll()
        //{
        //    foreach (var myvar in orderlist)
        //    {
        //        Console.WriteLine("Total Orderlist: " + myvar);
        //    }

        //    foreach (var myvar in listitems)
        //    {
        //        Console.WriteLine("List items: " + myvar);
        //    }

        //}           
    }
}