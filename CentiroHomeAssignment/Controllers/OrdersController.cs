using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CentiroHomeAssignment.Models;

//MVC
//C: Controller handles user's requests and returns a respons
namespace CentiroHomeAssignment.Controllers
{
     public class OrdersController : Controller
    {
        public void OrderStart()
        {
            Console.WriteLine("Hello world!");
            ReadCSV();
            GetAll();
        }
       
        public IActionResult GetAll()
        {
            // TODO: Return all orders to a view
             
            throw new NotImplementedException();
        }

        public IActionResult GetByOrderNumber(string orderNumber)
        {
            // TODO: Return the specific order to a view

            throw new NotImplementedException();
        }

        private void ReadCSV()
        {
            List<OrdersModel> orderLines = new List<OrdersModel>();

            string filePath = String.Empty;
            filePath = Environment.CurrentDirectory + "/App_Data/";
            Console.WriteLine("Filepath is: " + filePath);
            string[] AllFiles = Directory.GetFiles(@filePath);
           
            for (int i = 0; i < AllFiles.Length; i++)
            {
                /*Read the contents of CSV / txt file.      
                Read the file as one string.*/
                string csvData = System.IO.File.ReadAllText(@AllFiles[i]);

                List<string> orderlist = new List<string>();
                List<string> listitems = new List<string>();

                /* Split by row */
                foreach (string row in csvData.Split('\n'))
                {
                    /* Split by pipe */
                    foreach (string ord in row.Split('|'))
                    {
                        listitems.Add(ord);
                    }

                    orderlist.Add(row);
                } 

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
}







    // ##### TEST AREA #####


        // OrdersModel ordmod = new OrdersModel()
        // {
        //     OrderNumber = orderNumber,
        //     OrderLineNumber = 0,
        //     ProductNumber = 0,
        //     Quantity = 0,
        //     Name = "",
        //     Description = "",
        //     Price = 0,
        //     ProductGroup = "",
        //     OrderDate = "",
        //     CustomerName = "",
        //     CustomerNumber = 0,
        // };

        // return ordmod;

    //Read each char
    // foreach (char c in csvData)
    // {
    //     if (c == '\n')
    //         Console.WriteLine("NEWLINE!");
    //     else
    //         Console.WriteLine("\t" + c);
    // }

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