using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubApp1.Models;
using System.Diagnostics;



namespace SubApp1.Controllers;

public class UserController : Controller
{
    private readonly UserDbContext _registerDbContext;

    public UserController(UserDbContext registerDbContext)
    {
        _registerDbContext = registerDbContext;
    }

    [HttpPost]

    public IActionResult Signup(string username, string email, string passord, string passord2)
    {
        if (ModelState.IsValid)
        
        {var newUser=new User{
           Username= username,
           Email= email,
           Passord= passord, 
           Passord2= passord2 
        };
        _registerDbContext.Users.Add(newUser);
        _registerDbContext.SaveChanges();
        return RedirectToAction("Index");
        }
        return View("signup");
    }
     
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}