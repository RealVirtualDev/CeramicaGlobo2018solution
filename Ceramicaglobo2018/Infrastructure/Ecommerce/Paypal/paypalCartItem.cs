using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecommerce.paypal
{

/// <summary>
/// Summary description for paypalCartItem
/// </summary>
public class paypalCartItem
{
    public string name { get; set; }
    public string description { get; set; }
    public string qta { get; set; }
    public string amt { get; set; }
    public Int32 index { get; set; }
}

}

