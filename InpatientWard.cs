using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCovidApp
{
    internal class InpatientWard
    {
        public int Beds { get; set; }

        public Person Start { get; set; }

        public Person End { get; set; }

        public Person FirstBed { get; set; }

        public Person LastBed { get; set; }

        public InpatientWard(int beds)
        {
            Beds = beds;
            Start = null;
            End = null;
            FirstBed = null;
            LastBed = null;
        }

        public bool Empty()
        {

            if (Start == null && End == null)
                return true;
            else
                return false;

        }

        public bool AllBedsEmpty()
        {

            if (FirstBed == null && LastBed == null)
                return true;
            else
                return false;

        }

        public void InsertionInpatientWard(Person patient)
        {
            if (Beds > 0)
            {

                InsertPatientInTheBed(patient);

            }
            else
            {

                if (Empty())
                {
                    Start = patient;
                    End = patient;
                }
                else
                {
                    End.Next = patient;
                    End = patient;
                }

            }

        }

        public void InsertPatientInTheBed(Person patient)
        {
            if (AllBedsEmpty())
            {

                FirstBed = patient;
                LastBed = patient;
                Beds--;

            }
            else
            {

                LastBed.Next = patient;
                LastBed = patient;
                Beds--;
            }
        }

        public Person InpatientWardHold()
        {

            Person patient = null;

            if (Empty())
            {
                patient = null;
            }
            else
            {
                patient = Start;
                Start = Start.Next;
            }

            if (Start == null)
                End = null;

            return patient;

        }

        public void BedHold(Person patient)
        {

            Person patientPosition = FirstBed;
            Person patientReference = FirstBed;


            if (AllBedsEmpty())
            {
                patient = null;
            }
            else
            {

                if (string.Compare(patient.CPF, FirstBed.CPF) == 0)
                {
                    FirstBed = FirstBed.Next;
                    Beds++;
                }
                else
                {
                    patientReference = patientReference.Next;

                    do
                    {
                        if (string.Compare(patient.CPF, patientReference.CPF) == 0)
                        {

                            if (string.Compare(patient.CPF, LastBed.CPF) == 0)
                            {

                                patientPosition.Next = patientReference.Next;
                                LastBed = patientPosition;
                                Beds++;

                            }
                            else
                            {

                                patientPosition.Next = patientReference.Next;
                                Beds++;

                            }
                        }

                        patientReference = patientReference.Next;
                        patientPosition = patientPosition.Next;
                    }
                    while (patientReference != null);

                }

            }


        }

        public int InpatientWardCounter()
        {
            int count = 0;
            Person patient = Start;

            if (Empty())
            {
                return 0;
            }
            else
            {

                do
                {
                    patient = patient.Next;
                    count++;
                }
                while (patient != null);


            }

            return count;
        }

        public int BedCounter()
        {
            int count = 0;
            Person patient = FirstBed;

            if (AllBedsEmpty())
            {
                return 0;
            }
            else
            {

                do
                {
                    patient = patient.Next;
                    count++;
                }
                while (patient != null);


            }

            return count;
        }

    }
}
