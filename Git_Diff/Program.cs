using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git_Diff
{
    class Program
    {
        static void Main(string[] args)
        {
            while(App()) {} //The core loop of the application so it can run infinetly
        }
        public static bool App() //The start of the application
        {
            Console.WriteLine("Welcome to this Compare Files Application\n"); // Welcome message with arrows showing user where text will show
            Console.WriteLine("To use this application please use 'diff' followed by the names of the two files you wish to compare with a space seperating them'");
            Console.WriteLine("Don't forget to add the filename extension, i.e '.txt'");
            Console.Write(">>>");
            string[] input = Console.ReadLine().Split(' '); //Input is asked for from the user

            //Series of commands for the user to use
            switch (input[0])
            {
                case "diff": //Diff command for the user to compare two files
                    if (input.Count() == 3) Diff(input[1], input[2]); 
                    else Functions.InvalidArgument("diff", input.Count()-1, 2); 
                    break;
                case "exit": //Exit command to terminate the program
                    if (input.Count() == 1) return false; // The loop is stopped, closing the program.
                    else Functions.InvalidArgument("exit", input.Count()-1, 0); 
                    break;
                default: //Error message if unrecognised command entered
                    Functions.InvalidCommand(input[0]); 
                    break;
            }
            Functions.Output("", "Press Enter to continue... ", false, "write"); 
            Console.ReadLine(); 
            return true; //After application is finished will loop back to beginning
        }
        //Method to compare two strings and display the differences to the console
        static void Diff(string filename1, string filename2)
        {
            File file1 = new File(filename1); //The files are loaded into two objects
            File file2 = new File(filename2);
            Console.ForegroundColor = ConsoleColor.Green; //The text colour is set to green.

            string log_file = $"{filename1} - {filename2}";
            int lines;
            if 
                (file1.Count() >= file2.Count()) lines = file1.Count(); 
            else 
                lines = file2.Count();

            for (int i = 0; i < lines; i++) //Loop to iterate through each line of the files.
            {
                bool result = false;
               { 
                    if (file1[i] != file2[i]) result = true; 
               } 
                    if (result)
                    {   
                    List<string> words1 = file1[i].Split(' ').ToList(); //Each line is split into words and stored in lists
                    List<string> words2 = file2[i].Split(' ').ToList();

                    Functions.Output(log_file, $"\n{i+1}", true);
                    Functions.Output(log_file, "| ");

                    for(int j = 0; j < words1.Count(); j++) //Loops through each word in the line
                    {
                        if (words1[j] == words2[j]) 
                        {
                            Functions.Output(log_file, words1[j]+" "); //If the words are the same in both files then it is displayed in green.
                        }
                        else
                        {
                            Functions.Output(log_file, words1[j], true); //Otherwise, it's done in red.
                            Functions.Output("", " ", false, "write"); 
                        }    
                    }
                }
                else 
                {
                   Functions.Output(log_file, $"\n{i+1}");
                   Functions.Output(log_file, "  ", false, "log");
                   Functions.Output(log_file, $"| {file1[i]}");
                   Functions.Output(log_file, "\n");
                }
            }
             
        }
    }
    class File : List<string> 
    {
        //Inherits List from first file uploaded and creates a file that is loaded to the base to be appended later
        public File(string filename)
        {
            if (System.IO.File.Exists($"{filename}")) //Checks if file exists
            {
                AddRange(System.IO.File.ReadAllLines($"{filename}").ToList());//Reads the contents of the file and then adds to this file
            }
            else
            {
                Functions.FileNotFound(filename); //Error message if file does not exist
            }
        }
    }
}
    class Functions
    {
    //What is shown in the console
        private static void Result(string lines, bool modified=false)
        {
            if (!modified) 
            Console.ForegroundColor = ConsoleColor.Green; //Text that is the same has a green font colour
            else 
            Console.ForegroundColor = ConsoleColor.Red; //If text is different font colour is red
            Console.Write(lines);
            Console.ForegroundColor = ConsoleColor.White; //The colour is reset to white and the application goes back to the start
        }
        //Error Handling for Application, all exceptions shown are valid for this application
        public static void FileNotFound(string filename)
        {
            Console.WriteLine($"File Not Found error detected: {filename} not found.");
        }
        public static void InvalidArgument(string command, int arguments_given, int arguments_expected)
        {
            Console.WriteLine($"Invalid Arguement error detected: '{command}' expects {arguments_expected} arguments, but {arguments_given} are given.");
        }
        public static void InvalidCommand(string command)
        {
            Console.WriteLine($"Invalid Command error detected: Command Not Recognised: {command}");
        }
        private static void LogFile(string filename, string text, bool modified=false)
        {
            if (!System.IO.File.Exists($"{filename}"))System.IO.File.AppendAllText($"{filename}", "Asterix marks changed words.\n"); //Appends the file of any changes 
            if (!modified) System.IO.File.AppendAllText($"{filename}", text); 
            else System.IO.File.AppendAllText($"{filename}", $"{text}* "); //Modified text is marked with an asterix to clearly show the user what has changed
        }
        public static void Output(string filename, string text, bool modified=false, string mode="all")
        {
            if (mode == "all" || mode == "write") Result(text, modified); //User is shown result in the console
            if (mode == "all" || mode == "log") LogFile(filename, text, modified); //Result is also logged in bin/debug folder
        }
    }
