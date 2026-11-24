using CinemaGoProject.Models;
using CinemaGoProject.Repositories;
using System.Collections.Generic;

namespace CinemaGoProject.Services
{
    internal class SessionService
    {
        private readonly SessionRepository _sessionRepository;
        private readonly SeatRepository _seatRepository;

        public SessionService(
            SessionRepository sessionRepository,
            SeatRepository seatRepository)
        {
            _sessionRepository = sessionRepository;
            _seatRepository = seatRepository;
        }

        public List<Session> GetAllSessions()
        {
            return _sessionRepository.GetAllSessions();
        }

        public Session GetSessionById(int id)
        {
            return _sessionRepository.GetSessionById(id);
        }

        public void AddSession(Session session)
        {
            _sessionRepository.SaveSession(session);

            Hall hall = new Hall
            {
                Name = session.Hall,
                Rows = session.Rows,
                SeatsPerRow = session.SeatsPerRow,
                VipRows = session.VipRows
            };

            hall.CreateSeats();
            _seatRepository.SaveSeats(session.Id, hall);
        }

        public Hall GetHall(int sessionId)
        {
            Session session = GetSessionById(sessionId);

            if (session == null)
            {
                return null;
            }

            Hall hall = new Hall
            {
                Name = session.Hall,
                Rows = session.Rows,
                SeatsPerRow = session.SeatsPerRow,
                VipRows = session.VipRows
            };

            hall.CreateSeats();
            _seatRepository.LoadSeats(sessionId, hall);

            return hall;
        }

        public bool BookSeat(int sessionId, char row, int number)
        {
            Hall hall = GetHall(sessionId);

            if (hall == null)
            {
                return false;
            }

            Seat seat = hall.GetSeat(row, number);

            if (seat == null || !seat.Book())
            {
                return false;
            }

            _seatRepository.SaveSeats(sessionId, hall);
            return true;
        }

        public bool CancelSeat(int sessionId, char row, int number)
        {
            Hall hall = GetHall(sessionId);

            if (hall == null)
            {
                return false;
            }

            Seat seat = hall.GetSeat(row, number);

            if (seat == null || !seat.Cancel())
            {
                return false;
            }

            _seatRepository.SaveSeats(sessionId, hall);
            return true;
        }

        public void CancelSession(int id)
        {
            Session session = GetSessionById(id);

            if (session != null)
            {
                session.IsCancelled = true;
                _sessionRepository.UpdateSession(session);
            }
        }
    }
}