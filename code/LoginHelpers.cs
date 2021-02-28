using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace imagesharing
{
    public static class LoginHelper
    {
        const string DBNAME = @"C:\home\site\wwwroot\baddb.json";
        private static List<User> LoadDB(){
            return JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(DBNAME));
        }
        
        public static void SetCookie(HttpRequest req)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(5);
            // Make the cookie available for the browser
            option.SameSite = SameSiteMode.None;
            option.Secure = true;
            option.Domain = "https://imagesharing.azurewebsites.net";
            // A little non logical way to actually get the HttpResponse (from the HttpRequest and its HttpContext)
            req.HttpContext.Response.Cookies.Append("MyCookie", "MyValue", option);
        }

        public static bool RegisterAccount(string username, string password)
        {
            List<User> db = LoadDB();
            // if a user with this name already exists, then the register fails
            for(int i = 0; i < db.Count; i++){
                if(db[i].username == username){
                    return false;
                }
            }
            db.Add(new User(username,password));
            using (FileStream jsonfile = new FileStream(DBNAME, FileMode.Open, FileAccess.ReadWrite))
                using (StreamWriter sw = new StreamWriter(jsonfile))
                    sw.Write(JsonConvert.SerializeObject(db));
            return true;
        }

        public static bool Login(string username, string password){
            List<User> db = LoadDB();
            // if a user with this name already exists, then the register fails
            for(int i = 0; i < db.Count; i++){
                if(db[i].username == username && db[i].password == password){
                    return true;
                }
            }
            return false;
        }
    }

    public class User{
        public string username;
        public string password;

        public User(string username, string password){
            this.username = username;
            this.password = password;
        }
    }

}