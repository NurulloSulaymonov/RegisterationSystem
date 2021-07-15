using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SchoolSystem.Models.ViewModel;

namespace SchoolSystem.Models.Services
{
    public class UserService
    {
        public UserService()
        {
            
        }
        
        
        //register
        public bool Register(RegisterViewModel model)
        {
            //get all list of users
            var listofUsers = GetListOfUsers();
            var userExists = listofUsers.Any(x => x.UserName == model.UserName);
            if (userExists) return false; // if user with uesrname exists just return back
            
            var user = new User()
            {
                Password = model.Password,
                Phone = model.Phone,
                UserName = model.UserName
            };
            if (listofUsers.Count == 0)
            {
                user.Id = 1;
            }
            else
            {
                var last = listofUsers.OrderBy(x => x.Id).Last().Id;
                user.Id = last + 1;
            }

            listofUsers.Add(user);

            var text = JsonConvert.SerializeObject(listofUsers);
            File.WriteAllText("users.json", text);

            return true;
        }
        
        
        //login
        public bool Login(LoginViewModel model)
        {

            var users = GetListOfUsers();

            var success = users.Any(x => x.UserName == model.UserName && x.Password == model.Password);
            return success;
        }
        
        //list
        public List<User> GetListOfUsers()
        {
            var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("users.json"));
            return users;
        }
    }
}