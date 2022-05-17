
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ConsoleApp1
{
    class BoardManagement
    {
        List<int> Dx = new List<int>() { -1, 1, 0, 0 }; //row
        List<int> Dy = new List<int>() { 0, 0, -1, 1 }; //column
        ISet<string> VisitedStates = new HashSet<string>();
        List<BoardState> ChildrenBoards = new List<BoardState>();
        PriorityQueue<BoardState> priority = new PriorityQueue<BoardState>();
        Queue<BoardState> queue =  new Queue<BoardState>();
        int x, y;
        public BoardManagement(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        string ConvertIntoNumber(int[,] boardState, int BoardStateSize)
        {
            string CombinedNumbers = "";                              //O(1)
            for (int i = 0; i < BoardStateSize; i++)                  //THETA(N)
            {
                for (int j = 0; j < BoardStateSize; j++)              //THETA(N)
                {
                    CombinedNumbers += boardState[i, j];              //O(1)

                }
            }
            return CombinedNumbers;                                   //O(1)
        }
        void DisplayBoard(int[,] puzzle)
        {
            for (int i = 0; i < puzzle.GetLength(0); i++)       //THETA(N)
            {
                for (int j = 0; j < puzzle.GetLength(1); j++)    //THETA(N)
                {
                    Console.Write(puzzle[i, j]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }
        BoardState CreateInitialBoard(int[,] Input, int choice, int size)
        {
            BoardState boardState = new BoardState();                        //O(1)
            boardState.grid = Input;                                         //O(1)
            boardState.gridSize = size;                                      //O(1)
            boardState.zeroCoordinates.x = x;                                //O(1)
            boardState.zeroCoordinates.y = y;                                //O(1)
            if (choice == 1)
            {
                boardState.f = CalculateHeuristicManhattan(boardState);       //THETA(N^2)
                priority.push(boardState);                                    //O(log(V))
            }
            else if (choice == 2)
            {
                boardState.f = CalculateHeuristicHamming(boardState);          //THETA(N^2)
                priority.push(boardState);                                     //O(log(V))
            }
            else
            {
                boardState.color = "gray";                                     //O(1)
                queue.Enqueue(boardState);                                     //O(1)
            }
            boardState.numberOfMoves = 0;                                        //O(1)
            boardState.hash = ConvertIntoNumber(Input, boardState.gridSize);     //THETA(N^2)
            VisitedStates.Add(boardState.hash);                                  //O(1)
            return boardState;                                                   //O(1)
        }
        bool ValidState(int size, BoardState.ZeroCoordinates zeroC)  
        {
            if (zeroC.x < 0 || zeroC.y < 0 || zeroC.x >= size || zeroC.y >= size)    //O(1)
                return false;                                                        //O(1)
            else 
                return true;                                                         //O(1)
        } 
        BoardState CreateNewValidBoard(int[,] newBoardGrid, BoardState parentBoard)
        {
            BoardState newBoardState = new BoardState();                     //O(1)  
            newBoardState.grid = newBoardGrid.Clone() as int[,];             //THETA(N^2)
            newBoardState.gridSize = parentBoard.gridSize;                   //O(1)
            return newBoardState;                                            //O(1)
        }
        int[,] GoalState(int Size)   
        {
            int[,] goal = new int[Size, Size];                               //O(1)
            for (int i = 0; i < Size; i++)                                   //THETA(N)
            {
                for (int j = 0; j < Size; j++)                               //THETA(N)
                {
                    if (i == Size - 1 && j == Size - 1)                      //O(1)
                    {
                        goal[i, j] = 0;                                      //O(1)
                    }
                    else
                    {
                        goal[i, j] = (1 + j + (i % Size) * Size);            //O(1)
                    }
                }
            }
            return goal;
        }
        void GenerateState(BoardState boardState, int choice)
        {
            BoardState newBoardState = new BoardState();
            for (int i = 0; i < 4; i++)                                                                      //O(1)
            {
                BoardState.ZeroCoordinates newZeroCoordinates;                                              //O(1)
                newZeroCoordinates.x = (boardState.zeroCoordinates.x + Dx[i]);                              //O(1)
                newZeroCoordinates.y = (boardState.zeroCoordinates.y + Dy[i]);                              //O(1)
                if (ValidState(boardState.gridSize, newZeroCoordinates))                                    //O(1)
                {
                    int[,] newBoardGrid = boardState.grid.Clone() as int[,];                                //O(N^2)  
                    newBoardGrid[boardState.zeroCoordinates.x, boardState.zeroCoordinates.y] = 
                                    newBoardGrid[newZeroCoordinates.x, newZeroCoordinates.y];
                    newBoardGrid[newZeroCoordinates.x, newZeroCoordinates.y] = 0;
                    string hashValue = ConvertIntoNumber(newBoardGrid, boardState.gridSize);                        //THETA(N^2)
                    if (!(VisitedStates.Contains(hashValue)))                                                       //O(1)
                    {
                        newBoardState = CreateNewValidBoard(newBoardGrid, boardState);                      //O(N^2)
                        newBoardState.hash = hashValue;
                        newBoardState.root = boardState;
                        newBoardState.zeroCoordinates = newZeroCoordinates;
                        //newBoardState.zeroCoordinates.y = newZeroCoordinates.y;
                        newBoardState.numberOfMoves = boardState.numberOfMoves + 1;
                        if(choice == 1)
                            newBoardState.f = CalculateHeuristicManhattan(newBoardState) + 
                                                                    newBoardState.numberOfMoves;            //THETA(N^2)
                        else
                            newBoardState.f = CalculateHeuristicHamming(newBoardState) + 
                                                newBoardState.numberOfMoves;                               //THETA(N^2)
                        priority.push(newBoardState);                                                      //O(log(V))
                        VisitedStates.Add(newBoardState.hash);                                             //O(1)
                    }
                }
            }
        }

        int CalculateHeuristicHamming(BoardState boardState)                        
        {
            int heuristic = 0;
            for (int row = 0; row < boardState.gridSize; row++)                     //THETA(N)
            {
                for (int column = 0; column < boardState.gridSize; column++)        //THETA(N)
                {
                   if (boardState.grid[row, column] != 0 && boardState.grid[row, column] != (1 + column + (row % boardState.gridSize) * boardState.gridSize))
                            heuristic++;                                            //O(1)
                }
            }
            return heuristic;                                                      //O(1)
        } 
        int CalculateHeuristicManhattan(BoardState boardState) 
        {
            int heuristic = 0;                                                          //O(1)
            int xCoordinates;                                                           //O(1)
            int yCoordinates;                                                           //O(1)
            for (int row = 0; row < boardState.gridSize; row++)                                            //THETA(N)
            {
                for (int column = 0; column < boardState.gridSize; column++)                              //THETA(N)
                { 
                    if (boardState.grid[row, column] != 0)                                                //O(1)
                    {
                        xCoordinates = (boardState.grid[row, column] - 1) / boardState.gridSize;            //O(1)
                        yCoordinates = (boardState.grid[row, column] - 1) % boardState.gridSize;           //O(1)
                        heuristic += Math.Abs(xCoordinates - row) + Math.Abs(yCoordinates - column);       //O(1)
                    }
                }
            }
            return heuristic;
        }
        List<BoardState> GenerateBFS(BoardState boardState)
        {
            BoardState newBoardState = new BoardState();
            for (int i = 0; i < 4; i++)                                                                 //O(1)
            {
                BoardState.ZeroCoordinates newZeroCoordinates;
                newZeroCoordinates.x = (boardState.zeroCoordinates.x + Dx[i]);                          //O(1)
                newZeroCoordinates.y = (boardState.zeroCoordinates.y + Dy[i]);                          //O(1)
                if (ValidState(boardState.gridSize, newZeroCoordinates))                                //O(1)
                {
                    int[,] newBoardGrid = boardState.grid.Clone() as int[,];                            //O(N^2)
                    newBoardGrid[boardState.zeroCoordinates.x, boardState.zeroCoordinates.y] = 
                                newBoardGrid[newZeroCoordinates.x, newZeroCoordinates.y];
                    newBoardGrid[newZeroCoordinates.x, newZeroCoordinates.y] = 0;                       //O(1)
                    newBoardState = CreateNewValidBoard(newBoardGrid, boardState);                      //O(N^2)
                    newBoardState.hash = ConvertIntoNumber(newBoardState.grid, boardState.gridSize);    //THETA(N^2)
                    if (!(VisitedStates.Contains(newBoardState.hash)))                                  //O(1)
                    {
                        newBoardState.root = boardState;                                                //O(1)
                        newBoardState.zeroCoordinates.x = newZeroCoordinates.x;                         //O(1)
                        newBoardState.zeroCoordinates.y = newZeroCoordinates.y;                         //O(1)
                        newBoardState.numberOfMoves = boardState.numberOfMoves + 1;                     //O(1)
                        VisitedStates.Add(newBoardState.hash);                                          //O(1)
                        ChildrenBoards.Add(newBoardState);                                              //O(1)
                    }
                }
            }
            return ChildrenBoards;
        }
        public int BFS(int[,] InputGrid)
        {
            string goal = ConvertIntoNumber(GoalState(InputGrid.GetLength(0)), InputGrid.GetLength(0));           //THETA(N^2)
            List<BoardState> Children = new List<BoardState>();
            BoardState InputBoard = CreateInitialBoard(InputGrid, 3, InputGrid.GetLength(0));                     //THETA(N^2)
            while (queue.Count != 0)                                                                              //O(V)
            {
                BoardState root = queue.Dequeue();                                                               //O(1)
                Children = GenerateBFS(root);                                                                    //O(N^2)
                foreach (BoardState child in Children)                                                           //O(E)
                {
                    if (child.color == "white")                                                                  //O(1)
                    {
                        child.color = "gray";                                                                    //O(1)
                        if (child.hash == goal)                                                                  //O(1)
                        {
                            Console.WriteLine(child.numberOfMoves);                                              //O(1)
                            return child.numberOfMoves;                                                          //O(1)
                        }
                        queue.Enqueue(child);                                                                    //O(1)
                    }
                }
                root.color = "black";                                                                             //O(1)
            }
            return 0;                                                                                            //O(1)
        }
        public void SolveNPuzzle(int[,] InputGrid, int choice, int puzzleSize)
        {
            BoardState InputBoard;                                                    //O(1)
            InputBoard = CreateInitialBoard(InputGrid, choice, puzzleSize);           //THETA(N^2)
            //DisplayBoard(InputGrid);                                                  //THETA(N^2)
            //Console.WriteLine("---------------------------");
            while (priority.getSize() != 0)                                           //O(log(V))
            {
                if (InputBoard.f == InputBoard.numberOfMoves)                        //O(1)
                    break;
                InputBoard = priority.pop();                                        //O(log(V))
                //DisplayBoard(InputBoard.grid);                                      //O(N^2)
                //Console.WriteLine("---------------------------");
                GenerateState(InputBoard, choice);                                  //O(N^2 + log(V))
            }
            Console.WriteLine(InputBoard.numberOfMoves);
        }
    }
}