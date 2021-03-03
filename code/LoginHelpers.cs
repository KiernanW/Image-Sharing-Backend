using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
//<!DOCTYPE html><html lang=""><head><meta charset="utf-8"><meta http-equiv="X-UA-Compatible" content="IE=edge"><meta name="viewport" content="width=device-width,initial-scale=1"><link rel="icon" href="/favicon.ico"><title>image-sharing-site</title><link href="/css/app.ad97253f.css" rel="preload" as="style"><link href="/Website/js/app.9892dd45.js" rel="preload" as="script"><link href="/Website/js/chunk-vendors.e9bd47a9.js" rel="preload" as="script"><link href="/Website/css/app.ad97253f.css" rel="stylesheet"></head><body><noscript><strong>We're sorry but image-sharing-site doesn't work properly without JavaScript enabled. Please enable it to continue.</strong></noscript><div id="app"></div><script src="/Website/js/chunk-vendors.e9bd47a9.js"></script><script src="/Website/js/app.9892dd45.js"></script></body></html>
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