using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//change 1
//change 2
namespace TicTacToe //Made in c#
{
    internal enum Symbol {X = 0, O = 1, Undecided = 2};
    internal class Game
    {
        
        static void Main(string[] args)
        {
            string player1, player2;
            Console.WriteLine("Welcome to Tic Tac Toe!");
            Console.WriteLine("Enter your name, Player 1");
            player1 = Console.ReadLine();
            Console.WriteLine("Enter your name, Player 2");
            player2 = Console.ReadLine();
            switch(GameLoop(player1, player2))
            {
                case Symbol.Undecided: Console.WriteLine("Everyone's a winner!");
                    break;
                case Symbol.O: Console.WriteLine(player1 + " Wins!");
                    break;
                case Symbol.X: Console.WriteLine(player2 + " Wins!");
                    break;
            }
            Console.ReadKey();
        }

        /// <summary>
        /// returns false if draw, true if win. 
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <returns></returns>
        static Symbol GameLoop(string player1, string player2)
        {
            BoardState board = new BoardState();
            Player player = new Player(board);
            WinChecker winChecker = new WinChecker(board, player);
            Console.WriteLine("Enter an integer from 1-9");
            board.Renderer();
            for (int i = 0; i < 9; i++)
            {
                while (!player.PlayerInput(Console.ReadLine()))
                {
                    Console.WriteLine("Invalid number.");
                }

                board.Renderer();

                switch (winChecker.CheckWin())
                {
                    case Symbol.Undecided: break;
                    case Symbol.O:
                        return Symbol.O;
                    case Symbol.X:
                        return Symbol.X;
                }

                player.SwapPlayer();
                Console.WriteLine("Current Player: " + player.currentPlayer);
            }
            return Symbol.Undecided; //if all moves are done without a win.
        }
    }









    internal class BoardState
    {
        private Symbol[,] board;

        internal Symbol[,] Board //ensures board isn't made accessible outside the class while still allowing get; set;
        {
            get { return board; }
        }            
        internal void setBoardArray(int row, int column, Symbol symbol)
        {
            board[row-1, column-1] = symbol;
        }
        internal void Renderer()
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                Console.Write("                                ");
                for (int column= 0; column <board.GetLength(1); column++)
                {
                    if (board[row, column] == Symbol.Undecided) Console.Write(" ");
                    else Console.Write(board[row, column]);
                    if (column < board.GetLength (1)-1) Console.Write("|"); //stops the line from being written at the end.
                }
                if (row < board.GetLength(0)-1) Console.WriteLine("\n                               -------"); //stops the line from being written at the bottom.
            }
            Console.WriteLine("\n\n\n\n");
        }
        internal BoardState() //initialise and fill in board array.
        {
            board = new Symbol[3, 3]; //Represents each square of the board.
            for (int row = 0; row < board.GetLength(0); row++) //Fills the array with Symbol.Undecided
            {
                for(int column = 0; column<board.GetLength(1); column++)
                {
                    board[row, column] = Symbol.Undecided;
                }
            }
        }
    }










    internal class WinChecker //CheckWin not working for diagonals(and square 1)
    {
        BoardState board; Player player;
        Symbol currentPlayer; Symbol[,] boardArray;
        int counter = 0;
        internal Symbol CheckWin() //needs simplification(too much repetition).
        {
            currentPlayer = player.currentPlayer; //Since it's used very often, making a local reference may be faster.
            boardArray = board.Board; 
            //Option 1. Iterate through each row and diagonal, and compare
            //* Option 2. Compare all 3 to current player. *
            for(int row = 0; row< boardArray.GetLength(0); row++) //horizontal
            {
                for(int column = 0; column < boardArray.GetLength(1); column++)
                {
                    if (boardArray[row, column] != currentPlayer) break; //doesn't iterate anymore if win condition not met for that row.
                    else counter++;//Checks if all 3 columns are currentPlayer.
                }
                if (CheckCounter()) return currentPlayer;
            }

            for (int column = 0; column < boardArray.GetLength(0); column++) //vertical
            {
                for (int row = 0; row < boardArray.GetLength(1); row++)
                {
                    if (boardArray[row, column] != currentPlayer) break;
                    else counter++; //Checks if all 3 columns are currentPlayer.
                }
                if (CheckCounter()) return currentPlayer;
            }

            for(int i = 0; i < boardArray.GetLength(0); i++) //  \ diagonal
            {
                if (boardArray[i, i] != currentPlayer) break;
                else counter++;
            }
            if (CheckCounter()) return currentPlayer;

            for (int i = boardArray.GetLength(0) -1; i >=0 ; i--) //  / diagonal
            {
                if (boardArray[i, boardArray.GetLength(0)-1-i] != currentPlayer) break;
                else counter++;
            }
            if (CheckCounter()) return currentPlayer;
            return Symbol.Undecided; //shows no winner yet.
        }

        private bool CheckCounter()
        {
            if (counter == 3) return true;
            else counter = 0; return false;
        }

        internal WinChecker(BoardState board, Player player)
        {
            this.board = board;
            this.player = player;
        }
    }










    internal class Player
    {
        internal Symbol currentPlayer; int input, row, column;
        BoardState board;
        /// <summary>
        /// Returns false if input is wrong, true if successful. Calls board.setBoardArray.
        /// </summary>
        /// <param name="positionInput"></param>
        /// <returns></returns>
        internal bool PlayerInput(string positionInput)
        {
            if (!int.TryParse(positionInput, out input)) return false; //checks if it's a integer(will fail if it's a float or letters). If successful, will assign value.
            else if (input < 0 || input > 9) return false; //checks if within range of board.
            row = (input - 1) / 3 + 1; 
            column = (input - 1) % 3 +1;
            if (board.Board[row - 1, column - 1] != Symbol.Undecided) return false; //stops input on a square already written on
            board.setBoardArray(row, column, currentPlayer);
            return true;
        }

        internal void SwapPlayer()
        {
            if (currentPlayer == Symbol.X) currentPlayer = Symbol.O;
            else currentPlayer = Symbol.X;
        }
        internal Player(BoardState board)
        {
            this.board = board;
            currentPlayer = Symbol.O;
        }

    }
}
