using System;
using System.Collections.Generic;

namespace RestaurantAppOOP;

public partial class OrderedDish
{
    private int id;
    private int orderID;
    private int productID;
    private int numMenu;
    public int Id {
        get { return id; }
        set { id = value; }
    }

    public int IdOrder
    {
        get { return orderID; }
        set { orderID = value; }
    }

    public int IdMenu {
        get { return productID; }
        set { productID = value; }
    }

    public int Number {
        get { return numMenu; }
        set { numMenu = value; } 
    }
    

    public virtual Menu IdMenuNavigation { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;
}
