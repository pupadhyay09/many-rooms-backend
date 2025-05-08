using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Infrastructure.Exceptions;
using ManyRoomStudio.Infrastructure.Helpers;
using ManyRoomStudio.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManyRoomStudio.Controllers
{
    public class LoginController : Controller
    {
        //private readonly IRepository<User> _userRepo;
        //private readonly IRepository<Role> _userRoleRepo;

        private readonly IUserGateway  _userGatway;
        private readonly IRoleGatway _roleGatway;

        public LoginController(IUserGateway userGatway, IRoleGatway roleGatway)
        {
            this._userGatway = userGatway;
            this._roleGatway = roleGatway;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new LoginRequest();
            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> Index(LoginRequest model)
        //{
        //    if (model != null)
        //    {
        //        try
        //        {
        //            var userDetails = await _userGatway.Get(m => m.Email == model.Email).ConfigureAwait(false);

        //            if (userDetails != null)
        //            {
        //                var Password = SecurityHelper.HashPassword(model.Password, userDetails.Salt);

        //                if (Password == userDetails.Password)
        //                {
        //                    var roleDetail = await _roleGatway.GetById(userDetails.RoleID).ConfigureAwait(false);

        //                    if (roleDetail != null)
        //                    {
        //                        if (!string.IsNullOrEmpty(roleDetail?.RoleName))
        //                        {
        //                            var token = TokenGenerator.GenerateToken(userDetails.ID.ToString(), userDetails.Email, roleDetail?.RoleName?.ToString());
        //                            if (!string.IsNullOrEmpty(token))
        //                            {
        //                                HttpContext.Session.SetInt32("UserId", userDetails.ID);
        //                                HttpContext.Session.SetString("token",Convert.ToString(token));
        //                                HttpContext.Session.SetString("UserRole", Convert.ToString(roleDetail.RoleName));
        //                                HttpContext.Session.SetString("UserEmail", Convert.ToString(userDetails.Email));
        //                                HttpContext.Session.SetString("UserName", Convert.ToString(userDetails.FirstName + " " + userDetails.LastName));
        //                            }
        //                            if (roleDetail.RoleName == ERole.SuperAdmin.ToString())
        //                                return RedirectToAction("Index", "Home");
        //                            else if (roleDetail.RoleName == ERole.FranchiseeAdmin.ToString())
        //                                return RedirectToAction("Index", "Franchisee");
        //                            else if (roleDetail.RoleName == ERole.Staff.ToString())
        //                                return RedirectToAction("Index", "Franchisee");
        //                            else if (roleDetail.RoleName == ERole.Manager.ToString())
        //                                return RedirectToAction("Index", "Franchisee");
        //                            else
        //                                return RedirectToAction("Index", "Franchisee");
        //                        }
        //                        else
        //                            TempData["ErrorMessage"] = "Sorry! User role details are not found.";
        //                    }
        //                    else
        //                        TempData["ErrorMessage"] = "Sorry! User role details are not found.";
        //                }
        //                else
        //                    TempData["ErrorMessage"] = "Please enter valid password.";
        //            }
        //            else
        //                TempData["ErrorMessage"] = "Sorry! User details are not found.";
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["ErrorMessage"] = ex.ToString();
        //        }
        //    }

        //    return View(model);
        //}


        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest model)
        {
            if (model == null)
                return View(model);

            try
            {
                var userDetails = await _userGatway.Get(m => m.Email == model.Email && m.IsDelete == false).ConfigureAwait(false);
                if (userDetails == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return View(model);
                }

                var hashedPassword = SecurityHelper.HashPassword(model.Password, userDetails.Salt);
                if (hashedPassword != userDetails.Password)
                {
                    TempData["ErrorMessage"] = "Invalid password.";
                    return View(model);
                }

                var roleDetail = await _roleGatway.GetById(userDetails.RoleID).ConfigureAwait(false);
                if (roleDetail == null || string.IsNullOrEmpty(roleDetail.RoleName))
                {
                    TempData["ErrorMessage"] = "User role not found.";
                    return View(model);
                }

                // Generate token
                var token = TokenGenerator.GenerateToken(userDetails.ID.ToString(), userDetails.Email, roleDetail.RoleName);
                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "Token generation failed.";
                    return View(model);
                }

                // Set session
                HttpContext.Session.SetInt32("UserId", userDetails.ID);
                HttpContext.Session.SetString("token", token);
                HttpContext.Session.SetString("UserRole", roleDetail.RoleName);
                HttpContext.Session.SetString("UserEmail", userDetails.Email);
                HttpContext.Session.SetString("UserName", $"{userDetails.FirstName} {userDetails.LastName}");

                // Redirect by role
                var redirect = roleDetail.RoleName switch
                {
                    nameof(ERole.SuperAdmin) => RedirectToAction(nameof(HomeController.Index), nameof(HomeController).GetControllerName()),
                    nameof(ERole.FranchiseeAdmin) => RedirectToAction(nameof(FranchiseeController.Index), nameof(FranchiseeController).GetControllerName()),
                    nameof(ERole.Staff) => RedirectToAction(nameof(FranchiseeController.Index), nameof(FranchiseeController).GetControllerName()),
                    nameof(ERole.Manager) => RedirectToAction(nameof(FranchiseeController.Index), nameof(FranchiseeController).GetControllerName()),
                    _ => null
                };
                if (redirect == null)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }
                return redirect;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred. " + ex.Message;
                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.SetInt32("UserId", 0);
            HttpContext.Session.SetString("UserRole", "");
            HttpContext.Session.SetString("UserEmail", "");
            HttpContext.Session.SetString("UserName", "");
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
