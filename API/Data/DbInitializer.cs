using GAE.AIQ.API.Data;
using API.Data.Entities;

using System.Linq;
using System.Threading.Tasks;
using System;

namespace API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Actors.Any())
            {
                return;
            }

            Actor[] Actors = new Actor[]
            {
                new Actor{Name="Terry Crews",Age=53},
                new Actor{Name="Jim Carrey",Age=59},
                new Actor{Name="Morgan Freeman",Age=84},
                new Actor{Name="Shawn Wayans",Age=50},
            };

            foreach (Actor a in Actors)
            {
                context.Actors.Add(a);
            }

            context.SaveChanges();

            Movie[] Movies = new Movie[]
            {
                new Movie{Name="White Chicks",Genre="Comedy",UrlImg="https://images-na.ssl-images-amazon.com/images/S/pv-target-images/035cd0240dd8b9dae16324c00c42eefc228a4d123fd7d55d9be8daf98e38e6f0._RI_V_TTW_.jpg"},
                new Movie{Name="Bruce Almighty",Genre="Comedy",UrlImg="https://static.wikia.nocookie.net/doblaje/images/9/9a/Todopoderoso.jpg/revision/latest?cb=20090914163435&path-prefix=es"},
                new Movie{Name="The Expendables 2 ",Genre="Action",UrlImg="https://images-na.ssl-images-amazon.com/images/I/91Jq6py2wTL._RI_.jpg"},
                new Movie{Name="Batman Begins",Genre="Action",UrlImg="https://pics.filmaffinity.com/Batman_Begins-413277928-large.jpg"},
            };

            foreach (Movie m in Movies)
            {
                context.Movies.Add(m);
            }

            context.SaveChanges();

            MovieActor[] movieActors = new MovieActor[]
            {
                new MovieActor{
                    PublicationDate=DateTime.Now,
                    MovieId=Movies[0].Id,
                    ActorId=Actors[0].Id
                },
                new MovieActor{
                    PublicationDate=DateTime.Now,
                    MovieId=Movies[0].Id,
                    ActorId=Actors[3].Id
                },
                new MovieActor{
                    PublicationDate=DateTime.Now,
                    MovieId=Movies[1].Id,
                    ActorId=Actors[1].Id
                },
                new MovieActor{
                    PublicationDate=DateTime.Now,
                    MovieId=Movies[1].Id,
                    ActorId=Actors[2].Id
                },
                new MovieActor{
                    PublicationDate=DateTime.Now,
                    MovieId=Movies[2].Id,
                    ActorId=Actors[0].Id
                },
                new MovieActor{
                    PublicationDate=DateTime.Now,
                    MovieId=Movies[3].Id,
                    ActorId=Actors[2].Id
                },
            };

            foreach (MovieActor ma in movieActors)
            {
                context.MovieActors.Add(ma);
            }

            context.SaveChanges();

        }
    }
}
