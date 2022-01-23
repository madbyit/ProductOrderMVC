/*MVC
M: The Models folder contains model class files. 
Typically model class includes public properties, 
which will be used by the application to hold and manipulate application data.
*/

using System;

namespace CentiroHomeAssignment.Models
{
    /* Analyze the CSV files and create a model that can represent 
    the order information contained in the files */
    public class OrdersModel
    {
        public int OrderNumber{ get; set; }

        public int OrderLineNumber { get; set; }

        public string ProductNumber { get; set; }

        public int Quantity { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string ProductGroup { get; set; }

        public string OrderDate { get; set; }

        public string CustomerName { get; set; }

        public int CustomerNumber { get; set; }

    }
}