using Microsoft.AspNetCore.Mvc;
using SubApp1.Models;
using System.Diagnostics;



namespace SubApp1.Controllers;

public class RegisterController : Controller
{
    private readonly RegisterDbContext _registerDbContext;

    public RegisterController(RegisterDbContext registerDbContext)
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