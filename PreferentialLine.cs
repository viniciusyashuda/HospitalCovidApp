using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCovidApp
{
    internal class PreferentialLine
    {

        public Person Start { get; set; }

        public Person End { get; set; }

        public Person Next { get; set; }

        public PreferentialLine()
        {
            Start = null;
            End = null;
            Next = null;
        }

        public bool Empty()
        {

            if (Start == null && End == null)
                return true;
            else
                return false;

        }

        public void PreferentialInsertion(Person person)
        {

            if (Empty())
            {
                Start = person;
                End = person;
            }
            else
            {
                End.Next = person;
                End = person;
            }

        }

        public int PreferentialCounter()
        {
            int count = 0;
            Person person = Start;

            if (Empty())
            {
                return 0;
            }
            else
            {

                do
                {
                    person = person.Next;
                    count++;
                }
                while (person != null);


            }

                return count;
        }

        public Person PreferentialHold()
        {
            Person person = null;

            if (Empty())
            {
                person = null;
            }
            else
            {
                person = Start;
                Start = Start.Next;
            }

            if (Start == null)
                End = null;

            return person;

        }
    }
}
