using CinemaGoProject.Models;
using CinemaGoProject.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CinemaGoProject.Services
{
    internal class FilmService
    {
        private readonly FilmRepository _filmRepository;

        public FilmService(FilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        public List<Film> GetAllFilms()
        {
            return _filmRepository.GetAllFilms();
        }

        public Film GetFilmById(int id)
        {
            return _filmRepository.GetFilmById(id);
        }

        public List<string> GetGenres()
        {
            return GetAllFilms()
                .Select(x => x.Genre)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        public List<Film> GetFilmsByGenre(string genre)
        {
            return GetAllFilms()
                .Where(x => x.Genre.Equals(genre, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void AddFilm(Film film)
        {
            _filmRepository.SaveFilm(film);
        }

        public void UpdateFilm(Film film)
        {
            _filmRepository.UpdateFilm(film);
        }

        public void DeleteFilm(int id)
        {
            _filmRepository.DeleteFilm(id);
        }
    }
}