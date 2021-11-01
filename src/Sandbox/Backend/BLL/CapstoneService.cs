using Backend.DAL;
using Backend.Entities;
using Backend.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.BLL
{
    public class CapstoneService
    {
        #region Constructor and Dependencies
        private readonly CapstoneContext _context;
        internal CapstoneService(CapstoneContext context)
        {
            _context = context;
        }
        #endregion

        #region A loose representation of CQRS - Command/Query Responsibility Segregation
        #region Queries (reading the database)
        public List<StudentAssignment> ListStudentAssignments(int pageNumber, int pageSize, out int totalRows)
        {
            totalRows = _context.Students.Count();
            var result = from person in _context.Students
                         orderby person.LastName, person.FirstName, person.SchoolId
                         select new StudentAssignment
                         {
                             LastName = person.LastName,
                             FirstName = person.FirstName,
                             Assignment = (from team in person.TeamAssignments
                                           select new Team
                                           {
                                               Number = team.TeamNumber,
                                               Client = team.Client.CompanyName,
                                               IsConfirmed = team.Client.Confirmed
                                           }).SingleOrDefault()
                         };
            // Limit how much data I'm getting back from the database
            int skipRows = (pageNumber - 1) * pageSize; // page 1 = skip 0 rows, page 2 = skip pageSize
            return result.Skip(skipRows).Take(pageSize).ToList();
            // Offset ->  rows ommitted, # rows we want
        }
        #endregion

        #region Commands (modifying the database)
        #endregion
        #endregion

        #region CRUD behaviour for Student information
        public List<Backend.Models.StudentName> ListCapstoneStudents()
        {
            var result = _context.Students.Select(person => 
                new Backend.Models.StudentName 
                { 
                    StudentId = person.StudentId, FormalName = $"{person.LastName}, {person.FirstName}" 
                });
            return result.ToList();
        }

        /// <summary>
        /// Finds a <see cref="Models.Student"/> from a given database PK value.
        /// </summary>
        /// <param name="studentIdPk">The Primary Key for the Entity (not to be confused with the School ID)</param>
        /// <returns><see cref="Models.Student"/> instance or <code>null</code> if no information found in the database</returns>
        public Models.Student LookupStudent(int studentIdPk)
        {
            // Remember to map our database Entity results from the lookup to our View Model type
            Models.Student result = null;
            var found = _context.Students.Find(studentIdPk);
            if (found != null)
                result = new(found.SchoolId, found.FirstName, found.LastName);
            return result;
        }

        public int AddStudent(Backend.Models.Student student)
        {
            // Validation of no duplicates
            if (_context.Students.Any(person => person.SchoolId == student.ID))
                throw new ArgumentOutOfRangeException(nameof(student.ID), "A student already exists with that school id");

            Student newData = new()
            {
                // Transferring the data from our public model to the internal Entity type
                SchoolId = student.ID,
                FirstName = student.FirstName,
                LastName = student.LastName
            };
            _context.Students.Add(newData);
            _context.SaveChanges(); // This is where the Transaction processing occurs.
            return newData.StudentId;
        }

        public void UpdateStudent(int studentId, Backend.Models.Student student)
        {
            // Validation of no duplicates
            if (_context.Students.Any(person => person.SchoolId == student.ID && person.StudentId != studentId))
                throw new ArgumentOutOfRangeException(nameof(student.ID), $"Another student already exists with that school id; correct that student before reassigning the {student.ID} to this student");
            // Update
            var existing = _context.Students.Find(studentId);
            if (existing is null)
                throw new ArgumentException("Could not find the specified student", nameof(studentId));

            existing.FirstName = student.FirstName;
            existing.LastName = student.LastName;
            existing.SchoolId = student.ID;
            _context.SaveChanges();
        }

        public void DeleteStudent(int studentId)
        {
            var existing = _context.Students.Find(studentId);
            if (existing is null)
                throw new ArgumentException("Could not find the specified student", nameof(studentId));
            _context.Students.Remove(existing);
            _context.SaveChanges();
        }
        #endregion
    }
}
