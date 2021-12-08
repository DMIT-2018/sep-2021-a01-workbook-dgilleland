using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.BLL
{
    public class RandomDataService
    {
        public List<Applicant> ListJobApplicants()
        {
            return new List<Applicant>()
            {
                new Applicant(101, "Terrance", "O'Reilly"),
                new Applicant(102, "Hanna", "Sumner"),
                new Applicant(103, "Soraya", "Greenwood"),
                new Applicant(104, "Zakk", "Harwood"),
                new Applicant(105, "Lilianna", "Yu"),
                new Applicant(107, "Malia", "Nelson"),
                new Applicant(108, "Madihah", "Marsden"),
                new Applicant(109, "Amy-Leigh", "Parkes"),
                new Applicant(112, "Cieran", "Guevara"),
                new Applicant(115, "Kishan", "Corona")
            };
        }

        public List<string> ListJobOpenings()
        {
            return new List<string>()
            {
                "Jr. Developer",
                "Sr. Developer",
                "Team Lead",
                "Product Manager",
                "Project Manager"
            };
        }

        public void ScheduleInterviews(List<Candidate> jobCandidates)
        {

        }
    }
}
