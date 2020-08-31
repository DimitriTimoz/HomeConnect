#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
using HomeConnect.Class;
using HomeConnect.Models;
using Java.Sql;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Xamarin.Essentials;

using Application = Xamarin.Forms.Application;

namespace HomeConnect.Class
{
    public static class Persistence 
    {
        public const string KEY_ROOMLIST= "roomlist";
        public const string KEY_DEVICESLIST = "deviceslist";
        public const string KEY_SETTINGS = "settings";
        public const string KEY_TEMPS = "temp";
        public static List<DeviceModel> listDevices = new List<DeviceModel>();
        public static List<RoomModel> listRoom = new List<RoomModel>();
        public static List<GetTemps> GetTemps = new List<GetTemps>();
        public static Settings settings = new Settings();

        public static void LoadListTempModels()
        {
            if (Application.Current.Properties.ContainsKey(KEY_TEMPS))
            {
                try 
                { 
                    string json = Application.Current.Properties[KEY_TEMPS].ToString();
                    CallAPITemp();
                    Persistence.GetTemps = JsonConvert.DeserializeObject<List<GetTemps>>(json);
                    
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }

            }
        }

        public static void LoadSettings()
        {
            if (Application.Current.Properties.ContainsKey(KEY_SETTINGS))
            {
                try
                {
                    string json = Application.Current.Properties[KEY_SETTINGS].ToString();
                    settings = JsonConvert.DeserializeObject<Settings>(json);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }

            }
        }
        public static void CallAPITemp()
        {
            if (string.IsNullOrEmpty(Persistence.settings.HubIP) || !Persistence.settings.ISInLocalNetwork)
            {
                return;
            }
            
            DateTime last = (GetTemps.Count > 0) ? DateTime.Parse(GetTemps[GetTemps.Count - 1].date) : new DateTime(5, 5, 5);
                string URL = "http://"+ Persistence.settings.HubIP +"/get_temp.php?date="+ last.ToShortDateString(); //sécuriser si vide
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
                    webClient.DownloadStringAsync(new Uri(URL));

                }
            }catch(Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                var delTemps = JsonConvert.DeserializeObject<List<GetTemps>>(e.Result);
                FusionElements(delTemps);


            }
            catch (Exception exep)
            {
                Console.Error.WriteLine(exep.Message);
            }

            Save(GetTemps, KEY_TEMPS);
        }

        private static void FusionElements(List<GetTemps> delTemps)
        {
     
            DateTime last = (GetTemps.Count > 0) ? DateTime.Parse(GetTemps[GetTemps.Count - 1].date) : new DateTime(5, 5, 5);
            GetTemps.RemoveAll(temp => DateTime.Parse(temp.date) == last);
            List<GetTemps> gets = (List<GetTemps>)delTemps.Where(temp => DateTime.Parse(temp.date) >= last).ToList();
            GetTemps.AddRange(gets);
            
        }

        public static void LoadListDevicesModels()
        {
            if (Application.Current.Properties.ContainsKey(KEY_DEVICESLIST))
            {
                try
                {
                    string json = Application.Current.Properties[KEY_DEVICESLIST].ToString();
                    Persistence.listDevices = JsonConvert.DeserializeObject<List<DeviceModel>>(json);
                    CallAPIDevices();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }

            }
        }

        public static void CallAPIDevices()
        {
            if (string.IsNullOrEmpty(Persistence.settings.HubIP) || !Persistence.settings.ISInLocalNetwork)
            {
                return;
            }

            string URL = "http://" + Persistence.settings.HubIP + "/get_devices.php"; //sécuriser si vide
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.DownloadStringCompleted += WebClient_DownloadStringCompletedDevice;
                    webClient.DownloadStringAsync(new Uri(URL));

                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void WebClient_DownloadStringCompletedDevice(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                Persistence.listDevices = JsonConvert.DeserializeObject<List<DeviceModel>>(e.Result);
                Save(listDevices, KEY_DEVICESLIST);
            }
            catch (Exception)
            {

            }
        }
        public static void CallApiRoom()
        {
            if (string.IsNullOrEmpty(Persistence.settings.HubIP) || !Persistence.settings.ISInLocalNetwork)
            {
                return;
            }
            string URL = "http://" + Persistence.settings.HubIP + "/get_rooms.php";
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.DownloadStringCompleted += WebClient_DownloadStringCompletedRooms;
                    webClient.DownloadStringAsync(new Uri(URL));

                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void WebClient_DownloadStringCompletedRooms(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                Persistence.listRoom = JsonConvert.DeserializeObject<List<RoomModel>>(e.Result);
                Save(listRoom, KEY_ROOMLIST);
            }
            catch (Exception)
            {

            }
            
        }

        public static void LoadListRoomModels()
        {
            if (Application.Current.Properties.ContainsKey(KEY_ROOMLIST))
            {
                try
                {
                    string json = Application.Current.Properties[KEY_ROOMLIST].ToString();
                    Persistence.listRoom = JsonConvert.DeserializeObject<List<RoomModel>>(json);
                    CallApiRoom();
                }catch(Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }
                
            }
           

        }

   
        public static void LoadAll()
        {
            LoadSettings();
            LoadListRoomModels();
            LoadListDevicesModels();
            LoadListTempModels();
        }
       

        public static void Save(object toSave, string KEY)
        {
            try
            {
                string json = JsonConvert.SerializeObject(toSave);
                Application.Current.Properties[KEY] = json;
                Application.Current.SavePropertiesAsync();
            }catch(Exception exp)
            {
                Console.Error.WriteLine(exp.Message);
            }
            
        }

        public static void SaveAll()
        {
            Save(Persistence.settings, KEY_SETTINGS);
            Save(Persistence.GetTemps, KEY_TEMPS);
            Save(Persistence.listDevices, KEY_DEVICESLIST);
            Save(Persistence.listRoom, KEY_ROOMLIST);
        }
        
       
    }
}

#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
