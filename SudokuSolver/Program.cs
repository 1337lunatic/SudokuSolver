using System;
using System.Diagnostics;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            // Predefined boards for testing the solver
            int[,] board = new int[9, 9] { { 1, 9, 6, 0, 5, 3, 0, 0, 0 },  // 1             // Takes about 0.00133 seconds to complete without debugger
                                           { 7, 0, 0, 2, 1, 0, 3, 0, 0 },  // 2
                                           { 0, 3, 0, 0, 0, 6, 0, 0, 7 },  // 3
                                           { 0, 7, 0, 0, 0, 2, 4, 1, 0 },  // 4
                                           { 5, 6, 1, 3, 7, 4, 0, 0, 0 },  // 5
                                           { 4, 0, 0, 0, 0, 0, 7, 0, 0 },  // 6
                                           { 6, 1, 0, 0, 2, 0, 5, 0, 9 },  // 7
                                           { 2, 0, 0, 6, 0, 0, 8, 3, 0 },  // 8
                                           { 0, 8, 0, 5, 3, 0, 0, 7, 2 }}; // 9

            int[,] boardexpert = new int[9, 9] { { 0, 0, 0, 0, 0, 0, 0, 0, 9 },  // 1       // Takes about 0.00260 seconds to complete without debugger
                                                 { 0, 4, 0, 8, 3, 0, 0, 0, 0 },  // 2
                                                 { 8, 2, 0, 0, 4, 0, 0, 1, 0 },  // 3
                                                 { 0, 0, 6, 0, 0, 7, 0, 0, 0 },  // 4
                                                 { 0, 0, 8, 0, 0, 0, 5, 0, 7 },  // 5
                                                 { 0, 5, 3, 2, 0, 0, 0, 0, 0 },  // 6
                                                 { 0, 7, 0, 9, 2, 0, 0, 0, 5 },  // 7
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0 },  // 8
                                                 { 1, 0, 5, 0, 6, 0, 0, 0, 0 }}; // 9

            int[,] unsolveable = new int[9, 9] { { 0, 0, 0, 0, 0, 0, 0, 0, 0 },  // 1       // Takes about 0.00150 seconds to complete without debugger
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0 },  // 2
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0 },  // 3
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0 },  // 4
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0 },  // 5
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0 },  // 6
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0 },  // 7
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0 },  // 8
                                                 { 0, 0, 0, 0, 0, 0, 0, 0, 0 }}; // 9
            // Stopwatch for measuring time. Not essential at all.
            Stopwatch sw = new Stopwatch();

            sw.Start();
            Solve(unsolveable);                     // Method that solves the sudoku board
            sw.Stop();
            TimeSpan sp = sw.Elapsed;

            PrintBoard(unsolveable);                // Pretty print
            
            Console.WriteLine(sp.TotalSeconds);     // Total time measured
        }

        // Solves the sudoku board.
        /*
         :Param :
                : board: A 2D array for solving a Sudoku;
         :Return: 
                : Bool;
         */
        static bool Solve(int[,] board)
        {
            PositionModel pos = new PositionModel();
            pos = Find_empty(board);
         
            // Checks if pos returns null, if it does, then the board is solved.
            if (pos != null)
                for (int i = 1; i < 10; i++)
                    // Checks if the number is a valid number
                    if (Valid(board, pos, i))
                    {
                        board[pos.row, pos.col] = i;

                        if (Solve(board))
                            return true;

                        else
                            board[pos.row, pos.col] = 0;
                    }
            else
                return true;

            return false;
        }

        // Returns a object with the indexes that are empty
        /*
         :Param :
                : board: A 2D Array;
         :Return: PositionModel, Null
                : Returns a custom object with the x and y index of a zero;
                : Returns Null if there are no more zero's on the board;
         */
        static PositionModel Find_empty(int[,] board)
        {
            PositionModel fe = new PositionModel();
            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                    // Gets the first available "0" and returns the position of it
                    if (board[i, j] == 0)
                    {
                        fe.row = i;
                        fe.col = j;
                        return fe;
                    }
            return null;
        }


        // Checks if any given number(1-9) is valid in one of the squares on the board.
        /*
        :Param :
               : board:  A 2D array;
               : pos:     A custom object for getting the x and y coordinates of the board;
               : num:    The number which is being validated on;
        :Return:
               : Bool;
         */
        static bool Valid(int[,] board, PositionModel pos, int num)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                // Checks the y axis
                if (board[i, pos.col] == num)
                    return false;

                // Checks the x axis
                if (board[pos.row, i] == num)
                    return false;

                // Checks the box
                if (board[3 * (pos.row / 3) + i / 3, 3 * (pos.col / 3) + i % 3] == num)
                    return false;
            }
            return true;
        }


        // A method for pretty printing the Sudoku board
        /*
         :Param :
                : board: A 2D Array;
         :Return: 
                : Void
         */
        static void PrintBoard(int[,] board)
        {
            Console.WriteLine(" - - - - - - - - - - - - - - - -");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (j == 0)
                        Console.Write(" | ");

                    Console.Write("{0}, ", board[i, j]);

                    if (j == 8)
                        Console.WriteLine(" | ");
                }
            }
            Console.WriteLine(" - - - - - - - - - - - - - - - -");
        }
    }

    public class PositionModel
    {
        public int row { get; set; }
        public int col { get; set; }
    }
}