using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccount.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace BankAccount.Controllers
{
    public class HomeController : Controller
    {
        private bankContext dbContext;
        public HomeController(bankContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("makeUser")]
        public IActionResult createUser(User newUser)
        {
            if(ModelState.IsValid)
            {              
                if ( dbContext.users.Any(u => u.email == newUser.email)){
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.password = Hasher.HashPassword(newUser, newUser.password);
                System.Console.WriteLine(newUser.password.ToString());
                System.Console.WriteLine("--------------------------------------------------------------------------");
                dbContext.users.Add(newUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("id", newUser.userid);
                System.Console.WriteLine(newUser.userid);
                return RedirectToAction("Account");
                System.Console.WriteLine("*******************************************************************");
            }
            else{
                return View("Index", newUser);
            }
                 
                

        }
        [HttpGet("login")]
        public IActionResult Login(){
            return View("Login");
        }
        [HttpPost("loginaction")]
        public IActionResult LoginAction(LoginUser userSubmission)
        {            
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.users.FirstOrDefault(u => u.email == userSubmission.email);
                if(userInDb == null)
                {             
                    return View("Login");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.password, userSubmission.password);
                if(result == 0)
                {
                    return View("Login");
                }
            System.Console.WriteLine("*******************************************************************");
            System.Console.WriteLine("*******************************************************************");
            System.Console.WriteLine(userInDb.userid);
            System.Console.WriteLine("*******************************************************************");
            System.Console.WriteLine("*******************************************************************");
            System.Console.WriteLine("*******************************************************************");
                HttpContext.Session.SetInt32("id", userInDb.userid);
                return RedirectToAction("Account");
            } 
            return View("Login"); 
            
        }
        [HttpGet("account")]
        public IActionResult Account(){
            if(HttpContext.Session.GetInt32("id") == null){
                return RedirectToAction("Login");
                
            }
            int? userid = HttpContext.Session.GetInt32("id");
            User user = dbContext.users.Include(p=>p.Transactions).SingleOrDefault(u => u.userid == userid);
            System.Console.WriteLine(user.Sum.ToString());
            List<Transaction> usersTransaction = dbContext.transactions.Where(t=>t.users_id == userid).OrderByDescending(t=>t.created_at).ToList();
            ViewBag.sum = user.Sum;
            ViewBag.trans = usersTransaction;
            System.Console.WriteLine(userid.ToString());
            System.Console.WriteLine("*******************************************************************");
            // System.Console.WriteLine(user.ToString());
            return View(user);
        }
        [HttpPost("processmoney")]
          public IActionResult processmoney(int Amount){
            if(HttpContext.Session.GetInt32("id") == null){
                return RedirectToAction("Login");
                
            }
            User user = dbContext.users.Include(p=>p.Transactions).SingleOrDefault(u => u.userid == HttpContext.Session.GetInt32("id"));

            if (user.Sum + (decimal)Amount > 0){
                Transaction trans = new Transaction{amount = Amount, users_id = (int)HttpContext.Session.GetInt32("id") };
                dbContext.Add(trans);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Account");
        }
        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        
    }
}
