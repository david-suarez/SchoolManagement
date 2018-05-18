using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using SchoolManagement.Exceptions;
using SchoolManagement.Factory;
using SchoolManagement.Validators;

namespace SchoolManagement
{
    public class Program
    {
        private static DataTable SchoolData = null;

        private static string FileName = string.Empty;

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
            if (SchoolData != null || SchoolData.Rows.Count > 0)
            {
                var queryValidator = new QueryValidator();
                Console.WriteLine("The query must have the next format, ex: name=John type=high");
                Console.Write("Enter the query: ");
                var userInput = Console.ReadLine();
                try
                {
                    queryValidator.Validate(userInput);

                }
                catch (QueryNotMatchException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Feature not implemented yet");
            Console.ReadKey();
        }

        private static void AddNewStudent()
        {
            Console.WriteLine("Feature not implemented yet");
            Console.ReadKey();

        }

        private static void LoadDataFromCsvFileAndSearch()
        {
            Console.WriteLine("Feature not implemented yet");
            Console.ReadKey();
        }

        private static void ExitApplication()
        {
            throw new ApplicationException("exit application");
        }

        private static void LoadDataFromCsvFile()
        {
            Log.Info("Load data from csv File");
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
                    return;
                }
                else if (userInput != "1" && userInput != "1.")
                {
                    Console.WriteLine("Invalid option.");
                    Console.ReadKey();
                    return;
                }
            }

            Console.Clear();
            Console.WriteLine("== Load data from csv file ==");
            Console.Write("Enter the full path of the csv file: ");
            var filePath = Console.ReadLine();
            var dataReaderFactory = new DataReaderFactory(
                filePath,
                new CompositeValidator<string>(
                    new PathValidator(),
                    new FileValidator()));
            SchoolData = dataReaderFactory.Get().ReadData();
            FileName = Path.GetFileName(filePath);
            PrintDataLoaded();
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

                foreach (var row in currentRows)
                {
                    foreach (DataColumn column in SchoolData.Columns)
                    {
                        Console.Write("\t{0}", row[column]);
                    }
                }
            }
            Console.WriteLine($"\nTotal rows: {currentRows.Length}");
            Console.ReadKey();
        }
    }
}
