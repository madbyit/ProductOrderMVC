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
        public List<OrdersModel> _orderlist = new();
        
        public ActionResult Index()
        {
            GetAll();
            ViewData["OrderList"] = _orderlist;
            return View();
        }

        [HttpPost]
        public IActionResult Start()
        {
            DbProductOrders dbpo = new();
            dbpo.OpenConn();
            dbpo.DbCreateTable();
            dbpo.CsvFileHandler();
            dbpo.CloseConn();
            GetAll();
            return Redirect("localhost:5001/Orders");
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
            
            DbProductOrders dbpo = new();
            dbpo.OpenConn();

            List<string> str_list = new();
            List<List<string>> byOrderNumberList = new();

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
            
            dbpo.CloseConn();
            
            ViewData["OrderList"] = _orderlist;
            return View("Index");
        }

        public void GetAllData()
        {
            DbProductOrders dbpo = new();
            dbpo.OpenConn();

            List<string> str_list;
            List<List<string>> customerOrderList = new();

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
            dbpo.CloseConn();
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

        [HttpPost]
        public IActionResult AddOrder(string addOrderNr, string addCustName, string addOrderDate, 
                                        string addCustNr, string addOrderlineNr, string addProdNr, 
                                        string addQuantity, string addOrderName, string addOrderDesc, 
                                        string addOrderPrice, string addOrderProdGrp)
        {
            List<string> order = new();
            List<List<string>> orderList = new();
        
            order.Add("");
            order.Add(addOrderNr);
            order.Add(addOrderlineNr);
            order.Add(addProdNr);
            order.Add(addQuantity);
            order.Add(addOrderName);
            order.Add(addOrderDesc);
            order.Add(addOrderPrice);
            order.Add(addOrderProdGrp);
            order.Add(addOrderDate);
            order.Add(addCustName);
            order.Add(addCustNr);
            
            bool abort = false;

            foreach(string str in order)
            {
                if (str == null)
                {
                    abort = true;
                    break;
                }
            }

            if (!abort)
            {
                DbProductOrders dbpo = new();
                dbpo.OpenConn();
                orderList.Add(order);
                dbpo.PopulateOrderDB(orderList);
                dbpo.CloseConn();
            }

            ViewData["OrderList"] = _orderlist;
            GetAll();
            return View("Index");
        }           
    }
}