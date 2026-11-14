using CinemaGoProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CinemaGoProject.Repositories
{
    internal class FilmRepository
    {
        private readonly string _filePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Data", "film.txt");

        public void SaveFilm(Film film)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            File.AppendAllLines(_filePath, new[] { film.ToString() });
        }

        public List<Film> GetAllFilms()
        {
            List<Film> films = new List<Film>();

            if (!File.Exists(_filePath))
            {
                return films;
            }

            var lines = File.ReadAllLines(_filePath);

            foreach (var item in lines)
            {
                var line = item.Split('|');

                if (line.Length != 3)
                {
                    continue;
                }

                Film film = new Film();

                film.Id = int.Parse(line[0]);
                film.Name = line[1];
                film.Description = line[2];

                films.Add(film);
            }

            return films;
        }

        public Film GetFilmById(int id)
        {
            var films = GetAllFilms();

            return films.FirstOrDefault(x => x.Id == id);
        }

        public void DeleteFilm(int id)
        {
            var films = GetAllFilms();

            Film film = films.FirstOrDefault(x => x.Id == id);

            if (film == null)
            {
                Console.WriteLine("Film not found.");
                return;
            }

            films.Remove(film);

            SaveAllFilms(films);
        }

        public void UpdateFilm(Film film)
        {
            var films = GetAllFilms();

            Film filmToUpdate = films.FirstOrDefault(x => x.Id == film.Id);

            if (filmToUpdate == null)
            {
                Console.WriteLine("Film not found.");
                return;
            }

            filmToUpdate.Name = film.Name;
            filmToUpdate.Description = film.Description;

            SaveAllFilms(films);
        }

        private void SaveAllFilms(List<Film> films)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            string[] filmText = new string[films.Count];

            for (int i = 0; i < films.Count; i++)
            {
                filmText[i] = films[i].ToString();
            }

            File.WriteAllLines(_filePath, filmText);
        }
    }
}
