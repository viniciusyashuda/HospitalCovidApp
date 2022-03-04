using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HospitalCovidApp
{
    internal class ControlClass
    {

        public int CommonPasswordCounter { get; set; }

        public int PreferentialPasswordCounter { get; set; }

        public int EmergencyCounter { get; set; }

        public int Next { get; set; }

        public PreferentialLine PreferentialLine { get; set; }

        public CommonLine CommonLine { get; set; }

        public InpatientWard InpatientWard { get; set; }

        public ExamsWard ExamsWard { get; set; }

        public ControlClass(int beds)
        {
            PreferentialLine = new PreferentialLine();
            CommonLine = new CommonLine();
            InpatientWard = new InpatientWard(beds);
            PreferentialPasswordCounter = 1;
            CommonPasswordCounter = 1;
            EmergencyCounter = 0;
            Next = 1;
        }

        public void GetPassword()
        {

            Person person = null;
            int day, month, year;
            string password = "", cpf = "", age, birthDateToString, entryDateTime;
            string preferentialPath = @"C:\5by5\BasicHealthUnit\Preferential\", commonPath = @"C:\5by5\BasicHealthUnit\Common\";

            entryDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            Console.WriteLine("Birth date: ");

            Console.Write("Day: ");
            day = int.Parse(Console.ReadLine());

            Console.Write("Month: ");
            month = int.Parse(Console.ReadLine());

            Console.Write("Year: ");
            year = int.Parse(Console.ReadLine());

            DateTime birthDate = new DateTime(year, month, day);

            birthDateToString = birthDate.ToString("dd/MM/yyyy");

            age = Convert.ToString(Math.Floor(DateTime.Today.Subtract(birthDate).TotalDays / 365));


            Console.WriteLine("CPF: ");
            cpf = Console.ReadLine();


            if (int.Parse(age) > 59)
            {

                password = "P-" + PreferentialPasswordCounter;

                Console.WriteLine("Your password is " + password);

                Console.ReadKey();

                person = new Person();

                person.Age = age;
                person.CPF = cpf;
                person.Password = password;

                PreferentialLine.PreferentialInsertion(person);

                PreferentialPasswordCounter++;

                using (StreamWriter sw = new StreamWriter(preferentialPath + password + "_" + cpf + ".txt"))
                {

                    sw.WriteLine(cpf);
                    sw.WriteLine(birthDateToString);
                    sw.WriteLine(age);
                    sw.WriteLine(password);
                    sw.WriteLine(entryDateTime);

                }

            }
            else
            {

                password = "C-" + CommonPasswordCounter;

                Console.WriteLine("Your password is " + password);

                Console.ReadKey();

                person = new Person();

                person.Age = age;
                person.CPF = cpf;
                person.Password = password;

                CommonLine.CommonInsertion(person);

                CommonPasswordCounter++;

                using (StreamWriter sw = new StreamWriter(commonPath + password + "_" + cpf + ".txt"))
                {

                    sw.WriteLine(cpf);
                    sw.WriteLine(birthDateToString);
                    sw.WriteLine(age);
                    sw.WriteLine(password);
                    sw.WriteLine(entryDateTime);

                }

            }

            person.BirthDate = birthDateToString;
            person.EntryDateTime = entryDateTime;


        }

        public void Call()
        {
            bool control = true;
            Person patient = null;

            do
            {

                if (CommonLine.CommonCounter() == 0 && PreferentialLine.PreferentialCounter() == 0)
                {
                    Console.WriteLine("There is no one in the queue!");
                    control = false;
                }
                else if (PreferentialLine.PreferentialCounter() > 0 && Next < 3)
                {
                    patient = PreferentialLine.PreferentialHold();
                    Triage(patient);
                    DeleteData(patient.CPF, patient.Password, "_", "preferential");
                    Next++;
                    control = false;
                }
                else if (CommonLine.CommonCounter() > 0 && Next == 3)
                {
                    patient = CommonLine.CommonHold();
                    Triage(patient);
                    DeleteData(patient.CPF, patient.Password, "_", "common");
                    Next = 1;
                    control = false;
                }
                else if (PreferentialLine.PreferentialCounter() < 1)
                {
                    Next = 3;
                }

            }
            while (control != false);
            Console.ReadKey();
        }

        public void Triage(Person person)
        {

            string name, daysSinceSymptomsStarted = "0", comorbidities, symptoms, hasComorbidities, hasSymptoms, gender, temperature, saturation;


            Console.WriteLine("Starting now the triage, here will be saved the patient's data...");


            Console.Write("Name:");
            name = Console.ReadLine();
            person.Name = name;

            Console.Write("Gender: ");
            gender = Console.ReadLine();
            person.Gender = gender;


            Console.WriteLine("Is the patient having symptons? 1- Yes or 2- No");
            hasSymptoms = Console.ReadLine();

            if (int.Parse(hasSymptoms) == 1)
            {

                Console.Write("Symptons: ");
                symptoms = Console.ReadLine();
                person.Symptoms = symptoms;

                Console.Write("Days since the symptons started: ");
                daysSinceSymptomsStarted = Console.ReadLine();
                person.DaysSinceSymptomsStarted = daysSinceSymptomsStarted;

            }


            Console.WriteLine("Does the patient has any comorbidity? 1- Yes or 2- No");
            hasComorbidities = Console.ReadLine();

            if (int.Parse(hasComorbidities) == 1)
            {

                Console.WriteLine("Comorbities: ");
                comorbidities = Console.ReadLine();
                person.Comorbidities = comorbidities;

            }


            Console.Write("Temperature: ");
            temperature = Console.ReadLine();
            person.Temperature = temperature;


            Console.Write("Saturation: ");
            saturation = Console.ReadLine();
            person.Saturation = saturation;


            if ((int.Parse(hasComorbidities) == 2) && (int.Parse(hasSymptoms) == 2) && (int.Parse(temperature) < 37) && (int.Parse(saturation) > 90))
            {
                Console.WriteLine("The conditions indicates that it is not covid!");
                RecordData(person, "history");
            }
            else
            {
                if ((int.Parse(hasComorbidities) == 1) && (int.Parse(hasSymptoms) == 1) && (int.Parse(temperature) > 36) && (int.Parse(saturation) < 90))
                {

                    Console.WriteLine("This patient needs to be hospitalized!");
                    InpatientWard.InsertionInpatientWard(person);
                    RecordData(person, "hospitalization");

                }
                else if ((int.Parse(hasComorbidities) == 1) && (int.Parse(hasSymptoms) == 1) && (int.Parse(person.Age) > 59))
                {

                    Console.WriteLine("This patient needs to be hospitalized!");
                    InpatientWard.InsertionInpatientWard(person);
                    RecordData(person, "hospitalization");

                }
                else if ((int.Parse(hasComorbidities) == 1 || int.Parse(hasSymptoms) == 1) && (int.Parse(temperature) > 36 || int.Parse(saturation) < 90))
                {

                    Console.WriteLine("It is necessary to do a covid-19 exam!");
                    ExamsWard.InsertionExamsWard(person);
                    RecordData(person, "exams");

                }
                else if ((int.Parse(daysSinceSymptomsStarted) < 16) && (int.Parse(daysSinceSymptomsStarted) > 2))
                {
                    Console.WriteLine("It is necessary to do a covid-19 exam!");
                    ExamsWard.InsertionExamsWard(person);
                    RecordData(person, "exams");

                }
            }
        }

        public void CovidExam()
        {

            Person suspect = null;

            suspect = ExamsWard.ExamsWardHold();

            string examType, examResult, condition;


            if (suspect == null)
            {

                Console.WriteLine("There is no one waiting to take the covid exam!");

            }
            else
            {

                Console.WriteLine("Starting now the Covid-19 exam...");

                Console.WriteLine("What type of exam the patient wants to do? 1- PCR or 2- Antigen or 3- X-Ray");

                examType = Console.ReadLine();
                suspect.ExamType = examType;

                if (int.Parse(examType) == 1)
                {

                    Console.WriteLine("PCR exam...");

                    Console.WriteLine("Exam result: 1- Positive or 2- Negative");

                    examResult = Console.ReadLine();
                    suspect.ExamResult = examResult;

                    if (int.Parse(examResult) == 2)
                    {

                        Console.WriteLine("It is everything ok, the pacient can go home without any problem!");
                        RecordData(suspect, "history");
                        DeleteData(suspect.CPF, suspect.Password, "_", "exam");


                    }
                    else if (int.Parse(examResult) == 1)
                    {

                        Console.WriteLine("The condition of the patient is: 1- Normal or 2- Serious");

                        condition = Console.ReadLine();

                        if (int.Parse(condition) == 1)
                        {

                            Console.WriteLine("The patient is with the virus, but nothing serious! So, he must go home and remain in social isolation for 14 days!");
                            RecordData(suspect, "history");
                            DeleteData(suspect.CPF, suspect.Password, "_", "exam");

                        }
                        else if (int.Parse(condition) == 2)
                        {

                            Console.WriteLine("The patient is with the virus and his condition is serious, he will need to be hospitalizated!");

                            InpatientWard.InsertionInpatientWard(suspect);
                            RecordData(suspect, "hospitalization");
                            DeleteData(suspect.CPF, suspect.Password, "_", "exams");

                        }

                    }


                }
                else if (int.Parse(examType) == 2)
                {

                    Console.WriteLine("Antigen exam...");

                    Console.WriteLine("Exam result: 1- Positive or 2- Negative");

                    examResult = Console.ReadLine();
                    suspect.ExamResult = examResult;

                    if (int.Parse(examResult) == 2)
                    {

                        Console.WriteLine("It is everything ok, the pacient can go home without any problem!");
                        RecordData(suspect, "history");
                        DeleteData(suspect.CPF, suspect.Password, "_", "exam");

                    }
                    else if (int.Parse(examResult) == 1)
                    {

                        Console.WriteLine("The condition of the patient is: 1- Normal or 2- Serious");

                        condition = Console.ReadLine();

                        if (int.Parse(condition) == 1)
                        {

                            Console.WriteLine("The patient is with the virus, but nothing serious! So, he must go home and remain in social isolation for 14 days!");
                            RecordData(suspect, "history");
                            DeleteData(suspect.CPF, suspect.Password, "_", "exam");

                        }
                        else if (int.Parse(condition) == 2)
                        {

                            Console.WriteLine("The patient is with the virus and his condition is serious, he will need to be hospitalizated!");

                            InpatientWard.InsertionInpatientWard(suspect);
                            RecordData(suspect, "hospitalization");
                            DeleteData(suspect.CPF, suspect.Password, "_", "exam");

                        }

                    }

                }
                else if (int.Parse(examType) == 3)
                {

                    Console.WriteLine("X-Ray exam...");
                    Console.WriteLine("Exam result: 1- Positive or 2- Negative");

                    examResult = Console.ReadLine();
                    suspect.ExamResult = examResult;

                    if (int.Parse(examResult) == 2)
                    {

                        Console.WriteLine("It is everything ok, the pacient can go home without any problem!");
                        RecordData(suspect, "history");
                        DeleteData(suspect.CPF, suspect.Password, "_", "exam");

                    }
                    else if (int.Parse(examResult) == 1)
                    {

                        Console.WriteLine("The condition of the patient is: 1- Normal or 2- Serious");

                        condition = Console.ReadLine();

                        if (int.Parse(condition) == 1)
                        {

                            Console.WriteLine("The patient is with the virus, but nothing serious! So, he must go home and remain in social isolation for 14 days!");
                            RecordData(suspect, "history");
                            DeleteData(suspect.CPF, suspect.Password, "_", "exam");

                        }
                        else if (int.Parse(condition) == 2)
                        {

                            Console.WriteLine("The patient is with the virus and his condition is serious, he will need to be hospitalizated!");

                            InpatientWard.InsertionInpatientWard(suspect);
                            RecordData(suspect, "hospitalization");
                            DeleteData(suspect.CPF, suspect.Password, "_", "exam");

                        }

                    }

                }

            }

        }

        public void Discharge()
        {

            Person patient = null;

            int patientsHospitalized = InpatientWard.BedCounter();
            int option = 0;

            if (patientsHospitalized < 1)
            {
                Console.WriteLine("There is no one hospitalized!");
            }
            else
            {

                patient = InpatientWard.FirstBed;

                for (int aux = 0; aux < patientsHospitalized; aux++)
                {

                    Console.WriteLine(patient.ToString());

                    Console.WriteLine("Is this patient the one you want to discharge? 1- Yes or 2- No");
                    option = int.Parse(Console.ReadLine());

                    if (option == 1)
                    {

                        InpatientWard.BedHold(patient);
                        patient.Hospitalization = "Discharged";
                        RecordData(patient, "history");
                        DeleteData(patient.CPF, patient.Password, "_", "hospitalization");
                        Console.ReadKey();


                        if (InpatientWard.InpatientWardCounter() > 0)
                        {
                            patient = InpatientWard.InpatientWardHold();
                            InpatientWard.InsertionInpatientWard(patient);
                            RecordData(patient, "hospitalization");

                        }

                        break;

                    }
                    else
                    {
                        patient = patient.Next;
                    }

                }

            }

        }

        public void Emergency()
        {

            Person inEmergency = null;

            EmergencyCounter++;

            int day, month, year;
            string password = "E-" + EmergencyCounter, age, birthDateToString, gender, entryDateTime, name, cpf, hasSymptoms, symptoms, comorbidities, hasComorbidities, temperature, saturation, daysSinceSymptonsStarted;
            entryDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            Console.WriteLine("Name: ");
            name = Console.ReadLine();

            Console.WriteLine("CPF: ");
            cpf = Console.ReadLine();

            inEmergency = new Person();
            inEmergency.Password = password;
            inEmergency.CPF = cpf;
            inEmergency.Name = name;
            inEmergency.EntryDateTime = entryDateTime;

            InpatientWard.InsertionInpatientWard(inEmergency);
            Console.WriteLine("The patient was moved to the inpatient ward");
            Console.ReadKey();
            Console.Clear();


            Console.WriteLine("Birth date:");

            Console.Write("Day: ");
            day = int.Parse(Console.ReadLine());

            Console.Write("Month: ");
            month = int.Parse(Console.ReadLine());

            Console.Write("Year: ");
            year = int.Parse(Console.ReadLine());

            DateTime birthDate = new DateTime(year, month, day);

            birthDateToString = birthDate.ToString("dd/MM/yyyy");

            inEmergency.BirthDate = birthDateToString;

            age = Convert.ToString(Math.Floor(DateTime.Today.Subtract(birthDate).TotalDays / 365));
            inEmergency.Age = age;

            Console.Write("Gender: ");
            gender = Console.ReadLine();
            inEmergency.Gender = gender;


            Console.WriteLine("Is the patient having symptons? 1- Yes or 2- No");
            hasSymptoms = Console.ReadLine();

            if (int.Parse(hasSymptoms) == 1)
            {

                Console.Write("Symptons: ");
                symptoms = Console.ReadLine();
                inEmergency.Symptoms = symptoms;

                Console.Write("Days since the symptons started: ");
                daysSinceSymptonsStarted = Console.ReadLine();
                inEmergency.DaysSinceSymptomsStarted = daysSinceSymptonsStarted;

            }


            Console.WriteLine("Does the patient has any comorbidity? 1- Yes or 2- No");
            hasComorbidities = Console.ReadLine();

            if (int.Parse(hasComorbidities) == 1)
            {

                Console.WriteLine("Comorbities: ");
                comorbidities = Console.ReadLine();
                inEmergency.Comorbidities = comorbidities;

            }


            Console.Write("Temperature: ");
            temperature = Console.ReadLine();
            inEmergency.Temperature = temperature;


            Console.Write("Saturation: ");
            saturation = Console.ReadLine();
            inEmergency.Saturation = saturation;

            RecordData(inEmergency, "hospitalization");


        }

        public void UploadData()
        {

            string examsPath = @"C:\5by5\BasicHealthUnit\Exams\", hospitalizationPath = @"C:\5by5\BasicHealthUnit\Hospitalizated\", historyPath = @"C:\5by5\BasicHealthUnit\History\";
            string preferentialPath = @"C:\5by5\BasicHealthUnit\Preferential\", commonPath = @"C:\5by5\BasicHealthUnit\Common\";

            Person person = null;
            string[] data = new string[16];
            int i = -1;

            Console.WriteLine("\n File recovery system");

            // recupera fila preferencial
            foreach (string file in Directory.GetFiles(preferentialPath))
            {
                i = -1;
                using (StreamReader sr = new StreamReader(file))
                {
                    do
                    {
                        i++;
                        data[i] = sr.ReadLine();
                    } while (data[i] != null);
                }

                person = new Person();

                person.CPF = data[0];
                person.BirthDate = data[1];
                person.Age = data[2];
                person.Password = data[3];
                person.EntryDateTime = data[4];

                PreferentialLine.PreferentialInsertion(person);
            }

            // recupera fila principal
            foreach (string file in Directory.GetFiles(commonPath))
            {
                i = -1;
                using (StreamReader sr = new StreamReader(file))
                {
                    do
                    {
                        i++;
                        data[i] = sr.ReadLine();
                    } while (data[i] != null);
                }

                person = new Person();

                person.CPF = data[0];
                person.BirthDate = data[1];
                person.Age = data[2];
                person.Password = data[3];
                person.EntryDateTime = data[4];

                CommonLine.CommonInsertion(person);

            }

            // recupera fila exame
            foreach (string file in Directory.GetFiles(examsPath))
            {
                i = -1;
                using (StreamReader sr = new StreamReader(file))
                {
                    do
                    {
                        i++;
                        data[i] = sr.ReadLine();
                    } while (data[i] != null);
                }

                person = new Person();

                person.Name = data[0];
                person.CPF = data[1];
                person.BirthDate = data[2];
                person.Age = data[3];
                person.Gender = data[4];
                person.EntryDateTime = data[5];
                person.Password = data[7];
                person.Temperature = data[8];
                person.Saturation = data[9];
                person.Comorbidities = data[10];
                person.Symptoms = data[11];
                person.DaysSinceSymptomsStarted = data[12];
                person.ExamType = data[13];
                person.ExamResult = data[14];
                person.Hospitalization = data[15];

                ExamsWard.InsertionExamsWard(person);
            }

            // recupera fila hospital
            foreach (string file in Directory.GetFiles(hospitalizationPath))
            {
                i = -1;
                using (StreamReader sr = new StreamReader(file))
                {
                    do
                    {
                        i++;
                        data[i] = sr.ReadLine();
                    } while (data[i] != null);
                }

                person = new Person();

                person.Name = data[0];
                person.CPF = data[1];
                person.BirthDate = data[2];
                person.Age = data[3];
                person.Gender = data[4];
                person.EntryDateTime = data[5];
                person.Password = data[7];
                person.Temperature = data[8];
                person.Saturation = data[9];
                person.Comorbidities = data[10];
                person.Symptoms = data[11];
                person.DaysSinceSymptomsStarted = data[12];
                person.ExamType = data[13];
                person.ExamResult = data[14];
                person.Hospitalization = data[15];

                InpatientWard.InsertionInpatientWard(person);
            }

            Console.WriteLine("\n\n\n Files were sucessfully recovered\n");
            Console.WriteLine("  Every patient were sucessfully moved to the correct line");
            Console.WriteLine("\n Press [ENTER] to continue");
            Console.ReadKey();
        }

        public void DeleteData(string cpf, string password, string underline, string sector)
        {
            underline = "_";
            string examsPath = @"C:\5by5\BasicHealthUnit\Exams\", hospitalizationPath = @"C:\5by5\BasicHealthUnit\Hospitalizated\", historyPath = @"C:\5by5\BasicHealthUnit\History\";
            string preferentialPath = @"C:\5by5\BasicHealthUnit\Preferential\", commonPath = @"C:\5by5\BasicHealthUnit\Common\";

            string data = "";

            if (sector == "exams")
                data = examsPath + password + underline + cpf + ".txt";
            if (sector == "hospitalization")
                data = hospitalizationPath + password + underline + cpf + ".txt";
            if (sector == "preferential")
                data = preferentialPath + password + underline + cpf + ".txt";
            if (sector == "common")
                data = commonPath + password + underline + cpf + ".txt";

            File.Delete(data);

        }

        public void RecordData(Person person, string sector)
        {
            string examsPath = @"C:\5by5\BasicHealthUnit\Exams\", hospitalizationPath = @"C:\5by5\BasicHealthUnit\Hospitalizated\", historyPath = @"C:\5by5\BasicHealthUnit\History\";
            string preferentialPath = @"C:\5by5\BasicHealthUnit\Preferential\", commonPath = @"C:\5by5\BasicHealthUnit\Common\";

            if (sector == "history")
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(historyPath + person.Password + person.CPF + ".txt"))
                    {
                        sw.WriteLine(person.Name);
                        sw.WriteLine(person.CPF);
                        sw.WriteLine(person.BirthDate);
                        sw.WriteLine(person.Age);
                        sw.WriteLine(person.Gender);
                        sw.WriteLine(person.EntryDateTime);
                        sw.WriteLine(person.Password);
                        sw.WriteLine(person.Temperature);
                        sw.WriteLine(person.Saturation);
                        sw.WriteLine(person.Comorbidities);
                        sw.WriteLine(person.Symptoms);
                        sw.WriteLine(person.DaysSinceSymptomsStarted);
                        sw.WriteLine(person.ExamType);
                        sw.WriteLine(person.ExamResult);
                        sw.WriteLine(person.Hospitalization);
                    }
                }
                catch
                {
                    Console.WriteLine("\n Something went wrong");
                    Console.WriteLine("\n Press [ENTER] to continue");
                    Console.ReadKey();
                }
            }
            else if (sector == "hospitalization")
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(hospitalizationPath + person.Password + person.CPF + ".txt"))
                    {
                        sw.WriteLine(person.Name);
                        sw.WriteLine(person.CPF);
                        sw.WriteLine(person.BirthDate);
                        sw.WriteLine(person.Age);
                        sw.WriteLine(person.Gender);
                        sw.WriteLine(person.EntryDateTime);
                        sw.WriteLine(person.Password);
                        sw.WriteLine(person.Temperature);
                        sw.WriteLine(person.Saturation);
                        sw.WriteLine(person.Comorbidities);
                        sw.WriteLine(person.Symptoms);
                        sw.WriteLine(person.DaysSinceSymptomsStarted);
                        sw.WriteLine(person.ExamType);
                        sw.WriteLine(person.ExamResult);
                        sw.WriteLine(person.Hospitalization);
                    }
                }
                catch
                {
                    Console.WriteLine("\n Something went wrong");
                    Console.WriteLine("\n Press [ENTER] to continue");
                    Console.ReadKey();
                }
            }
            else if (sector == "exams")
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(examsPath + person.Password + person.CPF + ".txt"))
                    {
                        sw.WriteLine(person.Name);
                        sw.WriteLine(person.CPF);
                        sw.WriteLine(person.BirthDate);
                        sw.WriteLine(person.Age);
                        sw.WriteLine(person.Gender);
                        sw.WriteLine(person.EntryDateTime);
                        sw.WriteLine(person.Password);
                        sw.WriteLine(person.Temperature);
                        sw.WriteLine(person.Saturation);
                        sw.WriteLine(person.Comorbidities);
                        sw.WriteLine(person.Symptoms);
                        sw.WriteLine(person.DaysSinceSymptomsStarted);
                        sw.WriteLine(person.ExamType);
                        sw.WriteLine(person.ExamResult);
                        sw.WriteLine(person.Hospitalization);
                    }
                }
                catch
                {
                    Console.WriteLine("\n Something went wrong");
                    Console.WriteLine("\n Press [ENTER] to continue");
                    Console.ReadKey();
                }
            }
        }
    }
}