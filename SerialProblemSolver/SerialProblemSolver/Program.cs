﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SerialProblemSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Secret to Solve:");
            byte[] secretHash = null;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                while (secretHash == null)
                {
                    var input = Console.ReadLine();
                    if (int.TryParse(input, out int secretNumber))
                    {
                        if (secretNumber < 0)
                        {
                            secretHash = sha256Hash.ComputeHash(Encoding.ASCII.GetBytes(9999999999.ToString()));
                        }
                        else
                        {
                            secretHash = sha256Hash.ComputeHash(Encoding.ASCII.GetBytes(input.ToString()));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Please enter a valid number");
                    }
                }
            }

            Console.WriteLine("Calculating Secret");
            var stopWatch = Stopwatch.StartNew();
            var solvedSecretNumber = SolveProblem(secretHash);
            stopWatch.Stop();
            Console.WriteLine($"Your Secret Is: {solvedSecretNumber}");
            Console.WriteLine($"Solved In: {stopWatch.ElapsedMilliseconds}ms");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static int SolveProblem(byte[] secretHash)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                for (int i = 0; true; i++)
                {
                    var hash = sha256Hash.ComputeHash(Encoding.ASCII.GetBytes(i.ToString()));
                    if (secretHash.SequenceEqual(hash))
                    {
                        return i;
                    }
                }
            }
        }
    }
}
