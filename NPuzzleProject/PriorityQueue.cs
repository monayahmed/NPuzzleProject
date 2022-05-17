using System.Collections.Generic;

namespace ConsoleApp1
{
    public class PriorityQueue<board> where board : BoardState
    {
        private List<board> data;
        int size;
        public PriorityQueue()
        {
            this.data = new List<board>();
            size = 0;
        }
        public void push(board item)
        {
            data.Add(item);
            size++;
            int ChildIndex = size - 1;                                 //O(1)
            while (ChildIndex >= 0)                                    //O(log(V))
            {      
                int ParentIndex = (ChildIndex - 1) / 2;                //O(1)
                if (data[ChildIndex].f >= data[ParentIndex].f)         //O(1)
                    break;                            
                board tmp = data[ChildIndex];                         //O(1)
                data[ChildIndex] = data[ParentIndex];                 //O(1)
                data[ParentIndex] = tmp;                              //O(1)
                ChildIndex = ParentIndex;                             //O(1)
            }
        }

        public int getSize()   
        {
            return size;            //O(1)
        }

        public BoardState pop() 
        {
            int lastIndex = size - 1;                           //O(1)
            BoardState MinBoard = data[0];                      //O(1)
            data[0] = data[lastIndex];                          //O(1)
            data.RemoveAt(lastIndex);                           //O(1)
            size--;                                             //O(1)
            MinHeapify(data, size, 0);                          //O(log(V))
            return MinBoard;                                    //O(1)
        }
        void MinHeapify(List<board> boardChildren, int Size, int index) 
        {
            int leftIndex = 2 * index + 1;                                                        //O(1)
            int rightIndex = 2 * index + 2;                                                       //O(1)
            int minIndex = index;                                                                 //O(1)
            board temp;                                                                           //O(1)
            if (leftIndex < Size && boardChildren[leftIndex].f < boardChildren[minIndex].f)       //O(1)
                minIndex = leftIndex;
            if (rightIndex < Size && boardChildren[rightIndex].f < boardChildren[minIndex].f)      //O(1)
                minIndex = rightIndex;
            if (minIndex != index)                                                                 //O(1)
            {
                temp = boardChildren[minIndex];                                                    //O(1)
                boardChildren[minIndex] = boardChildren[index];                                    //O(1)
                boardChildren[index] = temp;                                                       //O(1)
                MinHeapify(boardChildren, Size, minIndex);                                         //O(log(V))
            }
        }

    }
}