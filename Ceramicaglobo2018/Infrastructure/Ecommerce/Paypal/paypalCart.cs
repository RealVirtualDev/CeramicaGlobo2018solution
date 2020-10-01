using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Text;

namespace ecommerce.paypal
{

/// <summary>
/// Summary description for paypalCart
/// </summary>
public class paypalCart:List<paypalCartItem>
{

    public string getTotalAmt()
    {
        decimal totdec= 0;
            foreach (paypalCartItem i in this){
                totdec += Convert.ToDecimal(i.amt.Replace(".", ",")) *  Convert.ToInt32(i.qta);
            }

            return totdec.ToString().Replace(",", ".");
    }

    public void Add()
    {
        reindex();
    }
    public void Remove()
    {
        reindex();
    }

    void reindex()
    {
        Int32 idx = 0;
        foreach (paypalCartItem i in this)
        {
            i.index = idx;
            idx += 1;
        }
                
           
    }
}

}
