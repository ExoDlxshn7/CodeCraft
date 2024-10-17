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
    // Sjekk om passordene matcher
    if (passord != passord2)
    {
        ModelState.AddModelError("PassordMismatch", "Passwords do not match.");
        return View("signup");
    }

    // Sjekk om brukeren allerede eksisterer (brukernavn eller e-post)
    var existingUser = _registerDbContext.Users
        .FirstOrDefault(u => u.Email == email || u.Username == username);

    if (existingUser != null)
    {
        ModelState.AddModelError("UserExists", "A user with this email or username already exists.");
        return View("signup");
    }

    // Hvis modellen er gyldig, opprett ny bruker
    if (ModelState.IsValid)
    {
        var newUser = new User
        {
            Username = username,
            Email = email,
            Passord = passord,  // Merk: Du bør vurdere å hashe passordet før du lagrer det
            Passord2 = passord2
        };

        // Legg til brukeren i databasen
        _registerDbContext.Users.Add(newUser);
        _registerDbContext.SaveChanges();

        // Omdiriger til en velkomstside eller innlogging
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