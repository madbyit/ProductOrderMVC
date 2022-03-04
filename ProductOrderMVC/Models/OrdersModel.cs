using System;
using System.Collections.Generic;

namespace ProductOrderWebApp.Models
{
    /* Analyze the CSV files and create a model that can represent 
    the order information contained in the files */
    public class OrdersModel
    {
        public string OrderNumber { get; set; }

        public string CustomerName { get; set; }

        public string CustomerNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}