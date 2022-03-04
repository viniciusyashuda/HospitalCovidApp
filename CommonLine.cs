using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCovidApp
{
    internal class CommonLine
    {
        public Person Start { get; set; }

        public Person End { get; set; }

        public Person Next { get; set; }

        public CommonLine()
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

        public void CommonInsertion(Person person)
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

        public int CommonCounter()
        {
            int count = 0;
            Person person = Start;

            if (Empty())
            {
                count = 0;
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

        public Person CommonHold()
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
