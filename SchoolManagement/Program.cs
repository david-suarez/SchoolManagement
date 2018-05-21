using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;

using SchoolManagement.Exceptions;
using SchoolManagement.Factory;
using SchoolManagement.Mapping;
using SchoolManagement.Models;
using SchoolManagement.Validators;
using SchoolManagement.Repository;

namespace SchoolManagement
{
    public class Program
    {
        private static DataTable SchoolData = null;

        private static string FileName = string.Empty;

        private static Repository<Student> StudentRepository = null;

        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            Log.Info("Application started");
            while (true)
            {
                var exitApp = PrintUserMenu();
                if (exitApp)
                {
                    break;
                }
            }
        }

        private static bool PrintUserMenu()
        {
            var quitApp = false;
            var commands = new Dictionary<string, Action>();
            commands.Add("1", LoadDataFromCsvFile);
            commands.Add("2", LoadDataFromCsvFileAndSearch);
            commands.Add("3", AddNewStudent);
            commands.Add("4", SearchRecord);
            commands.Add("5", ExitApplication);

            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"SCHOOL MANAGEMENT");
                Console.WriteLine();
                Console.WriteLine("=========== MENU ============");
                Console.WriteLine("1. Load data from csv file");
                Console.WriteLine("2. Load data from csv and search");
                Console.WriteLine("3. Add new student record");
                Console.WriteLine("4. Search");
                Console.WriteLine("5. Exit");
                Console.Write("Please select an option: ");
                var userInput = Console.ReadLine();
                quitApp = userInput == "5";

                if (commands.ContainsKey(userInput))
                {
                    try
                    {
                        commands[userInput].Invoke();
                    }
                    catch (ApplicationException exc)
                    {
                        Log.Info($"Command return, {exc.Message}");
                        break;
                    }
                    catch (FileNotFoundException exc)
                    {
                        Log.Info($"File not found, {exc.Message}");
                        Console.WriteLine("The file was not found or not exists. Please try again.");
                        Console.ReadKey();
                    }
                    catch (NotSupportedException exc)
                    {
                        Log.Info($"File is not supported, {exc.Message}");
                        Console.WriteLine("The file that you are trying to upload is not supported yet.");
                        Console.ReadKey();
                    }
                    catch (InvalidPathException exc)
                    {
                        Log.Info($"The path is invalid, {exc.Message}");
                        Console.WriteLine("The path of the file is not correct. Please try again.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid option");
                    Console.ReadLine();
                }
            }

