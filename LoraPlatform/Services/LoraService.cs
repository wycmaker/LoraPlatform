using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;
using LoraPlatform.Models;

namespace LoraPlatform.Services
{
    public class LoraService
    {
        public static SerialPort serialPort1 = new SerialPort()
        {
            PortName = "COM3",
            BaudRate = 115200,
            Parity = Parity.None,
            DataBits = 8,
            StopBits = StopBits.One,
            ReadTimeout = 10000
        };

        public bool ConnectCheck()
        {
            string[] ComPorts = SerialPort.GetPortNames();
            bool result = false;
            for (int i=0; i<ComPorts.Length; i++)
            {
                if (ComPorts[i] == "COM3")
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    result = true;
                }
                else result = false;
            }
            return result;
            
        }

        public LoraData GetLoraData()
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            LoraData data = new LoraData();
            if (ConnectCheck())
            {
                string DataString = serialPort1.ReadLine();
                if (DataString == "NO_GPS\r") data = null;
                else
                {
                    List<string> DataList = DataString.Split(',').ToList();
                    Device device = db.Device.Find(Convert.ToInt32(DataList[3]));
                    data.DeviceId = device.DeviceId;
                    data.Latitude = DataList[0];
                    data.Longitude = DataList[1];
                    data.Time = DateTime.Now;
                }
                db.Dispose();
            }
            return data;
        }

        public bool ReadData(string deviceid)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            if (ConnectCheck())
            {
                string DataString = serialPort1.ReadLine();
                if (DataString == "NO_GPS\r") return false;
                else
                {
                    List<string> DataList = DataString.Split(',').ToList();
                    Device device = db.Device.Find(Convert.ToInt32(DataList[3]));
                    if (device.DeviceId == deviceid) return true;
                    else return false;
                }
            }
            else return false;
        }

        public void SaveLoraData()
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            if(ConnectCheck())
            {
                string DataString = serialPort1.ReadLine();
                if (DataString != "NO_GPS\r")
                {
                    LoraData data = new LoraData();
                    List<string> DataList = DataString.Split(',').ToList();
                    Device device = db.Device.Find(Convert.ToInt32(DataList[3]));
                    device.ConnectState = "已連接";
                    device.ConnectTime = DateTime.Now;
                    data.DeviceId = device.DeviceId;
                    data.Latitude = DataList[0];
                    data.Longitude = DataList[1];
                    data.Time = DateTime.Now;
                    db.LoraData.Add(data);
                    db.SaveChanges();
                    db.Dispose();
                }
            }
        }

        public List<LoraData> GetHistory(string deviceid)
        {
            LoraPlatformEntities db = new LoraPlatformEntities();
            List<LoraData> data = db.LoraData.Where(model => model.DeviceId == deviceid).Take(30).ToList();
            return data;
        }
    }
}