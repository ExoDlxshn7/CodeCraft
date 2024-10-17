using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SubApp1.Models;

namespace SubApp1.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserDbContext _userDbcontext;
        public ProfileController(UserDbContext userDbContext){
              _userDbcontext = userDbContext;
        }

        public IActionResult Index(){
            var friends = _userDbcontext.Friends.ToList();
            return View(friends);
        }

        [HttpPost]
        public IActionResult Edit(int id, User user){
            return View();
        }

        public IActionResult Friends(){
            var friends = _userDbcontext.Friends.ToList();
            return View("~/Views/Home/Friends.cshtml",friends);
        }

        public IActionResult AddFriend(Friend friend){
            _userDbcontext.Friends.Add(friend);
            _userDbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFriend(int id){
            var friend = _userDbcontext.Friends.Find(id);
            if(friend != null){
                _userDbcontext.Friends.Remove(friend);
                _userDbcontext.SaveChanges();
            }
            return RedirectToAction("Index");
        }



           public IActionResult Logout(){
            return RedirectToAction("Index", "Login");
           }
    }
}