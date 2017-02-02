using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Device.Location;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RegattaSailorAPI.Models
{
    [DataContract(IsReference = true)]
    public class RaceLegModel
    {
        [Key]
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public double? StartLatitude { get; set; }
        [DataMember]
        public double? StartLongitude { get; set; }
        [DataMember]
        public double? EndLatitude { get; set; }
        [DataMember]
        public double? EndLongitude { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Length { get; set; }
        public virtual RaceModel Race { get; set; }
        public List<LegResultModel> LegResults { get; set; }
    }
}