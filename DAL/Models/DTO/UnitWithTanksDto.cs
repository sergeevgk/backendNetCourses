using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DTO
{
    public class UnitWithTanksDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Tank> Tanks { get; set; }
    }

}
