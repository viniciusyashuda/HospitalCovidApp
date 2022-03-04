using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCovidApp
{
    internal class ExamsWard
    {
        public Person Start { get; set; }

        public Person End { get; set; }

        public ExamsWard()
        {
            Start = null;
            End = null;
        }

        public bool Empty()
        {

            if (Start == null && End == null)
                return true;
            else
                return false;

        }

        public void InsertionExamsWard(Person patient)
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

        public Person ExamsWardHold()
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

        public int ExamsWardCounter()
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
    }
}
