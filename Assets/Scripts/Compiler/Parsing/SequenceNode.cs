using System.Collections.Generic;
using System.Collections;
using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Compiler
{
    public class SequenceNode:Node
    {
        public List<object> Elements { get; }
        public string Type { get; }
        public int Index { get; set; }
        public bool IsInfinite { get; }
        public int Initial { get; set; }
        public int End { get; }

        public SequenceNode( List<object> elements,string type,int index=0,bool isInfinite = true, int initial = 0, int end = 0)
        {
            Elements = elements;
            Type = type;
            Index = index;
            IsInfinite = isInfinite;
            Initial = initial;
            End = end;
        }
        
        public IEnumerator<object> GetEnumerator()
        {
            if (Type == "")
            {

                while (true)
                {
                    yield return new Undefined();
                }
            }
            else if (Type=="point")
            {
                NumGenerator numGen1 = new NumGenerator(1,0, 178, 1019);
                NumGenerator numGen2 = new NumGenerator(2, 0, 5, 568);

                IEnumerator<int> enumerator1 = numGen1.GetEnumerator();
                IEnumerator<int> enumerator2 = numGen2.GetEnumerator();
                for(int i = 0; i < Index; i++)
                {
                    enumerator1.MoveNext();
                    enumerator2.MoveNext();
                }
                while (enumerator1.MoveNext() && enumerator2.MoveNext())
                {

                    Index++;
                    int number1 = enumerator1.Current;
                    int number2 = enumerator2.Current;
                    Debug.Log("point " +number1+ " "+number2);

                    yield return new Point(number1.ToString() + number2.ToString(), number1, number2);
                }

            }
            else if (Elements.Count == 0)
            {
                if (Type == "number")
                {
                    if (IsInfinite)
                    {
                        int currentValue = Initial+Index;
                        while (true)
                        {
                            yield return currentValue;
                            currentValue++;
                            Index++;
                        }

                        
                    }
                    else
                    {
                        
                        if (Initial+Index <= End)
                        {
                            for (int i = Initial+Index; i <= End; i++)
                            {
                                
                                yield return i;
                                Index++;
                            }
                        }
                        else
                        {
                            
                            yield return new Undefined();
                            Index++;
                        }
                    }
                }
                else
                {
                    yield return new Undefined();;
                }
            }
            else if(Index<Elements.Count)
            {
                for(int i = Index; i< Elements.Count; i++)
                {
                    Index++;
                    yield return Elements[i];
                }
            }
        }

        public bool IsEmpty()
        {
            if(Type == "")
            {
                return true;
            }
            else if(Elements.Count>0 && Index >= Elements.Count)
            {
                return true;
            }
            else if (Index+Initial > End)
            {
                return true;
            }
            return false;
        }
        
    }
}