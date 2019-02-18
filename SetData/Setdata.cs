using Auto_Parts_2019.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace SetData
{
    class Setdata
    {
        private readonly AUTO_PARTS_2019Context local= new AUTO_PARTS_2019Context();
        public ApplicationDbContext db = new ApplicationDbContext();
        
        public void Update()
        {
            Auto_Parts_2019.Models.Parts.Part part = new Auto_Parts_2019.Models.Parts.Part();
            Console.WriteLine("Обновляется Parts");
            foreach (var i in local.Parts)
            {
                part.Analogues = i.Analogues;
                part.Brand = i.Brand;
                part.Description = i.Description;
                part.Foto_link = i.FotoLink;
                part.Group_Auto = i.GroupAuto;
                part.Group_Parts = i.GroupParts;
                part.ID = i.Id;
                part.Number = i.Number;
                part.OneCCreate = i.OneCcreate;
                part.Price = i.Price;
                part.Quantity = i.Quantity;
                Console.WriteLine($"Копируется {i.Number}");
                db.Parts.Add(part);
                db.SaveChanges();
            }
            Auto_Parts_2019.Models.Parts.Cros cros = new Auto_Parts_2019.Models.Parts.Cros();
            Console.WriteLine("Обновляется Cros");
            foreach (var i in local.Cross)
            {
                cros.Brand = i.Brand;
                cros.ID = i.Id;
                cros.Number = i.Number;
                cros.SearchBrand = i.SearchBrand;
                cros.SearchNumber = i.SearchNumber;
                Console.WriteLine($"Копируется {i.Number}");
                db.Cross.Add(cros);
                db.SaveChanges();
            }
            

        }
    }
}
