using Personal_Finance_Tracker.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Personal_Finance_Tracker.Data
{
    public class Repository
    {
        private static readonly string FILE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");

        public void SaveData(PFTData data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(FILE_PATH, jsonString);
        }

        public PFTData LoadData()
        {
            if (!File.Exists(FILE_PATH))
                return new PFTData();

            try
            {
                string jsonString = File.ReadAllText(FILE_PATH);
                var data = JsonSerializer.Deserialize<PFTData>(jsonString);
                return data ?? new PFTData();
            }
            catch 
            {
                return new PFTData();
            }
        }
    }
}
