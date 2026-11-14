using CinemaGoProject.Models;
using CinemaGoProject.Repositories;
using System.Collections.Generic;

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
