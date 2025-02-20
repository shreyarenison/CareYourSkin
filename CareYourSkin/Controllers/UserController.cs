using CareYourSkin.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareYourSkin.Controllers
{
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly SkinCareManagementContext Context;

        public UserController(SkinCareManagementContext context , IWebHostEnvironment webHostEnvironment)
        {
            Context = context;
            this.webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user, IFormFile? imageFile)
        {

            if (ModelState.IsValid)
            {
                // Handle image upload
                if (imageFile != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);

                    Directory.CreateDirectory(uploadFolder); // Ensure the folder exists
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    user.ImagePath = "Images/" + fileName;

                }
                else
                {
                    imageFile = null;
                }


                if (Context.AppUser.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("", "Username already exists");
                    return View(user);
                }
                user.Role = "User";
                Context.AppUser.Add(user);
                Context.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();
        }

        // GET: Login Page
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = Context.AppUser.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Username or Password");
                return View();
            }

            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserId", user.Id.ToString());

            if (user.Role == "Expert")
            {
                // Default mode for expert is "Expert"
                HttpContext.Session.SetString("CurrentMode", "Expert");
                return RedirectToAction("IsExpert", "Home");
            }
            else if (user.Role == "User")
            {
                return RedirectToAction("IsUser", "Home");
            }

            return RedirectToAction("Home", "Home");
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult SwitchMode()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == "Expert")
            {
                var currentMode = HttpContext.Session.GetString("CurrentMode");
                if (currentMode == "Expert")
                {
                    HttpContext.Session.SetString("CurrentMode", "User");
                    return RedirectToAction("IsUser", "Home");
                }
                else
                {
                    HttpContext.Session.SetString("CurrentMode", "Expert");
                    return RedirectToAction("IsExpert", "Home");
                }
            }

            return RedirectToAction("IsUser", "Home"); 
        }

        public IActionResult Profile(int id)
        {
            var image = Context.AppUser.Find(id);
            if (image != null && !string.IsNullOrEmpty(image.ImagePath))
            {
                var imagePath = Path.Combine(webHostEnvironment.WebRootPath, image.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    var imageFileStream = System.IO.File.OpenRead(imagePath);
                }

            }

            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var user = Context.AppUser.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View(user);
        }
        public IActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var user = Context.AppUser.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View(user); 
        }

        [HttpPost]
        public IActionResult EditProfile(User updatedUser, IFormFile? imageFile)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var user = Context.AppUser.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                // Update user details
                user.Username = updatedUser.Username;
                user.Name = updatedUser.Name;
                user.Password = updatedUser.Password; // In a real app, hash the password
                user.ConfirmPassword = updatedUser.ConfirmPassword;
                user.Age = updatedUser.Age;
                user.DateOfBirth = updatedUser.DateOfBirth;

                if (imageFile != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    Directory.CreateDirectory(uploadFolder);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);

                    // Save the new image
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }

                    // Update ImagePath
                    user.ImagePath = Path.Combine("Images", fileName);
                }
                Context.SaveChanges(); // Save changes to the database

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Profile"); // Redirect back to the Profile page
            }

            return View(updatedUser); // Return the form with validation errors
        }



    }

}
