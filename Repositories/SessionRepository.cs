using CinemaGoProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CinemaGoProject.Repositories
{
    internal class SessionRepository
    {
        private readonly string _filePath = Path.Combine(
            Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                .Parent.Parent.FullName,
            "Data",
            "sessions.txt");

        public void SaveSession(Session session)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            File.AppendAllLines(_filePath, new[] { session.ToString() });
        }

        public List<Session> GetAllSessions()
        {
            List<Session> sessions = new List<Session>();

            if (!File.Exists(_filePath))
            {
                return sessions;
            }

            foreach (var item in File.ReadAllLines(_filePath))
            {
                var line = item.Split('|');

                if (line.Length != 7 && line.Length != 10)
                {
                    continue;
                }

                try
                {
                    Session session = new Session
                    {
                        Id = int.Parse(line[0]),
                        FilmId = int.Parse(line[1]),
                        StartTime = DateTime.Parse(line[2]),
                        Hall = line[3],
                        StandardPrice = decimal.Parse(line[4]),
                        VipPrice = decimal.Parse(line[5]),
                        IsCancelled = bool.Parse(line[6])
                    };

                    if (line.Length == 10)
                    {
                        session.Rows = int.Parse(line[7]);
                        session.SeatsPerRow = int.Parse(line[8]);
                        session.VipRows = int.Parse(line[9]);
                    }

                    sessions.Add(session);
                }
                catch
                {
                    continue;
                }
            }

            return sessions;
        }

        public Session GetSessionById(int id)
        {
            return GetAllSessions().FirstOrDefault(x => x.Id == id);
        }

        public void UpdateSession(Session session)
        {
            var sessions = GetAllSessions();

            Session existingSession = sessions.FirstOrDefault(x => x.Id == session.Id);

            if (existingSession == null)
            {
                return;
            }

            existingSession.IsCancelled = session.IsCancelled;
            SaveAllSessions(sessions);
        }

        private void SaveAllSessions(List<Session> sessions)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            string[] sessionText = new string[sessions.Count];

            for (int i = 0; i < sessions.Count; i++)
            {
                sessionText[i] = sessions[i].ToString();
            }

            File.WriteAllLines(_filePath, sessionText);
        }
    }
}