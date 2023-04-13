using System;
using System.Collections.Generic;

namespace RestaurantAppOOP.models;

public class OrderedDish
{
    public int Id { get; set; }

    public int IdOrder { get; set; }

    public int IdMenu { get; set; }

    public int Number { get; set; }


    public virtual Menu IdMenuNavigation { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;
}