/*MVC
M: The Models folder contains model class files. 
Typically model class includes public properties, 
which will be used by the application to hold and manipulate application data.
*/

namespace CentiroHomeAssignment.Models
{
    /* Analyze the CSV files and create a model that can represent 
    the order information contained in the files */
    public class OrderItem
    {
        public int OrderLineNumber { get; set; }

        public string ProductNumber { get; set; }

        public string Quantity { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /*
        Price should be of type double,
        but the parser wouldn't handle a dot
        */
        public string Price { get; set; }

        public string ProductGroup { get; set; }
    }
}