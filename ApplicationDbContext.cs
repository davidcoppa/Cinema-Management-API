using Cinema.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace Cinema
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieByActors>()
                .HasKey(x => new { x.ActorId, x.MovieId });

            modelBuilder.Entity<MoviesByGender>()
                .HasKey(x => new { x.GenderId, x.MovieId });

            modelBuilder.Entity<MovieCinema>()
                .HasKey(x => new { x.MovieId, x.CinemaRoomId });

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {

            var rolAdminId = "9aae0b6d-d50c-4d0a-9b90-2a6873e3845d";
            var userAdminId = "5673b8cf-12de-44f6-92ad-fae4a77932ad";

            var rolAdmin = new IdentityRole()
            {
                Id = rolAdminId,
                Name = "Admin",
                NormalizedName = "Admin"
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();

            var username = "email@email.com";

            var userAdmin = new IdentityUser()
            {
                Id = userAdminId,
                UserName = username,
                NormalizedUserName = username,
                Email = username,
                NormalizedEmail = username,
                PasswordHash = passwordHasher.HashPassword(null, "Aa123456!")
            };

            //modelBuilder.Entity<IdentityUser>()
            //    .HasData(userAdmin);

            //modelBuilder.Entity<IdentityRole>()
            //    .HasData(rolAdmin);

            //modelBuilder.Entity<IdentityUserClaim<string>>()
            //    .HasData(new IdentityUserClaim<string>()
            //    {
            //        Id = 1,
            //        ClaimType = ClaimTypes.Role,
            //        UserId = userAdminId,
            //        ClaimValue = "Admin"
            //    });

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            modelBuilder.Entity<CinemaRoom>()
               .HasData(new List<CinemaRoom>
               {
                    new CinemaRoom{Id = 1, Name = "Hoyts", Location = geometryFactory.CreatePoint(new Coordinate(-41.27276606753421, 173.2763940696728))},
                    new CinemaRoom{Id = 2, Name = "Nelson Cinema", Location = geometryFactory.CreatePoint(new Coordinate(-41.27112008715975, 173.2840804261483))},
                    new CinemaRoom{Id = 3, Name = "Cinema Paradiso", Location = geometryFactory.CreatePoint(new Coordinate(-41.33963794539795, 173.1855477671311))}
               });

            var adventure = new Gender() { Id = 4, Name = "Adventure" };
            var animatión = new Gender() { Id = 3, Name = "Animatión" };
            var suspense = new Gender() { Id = 2, Name = "Suspense" };
            var thriller = new Gender() { Id = 1, Name = "Thriller" };

            modelBuilder.Entity<Gender>()
                .HasData(new List<Gender>
                {
                    adventure, animatión, suspense, thriller
                });

            var jimCarrey = new Actor() { Id = 1, Name = "Jim Carrey", DateBirth = new DateTime(1962, 01, 17) };
            var robertDowney = new Actor() { Id = 2, Name = "Robert Downey Jr.", DateBirth = new DateTime(1965, 4, 4) };
            var chrisEvans = new Actor() { Id = 3, Name = "Chris Evans", DateBirth = new DateTime(1981, 06, 13) };

            modelBuilder.Entity<Actor>()
                .HasData(new List<Actor>
                {
                    jimCarrey, robertDowney, chrisEvans
                });

            var endgame = new Movie()
            {
                Id = 2,
                Tittle = "Avengers: Endgame",
                OnCinema = true,
                ReleaseDate = new DateTime(2019, 04, 26)
            };

            var iw = new Movie()
            {
                Id = 3,
                Tittle = "Avengers: Infinity Wars",
                OnCinema = false,
                ReleaseDate = new DateTime(2019, 04, 26)
            };

            var sonic = new Movie()
            {
                Id = 4,
                Tittle = "Sonic the Hedgehog",
                OnCinema = false,
                ReleaseDate = new DateTime(2020, 02, 28)
            };
            var wonderwoman = new Movie()
            {
                Id = 6,
                Tittle = "Wonder Woman 1984",
                OnCinema = false,
                ReleaseDate = new DateTime(2020, 08, 14)
            };

            modelBuilder.Entity<Movie>()
                .HasData(new List<Movie>
                {
                    endgame, iw, sonic, wonderwoman
                });

            modelBuilder.Entity<MoviesByGender>().HasData(
                new List<MoviesByGender>()
                {
                    new MoviesByGender(){MovieId= endgame.Id, GenderId = suspense.Id},
                    new MoviesByGender(){MovieId = endgame.Id, GenderId = adventure.Id},
                    new MoviesByGender(){MovieId = iw.Id, GenderId = suspense.Id},
                    new MoviesByGender(){MovieId = iw.Id, GenderId = adventure.Id},
                    new MoviesByGender(){MovieId = sonic.Id, GenderId = adventure.Id},
                    new MoviesByGender(){MovieId = wonderwoman.Id, GenderId = suspense.Id},
                    new MoviesByGender(){MovieId = wonderwoman.Id, GenderId = adventure.Id},
                });

            modelBuilder.Entity<MovieByActors>().HasData(
                new List<MovieByActors>()
                {
                    new MovieByActors(){MovieId = endgame.Id, ActorId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new MovieByActors(){MovieId = endgame.Id, ActorId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new MovieByActors(){MovieId = iw.Id, ActorId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new MovieByActors(){MovieId = iw.Id, ActorId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new MovieByActors(){MovieId = sonic.Id, ActorId = jimCarrey.Id, Character = "Dr. Ivo Robotnik", Order = 1}
                });
        }

        public DbSet<Gender> Genders { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieByActors> MovieByActors { get; set; }
        public DbSet<MoviesByGender> MoviesByGenders { get; set; }
        public DbSet<MovieCinema> MovieCinema { get; set; }
        public DbSet<CinemaRoom> CinemaRooms { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
