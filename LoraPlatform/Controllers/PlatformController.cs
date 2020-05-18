using LoraPlatform.Models;
using LoraPlatform.Services;
using LoraPlatform.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LoraPlatform.Controllers
{
    public class PlatformController : Controller
    {
        private readonly MemberService memberService = new MemberService();
        private readonly AdminService adminService = new AdminService();
        private readonly LoraService loraService = new LoraService();
        // GET: Platform
        public ActionResult Index()
        {
            Members user = memberService.GetAccount(User.Identity.Name);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else if (user.isAdmin == true)
            {
                return RedirectToAction("AdminIndex", "Platform");
            }
            else
            {
                PlatformIndexViewModel data = new PlatformIndexViewModel();
                data.ConnectDevice = adminService.GetConnectTable(User.Identity.Name);
                data.Wears = adminService.GetElderList();
                return View(data);
            }
        }

        public ActionResult AdminIndex()
        {
            Members user = memberService.GetAccount(User.Identity.Name);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else if (user.isAdmin == false)
            {
                return RedirectToAction("Index", "Platform");
            }
            else
            {
                AdminIndexViewModel data = new AdminIndexViewModel();
                data.ConnectDevice = adminService.GetDeviceList();
                data.Wears = adminService.GetElderList();
                return View(data);
            }
        }

        [HttpPost]
        public ActionResult AdminIndex(AdminIndexViewModel model)
        {
            model.Wears = adminService.GetElderList();
            if (model.Device == "" || model.Device == null)
            {
                model.ConnectDevice = adminService.GetDeviceList();
                return View(model);
            }
            else
            {
                List<Device> SearchDevice = adminService.SearchDevice(model.Device);
                model.ConnectDevice = SearchDevice;
                return View(model);
            }
        }

        public ActionResult MemberCenter()
        {
            Members user = memberService.GetAccount(User.Identity.Name);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else if (user.isAdmin == true)
            {
                return RedirectToAction("AdminMemberCenter", "Platform");
            }
            else
            {
                MemberCenterViewModel data = new MemberCenterViewModel();
                data.ConnectDevice = adminService.GetConnectTable(User.Identity.Name);
                data.Wears = adminService.GetElderList();
                data.Account = memberService.GetAccount(User.Identity.Name);
                return View(data);
            }

        }

        public ActionResult AdminMemberCenter()
        {
            Members user = memberService.GetAccount(User.Identity.Name);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else if (user.isAdmin == false)
            {
                return RedirectToAction("MemberCenter", "Platform");
            }
            else
            {
                ElderManageViewModel data = new ElderManageViewModel();
                data.DataList = adminService.GetElderList();
                return View(data);
            }
        }

        public ActionResult DeviceManage()
        {
            Members user = memberService.GetAccount(User.Identity.Name);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else if (user.isAdmin == false)
            {
                return RedirectToAction("MemberCenter", "Platform");
            }
            else
            {
                DeviceManageViewModel data = new DeviceManageViewModel();
                List<Elder> wearers = new List<Elder>();
                data.DataList = adminService.GetDeviceList();
                foreach(var item in data.DataList)
                {
                    wearers.Add(adminService.GetElder(item.Elder));
                }
                data.Wearers = wearers;
                return View(data);
            }
        }

        public ActionResult ConnectTable()
        {
            Members user = memberService.GetAccount(User.Identity.Name);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else if (user.isAdmin == false)
            {
                return RedirectToAction("MemberCenter", "Platform");
            }
            else
            {
                ConnectTableViewModel data = new ConnectTableViewModel();
                data.connectTables = adminService.GetConnectTableList();
                data.Wears = adminService.GetElderList();
                data.ConnectAccount = memberService.GetAccountList();
                return View(data);
            }
        }

        public ActionResult AreaSetting()
        {
            Members user = memberService.GetAccount(User.Identity.Name);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else if (user.isAdmin == false)
            {
                return RedirectToAction("MemberCenter", "Platform");
            }
            else return View();
        }

        //Elder

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult AddElder(Elder NewElder)
        {
            if (ModelState.IsValid)
            {
                adminService.AddElder(NewElder);
                ModelState.Clear();
                TempData["x"] = NewElder.Birthday.ToString();
                return JavaScript("location.reload(true)");
            }
            else return PartialView("_AddElder", NewElder); 
        }

        [HttpPost]
        public ActionResult EditElderModal(string idcard)
        {
            ElderManageViewModel data = new ElderManageViewModel();
            data.NewElder = adminService.GetElder(idcard);
            data.birthday = data.NewElder.Birthday.ToString("yyyy-MM-dd");
            return PartialView("_EditElder", data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult EditElder(ElderManageViewModel elder)
        {
            if (ModelState.IsValid)
            {
                elder.NewElder.Birthday = Convert.ToDateTime(elder.birthday);
                adminService.UpdateElder(elder.NewElder);
                return JavaScript("location.reload(true)");
            }
            else return PartialView("_EditElder", elder.NewElder);
        }

        [HttpPost]
        public ActionResult DeleteElder(string idcard)
        {
            adminService.DeleteElder(idcard);
            return JavaScript("location.reload(true)");
        }

        //Device

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult AddDevice(DeviceManageViewModel data)
        {
            if (ModelState.IsValid)
            {
                adminService.AddDevice(data.IdCard);
                return JavaScript("location.reload(true)");
            }
            else return PartialView("_AddDevice", data);
        }

        [HttpPost]
        public ActionResult EditDeviceModal(string idcard, int id)
        {
            DeviceManageViewModel data = new DeviceManageViewModel();
            Elder elder = adminService.GetElder(idcard);
            Device device = adminService.GetDevice(id);
            data.device = device;
            data.IdCard = elder.IdCard;
            data.Wearer = elder.Name;
            return PartialView("_EditDevice", data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult EditDevice(DeviceManageViewModel editdevice)
        {
            if (ModelState.IsValid)
            {
                adminService.UpdateDevice(editdevice.device.Id, editdevice.IdCard);
                return JavaScript("location.reload(true)");
            }
            else return PartialView("_EditDevice");
        }

        //Connect Table
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult AddConnect(PlatformIndexViewModel newconnect)
        {
            if (ModelState.IsValid)
            {
                adminService.AddConnect(newconnect.DeviceId, newconnect.IdCard, User.Identity.Name);
                return JavaScript("location.reload(true)");
            }
            else return PartialView("_AddConnect", newconnect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult AddConnectDevice(MemberCenterViewModel newconnect)
        {
            if (ModelState.IsValid)
            {
                adminService.AddConnect(newconnect.DeviceId, newconnect.IdCard, User.Identity.Name);
                return JavaScript("location.reload(true)");
            }
            else return PartialView("_AddConnectDevice", newconnect);
        }

        [HttpPost]
        public void DeleteConnect(int id)
        {
            adminService.DeleteConnect(id);
        }


        //Check
        public JsonResult DeviceCheck(PlatformIndexViewModel NewConnect)
        {
            return Json(!adminService.FindDevice(NewConnect.DeviceId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ElderCheck(PlatformIndexViewModel NewConnect)
        {
            return Json(adminService.ElderCheck(NewConnect.Name, NewConnect.IdCard), JsonRequestBehavior.AllowGet);
        }

        //Edit Member
        public ActionResult EditMember()
        {
            EditMemberViewModel data = new EditMemberViewModel();
            Members member = memberService.GetAccount(User.Identity.Name);
            data.account = member.Account;
            data.name = member.Name;
            data.email = member.Email;
            data.phone = member.Phonenumber;
            return View(data);
        }

        [HttpPost]
        public ActionResult EditMember(EditMemberViewModel member)
        {
            if (ModelState.IsValid)
            {
                memberService.EditMember(member.account, member.name, member.email, member.phone);
                return RedirectToAction("MemberCenter", "Platform");
            }
            else return View(member);
        }

        //Lora data
        [HttpGet]
        public ActionResult GetData()
        {
            LoraData data = loraService.GetLoraData();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetHistory(string DeviceId)
        {
            List<LoraData> data = loraService.GetHistory(DeviceId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}