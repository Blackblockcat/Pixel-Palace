    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Pixel_Palace.Models;
    using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


    namespace Pixel_Palace.Controllers
    {
        public class UsersController : Controller
        {
            private readonly PixelEntity db;

            public UsersController(PixelEntity context)
            {
                db = context;
            }

            private readonly IHttpContextAccessor? _context;
            [Obsolete]
            private readonly IHostingEnvironment? _host;



         
            public ActionResult Register()
            {
                ViewData["Category_id"] = new SelectList(db.categories, "Id", "Type");

                return View();
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Handle the file upload
                if (user.ClientFile != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        user.ClientFile.CopyTo(stream);
                        user.Photo = stream.ToArray();
                    }
                }

                // Check if the email already exists
                var check = db.users.FirstOrDefault(e => e.Email == user.Email);
                if (check == null)
                {
                    // Hash the password if it's not null or empty
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        user.Password = pass_hash.Hashpassword(user.Password);
                    }

                    // Add the user to the database
                    db.users.Add(user);
                    db.SaveChanges();

                    // Redirect to the home page after successful registration
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Email already exists, return the view with an error message
                    ViewData["error"] = "Email already exists";
                    return View(user);
                }
            }

            // If the model state is not valid, return the view with the validation errors
            return View("Register");
        }


        public ActionResult Login()
            {
                return View();
            }


            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Login(User user, string? returnUrl)
            {

                var obj = db.users.FirstOrDefault(a => a.Email.Equals(user.Email) && a.Password.Equals(pass_hash.Hashpassword(user.Password)));
                if (obj == null)
                {
                    ViewBag.ErrorMessage = "Email or Password Is Not Correct";
                    return View();
                }

                else
                {

                        var sessionId = HttpContext.Session.Id;
                        HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("IsAdmin", obj.AdminRole == "Admin" ? "true" : "false");
                Response.Cookies.Append("SessionId", sessionId, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            Expires = DateTime.UtcNow.AddMinutes(30)
                        });
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                            return RedirectToAction("index", "Home");

                
                }
            }
            public async Task<IActionResult> Profile()
            {
                dynamic viewmodel = new ExpandoObject();

                Response.Headers.Add("Cache-Control", "no-cache,no-store,must-revalidate");
                Response.Headers.Add("Pragma", "no-cache");
                var name = HttpContext.Session.GetString("Email");

                if (String.IsNullOrEmpty(name))
                {

                    var returnUrl = Request.Path.Value;
                    return RedirectToAction("Login", "Users", new { returnUrl });
                }
                var user = await db.users.FirstOrDefaultAsync(m => m.Email == HttpContext.Session.GetString("Email"));

                viewmodel.user = user;

                viewmodel.cat = await db.categories.FirstOrDefaultAsync(m => m.Id == user.Category_id);

                var categories = await db.GetCategoriesAsync();
                viewmodel.Categories = categories;
            
                var games= db.games.Where(p=>p.Category_id == user.Category_id ).ToList();
                viewmodel.favgames = games;

                return View(viewmodel);
            }
         

            public ActionResult Logout()
            {
                Response.Cookies.Delete("SessionId");
                HttpContext.Session.Clear();
                return RedirectToAction("index", "Home");
            }

            // GET: Users/Edit/5
            public async Task<IActionResult> Edit()
            {
                Response.Headers.Add("Cache-Control", "no-cache,no-store,must-revalidate");
                Response.Headers.Add("Pragma", "no-cache");
                var name = HttpContext.Session.GetString("Email");
                var user = await db.users.FirstOrDefaultAsync(m => m.Email == HttpContext.Session.GetString("Email"));

           
                ViewData["Category_id"] = new SelectList(db.categories, "Id", "Type", user.Category_id);
                return View(user);
            }

            // POST: Users/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("Id,User_name,Gender,Region,Category_id,Email,Password,ClientFile")] User user)
            {
                if (id != user.Id)
                {
                    return NotFound();
                }



                if (user.ClientFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    user.ClientFile.CopyTo(stream);
                    user.Photo = stream.ToArray();
                }
         
                string input = user.Password;
                    if (!string.IsNullOrEmpty(input))
                    {
                        user.Password = pass_hash.Hashpassword(input);

                    }
                    try
                    {
                        db.Update(user);
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Profile");
            
         
            }
            public async Task<IActionResult> Delete()
            {
                var user = await db.users.FirstOrDefaultAsync(m => m.Email == HttpContext.Session.GetString("Email"));
                db.users.Remove(user);
                await db.SaveChangesAsync();
                Response.Cookies.Delete("SessionId");
                HttpContext.Session.Clear();
            
                return RedirectToAction("Index","Home");

            }


 

            private bool UserExists(int id)
            {
                return (db.users?.Any(e => e.Id == id)).GetValueOrDefault();
            }

        }
    }