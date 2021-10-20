using Backend.DAL;
using Backend.Entities;
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
            throw new NotImplementedException(nameof(UpdateStudent)); // TODO: Finish this....
        }

        public void DeleteStudent(int studentId)
        {
            throw new NotImplementedException(nameof(DeleteStudent)); // TODO: Finish this....
        }
        #endregion
    }
}
