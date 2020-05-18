using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoraPlatform.ViewModel;
using LoraPlatform.Services;
using LoraPlatform.Security;
using System.Web.Configuration;
using System.Text;
using LoraPlatform.Models;

namespace LoraPlatform.Controllers
{
    public class HomeController : Controller
    {
        MailService mailservice = new MailService();
        MemberService memberservice = new MemberService();

        public JsonResult AccountCheck(SignupViewModel member)
        {
            return Json(memberservice.Check(member.NewMember.Account), JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult AccountExist(ForgetPasswordViewModel member)
        {
            return Json(!memberservice.Check(member.Account), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            string ValidateCode = memberservice.GetAuthCode();
            Members account = memberservice.GetAccount(User.Identity.Name);
            memberservice.AddValidateCode(account.Account, ValidateCode);
            string mailbody = mailservice.ChangePasswordMailBody(account.Name, ValidateCode);
            mailservice.MailBuilder(account.Email, mailbody, "LoraPlatform — 更改密碼");
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordViewModel member)
        {
            string Validatestr = "";
            if (ModelState.IsValid)
            {
                Members account = memberservice.GetAccount(User.Identity.Name);
                if(member.ValidateCode == account.AuthCode)
                {
                    if(memberservice.PasswordCheck(account.Password, member.OldPassword))
                    {
                        memberservice.ChangePassword(account.Account, member.NewPassword);
                        return RedirectToAction("LogOut", "Home");
                    } else
                    {
                        Validatestr = "密碼錯誤，請重新輸入";
                    }
                } else
                {
                    Validatestr = "驗證碼錯誤，無法進行帳號更改";
                }
            }
            ModelState.AddModelError("", Validatestr);
            return View(member);
        }

        public ActionResult EmailValidate(string Account, string Authcode)
        {
            ViewData["ValidateMessage"] = memberservice.EmailValidate(Account, Authcode);
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult LogOut()
        {
            string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();

            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Values.Clear();
            Response.Cookies.Set(cookie);
            return RedirectToAction("SignIn");
        }

        public ActionResult RegisterResult()
        {
            return View();
        }

        public ActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Platform");
            }
            else return View();
        }

        [HttpPost]
        public ActionResult SignIn(SigninViewModel member)
        {
            string Validatestr = memberservice.SigninCheck(member.Account, member.Password);
            if (String.IsNullOrEmpty(Validatestr))
            {
                string RoleData = memberservice.GetRole(member.Account);

                JwtService jwtService = new JwtService();
                string CookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
                string Token = jwtService.GenerateToken(member.Account, RoleData);

                HttpCookie cookie = new HttpCookie(CookieName);
                cookie.Value = Server.UrlEncode(Token);

                Response.Cookies.Add(cookie);
                Response.Cookies[CookieName].Expires = DateTime.Now.AddMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["ExpireMinutes"]));

                return RedirectToAction("Index", "Platform");
            }
            else
            {
                ModelState.AddModelError("", Validatestr);
                return View(member);
            }
        }

        public ActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Platform");
            }
            else return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignupViewModel SignMember)
        {
            if (ModelState.IsValid)
            {
                SignMember.NewMember.Password = SignMember.Password;
                string AuthCode = memberservice.GetAuthCode();
                SignMember.NewMember.AuthCode = AuthCode;

                memberservice.Register(SignMember.NewMember);

                UriBuilder UserUrl = new UriBuilder(Request.Url)
                {
                    Path = Url.Action("EmailValidate", "Home",
                    new
                    {
                        account = SignMember.NewMember.Account,
                        authcode = AuthCode
                    })
                };
                string mailbody = mailservice.ValidateMailBody(SignMember.NewMember.Name, UserUrl.ToString().Replace("%3F", "?"));
                mailservice.MailBuilder(SignMember.NewMember.Email, mailbody, "LoraPlatform — 帳號驗證");
                return RedirectToAction("RegisterResult");
            }

            else
            {
                return View(SignMember);
            }
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(ForgetPasswordViewModel member)
        {
            string Validatestr = "";
            if (ModelState.IsValid)
            {
                Members user = memberservice.GetAccount(member.Account);
                if (member.Email == user.Email)
                {
                    string TemporaryPassword = memberservice.GetTemporaryPassword();
                    memberservice.AddTemporaryPassword(member.Account, TemporaryPassword);
                    string mailbody = mailservice.TemporaryPasswordMailBody(user.Name, TemporaryPassword);
                    mailservice.MailBuilder(member.Email, mailbody, "LoraPlatform — 臨時密碼");
                    return RedirectToAction("ValidateTemporaryPassword", "Home");
                }
                else Validatestr = "Emial與註冊時不符";

            }
            ModelState.AddModelError("", Validatestr);
            return View(member);

        }

        public ActionResult ValidateTemporaryPassword()
        {
            return View();
        }


    }
}