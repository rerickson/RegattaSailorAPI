using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RegattaSailorAPI.Models
{
    [DataContract(IsReference = true)]
    public class YachtModel
    {
        [Key]
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string MakeModel { get; set; }
        [DataMember]
        public int LengthFeet { get; set; }
        [DataMember]
        public string SailNumber { get; set; }
        [DataMember]
        public string YachtClub { get; set; }
        [DataMember]
        public int PhrfRating { get; set; }
        [DataMember]
        public string SkipperName { get; set; }
        public virtual List<DivisionModel> Divisions { get; set; }

    }
}