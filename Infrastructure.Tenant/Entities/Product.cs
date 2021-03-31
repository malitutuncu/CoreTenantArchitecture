using Core.Global.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Tenant.Entities
{
    public class Product : ModifiableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
    }
}
