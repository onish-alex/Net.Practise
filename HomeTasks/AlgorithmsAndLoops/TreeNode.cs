using System;
using System.Collections.Generic;
using System.Text;

namespace AlgorithmsAndLoops
{
    public class TreeNode<T>
    {
        public T Data { get; }

        public List<TreeNode<T>> Child { get; set; }

        public TreeNode(T data)
        {
            this.Data = data;
            this.Child = new List<TreeNode<T>>();
        }

        public IEnumerable<TreeNode<T>> DepthFirstTraversal()
        {
            var mainStack = new Stack<TreeNode<T>>();
            var visitedStack = new Stack<TreeNode<T>>();
            mainStack.Push(this);
            while(mainStack.Count != 0)
            {
                var top = mainStack.Peek();
                
                visitedStack.Push(mainStack.Peek());

                if (top.Child.Count == 0)
                {
                    while(mainStack.Count != 0 && mainStack.Peek().Child.Find(a => !visitedStack.Contains(a)) == null)
                    {
                        mainStack.Pop();
                    }
                }

                if (mainStack.Count != 0)
                {
                    var nextUnvisitedChild = mainStack.Peek().Child.Find(a => !visitedStack.Contains(a));
                    mainStack.Push(nextUnvisitedChild);
                }

                yield return top;
            }
        }

        public IEnumerable<TreeNode<T>> BreadthFirstTraversal()
        {
            var list = new List<TreeNode<T>>();
            list.Add(this);
            for (int i = 0; i < list.Count; i++)
            {
                list.AddRange(list[i].Child);
                yield return list[i];
            }
        }

        public static TreeNode<int> GetDefaultTree()
        {
            var tree = new TreeNode<int>(0)
            {
                Child = new List<TreeNode<int>>
                {
                    new TreeNode<int>(10)
                    {
                        Child = new List<TreeNode<int>>
                        {
                            new TreeNode<int>(11),
                            new TreeNode<int>(12)
                        }
                    },
                    new TreeNode<int>(20)
                    {
                        Child = new List<TreeNode<int>>
                        {
                            new TreeNode<int>(21),
                            new TreeNode<int>(22)
                        }
                    },
                    new TreeNode<int>(30)
                    {
                        Child = new List<TreeNode<int>>
                        {
                            new TreeNode<int>(31)
                        }
                    }
                }
            };

            return tree;
        }
    }
}
