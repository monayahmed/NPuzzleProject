using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class BoardState
    {
        public int id;
        public int gridSize;
        public int numberOfMoves = 0;
        public int h;
        public int f;
        public int[,] grid;
        public BoardState root;
        public string hash;
        //public int hash;
        public string color = "white";
        public struct ZeroCoordinates{ public int x; public int y; }
        public ZeroCoordinates zeroCoordinates;

    }
    //List<BoardState> states;

    //BoardState newState=new BoardState();
    //newState.id = states.size();
    //states.push(newState);
    
}
