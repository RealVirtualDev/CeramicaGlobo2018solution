using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WebSite.Models;

namespace WebSite.Infrastructure
{
    public static class ResourceExtension
    {
        // Mappa la risorsa raw su un modello interno del progetto web attuale
        public static void map(this iMappableResource res, List<ResourceModelDatas> resdatas)
        {
            res.itemgroup = resdatas.First().itemgroup;

            foreach (ResourceModelDatas dr in resdatas)
            {

                PropertyInfo property = res.GetType().GetProperty(dr.name);
                string txtType = property.PropertyType.Name.ToString().ToLower();

                property.SetValue(res, Convert.ChangeType(dr.val, property.PropertyType), null);

            }
        }

        public static void fill<T>(this List<T> reslist, List<ResourceModelDatas> resdatas) where T:iMappableResource
        {
            int[] listaitemgroup = resdatas.Select(x => x.itemgroup).Distinct().ToArray();
            Type type = typeof(T);

            foreach (int itemgroup in listaitemgroup)
            {
                //List<ResourceModelDatas> resdatastemp = 
                List<ResourceModelDatas> resdatatemp = resdatas.Where(x => x.itemgroup == itemgroup).ToList();
                var obj = (T)Activator.CreateInstance(type);
                obj.map(resdatatemp);
                reslist.Add(obj);
            }

           
        }

        //public static void map(this List<iMappableResource> res,List<ResourceModelDatas> resdatas)
        //{

        //    int[] listaitemgroup = resdatas.Select(x => x.itemgroup).Distinct().ToArray();


        //    foreach (int itemgroup in listaitemgroup)
        //    {
        //        List<ResourceModelDatas> resdatastemp = resdatas.Where(x => x.itemgroup == itemgroup).ToList();

        //    }

        //}


        //void map(List<ResourceModelDatas> resdatas)
        //{
        //    //int ig = resdatas[0].itemgroup;
        //    // itero tutte le righe per l'itemgroup corrente



        //    foreach (ResourceModelDatas dr in resdatas)
        //    {

        //        PropertyInfo property = this.GetType().GetProperty(dr.name);
        //        string txtType = property.PropertyType.Name.ToString().ToLower();

        //        property.SetValue(this, Convert.ChangeType(dr.val, property.PropertyType), null);




        //    }

        //}

    }
}