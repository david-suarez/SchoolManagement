using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using SchoolManagement.Exceptions;
using SchoolManagement.Factory;
using SchoolManagement.Interfaces;
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
                bool exitApp = PrintUserMenu();
                if (exitApp)
                {
                    break;
                }
            }
        }

        private static bool PrintUserMenu()
        {
            var quitApp = false;
            Dictionary<string, Action> commands = new Dictionary<string, Action>();
            commands.Add("1", new Action(LoadDataFromCsvFile));
            commands.Add("2", new Action(LoadDataFromCsvFileAndSearch));
            commands.Add("3", new Action(AddNewStudent));
            commands.Add("4", new Action(SearchRecord));
            commands.Add("5", new Action(ExitApplication));

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
                    catch (Exception)
                    {
                        throw;
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
            if(SchoolData != null)
            {
                Console.Clear();
                Console.WriteLine($"There is already a file loaded ({FileName}). Do you want to overwrite the data?");
                Console.WriteLine("1. Yes  ");
                Console.WriteLine("2. No  ");
                Console.Write("Please select an option: ");
                var userInput = Console.ReadLine();
                if(userInput == "2" || userInput == "2.")
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
            var validators = new List<IValidator> {
                new PathValidator(filePath),
                new FileValidator(filePath)
            };
            var dataReaderFactory = new DataReaderFactory(filePath, new CompositeValidator(validators));
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

                Console.WriteLine("\tRowState");

                foreach (DataRow row in currentRows)
                {
                    foreach (DataColumn column in SchoolData.Columns)
                        Console.Write("\t{0}", row[column]);

                    Console.WriteLine("\t" + row.RowState);
                }
            }
            Console.WriteLine($"\nTotal rows loaded: {currentRows.Length}");
            Console.ReadKey();
        }
    }
}
