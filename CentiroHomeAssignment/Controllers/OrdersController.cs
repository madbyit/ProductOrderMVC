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
        public List<string> orderlist = new List<string>();
        public List<string> listitems = new List<string>(); 
        
        public OrdersController()
        {
            OrderStart();
        }

        public void OrderStart()
        {
            Console.WriteLine("Welcome to the orders controller.");
            StreamReadCSV();
            //HandleOrder();
            GetAll();
            PrintAll();
        }
       
        public IActionResult GetAll()
        {
            // TODO: Return all orders to a view
            OrdersModel orm = HandleOrder();

            Console.WriteLine(orm.OrderNumber);
            return View(orm); 
        }

        public IActionResult GetByOrderNumber(string orderNumber)
        {
            // TODO: Return the specific order to a view

            throw new NotImplementedException();
        }

        private OrdersModel HandleOrder()
        {
            /* For now this only handle first row*/
            OrdersModel om = new OrdersModel()
            {
                OrderNumber = Convert.ToInt32(listitems.ElementAt(1)),
                OrderLineNumber = Convert.ToInt32(listitems.ElementAt(2)),
                ProductNumber = listitems.ElementAt(3),
                Quantity = Convert.ToInt32(listitems.ElementAt(4)),
                Name = listitems.ElementAt(5),
                Description = listitems.ElementAt(6),
                Price = listitems.ElementAt(7),
                ProductGroup = listitems.ElementAt(8),
                OrderDate = DateTime.Parse(listitems.ElementAt(9)),
                CustomerName = listitems.ElementAt(10),
                CustomerNumber = Convert.ToInt32(listitems.ElementAt(11)),
            };
            
            return om;
        }

        private void StreamReadCSV()
        {
            string filePath = String.Empty;
            filePath = Environment.CurrentDirectory + "/App_Data/";
            string[] AllFiles = Directory.GetFiles(@filePath);

            for (int i = 0; i < AllFiles.Length; i++)
            {
                /*Read the file as one string.*/
                using (StreamReader sr = new StreamReader(AllFiles[i]))
                {
                    /* Jumping first line since it is the header */
                    string headerLine = sr.ReadLine();
                    
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string csvData = line;

                        /* Split by row / ordered item */
                        foreach (string row in csvData.Split('\n'))
                        {
                            /* Split by pipe / order description */
                            foreach (string ord in row.Split('|'))
                            {
                                listitems.Add(ord);
                            }
                            orderlist.Add(row);
                        } 

                    }
                }
            }
        }

        private void PrintAll()
        {
            foreach (var myvar in orderlist)
            {
                Console.WriteLine("Total Orderlist: " + myvar);
            }

            foreach (var myvar in listitems)
            {
                Console.WriteLine("List items: " + myvar);
            }
        }           
    }
}







    // ##### TEST AREA #####

    // Possible split csv solution
    //         //Execute a loop over the rows.
    //         foreach (string row in csvData.Split('\n'))
    //         {
    //             if (!string.IsNullOrEmpty(row))
    //             {
    //                 orderLines.Add(new OrdersModel
    //                 {
    //                     OrderNumber = Convert.ToInt32(row.Split('|')[0]),
    //                     OrderLineNumber = Convert.ToInt32(row.Split('|')[1]),
    //                     ProductNumber = row.Split('|')[2],
    //                     Quantity = Convert.ToInt32(row.Split('|')[3]),
    //                     Name = row.Split('|')[4],
    //                     Description = row.Split('|')[5],
    //                     Price = Convert.ToInt32((row.Split('|')[6])),
    //                     ProductGroup = row.Split('|')[8],
    //                     OrderDate = (row.Split('|')[9]),
    //                     CustomerName = row.Split('|')[10],
    //                     CustomerNumber = Convert.ToInt32((row.Split('|')[11]))
    //                 });
    //             }
    //         }
    //         return View(orderLines);