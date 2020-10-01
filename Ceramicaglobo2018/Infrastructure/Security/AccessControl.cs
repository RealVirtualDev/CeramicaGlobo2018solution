using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using WebSite.Models;

namespace WebSite.Infrastructure.Security
{
    public class AccessControl
    {
        public bool CheckLogin(string username,string password, LoginType loginType)
        {
            DbModel db = new DbModel();
            bool logged = false;

            // in questo progetto uso password in chiaro
            if (loginType == LoginType.admin)
            {
                // AMMINISTRATORE
                string clearpass = db.Administrators.Where(x => x.email == username && x.password==password).Select(x=>x.name).FirstOrDefault();
                if (string.IsNullOrEmpty(clearpass))
                    logged = false;
                else
                    logged = true;
            }
            else
            {
                // UTENTE
                string clearpass = db.Utenti.Where(x => x.email == username && x.password == password).Select(x => x.email).FirstOrDefault();
                if (string.IsNullOrEmpty(clearpass))
                    logged = false;
                else
                    logged = true;
            }

            // PASS CRIPTATA
            //if (loginType == LoginType.admin)
            //{
            //    string encpass = db.Administrators.Where(x => x.email == username).Select(x => x.password).FirstOrDefault();
            //    if (string.IsNullOrEmpty(encpass))
            //        return logged;
            //    logged = testPassword(encpass, password);
            //}
     
            //if (loginType == LoginType.admin) {
            //    logged = db.Administrators.Where(x => x.email == username &&  x.password == calcPass).Count() > 0;
            //}
            
            //if (loginType == LoginType.user) { } // non necessario nel progetto
            return logged;

        }

        public string encryptPassword(string password)
        {
            int saltSize = 16;
            int bytesRequired = 32;
           // byte[] array = new byte[1 + saltSize + bytesRequired];
            int iterations = 1000; // 1000, afaik, which is the min recommended for Rfc2898DeriveBytes
            byte[] salt;
            byte[] key;

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltSize, iterations))
            {
                salt =  pbkdf2.Salt;
                key = pbkdf2.GetBytes(bytesRequired);

                //Buffer.BlockCopy(salt, 0, array, 1, saltSize);
                //byte[] bytes = pbkdf2.GetBytes(bytesRequired);
                //Buffer.BlockCopy(bytes, 0, array, saltSize + 1, bytesRequired);

            }
            // 44 caratteri + 24 per il salt
            string stringsalt = Convert.ToBase64String(salt);
            string stringkey= Convert.ToBase64String(key);
            return stringkey + stringsalt;
        }


        private bool testPassword(string encodedPassword,string clearPassword)
        {
            //int saltSize = 16;
            int bytesRequired = 32;
            
            int iterations = 1000; // 1000, afaik, which is the min recommended for Rfc2898DeriveBytes
            byte[] salt= Convert.FromBase64String(encodedPassword.Substring(44));
            byte[] key = Convert.FromBase64String(encodedPassword.Substring(0,44));

            using (var pbkdf2 = new Rfc2898DeriveBytes(clearPassword,salt,iterations))
            {
              
                byte[] bytes = pbkdf2.GetBytes(bytesRequired);
                if (!bytes.SequenceEqual(key))
                {
                    return false;
                }
                else
                    return true;
                
            }
        
        }

        public enum LoginType
        {
            admin,
            user
        }
    }
}