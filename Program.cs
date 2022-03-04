using System;

namespace HospitalCovidApp
{
    internal class Program
    {

        public static int Menu(int bedsAvailable)
        {

            bool control = true;
            int option = 0;
            string choice;

            do
            {
                Console.Clear();

                Console.WriteLine("There are " + bedsAvailable + " beds available!\n");

                Console.WriteLine("Menu");
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1- Get a password ");
                Console.WriteLine("2- Call a person for the triage ");
                Console.WriteLine("3- Call a person to do the covid exam ");
                Console.WriteLine("4- Discharge a patient ");
                Console.WriteLine("5- Emergency ");
                Console.WriteLine("6- Upload data ");
                Console.WriteLine("7- Exit system ");

                choice = Console.ReadLine();

                int.TryParse(choice, out option);

                Console.Clear();

                if (option < 1 || option > 6)
                {
                    Console.WriteLine("Enter a valid option!");
                    Console.ReadKey();
                }
                else
                {
                    control = false;
                }

            }
            while (control != false);

            return option;


        }

        public static int BedsAvailable()
        {
            int beds;

            Console.Write("Beds available: ");
            beds = int.Parse(Console.ReadLine());

            return beds;
        }

        static void Main(string[] args)
        {
            ControlClass controller = null;
            

            bool control = true;

            int bedsAvailable = BedsAvailable();

            int option = 0;

            controller = new ControlClass(bedsAvailable);

            option =  Menu(controller.InpatientWard.Beds);

            do
            {


                switch (option)
                {
                    case 1:

                        controller.GetPassword();

                        option = Menu(controller.InpatientWard.Beds);

                        break;
                    case 2:

                        controller.Call();

                        option = Menu(controller.InpatientWard.Beds);

                        break;
                    case 3:

                        controller.CovidExam();

                        option = Menu(controller.InpatientWard.Beds);

                        break;
                    case 4:

                        controller.Discharge();

                        option = Menu(controller.InpatientWard.Beds);

                        break;
                    case 5:

                        controller.Emergency();

                        option = Menu(controller.InpatientWard.Beds);

                        break;

                    case 6:

                        controller.UploadData();

                        option = Menu(controller.InpatientWard.Beds);

                        break;

                    case 7:

                        control = false;

                        break;
                }
            }
            while (control != false);

        }
    }
}
