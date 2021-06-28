using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DTO
{
    public class TankDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxVolume { get; set; }
        public int Volume { get; set; }
    }

}
