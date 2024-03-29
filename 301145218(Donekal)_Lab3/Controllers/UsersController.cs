﻿
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using _301145218_Donekal__Lab3.DbData;
using _301145218_Donekal__Lab3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _301145218_Donekal__Lab3.Controllers
{
    
    public class UsersController : Controller
    {
        static Connection conn1 = new Connection();
        AmazonS3Client amazonS3 = conn1.ConnectS3();
        private readonly MovieAppDbContext _context;

        public UsersController(MovieAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ViewResult Signin()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Signin(User userLogin)
        {

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userLogin.Email);
            TempData["UserId"] = user.UserId;
            TempData["UserEmail"] = user.Email;
            if (user == null)
            {
                TempData["LoginError"] = $"{userLogin.Email} does not exist";
                return View(userLogin);

            }
            return RedirectToAction("Index", "Movies");
        }
        [HttpGet]
  
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup([Bind("UserId,Email,Password,ConfirmPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Signin");
            }
            return View(user);
        }
        
    }
}

