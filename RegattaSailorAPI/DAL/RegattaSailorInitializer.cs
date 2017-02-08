using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using RegattaSailorAPI.Models;

namespace RegattaSailorAPI.DAL
{
    public class RegattaSailorInitializer : System.Data.Entity.DropCreateDatabaseAlways<RegattaSailorContext>
    {
        protected override void Seed(RegattaSailorContext context)
        {
            base.Seed(context);

            var Yacht1 = new YachtModel { Id = Guid.Parse("b776b93a-51d9-40a3-93de-a8f61a2ec93a"), LengthFeet = 27, MakeModel = "Ericson 27", Name = "Anacreon", PhrfRating = 225, SailNumber = "1122", SkipperName = "Justin Broadbent", YachtClub = "WSCYC" };
            var Yacht2 = new YachtModel { Id = Guid.Parse("15863319-4a5d-4f03-b050-21249809ea96"), LengthFeet = 30, MakeModel = "Ranger 30", Name = "Jabez", PhrfRating = 192, SailNumber = "12724", SkipperName = "Gary Seibert", YachtClub = "WSCYC" };
            var Yacht3 = new YachtModel { Id = Guid.Parse("14424AE5-F9A5-455F-9E33-24C388B511A7"), LengthFeet = 30, MakeModel = "Catalina 30", Name = "Anica", PhrfRating = 225, SailNumber = "167", SkipperName = "Rick Kerby", YachtClub = "WSCYC" };

            var yachts = new List<YachtModel>
            {
                Yacht1, Yacht2, Yacht3
            };

            yachts.ForEach(s => context.Yachts.Add(s));
            context.SaveChanges();

            var races = new List<RaceModel>
            {
                new RaceModel {Id = Guid.Parse("ba4626bb-1ade-4dff-a7e0-214aa0629029"), Name="WSCYC Rich Passage Ramble", StartTime=(new DateTime(2017,2,1,10,00,00))  }
            };

            races.ForEach(s => context.Races.Add(s));
            context.SaveChanges();

            var divisions = new List<DivisionModel>
            {
                new DivisionModel { Id = Guid.Parse("902862f6-05b5-451a-970a-3bec7f64a160"), Index = 1, Name = "Division 1", StartTime = (new DateTime(2017, 2, 1, 10, 00, 00)), Yachts = new List<YachtModel> { yachts[0], yachts[1] }, RaceId=races[0].Id }
            };

            divisions.ForEach(s => context.Divisions.Add(s));
            context.SaveChanges();

            var legs = new List<RaceLegModel>
            {
                new RaceLegModel {Id = Guid.Parse("9e26a483-cb18-477e-9493-8ed86cd76d80"), Name="Leg 1", Race= races[0]  },
                new RaceLegModel {Id = Guid.Parse("9cb0bee3-3931-4f0b-856b-95b612f3b795"), Name="Leg 2", Race= races[0]   }
            };

            legs.ForEach(s => context.RaceLegs.Add(s));
            context.SaveChanges();

            var legResults = new List<LegResultModel>
            {
                new LegResultModel {Id = Guid.Parse("9e26a483-cb18-477e-9493-8ed86cd76cc2"), Yacht = yachts[1], Leg = legs[0], StartTime = (new DateTime(2017, 2, 1, 10, 00, 00)), EndTime = (new DateTime(2017, 2, 1, 18, 42, 17))}
            };

            legResults.ForEach(r => context.LegResults.Add(r));
            context.SaveChanges();

            //var courses = new List<Course>
            //{
            //new Course{CourseID=1050,Title="Chemistry",Credits=3,},
            //new Course{CourseID=4022,Title="Microeconomics",Credits=3,},
            //new Course{CourseID=4041,Title="Macroeconomics",Credits=3,},
            //new Course{CourseID=1045,Title="Calculus",Credits=4,},
            //new Course{CourseID=3141,Title="Trigonometry",Credits=4,},
            //new Course{CourseID=2021,Title="Composition",Credits=3,},
            //new Course{CourseID=2042,Title="Literature",Credits=4,}
            //};
            //courses.ForEach(s => context.Courses.Add(s));
            //context.SaveChanges();
        }
    }
}