using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebSite.Infrastructure
{
    public class Validator
    {
        public static bool checkDate(string val)
        {
            try
            {
                DateTime d = Convert.ToDateTime(val);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool checkCodiceFiscale(string val)
        {
            bool result = false;
            const int caratteri = 16;

            if (string.IsNullOrEmpty(val) || val.Length<caratteri)
                return result;

            const string omocodici = "LMNPQRSTUV";
            const string listaControllo = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int[] listaPari = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
            int[] listaDispari = { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23 };
            string CodiceFiscale = val.ToUpper();
            char[] cCodice = CodiceFiscale.ToCharArray();

            
            int somma = 0;
            for(int i = 0; i <= 14; i++)
            {
                char c = cCodice[i];
                int x = "0123456789".IndexOf(c);
                if (!(x == -1))
                    c = listaControllo.Substring(x, 1).ToCharArray()[0];

                x = listaControllo.IndexOf(c);
                if ((i % 2) == 0)
                    x = listaDispari[x];
                else
                    x = listaPari[x];

                somma += x;

            }

            result = (listaControllo.Substring((somma % 26), 1) == CodiceFiscale.Substring(15, 1));

            return result;
            
        }

        public static bool checkPartitaIva(string val)
        {
            bool result = false;
            const int caratteri = 11;

            //Const codfisc As Integer = 16
            string partitaiva = val;
            Regex pregex = new Regex("^\\d{" + caratteri.ToString() + "}$");
            if (string.IsNullOrEmpty(val))
                return result;
            if (partitaiva.Length != caratteri)
            {
                if (partitaiva.Length < caratteri)
                    partitaiva.PadLeft(caratteri, '0');
                else
                    partitaiva = partitaiva.Substring(2);
            }
            Match m = pregex.Match(partitaiva);
            result = m.Success;
            if (result)
                result = ((!(int.Parse(partitaiva.Substring(0, 7)) == 0)) && (int.Parse(partitaiva.Substring(7, 3)) >= 0) && (int.Parse(partitaiva.Substring(7, 3)) < 201));

            if (result)
            {
                int somma = 0;
                for(int i = 0; i <= caratteri - 2; i++)
                {
                    int j = int.Parse(partitaiva.Substring(i, 1));
                    if ((i + 1) % 2 == 0)
                    {
                        j *= 2;
                        char[] c = j.ToString("00").ToCharArray();
                        somma += int.Parse(c[0].ToString());
                        somma += int.Parse(c[1].ToString());
                    }
                    else
                    {
                        somma += j;
                    }
                }
                if((somma.ToString("00").Substring(1, 1) == "0") && (!(partitaiva.Substring(10, 1) == "0")))
                    result = false;

                somma = int.Parse(partitaiva.Substring(10, 1)) + int.Parse(somma.ToString("00").Substring(1, 1));
                if (result)
                    result = (somma.ToString("00").Substring(1, 1) == "0");
            

            }

            return result;

        }

        public static bool checkMail(string val)
        {

            Regex reg = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            Match m = reg.Match(val.ToLower());
            return m.Success;
        
        }

        public static bool checkTelefono(string val)
        {
            char[] arr = val.Trim().ToCharArray();
            for(int i = 0; i < arr.Length; i++)
            {
                if(!arr[i].ToString().IsNumeric())
                {
                    if(arr[i]!='+' & arr[i]!=' ')
                    {
                        return false;
                    }
                    else
                    {
                        if (i > 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }

}