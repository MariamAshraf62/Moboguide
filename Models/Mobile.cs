using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace Moboguide.Models
{
    public class Mobile
    {
        [Key]
        public string MobileName { get; set; }
        public string Producer { get; set; }
        public string ImagePath { get; set; }
        public string NetworkTech { get; set; }
        public string ReleaseDate { get; set; }
        public string Dimensions { get; set; }
        public string Weight { get; set; }
        public string materials { get; set; }//Build
        public string SIM { get; set; }
        public string DisplayType { get; set; }
        public string DisplayProtection { get; set; }
        public string DisplayResolution { get; set; }
        public string OS { get; set; }
        public string ChipSet { get; set; }
        public string CPU { get; set; }
        public string GPU { get; set; }
        public bool SDCard { get; set; }//card slot
        public string Storage_Memory { get; set; }
        public string MainCamera { get; set; }
        public string MainCameraVideo { get; set; }
        public string MainCameraFeatures { get; set; }
        public string FrontCamera { get; set; }
        public string FrontCameraVideo { get; set; }
        public string Speakers { get; set; }
        public bool Jack { get; set; }
        public string WLAN { get; set; }
        public string USB { get; set; }
        public string Sensors { get; set; }
        public string BatteryType { get; set; }
        public string BatteryCharging { get; set; }
        public string Colors { get; set; }
        public string Price { get; set; }
        public float AvgRate { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
        public virtual ICollection<MobileUser> MobilesUsers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
