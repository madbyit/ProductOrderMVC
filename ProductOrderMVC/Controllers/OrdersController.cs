using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductOrderWebApp.Models;
using ProductOrderWebApp.Database;
using Npgsql;

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
        public IActionResult GetByOrderNumber(string orderNumber)
        {
            /// Return specific order to a view
            
            dbpo = new DbProductOrders();

            List<string> str_list = new List<string>();
            List<List<string>> byOrderNumberList = new List<List<string>>();

            var sql = "SELECT * FROM orders WHERE ordernumber='"+orderNumber+"'";
            using var cmd = new NpgsqlCommand(sql, dbpo.Connection);
            using NpgsqlDataReader nrd = cmd.ExecuteReader();

            while (nrd.Read())
            {
                str_list = new List<string>(); 
                for (int str_index = 1; str_index <= 11; str_index++)
                {
                   str_list.Add(nrd.GetString(str_index));
                }
                byOrderNumberList.Add(str_list);
            }
            PublishOrderList(byOrderNumberList);
            
            ViewData["OrderList"] = _orderlist;
            return View("Index");
        }

        public void GetAllData()
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