namespace Persistence.Migrations
{
    using Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = false;
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppDbContext context)
        {
            var role1 = new UserType() { Id = 1, Type = "admin" };
            var role2 = new UserType() { Id = 2, Type = "Technical group" };
            var role3 = new UserType() { Id = 3, Type = "user" };

            context.UserTypes.AddOrUpdate(role1, role2, role3);
            context.Save();
            Console.WriteLine("Pass");

            var user1 = new User
            {
                Id = 1,
                Name = "ahmed",
                Email = "admin@test.com",
                IsEnabled = true,
                Login = "admin",
                PasswordHash = setPassword("admin"),
                Telephone = "123",
                UserTypeId = role1.Id
            };
            var user2 = new User
            {
                Id = 2,
                Name = "tech",
                Email = "tech@test.com",
                IsEnabled = true,
                Login = "tech",
                PasswordHash = setPassword("admin"),
                Telephone = "123",
                UserTypeId = role2.Id
            };
            var user3 = new User
            {
                Id = 3,
                Name = "user",
                Email = "user@test.com",
                IsEnabled = true,
                Login = "user",
                PasswordHash = setPassword("user"),
                Telephone = "123",
                UserTypeId = role3.Id
            };


            context.Users.AddOrUpdate(user1, user2, user3);

            var status1 = new IssueStatus {Id = 1, Status = "New" };
            var status2 = new IssueStatus {Id = 2, Status = "Processing (assigned)" };
            var status3 = new IssueStatus {Id = 3, Status = "Processing (planned)" };
            var status4 = new IssueStatus {Id = 4, Status = "Pending" };
            var status5 = new IssueStatus {Id = 5, Status = "Solved" };
            var status6 = new IssueStatus {Id = 6, Status = "Closed" };
            var status7 = new IssueStatus {Id = 7, Status = "Deleted" };

            context.Statuses.AddOrUpdate(status1, status2, status3, status4, status5, status6, status7);

            var connection1 = new ConnectionType {Id = 1, ConnType = "Office" };
            var connection2 = new ConnectionType {Id = 2, ConnType = "Remote" };
            var connection3 = new ConnectionType {Id = 3, ConnType = "StandBy" };

            context.ConnectionTypes.AddOrUpdate(connection1, connection2, connection3);

            var city1 = new City {Id = 1, CityName = "Chisnau" };
            var city2 = new City {Id = 2, CityName = "Balti" };
            var city3 = new City {Id = 3, CityName = "Tiraspol" };
            var city4 = new City {Id = 4, CityName = "Bender" };
            var city5 = new City {Id = 5, CityName = "Orhei" };
            var city6 = new City {Id = 6, CityName = "Cahul" };

            context.Cities.AddOrUpdate(city1, city2, city3, city4, city5, city6);
            context.Save();
            Console.WriteLine("pass2");

            var Pos1 = new Pos
            {
                Id = 1,
                Name = "Pos 1",
                Telephone = "123",
                Cellphone = "234",
                Address = "Stefan Cel Mare",
                Brand = "Brand 1",
                Model = "Model 1",
                CityId = city1.Id,
                ConnectionTypeId = connection1.Id,
                MorningOpening = new TimeSpan(9, 0, 0),
                MorningClosing = new TimeSpan(13, 0, 0),
                AfternoonOpening = new TimeSpan(14, 0, 0),
                AfternoonClosing = new TimeSpan(20, 0, 0),
                DaysClosed = "6,7",
                InsertDate = DateTime.Now
            };
            var Pos2 = new Pos
            {
                Id = 2,
                Name = "Pos 2",
                Telephone = "123",
                Cellphone = "234",
                Address = "Columna",
                Brand = "Brand 2",
                Model = "Model 2",
                CityId = city1.Id,
                ConnectionTypeId = connection2.Id,
                MorningOpening = new TimeSpan(8, 0, 0),
                MorningClosing = new TimeSpan(12, 0, 0),
                AfternoonOpening = new TimeSpan(13, 0, 0),
                AfternoonClosing = new TimeSpan(19, 0, 0),
                DaysClosed = "Fri,Sat",
                InsertDate = DateTime.Now
            };
            var Pos3 = new Pos
            {
                Id = 3,
                Name = "Pos 3",
                Telephone = "12344",
                Cellphone = "234",
                Address = "Ion Creanga",
                Brand = "Brand 3",
                Model = "Model 3",
                CityId = city3.Id,
                ConnectionTypeId = connection3.Id,
                MorningOpening = new TimeSpan(7, 0, 0),
                MorningClosing = new TimeSpan(11, 0, 0),
                AfternoonOpening = new TimeSpan(13, 0, 0),
                AfternoonClosing = new TimeSpan(19, 0, 0),
                DaysClosed = "Sun,Sat",
                InsertDate = DateTime.Now
            };

            context.Pos.AddOrUpdate(Pos1, Pos2, Pos3);

            var IssueType1 = new IssueType {Id = 1, IssueLevel = 1, Name = "Hardware", InsertDate = DateTime.Now };
            var IssueType2 = new IssueType {Id = 2, IssueLevel = 1, Name = "Software", InsertDate = DateTime.Now };
            var IssueType3 = new IssueType {Id = 3, IssueLevel = 1, Name = "Security", InsertDate = DateTime.Now };

            var Issue1SubType1 = new IssueType {Id = 4, IssueLevel = 2, ParentIssueId = 1, Name = "equipment Request", InsertDate = DateTime.Now };
            var Issue1SubType2 = new IssueType {Id = 5, IssueLevel = 2, ParentIssueId = 1, Name = "Hardware Malfunction", InsertDate = DateTime.Now };
            var Issue1SubType3 = new IssueType {Id = 6, IssueLevel = 2, ParentIssueId = 1, Name = "Laptop Request", InsertDate = DateTime.Now };

            var Issue2SubType1 = new IssueType {Id = 7, IssueLevel = 2, ParentIssueId = 2, Name = "Instalation Issues", InsertDate = DateTime.Now };
            var Issue2SubType2 = new IssueType {Id = 8, IssueLevel = 2, ParentIssueId = 2, Name = "Windows Configuration", InsertDate = DateTime.Now };
            var Issue2SubType3 = new IssueType {Id = 9, IssueLevel = 2, ParentIssueId = 2, Name = "Software Update", InsertDate = DateTime.Now };

            var Issue3SubType1 = new IssueType {Id = 10, IssueLevel = 2, ParentIssueId = 3, Name = "Aplication Permissions", InsertDate = DateTime.Now };
            var Issue3SubType2 = new IssueType {Id = 11, IssueLevel = 2, ParentIssueId = 3, Name = "Authorization Request", InsertDate = DateTime.Now };
            var Issue3SubType3 = new IssueType {Id = 12, IssueLevel = 2, ParentIssueId = 3, Name = "Unautharized Login", InsertDate = DateTime.Now };

            var Problem1Issue1SubType1 = new IssueType {Id = 13, IssueLevel = 3, ParentIssueId = 4, Name = "Request new headphones", InsertDate = DateTime.Now };
            var Problem2Issue1SubType1 = new IssueType {Id = 14, IssueLevel = 3, ParentIssueId = 4, Name = "Request new mouse", InsertDate = DateTime.Now };
            var Problem3Issue1SubType1 = new IssueType {Id = 15, IssueLevel = 3, ParentIssueId = 4, Name = "Request new Keyboard", InsertDate = DateTime.Now };
                                                      
            var Problem1Issue1SubType2 = new IssueType {Id = 16, IssueLevel = 3, ParentIssueId = 5, Name = "Faulty Monitor", InsertDate = DateTime.Now };
            var Problem2Issue1SubType2 = new IssueType {Id = 17, IssueLevel = 3, ParentIssueId = 5, Name = "Faulty Pc", InsertDate = DateTime.Now };
            var Problem3Issue1SubType2 = new IssueType {Id = 18, IssueLevel = 3, ParentIssueId = 5, Name = "Faulty Accessories", InsertDate = DateTime.Now };
                                                       
            var Problem1Issue1SubType3 = new IssueType {Id = 19, IssueLevel = 3, ParentIssueId = 6, Name = "Request new Laptop", InsertDate = DateTime.Now };
            var Problem2Issue1SubType3 = new IssueType {Id = 20, IssueLevel = 3, ParentIssueId = 6, Name = "Request laptop Change", InsertDate = DateTime.Now };
                                                       
            var Problem1Issue2SubType1 = new IssueType {Id = 21, IssueLevel = 3, ParentIssueId = 7, Name = "Installer broken", InsertDate = DateTime.Now };
            var Problem2Issue2SubType1 = new IssueType {Id = 22, IssueLevel = 3, ParentIssueId = 7, Name = "Version Mismatch", InsertDate = DateTime.Now };
            var Problem3Issue2SubType1 = new IssueType {Id = 23, IssueLevel = 3, ParentIssueId = 7, Name = "Requst update", InsertDate = DateTime.Now };
                                                       
            var Problem1Issue2SubType2 = new IssueType {Id = 24, IssueLevel = 3, ParentIssueId = 8, Name = "Request Reroll", InsertDate = DateTime.Now };
            var Problem2Issue2SubType2 = new IssueType {Id = 25, IssueLevel = 3, ParentIssueId = 8, Name = "Request Update", InsertDate = DateTime.Now };
            var Problem3Issue2SubType2 = new IssueType {Id = 26, IssueLevel = 3, ParentIssueId = 8, Name = "Requset Reinstall", InsertDate = DateTime.Now };
                                                       
            var Problem1Issue2SubType3 = new IssueType {Id = 27, IssueLevel = 3, ParentIssueId = 9, Name = "Request Software update", InsertDate = DateTime.Now };
            var Problem2Issue2SubType3 = new IssueType {Id = 28, IssueLevel = 3, ParentIssueId = 9, Name = "Request new software", InsertDate = DateTime.Now };
                                                       
            var Problem1Issue3SubType1 = new IssueType {Id = 29, IssueLevel = 3, ParentIssueId = 10, Name = "Request app access", InsertDate = DateTime.Now };
            var Problem2Issue3SubType1 = new IssueType {Id = 30, IssueLevel = 3, ParentIssueId = 10, Name = "Request app removal", InsertDate = DateTime.Now };
                                                       
            var Problem1Issue3SubType2 = new IssueType {Id = 31, IssueLevel = 3, ParentIssueId = 11, Name = "Request Admin Access", InsertDate = DateTime.Now };
            var Problem2Issue3SubType2 = new IssueType {Id = 32, IssueLevel = 3, ParentIssueId = 11, Name = "Requst new VPN", InsertDate = DateTime.Now };
            var Problem3Issue3SubType2 = new IssueType {Id = 33, IssueLevel = 3, ParentIssueId = 11, Name = "Request remote Access", InsertDate = DateTime.Now };

            var Problem1Issue3SubType3 = new IssueType {Id = 34, IssueLevel = 3, ParentIssueId = 12, Name = "Report Unautharized login", InsertDate = DateTime.Now };
            var Problem2Issue3SubType3 = new IssueType { Id = 35, IssueLevel = 3, ParentIssueId = 12, Name = "Report Auth problem", InsertDate = DateTime.Now };
           

            context.IssueTypes.AddOrUpdate(IssueType1, IssueType2, IssueType3, Issue1SubType1, Issue1SubType2, Issue1SubType3, Issue2SubType1, Issue2SubType2, Issue2SubType3, Issue3SubType1, Issue3SubType2, Issue3SubType3, Problem1Issue1SubType1, Problem1Issue1SubType2,Problem1Issue1SubType3, Problem1Issue2SubType1, Problem1Issue2SubType2, Problem1Issue2SubType3, Problem1Issue3SubType1, Problem1Issue3SubType2, Problem1Issue3SubType3, Problem2Issue1SubType1, Problem2Issue1SubType2, Problem2Issue1SubType3, Problem2Issue2SubType1, Problem2Issue2SubType2, Problem2Issue2SubType3, Problem2Issue3SubType1, Problem2Issue3SubType2, Problem2Issue3SubType3, Problem3Issue1SubType1, Problem3Issue1SubType2, Problem3Issue2SubType1, Problem3Issue2SubType2, Problem3Issue3SubType2);
            context.Save();
            Console.WriteLine("pass3");

            var Issue1 = new Issue
            {
                Name = "HDC00001",
                Priority = "Normal",
                Memo = "memo",
                Description = "this is my issue",
                Assigned = role2,
                Type = IssueType1,
                SubType = Issue1SubType2,
                Problem = Problem1Issue1SubType2,
                Pos = Pos1,
                CreatedBy = user1,
                Created = DateTime.Now,
                Status = status4
            };
            var Issue2 = new Issue
            {
                Name = "HBS00001",
                Priority = "High",
                Memo = "memo 2",
                Description = "this is my issue 2",
                Assigned = role3,
                Type = IssueType2,
                SubType = Issue1SubType1,
                Pos = Pos2,
                Solution = "this is the solution for problem",
                CreatedBy = user3,
                Created = DateTime.Now,
                Status = status4
            };
            var Issue3 = new Issue
            {
                Name = "HBS00002",
                Priority = "Very Low",
                Memo = "memo 3",
                Description = "this is my issue 3",
                AssignedId = role1.Id,
                TypeId = IssueType3.Id,
                SubTypeId = Issue1SubType3.Id,
                PosId = Pos3.Id,
                CreatedById = user3.Id,
                Created = DateTime.Now,
                LastModifiedById = user1.Id,
                LastModified = DateTime.Now,
                StatusId = status5.Id
            };

            context.Issues.AddOrUpdate(Issue1, Issue2, Issue3);
            context.Save();
            
            var Log1 = new Log {Id = 1, IssueId = Issue1.Id, UserId = user1.Id, Action = "Changed the status", Notes = "Nothing much", InsertDate = DateTime.Now };
            var Log2 = new Log {Id = 2, IssueId = Issue2.Id, UserId = user2.Id, Action = "Changed the Name", Notes = "big mistake", InsertDate = DateTime.Now };
            var Log3 = new Log {Id = 3, IssueId = Issue3.Id, UserId = user3.Id, Action = "Changed the Priority", Notes = "good", InsertDate = DateTime.Now };

            context.Logs.AddOrUpdate(Log1, Log2, Log3);

            base.Seed(context);

        }


        private string setPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
