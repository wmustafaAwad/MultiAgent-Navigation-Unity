using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T: IHeapItem<T> {
    T[] items;
    int currentItemCount;


    public Heap(int maxHeapSize) {
        items = new T[maxHeapSize];
    
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);

    }

    public int Count {
        get
        {
            return currentItemCount;
        }
    
    }

    public void UpdateItemDec(T item){ //EditHere //Update item and decrease its fCost (default, needed)
        SortUp(item);
    }

    public void UpdateItemInc(T item) { //EditHere //Update item and increase its fCost (extra, for future)
        SortDown(item);

    }

    public void Add(T item) {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    
    }

    public T RemoveFirst() {
        T firstItem = items[0];
        currentItemCount--;

        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    
    }

    void SortDown(T item) {
        while (true) {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int SwapIndex = 0;

            if (childIndexLeft < currentItemCount)
            { //i.e. has left child
                SwapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                { //i.e. has right child
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        SwapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[SwapIndex]) < 0)
                {
                    Swap(item, items[SwapIndex]);

                }
                else { return; }

            }
            else { return; }
        
        }
    
    }

    void SortUp(T item) {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true) {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;

        
        }
        
    }

    void Swap(T itemA, T itemB) {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex; //Temp integer assigned
     
    }

    
}


public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex{
        get;
        set;
    }



}
