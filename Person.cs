using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCovidApp
{
    internal class Person
    {
        public string Name { get; set; }

        public string Age { get; set; }

        public string CPF { get; set; }

        public string Gender { get; set; }

        public string Password { get; set; }


        public  string Symptoms { get; set; }

        public string DaysSinceSymptomsStarted { get; set; }

        public string Comorbidities { get; set; }

        public string Temperature { get; set; }

        public string Saturation { get; set; }

        public string BirthDate { get; set; }

        public string EntryDateTime { get; set; }

        public string ExamType { get; set; }

        public string ExamResult { get; set; }

        public string Hospitalization { get; set; }

        public Person Next { get; set; }


        public Person()
        {

            Name = null;
            Age = null;
            CPF = null;
            Gender = null;
            Password = null;
            Next = null;
            Symptoms = null;
            DaysSinceSymptomsStarted = null;
            Comorbidities = null;
            Temperature = null;
            Saturation = null;
            BirthDate = null;
            ExamType = null;
            ExamResult = null;
            Hospitalization = null;
            
        }

        public override string ToString()
        {
            return "Name: " + Name
                + "\nBirth date: " + BirthDate
                + "\nAge: " + Age
                + "\nCPF: " + CPF
                + "\nGender: " + Gender
                + "\nPassword: " + Password
                + "\nSymptons: " + Symptoms
                + "\nDays since symptons started: " + DaysSinceSymptomsStarted
                + "\nComorbidities: " + Comorbidities
                + "\nTemperature: " + Temperature
                + "\nSaturation: " + Saturation
                + "\nExam type: " + ExamType
                + "\nExam result: " + ExamResult
                + "\nHospitalization: " + Hospitalization;
        }


    }
}
