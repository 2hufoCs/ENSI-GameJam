using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class WordFusion : MonoBehaviour
{
        
        private readonly List<Stack<List<string>>> _stackList = new List<Stack<List<string>>>();
        public readonly Queue<List<String>> finalQueue = new Queue<List<string>>();

        [SerializeField] private string testWord;
        [SerializeField] private string testWord2;    

        public int InitiateKeys(string initialWord,string finalWord)
        {
                int numberOfWaves = GenerateListStack(initialWord);

                while (true)
                {
                        List<string> list = Combine();
                        if (list == null) break;
                        finalQueue.Enqueue(list);
                }
                finalQueue.Enqueue(GrabTopStacks());
                finalQueue.Enqueue(GenerateInitialList(finalWord));
                return numberOfWaves;
        }
        
        
        
        [Button]
        private List<string> Combine()
        {
                List<string> words = GrabTopStacks();
                List<int> indexes = new List<int>();
                int smallestIndex = int.MaxValue;
                for (int i = 0; i < _stackList.Count; i++)
                {
                        foreach (string word in _stackList[i].Peek())
                        {
                                if(_stackList[i].Count > 1)
                                {
                                        if (word.Length < smallestIndex)
                                        {
                                                indexes.Clear();
                                                indexes.Add(i);
                                                smallestIndex = word.Length;
                                        }
                                        else if (word.Length == smallestIndex)
                                        {
                                                indexes.Add(i);
                                        }
                                }
                        }
                }

                foreach (Stack<List<string>> stack in _stackList)
                {
                        if (stack.Count > 1)
                        {
                                _stackList[indexes[Random.Range(0, indexes.Count)]].Pop();
                                return words;
                        }
                }
                return null;

                //PrintStack();

        }
        private List<string> GrabTopStacks()
        {
                List<string> returnList = new List<string>();
                foreach (Stack<List<string>> stack in _stackList)
                {
                        returnList.AddRange(stack.Peek());
                }
                return returnList;
        }

        private int GenerateListStack(string initialWord)
        {
                _stackList.Clear();
                List<string> words = GenerateWordList(initialWord);
                int total = 0;
                foreach (string word in words)
                {
                        Stack<List<string>> stack = GenerateStack(word);
                        foreach (List<string> printStack in stack)
                        {
                                string printing = "";
                                foreach (string letter in printStack)
                                {
                                        printing += " , " + letter;
                                }
                        }
                        total += stack.Count - 1;
                        _stackList.Add(stack);
                }

                total+= 2;
                return total;
        }
        
        private List<string> GenerateWordList(string initialWord)
        {
                List<string> words = new List<string>();
                string temp = "";
                foreach (char letter in initialWord)
                {
                        if (String.IsNullOrWhiteSpace(letter.ToString()))
                        {
                                words.Add(temp);
                                temp = "";
                        }
                        else
                        {
                                temp += letter;
                        }
                }
                words.Add(temp);
                return words;
        }
        private Stack<List<string>> GenerateStack(string initialWord)
        {
                Stack<List<string>> finalStack = new Stack<List<string>>();
                finalStack.Push(GenerateInitialList(initialWord));
                while (NumberOfDivisible(finalStack.Peek()).Count > 0)
                {
                        List<int> indexes = NumberOfDivisible(finalStack.Peek());
                        Shuffle(indexes);
                        for (int y = 0; y < indexes.Count; y++)
                        {
                                int offset = 0;
                                for (int i = 0; i < y; i++)
                                {
                                        if (indexes[i] < indexes[y])
                                        {
                                                offset++;
                                        }
                                }
                        
                                string split = finalStack.Peek()[indexes[y] + offset];
                                List<string> start = new List<string>();
                                List<string> end = new List<string>();

                                for (int i = 0; i < finalStack.Peek().Count; i++)
                                {
                                        if (i < indexes[y] + offset)
                                        {
                                                start.Add(finalStack.Peek()[i]);
                                        }
                                        else if (i > indexes[y] + offset)
                                        {
                                                end.Add(finalStack.Peek()[i]);
                                        }
                                }
                                List<string> middle = new List<string>
                                {
                                        split.Substring(0, split.Length / 2),
                                        split.Substring(split.Length / 2)
                                };


                                List<string> final = new List<string>();
                                final.AddRange(start);
                                final.AddRange(middle);
                                final.AddRange(end);
                                
                                finalStack.Push(final);
                        }
                }
                return finalStack;
        }
        
        private List<string> GenerateInitialList(string initialWord)
        {
                List<string> returnList = new List<string> { initialWord };
                return returnList;
        }
        
        private List<int> NumberOfDivisible(List<string> words)
        {
                List<int> returnIndex = new List<int>();
                for (int i = 0; i < words.Count ; i++)
                {
                        if (words[i].Length > 1)
                        {
                                returnIndex.Add(i);
                        }
                }
                return returnIndex;
        }

        private void Shuffle<T>(IList<T> ts)
        {
                var count = ts.Count;
                var last = count - 1;
                for (var i = 0; i < last; ++i)
                {
                        var r = Random.Range(i, count);
                        (ts[i], ts[r]) = (ts[r], ts[i]);
                }
        }
        
        [Button]
        private void PrintStack()
        {
                foreach(Stack<List<string>> finalStack in _stackList)
                {
                        foreach (List<string> printStack in finalStack)
                        {
                                var printing = "";
                                foreach (string letter in printStack)
                                {
                                        printing += " , " + letter;
                                }

                                Debug.Log(printing);
                        }
                }
        }

        [Button]
        private void PrintQueue()
        {
                foreach (List<string> list in finalQueue)
                {
                        string printing = "";
                        foreach (string letter in list)
                        {
                                printing += " , " + letter;
                        }
                        Debug.Log(printing);
                }
        }
        
        
        [Button]
        private void GlobalTest()
        {
                print(GenerateListStack(testWord));
                //PrintStack();
                print("Count : " + _stackList.Count);
                Combine();
        }
        [Button]
        private void BigSpookyTest()
        {
                InitiateKeys(testWord, testWord2);
        }
}