using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using LoraPlatform.Models;
using LoraPlatform.ViewModel;
using System.Text;

namespace LoraPlatform.Services
{
    public class MemberService
    {

        public void AddValidateCode(string account, string ValidateCode)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            Members data = db.Members.Find(account);
            data.AuthCode = ValidateCode;
            db.SaveChanges();
            db.Dispose();
        }

        public void ChangePassword(string account, string newpassword)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            Members data = db.Members.Find(account);
            string changepassword = HashPassword(newpassword);
            data.Password = changepassword;
            data.AuthCode = "true";
            db.SaveChanges();
            db.Dispose();
        }
        public bool Check(string account)
        {
            Members data = GetAccount(account);
            bool result = (data == null);
            return result;
        }

        public string EmailValidate(string account, string authcode)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            string validatestr = "";
            Members ValidateMember = GetAccount(account);

            if(ValidateMember != null)
            {
                if(ValidateMember.AuthCode == authcode)
                {
                    try
                    {
                        Members data = db.Members.Find(account);
                        data.AuthCode = "true";
                        db.SaveChanges();
                    } catch (Exception ex)
                    {
                        throw new Exception(ex.Message.ToString());
                    } finally
                    {
                        db.Dispose();
                    }
                    validatestr = "信箱驗證成功，歡迎開始體驗LoraPlatform的各項服務！";
                } else {
                    validatestr = "驗證碼錯誤，請重新確認或再註冊一次！";
                }
            }
            else
            {
                validatestr = "傳送資料錯誤，請重新確認或再註冊一次！";
            }
            return validatestr;
        }

        public string HashPassword(string password)
        {
            SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();
            string key = "1z2x3c4v5b6n7m8k9l00a";
            string hashstring = String.Concat(key, password);
            Byte[] hashdata = Encoding.Default.GetBytes(hashstring);
            Byte[] hashcode = hasher.ComputeHash(hashdata);
            string hashpassword = Convert.ToBase64String(hashcode);
            return hashpassword;
        }

        public Members GetAccount(string account)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            Members data = new Members();
            try
            {
                data = db.Members.Find(account);
            } catch(Exception ex)
            {
                data = null;
                throw new Exception(ex.Message.ToString());
            } finally
            {
                db.Dispose();
            }
            return data;
        }
        public string GetAuthCode()
        {
            string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rd = new Random();
            string Code = "";
            for (int i = 0; i < 9; i++)
            {
                Code += str[rd.Next(0, str.Length)];
            }
            return Code;
        }

        public string GetRole(string account)
        {
            string Role = "User";
            Members data = GetAccount(account);

            if(data.isAdmin == true)
            {
                Role += ", Admin";
            }
            return Role;
        }

        public bool PasswordCheck(string ComparePassword, string Password)
        {
            bool result = ComparePassword.Equals(HashPassword(Password));
            return result;
        }

        public void Register(Members member)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            member.Password = HashPassword(member.Password);
            try
            {
                db.Members.Add(member);
                db.SaveChanges();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            } finally
            {
                db.Dispose();
            }
        }

        public string SigninCheck(string account, string password)
        {
            Members data = GetAccount(account);

            if(data != null)
           {
                if (data.AuthCode == "true")
                {
                    if(PasswordCheck(data.Password, password))
                    {
                        return "";
                    }
                    else
                    {
                        return "密碼輸入錯誤";
                    }
                } else
                {
                    return "信箱還未激活，請前往信箱進行Email認證";
                }
            } else
            {
                return "無此會員資料，請先註冊";
            }
        }

        public string GetTemporaryPassword()
        {
            string TemporaryPassword = GetAuthCode();
            return TemporaryPassword;
        }

        public void AddTemporaryPassword(string account, string TemporaryPassword)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            Members data = db.Members.Find(account);
            data.Password = HashPassword(TemporaryPassword);
            db.SaveChanges();
            db.Dispose();
        }

        public void EditMember(string account, string name, string email, string phone)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            try {
                Members data = db.Members.Find(account);
                data.Name = name;
                data.Email = email;
                data.Phonenumber = phone;
                
            } catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            } finally
            {
                db.SaveChanges();
                db.Dispose();
            }
        }

        public List<Members> GetAccountList()
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            List<Members> data = db.Members.ToList();
            db.Dispose();
            return data;
        }
    }
}