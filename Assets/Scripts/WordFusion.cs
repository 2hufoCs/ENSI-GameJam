using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class WordFusion : MonoBehaviour
{
        [ReadOnly]
        public Stack<List<string>> finalStack;
        [SerializeField]
        private List<string> testList =  new List<string>();

        [SerializeField] private string testWord;

        private List<string> GenerateInitialList(string initialWord)
        {
                List<string> returnList = new List<string>();
                returnList.Add(initialWord);
                return returnList;
        }
        
        public void GenerateStack(string initialWord)
        {
                finalStack = new Stack<List<string>>();
                finalStack.Push(GenerateInitialList(initialWord));
                while (NumberOfDivisible(finalStack.Peek()).Count > 0)
                {
                        List<int> indexs = NumberOfDivisible(finalStack.Peek());
                        Shuffle(indexs);
                        for (int y = 0; y < indexs.Count; y++)
                        {
                                int offset = 0;
                                for (int i = 0; i < y; i++)
                                {
                                        if (indexs[i] < indexs[y])
                                        {
                                                offset++;
                                        }
                                }
                        
                                string split = finalStack.Peek()[indexs[y] + offset];
                                List<string> start = new List<string>();
                                List<string> end = new List<string>();

                                for (int i = 0; i < finalStack.Peek().Count; i++)
                                {
                                        if (i < indexs[y] + offset)
                                        {
                                                start.Add(finalStack.Peek()[i]);
                                        }
                                        else if (i > indexs[y] + offset)
                                        {
                                                end.Add(finalStack.Peek()[i]);
                                        }
                                }
                                List<string> middle = new List<string>();
                                middle.Add(split.Substring(0, split.Length/ 2));
                                middle.Add(split.Substring(split.Length/ 2));
                        
                        
                                List<string> final = new List<string>();
                                final.AddRange(start);
                                final.AddRange(middle);
                                final.AddRange(end);
                                
                                finalStack.Push(final);
                        }
                }
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
                        var tmp = ts[i];
                        ts[i] = ts[r];
                        ts[r] = tmp;
                }
        }
        
        [Button]
        private void TestDivisible()
        {
                List<int> test = NumberOfDivisible(testList);
                string printing = "";
                foreach (int nmb in test)
                {
                        printing += " , " + nmb;
                }
                Debug.Log(printing);
        }
        [Button]
        private void TestShuffle()
        {
                
                List<string> test = testList;
                Shuffle(test);
                string printing = "";
                foreach (string nmb in test)
                {
                        printing += " , " + nmb;
                }
                Debug.Log(printing);
        }
        [Button]
        private void TestSplit()
        {
                List<string> words = testList;
                List<int> indexs = NumberOfDivisible(words);
                Shuffle(indexs);
                
                for (int y = 0; y < indexs.Count; y++)
                {
                        int offset = 0;
                        for (int i = 0; i < y; i++)
                        {
                                if (indexs[i] < indexs[y])
                                {
                                        offset++;
                                }
                        }
                        
                        string split = words[indexs[y] + offset];
                        List<string> start = new List<string>();
                        List<string> end = new List<string>();

                        for (int i = 0; i < words.Count; i++)
                        {
                                if (i < indexs[y] + offset)
                                {
                                        start.Add(words[i]);
                                }
                                else if (i > indexs[y] + offset)
                                {
                                        end.Add(words[i]);
                                }
                        }
                        List<string> middle = new List<string>();
                        middle.Add(split.Substring(0, split.Length/ 2));
                        middle.Add(split.Substring(split.Length/ 2));
                        
                        
                        words.Clear();
                        words.AddRange(start);
                        words.AddRange(middle);
                        words.AddRange(end);
                        
                        
                        string printing = "";
                        foreach (string letter in words)
                        {
                                printing += " , " + letter;
                        }
                        Debug.Log(printing);
                }
        }
        
        [Button]
        private void PrintStack()
        {
                string printing = "";
                foreach (List<string> printstack in finalStack)
                {
                        printing = "";
                        foreach (string letter in printstack)
                        {
                                printing += " , " + letter;
                        }
                        Debug.Log(printing);
                }
                
                print(NumberOfDivisible(finalStack.Peek()).Count);
        }

        [Button]
        private void GlobalTest()
        {
                GenerateStack(testWord);
                PrintStack();
        }
}
