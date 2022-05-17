using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
        static int PuzzleSize = 0;
        static int x, y;
        static int[,] ReadFromFile(string FilePath)
        {
            String ALLLines = File.ReadAllText(FilePath);                         //O(N)
            int i = 0, j;
            PuzzleSize = int.Parse(ALLLines.Split('\n')[0]);                      //O(N)
            int[,] Puzzle = new int[PuzzleSize, PuzzleSize];
            foreach (var row in ALLLines.Split('\n'))                             //THETA(N)                       
            {
                j = 0;
                if (row == ALLLines.Split('\n')[0])                               //O(1)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(row) || row == "\r")                     //O(1)
                {
                    continue;
                }
                foreach (var column in row.Trim().Split(' '))                     //THETA(N)
                {
                    Puzzle[i, j] = int.Parse(column.Trim());
                    j++;
                }
                i++;
            }
            return Puzzle;
        }                                                                         
        static int FindBottomZeroPosition(int[,] puzzle, int N)
        {
        // start from bottom-right corner of matrix
            for (int i = N - 1; i >= 0; i--)           //O(N)
                for (int j = N - 1; j >= 0; j--)       //O(N)
                    if (puzzle[i, j] == 0)             //O(1)
                        return N - i;
            return 0;
        }
        //This function converts the 2D array into 1D.
        static int[] convert(int[,] boardState, int BoardStateSize)
        {
            int iterator = 0;                                                      //O(1)
            int[] OneDimensionalArray = new int[BoardStateSize * BoardStateSize];   
            for (int i = 0; i < BoardStateSize; i++)                               //THETA(N)
            {
                for (int j = 0; j < BoardStateSize; j++)                           //THETA(N)
                {
                    OneDimensionalArray[iterator] = boardState[i, j];              //O(1)
                    iterator++;
                    if (boardState[i, j] == 0)                                     //O(1)
                    {
                        x = i;
                        y = j;                                                    
                    }
                }
            }
            return OneDimensionalArray;                                            //O(1)
        }
        //This function tests whether the initial puzzle is solvable. Even -> True -> Solvable.
        //Both odd/even -> false (unsolvable)
        //Board size is odd // Boardsize is even

        public static bool isSolvable(int[] InitialBoardState, int BoardSize, int Test)
        {
            int SolvableCounter = 0;                                                //O(1)
            for (int i = 0; i < (BoardSize * BoardSize) - 1; i++)                   //THETA(S)
            {
                for (int j = i + 1; j < BoardSize * BoardSize; j++)                 //THETA(S)
                {
                    if (InitialBoardState[j] > 0 && InitialBoardState[i] > 0 
                        && InitialBoardState[i] > InitialBoardState[j])            //O(1)
                        SolvableCounter++;                                         //O(1)
                }
            }
            bool result = SolvableCounter % 2 != 0 ? false : true;                 //O(1)
            if (BoardSize % 2 != 0)                                                //O(1)
                return (SolvableCounter % 2 == 0);                                 //O(1)
            else 
            {
                if ((Test % 2 != 0 && (SolvableCounter % 2 != 0)) || 
                    (Test % 2 == 0 && (SolvableCounter % 2 == 0)))                //O(1)
                    return false;                                                 //O(1)
                else
                    return true;                                                  //O(1)
            }
        }
        public static void Main(string[] args)
        {
            Stopwatch runTime = new Stopwatch();
            //string fileName = @"D:\Uni\Algo\Project\Project\[3] N Puzzle\Testcases\Testcases\Complete\Complete Test\Complete Test\Solvable puzzles\Manhattan Only\15 Puzzle 1.txt";
            //string fileName = @"D:\FCIS\CS6\Algorithms\Project\[3] N Puzzle\Testcases\Testcases\Complete\Complete Test\Complete Test\V. Large test case\TEST.txt";
            //string fileName = @"D:\FCIS\CS6\Algorithms\Project\[3] N Puzzle\Testcases\Testcases\Sample\Sample Test\Sample Test\Solvable Puzzles\24 Puzzle 2.txt";
            string fileName = @"D:\FCIS\CS6\Algorithms\Project\[3] N Puzzle\Testcases\Testcases\Complete\Complete Test\Complete Test\Solvable puzzles\Manhattan & Hamming\50 Puzzle.txt";
            //string fileName = @"D:\FCIS\CS6\Algorithms\Project\[3] N Puzzle\Testcases\Testcases\Complete\Complete Test\Complete Test\Solvable puzzles\Manhattan Only\15 Puzzle 5.txt";
            int[,] p = ReadFromFile(fileName);
            int P = FindBottomZeroPosition(p, PuzzleSize);
            int[] b = convert(p, PuzzleSize);
            Console.Write("1- Manhattan\n2- Hamming\n3- BFS\n");
            Console.Write("Choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            if (isSolvable(b, PuzzleSize, P))
            {
                Console.WriteLine("Puzzle is solvable.");
                BoardManagement boardManager = new BoardManagement(x, y);
                if (choice == 3)
                {
                    runTime.Start();
                    boardManager.BFS(p);
                    runTime.Stop();
                }
                else
                {
                    runTime.Start();
                    boardManager.SolveNPuzzle(p, choice, PuzzleSize);
                    runTime.Stop();
                }
                TimeSpan ts = runTime.Elapsed;
                Console.WriteLine("Elapsed Time is {0:00}:{1:00}:{2:00}.{3}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            }
            else
                Console.WriteLine("Puzzle is not solvable.");
        }
    }
}