using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Collections.Generic;
using ApplicationContext;

namespace Robusta
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext.ApplicationContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            var path = config.GetConnectionString("Path");
            if (!File.Exists(path))
            {
                Console.WriteLine("Некорретные данные");

            }
            else
            {
                var result = ImportingData(path);

                using (ApplicationContext.ApplicationContext db = new ApplicationContext.ApplicationContext(options))
                {
                    db.AddRangeAsync(result);
                    db.SaveChanges();
                }
            }
        }

        static List<Info> ImportingData(string path)
        {
            string line;
            int item;
            decimal dem;
            DateTime dateValue;
            List<Info> informations = new List<Info>();
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (DateTime.TryParse(line, out dateValue))
                    {
                        Info info = new Info() { Date = dateValue };

                        line = sr.ReadLine();
                        Int32.TryParse(sr.ReadLine(), out item);
                        info.Hosts = item;

                        line = sr.ReadLine();
                        Int32.TryParse(sr.ReadLine(), out item);
                        info.Registration = item;

                        Int32.TryParse(sr.ReadLine(), out item);
                        info.FDCount = item;

                        line = sr.ReadLine();
                        line = sr.ReadLine();
                        Decimal.TryParse(sr.ReadLine().Substring(1), out dem);
                        info.DepositSum = dem;

                        line = sr.ReadLine();
                        line = sr.ReadLine();
                        line = sr.ReadLine();
                        Decimal.TryParse(sr.ReadLine().Substring(1), out dem);
                        info.Profit = dem;
                        informations.Add(info);
                    }
                }
            }
            return informations;
        }
    }
}
