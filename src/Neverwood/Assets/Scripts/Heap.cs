using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Heap<T> where T : IHeapElement<T>      //Kopiec
{
    private int heapCapacity { get; }

    T[] elements;
    int heapSize; 

    public Heap(int _heapCapacity)
    {
        heapCapacity = _heapCapacity;
        elements = new T[heapCapacity];
    }
    public void Add(T item)
    {
        item.HeapIndex = heapSize;
        elements[heapSize] = item;
        SortHeapUp(item);
        heapSize++;
    }
    public T RemoveFirst()
    {
        T first = elements[0];
        heapSize--;
        elements[0] = elements[heapSize];
        elements[0].HeapIndex = 0;
        SortHeapDown(elements[0]);
        return first;
    }
    public bool Contains(T item)
    {
        return Equals(elements[item.HeapIndex], item);
    }
    /*public void UpdateItem(T item)
    {
        SortHeapUp(item);
    }*/
    public int Count
    {
        get
        {
            return heapSize;
        }
    }
    void SortHeapUp(T item)
    {
        while(true)
        {
            T parentItem = elements[(item.HeapIndex - 1) / 2];
            if (item.CompareTo(parentItem) > 0)
            {
                SwapElements(item, parentItem);
            }
            else
            {
                return; 
            }
        }
    }
    void SortHeapDown(T item)
    {
        while(true)
        {
            int leftChildI  = item.HeapIndex * 2 + 1;
            int rightChildI = item.HeapIndex * 2 + 2;
            int swapI = 0;

            if (leftChildI < heapSize)
            {
                swapI = leftChildI;
                if(rightChildI < heapSize)
                {
                    if(elements[leftChildI].CompareTo(elements[rightChildI]) < 0)
                    {
                        swapI = rightChildI;
                    }
                }
                if(item.CompareTo(elements[swapI]) < 0)
                {
                    SwapElements(item, elements[swapI]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
    void SwapElements(T a, T b)
    {
        elements[a.HeapIndex] = b;
        elements[b.HeapIndex] = a;
        int temp = a.HeapIndex;
        a.HeapIndex = b.HeapIndex;
        b.HeapIndex = temp;
    }
}
public interface IHeapElement<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}