            return quitApp;
        }

        private static void SearchRecord()
        {
            Log.Info("Search records from loaded data");
            if (SchoolData != null)
            {
                var properties = typeof(Student).GetProperties();
                Console.Clear();
                Console.WriteLine("The query must have the next format, ex: name=John type=high");
                Console.WriteLine("Use the next properties for the query(case sensitive):");
                foreach (var property in properties)
                {
                    Console.WriteLine(property.Name);
                }
                Console.Write("Enter the query: ");
                var userInput = Console.ReadLine();
                ExecuteSearchQuery(userInput);
            }
            else
            {
                Console.WriteLine("There is not data loaded. Please load data to search over it.");
            }

            Console.ReadKey();
        }

        private static void ExecuteSearchQuery(string userInput)
        {
            var queryValidator = new QueryValidator();
            try
            {
                queryValidator.Validate(userInput);
                var queryGenerator = new StudentQueryGenerator();
                PrintFoundData(StudentRepository.Find(queryGenerator.Generate(userInput)));
            }
            catch (QueryNotMatchException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            catch (ParseException ex)
            {
                Console.WriteLine("The query generator is key sensitive, please use the properties listed previously.");
                Console.ReadKey();
            }
        }

        private static void PrintFoundData(IEnumerable<Student> foundData)
        {
            foreach (var property in typeof(Student).GetProperties())
            {
                Console.Write("\t{0}", property.Name);
            }

            Console.WriteLine();
            if (!foundData.Any()) return;
            foreach (var student in foundData.OrderBy(s => s.Name).ToList())
            {
                Console.Write("\t{0}", student.Type);
                Console.Write("\t{0}", student.Name);
                Console.Write("\t{0}", student.Gender);
                Console.Write("\t{0}", student.LastUpdate);
                Console.WriteLine();
            }
        }

        private static void AddNewStudent()
        {
            var properties = typeof(Student).GetProperties();
            var newStudent = new Student();
            var userEntry = string.Empty;

            Console.Clear();
            Console.WriteLine("== Add new record ==");
            if (SchoolData == null)
            {
                Console.WriteLine("No data was loaded. To start adding new records please load data first.");
                Console.ReadKey();
                return;
            }

            foreach (var property in properties)
            {
                if (property.Name == "LastUpdate")
                {
                    var propertySet = false;
                    while (!propertySet)
                    {
                        Console.Write($"Please enter the {property.Name}"
                                      + $" in format <year><month><day><hour><minute><second>(yyyyMMddHHmmss): ");
                        userEntry = Console.ReadLine();
                        try
                        {
                            newStudent.LastUpdate = DateTime.ParseExact(
                                userEntry?.Trim() ?? string.Empty,
                                "yyyyMMddHHmmss",
                                System.Globalization.CultureInfo.InvariantCulture);
                            propertySet = true;
                        }
                        catch (ParseException e)
                        {
                            Console.WriteLine("Date format is wrong for field LastUpdate");
                        }
                    }
                }

                if (property.Name == "Name")
                {
                    Console.Write($"Please enter the {property.Name}: ");
                    userEntry = Console.ReadLine();
                    newStudent.Name = userEntry?.Trim() ?? string.Empty;
                }

                if (property.Name == "Type")
                {
                    Console.Write($"Please enter the {property.Name}: ");
                    userEntry = Console.ReadLine();
                    newStudent.Type = userEntry?.Trim() ?? string.Empty;
                }

                if (property.Name == "Gender")
                {
                    Console.Write($"Please enter the {property.Name} (M|F): ");
                    userEntry = Console.ReadLine();
                    newStudent.Gender = userEntry?.Trim() ?? string.Empty;
                }
            }

            StudentRepository.Add(newStudent);
            Console.WriteLine("New record of student was created.");
            Console.ReadKey();
        }

        private static void LoadDataFromCsvFileAndSearch()
        {
            Log.Info("Load data and Search from csv File");
            var isDataLoaded = SchoolData != null;
            if (isDataLoaded && VerifyOverwriteDataLoaded())
            {
                return;
            }

            Console.Clear();
            Console.WriteLine("== Load data and search from csv file ==");
            Console.WriteLine("Enter the full path of the csv file and the query separated by a pipe(|)");
            Console.WriteLine("ex: \"c:\\data.csv | Name=John \"");
            var properties = typeof(Student).GetProperties();
            Console.WriteLine("Use the next properties for the query(case sensitive):");
            foreach (var property in properties)
            {
                Console.WriteLine(property.Name);
            }

            Console.Write("Please enter your request: ");
            var userEntry = Console.ReadLine();
            var splitedUserEntry = userEntry.Split('|');
            if (splitedUserEntry.Length == 2)
            {
                var filePath = userEntry.Split('|')[0].Trim();
                var query = userEntry.Split('|')[1].Trim();
                LoadDataFromCsvFile(filePath);
                ExecuteSearchQuery(query);
            }
            else
            {
                Console.WriteLine("Entry invalid. Please try again.");
            }

            Console.ReadKey();
        }

        private static void ExitApplication()
        {
            throw new ApplicationException("exit application");
        }

        private static void LoadDataFromCsvFile()
        {
            Log.Info("Load data from csv File");
            var isDataLoaded = SchoolData != null; 
            if (isDataLoaded && VerifyOverwriteDataLoaded())
            {
                return;
            }

            Console.Clear();
            Console.WriteLine("== Load data from csv file ==");
            Console.Write("Enter the full path of the csv file: ");
            var filePath = Console.ReadLine();
            LoadDataFromCsvFile(filePath);
            PrintDataLoaded();
        }

        private static void LoadDataFromCsvFile(string filePath)
        {
            var dataReaderFactory = new DataReaderFactory(
                filePath,
                new CompositeValidator<string>(
                    new PathValidator(),
                    new FileValidator()));
            SchoolData = dataReaderFactory.Get().ReadData();
            StudentRepository = new Repository<Student>(SchoolData, new MappingStudent());
            FileName = Path.GetFileName(filePath);
        }

        private static bool VerifyOverwriteDataLoaded()
        {
            if (SchoolData != null)
            {
                Console.Clear();
                Console.WriteLine($"There is already a file loaded ({FileName}). Do you want to overwrite the data?");
                Console.WriteLine("1. Yes  ");
                Console.WriteLine("2. No  ");
                Console.Write("Please select an option: ");
                var userInput = Console.ReadLine();
                if (userInput == "2" || userInput == "2.")
                {
                    return true;
                }
                else if (userInput != "1" && userInput != "1.")
                {
                    Console.WriteLine("Invalid option.");
                    Console.ReadKey();
                    return true;
                }
            }

            return false;
        }

        private static void PrintDataLoaded()
        {
            var currentRows = SchoolData.Select(
                null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Current Rows Found");
            }
            else
            {
                foreach (DataColumn column in SchoolData.Columns)
                {
                    Console.Write("\t{0}", column.ColumnName);
                }

                Console.WriteLine();
                foreach (var row in currentRows)
                {
                    foreach (DataColumn column in SchoolData.Columns)
                    {
                        Console.Write("\t{0}", row[column]);
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine($"\nTotal rows: {currentRows.Length}");
            Console.ReadKey();
        }
    }
}
