using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoraPlatform.ViewModel;
using LoraPlatform.Models;

namespace LoraPlatform.Services
{
    public class AdminService
    {
        private readonly LoraService lora = new LoraService();
        //Elder Service
        public void AddElder(Elder NewElder)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            db.Elder.Add(NewElder);
            db.SaveChanges();
            db.Dispose();
        }

        public List<Elder> GetElderList()
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            List<Elder> data = db.Elder.ToList();
            db.Dispose();
            return data;
        }

        public Elder GetElder(string idcard)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            Elder data = new Elder();
            try
            {
                data = db.Elder.Find(idcard);
            } catch
            {
                data = null;
            } finally
            {
                db.Dispose();
            }
            return data;
        }

        public void UpdateElder(Elder elder)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            try
            {
                Elder data = db.Elder.Find(elder.IdCard);
                data.Name = elder.Name;
                data.Birthday = elder.Birthday;
                data.Sex = elder.Sex;
                data.ContactPhone = elder.ContactPhone;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                db.Dispose();
            }
        }

        public void DeleteElder(string idcard)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            try
            {
                Elder data = db.Elder.Find(idcard);
                db.Elder.Remove(data);
            } catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            } finally
            {
                db.SaveChanges();
                db.Dispose();
            }
        }

        //Device Service
        public List<Device> GetDeviceList()
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            ConnectCheck();
            List<Device> datalist = db.Device.ToList();
            db.Dispose();
            return datalist;
        }

        public void ConnectCheck()
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            List<Device> datalist = db.Device.ToList();
            foreach(var item in datalist)
            {
                Device target = db.Device.Find(item.Id);
                if(item.ConnectState != "未連接")
                {
                    if(!lora.ReadData(item.DeviceId))
                    {
                        target.ConnectState = "未連接";
                        db.SaveChanges();
                    }
                    
                }
            }
            db.Dispose();
        }

        public Device GetDevice(int id)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            Device data = db.Device.Find(id);
            db.Dispose();
            return data;
        }

        public void AddDevice(string idcard)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            Device data = new Device();
            Elder wear = GetElder(idcard);
            data.Elder = wear.IdCard;
            data.ConnectState = "未連接";
            data.DeviceId = Guid.NewGuid().ToString();
            db.Device.Add(data);
            db.SaveChanges();
            db.Dispose();
        }

        public void UpdateDevice(int id, string idcard)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            Device data = db.Device.Find(id);
            data.Elder = idcard;
            db.SaveChanges();
            db.Dispose();
        }

        public List<Device> SearchDevice(string idcard)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            List<Device> target = db.Device.Where(model => model.Elder == idcard).ToList();
            db.Dispose();
            return target;
        }

        //Connect Table
        public List<ConnectTable> GetConnectTable(string account)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            List<ConnectTable> data = db.ConnectTable.Where(model => model.Account == account).ToList();
            db.Dispose();
            return data;
        }

        public void AddConnect(string deviceid, string idcard, string account)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            ConnectTable data = new ConnectTable();
            data.DeviceId = deviceid;
            data.Elder = idcard;
            data.Account = account;
            db.ConnectTable.Add(data);
            db.SaveChanges();
            db.Dispose();
        }

        public List<ConnectTable> GetConnectTableList()
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            List<ConnectTable> data = db.ConnectTable.OrderBy(model => model.DeviceId).ToList();
            return data;
        }

        public void DeleteConnect(int id)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            ConnectTable data = db.ConnectTable.Find(id);
            db.ConnectTable.Remove(data);
            db.SaveChanges();
            db.Dispose();
        }

        //Check
        public bool FindDevice(string deviceid)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            Device data = new Device();
            List<Device> datalist = db.Device.ToList();
            try
            {
                foreach(var item in datalist)
                {
                    if(deviceid == item.DeviceId)
                    {
                        data = item;
                    }
                }
            } catch (Exception ex)
            {
                data = null;
                throw new Exception(ex.Message.ToString());
            } finally
            {
                db.Dispose();
            }
            bool result = (data == null);
            return result;
        }

        public bool ElderCheck(string name, string idcard)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            Elder data = new Elder();
            try
            {
                data = db.Elder.Find(idcard);
            } catch (Exception ex)
            {
                data = null;
                throw new Exception(ex.Message.ToString());
            } finally
            {
                db.Dispose();
            }
            if (data != null)
            {
                if (data.Name == name) return true;
                else return false;
            }
            else return false;
        }
    }
}