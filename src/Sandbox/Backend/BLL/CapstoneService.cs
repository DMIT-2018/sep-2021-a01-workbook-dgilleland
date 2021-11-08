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
        public List<StudentAssignment> ListStudentAssignments(string partialStudentName, int pageNumber, int pageSize, out int totalRows)
        {
            var result = from person in _context.Students
                         where string.IsNullOrEmpty(partialStudentName)
                            || person.FirstName.Contains(partialStudentName)
                            || person.LastName.Contains(partialStudentName)
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
            // Offset ->  rows ommitted, # rows we want
            var finalResult = result.Skip(skipRows).Take(pageSize).ToList();
            totalRows = result.Count();
            return finalResult;
        }

        public List<ClientInfo> ListConfirmedClients()
        {
            var result = from company in _context.CapstoneClients
                         where company.Confirmed
                         orderby company.CompanyName
                         select new ClientInfo
                         {
                             ClientId = company.Id,
                             Company = company.CompanyName
                         };
            return result.ToList();
        }

        public List<StudentTeamAssignment> ListTeamAssignments()
        {
            var result = from person in _context.Students
                         orderby person.FirstName, person.LastName
                         select new StudentTeamAssignment
                         {
                             StudentId = person.StudentId,
                             FullName = $"{person.FirstName} {person.LastName}",
                             ClientId = person.TeamAssignments.Any() ? person.TeamAssignments.First().ClientId : null,
                             TeamLetter = person.TeamAssignments.Any() ? person.TeamAssignments.First().TeamNumber : null
                         };
            return result.ToList();
        }
        #endregion

        #region Commands (modifying the database)
        #region Private helper methods & types
        void CheckForDuplicateStudents(List<Exception> errors, IEnumerable<IGrouping<GroupingKey, StudentTeamAssignment>> teams)
        {
            // TODO: Add this extra validation for duplicate students
        }

        private void CheckForTeamLetterWithoutClient(List<Exception> errors, IEnumerable<IGrouping<GroupingKey, StudentTeamAssignment>> teams)
        {
            //     - (xtra) No teams with a null client and a team letter ??
            if (teams.Any(team => !team.Key.ClientId.HasValue && !string.IsNullOrWhiteSpace(team.Key.TeamLetter)))
                errors.Add( new Exception("At least one student is assigned a team letter without a client"));
        }

        private static void CheckMaximumTeamSize(List<Exception> errors, IEnumerable<IGrouping<GroupingKey, StudentTeamAssignment>> teams)
        {
            //     - (2) The largest team size is seven students
            if (teams.Where(team => team.Key.ClientId.HasValue)
                // filter out the "no-client" group, so that I only regard students that are assigned to a client
                // (4 or less unassigned students is ok)
                .Any(team => team.Count() > 7))
                // check the team is not too large
                errors.Add(new Exception("You cannot have any team with more than 7 students"));
        }

        private static void CheckMinimumTeamSize(List<Exception> errors, IEnumerable<IGrouping<GroupingKey, StudentTeamAssignment>> teams)
        {
            //     - (1) The smallest team size is four students
            if (teams.Where(team => team.Key.ClientId.HasValue)
                // filter out the "no-client" group, so that I only regard students that are assigned to a client
                // (4 or less unassigned students is ok)
                .Any(team => team.Count() < 4))
                // check the team is not too small
                errors.Add(new Exception("You cannot have any team with less than 4 students"));
        }

        void CheckTeamLetters(List<Exception> errors, IEnumerable<IGrouping<GroupingKey, StudentTeamAssignment>> teams)
        {
            // TODO: - (3) Clients with more than seven students must be broken into separate teams, each with a team letter(starting with 'A').
        }

        void CheckClientsAreConfirmed(List<Exception> errors, IEnumerable<IGrouping<GroupingKey, StudentTeamAssignment>> teams)
        {
            // TODO: - (4) Only assign students to clients that have been confirmed as participating.

        }

        private record GroupingKey(int? ClientId, string TeamLetter)
        { public GroupingKey() : this(null, null) { } }
        #endregion

        public void ModifyTeamAssignments(List<StudentTeamAssignment> assignments)
        {
            // TEST: Write an automation test
            // 0) Validation
            //   a - Make sure we have data - This is a "full-stop" exception
            if (assignments is null || assignments.Count == 0)
                throw new ArgumentNullException(nameof(assignments), "You must supply student assignments to modify the current teas rosters");


            //   b - Enforce the business rules - Distinct problems with the data - Gather the problems as a "set" of exceptions
            List<Exception> errors = new(); // Create an empty list of problems with the data
            IEnumerable<IGrouping<GroupingKey, StudentTeamAssignment>> teams
                = assignments.GroupBy(member => new GroupingKey { ClientId = member.ClientId, TeamLetter = member.TeamLetter });

            CheckForDuplicateStudents(errors, teams);
            CheckForTeamLetterWithoutClient(errors, teams);
            CheckMinimumTeamSize(errors, teams);
            CheckMaximumTeamSize(errors, teams);
            CheckTeamLetters(errors, teams);
            CheckClientsAreConfirmed(errors, teams);

            if (errors.Any()) // Are there any business rule violations?
                throw new AggregateException("The following business rules were violated:", errors);

            // 1) Process the team assignments as a SINGLE TRANSACTION
            //    - The list of StudentTeamAssignments represents the current assignment for each student.
            //    - Since this may be a change in an assigned client or team letter, we need to think about
            //      how this affect the database tables in terms of Inserts/Updates/Deletes
            foreach(var change in assignments)
            {
                var existing = _context.TeamAssignments.SingleOrDefault(x => x.StudentId == change.StudentId);
                if(existing is null)
                {
                    // We only have to add this student
                    if (change.ClientId.HasValue) // as long as the student is assigned to a client
                    {
                        _context.TeamAssignments.Add(new TeamAssignment
                        {
                            StudentId = change.StudentId,
                            ClientId = change.ClientId.Value,
                            TeamNumber = change.TeamLetter
                        });
                    }
                }
                else
                {
                    // Determine if the client has changed (result in a delete followed by an insert)
                    // or if the client is the same and the team letter has changed (result in an update)
                }
            }


            // ... after all our processing of the data, we do a SINGLE CALL to .SaveChanges()
            _context.SaveChanges();
        }
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
