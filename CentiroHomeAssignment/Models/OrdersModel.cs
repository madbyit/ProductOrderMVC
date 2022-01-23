/*MVC
M: The Models folder contains model class files. 
Typically model class includes public properties, 
which will be used by the application to hold and manipulate application data.
*/

using System;
using System.Collections.Generic;

namespace CentiroHomeAssignment.Models
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