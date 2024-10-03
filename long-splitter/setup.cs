﻿using System;
using System.Runtime.CompilerServices;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Net.NetworkInformation;
namespace long_Splitter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int x = getInputInt("enter between 1 and 2",1, 2, 1);
            bool t = stringInFile(".txt");
            int a = getSplitFileSize(500001);
            string form = @"C:\ffishstix\from.txt";
            string e = getFromFile();
            string[] f = [e];
            string c = getToTotal(f);
            float d = getMemoryAvailable();
            
            Console.WriteLine($"input returned: {x}\nfile test worked: {t}\nnew file size: {a}\nto folder: {c}\navailable memory: {d}\nfrom file {e}");
            
        }

        static public float getMemoryAvailable() {
            PerformanceCounter availableMemoryCounter = new PerformanceCounter("Memory", "Available Bytes");
            float availableMemory = availableMemoryCounter.NextValue();
            return availableMemory;
            
        }

        static public int getInputInt(string prompt, int min = 1, int max = 2, int defaultNum = 1) {
            while (true) {
                Console.WriteLine(prompt);
                string userinput = Console.ReadLine();

                if (int.TryParse(userinput, out int number)){
                    if (number >= min && number <= max){
                        return number;
                    }
                    else {
                        Console.WriteLine("number is not within range: " + min + " to " + max);
                        Console.WriteLine("please try again ");
                    
                    }
                }
                else {
                    
                    if (string.IsNullOrWhiteSpace(userinput)) {
                        Console.WriteLine("default selected: " + defaultNum + " ");
                        return defaultNum;

                    }
                    else {
                        Console.WriteLine("input was not num");
                    }

                }
            }

        }

        static public bool stringInFile (string strSearch, string filePath="fileExtensions.txt") {
           try
        {
            // Read the file line by line and check if the string exists
            Console.WriteLine(filePath);
            int count = 0;
            foreach (string line in File.ReadLines(filePath))
            {
                count +=1;
                if (line == strSearch)  // You can use line.Contains(strSearch) for partial matches
                {
                    
                    Console.WriteLine(count);
                    return true;
                    
                }
                
            }return false;

            
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Error: The file was not found.");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
           
        } 
    
        static public int getSplitFileSize(int fromFileSize)
        {
            int x = int.MaxValue;
            while (x > fromFileSize) {
                x = getInputInt("\nenter size of smaller files in bytes (default= 50kb)\n>", 1, fromFileSize-1, 50000);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("as a safety measure, please make sure that you have more memory \nthan the size of each smaller file in bytes");
                Console.WriteLine("press enter to contine: ");
                Console.ReadLine();
                Console.ResetColor();
            }   
            return x;
        }
    
        static bool driveExists(string driveLetter)
        {
            if (!string.IsNullOrEmpty(driveLetter))
            {
                // Use DriveInfo to check if the drive exists
                DriveInfo driveInfo = new DriveInfo(driveLetter.TrimEnd('/')); // Trim any trailing slashes
                return driveInfo.IsReady; // Check if the drive is ready (exists and can be accessed)
            }
            return false;
        }
        static bool isValidFilePath(string path)
        {
            // Regular expression to validate a file path pattern like "C:/[word]/[word]"
            string pattern = @"^[a-zA-Z]:/(?:[^/<>:""|?*]+/)*[^/<>:""|?*]*$";

            // Check if the path matches the regular expression
            if (Regex.IsMatch(path, pattern) && driveExists(path.Substring(0, 2)))
            {
                // Get invalid path characters
                char[] invalidChars = Path.GetInvalidPathChars();  // Correct use of Path class

                // Check if the path contains any invalid characters
                foreach (char c in invalidChars)
                {
                    if (path.Contains(c.ToString()))
                    {
                        return false;
                    }
                }

                // If no invalid characters are found, return true
                return true;
            }
            else
            {
                return false;
            }
        }   

        static public string getToFolder(string form=@"C:\ffishstix\from.txt") {
            string docDir = Path.GetDirectoryName(form);
            string toLocation = "";
            while (true) {
                Console.WriteLine("\n\nFile location to deposit broken down file");
                Console.WriteLine("Must look like:");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("[drive]:/[folder]/[folder]");
                Console.ResetColor();
                Console.WriteLine($"Or press {Console.ForegroundColor = ConsoleColor.Blue}Enter{Console.ForegroundColor = ConsoleColor.Black} for the same location");
                Console.ResetColor();
                Console.Write("> ");

                toLocation = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(toLocation)) {
                    bool z = isValidFilePath(toLocation);
                    if (z) {
                        if (Directory.Exists(toLocation)) {
                        break;
                        }
                        else{  
                            int x = getInputInt("\nYou have two options:\n1. Create the folder\n2. Re-enter the folder location\n> ", 1, 2, 1);
                            if (x == 1) {
                                Directory.CreateDirectory(toLocation);
                                break;
                            }
                        }
                    }else {
                        Console.WriteLine("invalid file please enter again> ");
                    }
                
                }else {
                    toLocation = Path.Combine(Path.GetDirectoryName(form), "o");
                    Console.WriteLine($"default selected: {Path.GetDirectoryName(form)}");
                    if (!Directory.Exists(toLocation)) {
                        Directory.CreateDirectory (toLocation);
                        break;
                    } break;
                }
                    
            }
            return toLocation;
        }     
    
        static public string getFromFile() {
            int count = 0;
            bool isValid = false;
            string final;
            string y;
            string x;

            while (!isValid) {
                count++;
                Console.WriteLine("please input the file that you would like to split");
                Console.WriteLine("\nEnter full file name, including file location, if easier leave blank (will get more options)> ");
                final = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(final)) {
                    if (File.Exists(final)){return final;}
                    else {Console.WriteLine("invalid file restarting now> ");}
                }else {
                    Console.WriteLine("you chose to input the file location then the file name\nplease enter: [drive]:/[folder]/[folder]...\nand then [file].[blah]");
                    y = Console.ReadLine();
                    x = Console.ReadLine();
                    Console.WriteLine(Path.Combine(y, x));
                    if (File.Exists(Path.Combine(y, x))){return Path.Combine(y, x);}
                    else {
                        if (File.Exists(y)) {return y;}
                        else {if (File.Exists(x)) {return y;}}
                    }
                if (count >= 3)
                {
                    Console.WriteLine("\nYou may need to remember these crucial things:\n1. When inputting location remember to remove apostrophes.\n2. When inputting location remember to include [drive letter]:/[folder]/[folder]/.\n3. When inputting name remember to include the .txt extension.\nIf you do not include the .txt and it is another extension then it will only look for .txt and it will not work.\n");
                }
                
                }
            }
            return "";
        }
    
        static public string[] getToTotal (string fromFile) {
            string toLocation = getToFolder(fromFile);
            string fromExtension = Path.GetExtension(fromFile);
            
            Console.WriteLine("\nthe output file wil look something like [prefix] 1,2,3... qaswdtres [suffix]");
            Console.WriteLine($"\nthis is because it allows more files to be generated without running into the same file name,\n the qaswdtres are random letters\n that are generated each loop\n leave blank for defaults:\nrandom ammount: 8\nsuffix: file\nprefix: {fromExtension}");
            int randomAmount = getInputInt("\nenter the length of random characters 2-16> ", 2, 16, 8);
            
            Console.WriteLine("\nplease specify the file prefix> ");
            string prefix = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(toLocation)) {
                prefix = "";
            }

            Console.WriteLine("\nplease specify the file suffix> ");
            string suffix = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(toLocation)) {
                suffix = ".txt";
                Console.WriteLine($"default selected: ({suffix})");
            }
            if (!stringInFile(suffix)) {
                Console.WriteLine($"be warned, you have selected a non default file name\nthis means that it will just show up as {suffix} file and wont look good");
            }
            return new string[] {prefix.ToString(), toLocation.ToString(), suffix.ToString()};
            
            
        }


    }
}

