namespace MiFramework.Struct
{
    public class BinarySearchTree<T> where T : IComparable
    {
        private class TreeNode
        {
            public T element;
            public TreeNode? leftChild;
            public TreeNode? rightChild;

            public bool IsLeaf => leftChild == null && rightChild == null;

            public TreeNode(T element)
            {
                this.element = element;
            }

            public int CompareTo(TreeNode otherNode)
            {
                return element.CompareTo(otherNode.element);
            }
        }

        private TreeNode? root;

        public void Add(T element)
        {
            TreeNode newNode = new(element);

            if (root == null)
            {
                root = newNode;
                return;
            }

            TreeNode? node = root;

            while (node != null)
            {
                int compareResult = newNode.CompareTo(node);

                if (compareResult == 0)
                {
                    return;
                }
                else if (compareResult > 0)
                {
                    if (node.rightChild == null)
                    {
                        node.rightChild = newNode;
                        return;
                    }
                    
                    node = node.rightChild;
                }
                else
                {
                    if (node.leftChild == null)
                    {
                        node.leftChild = newNode;
                        return;
                    }
                    
                    node = node.leftChild;
                }
            }
        }

        private TreeNode? Remove(TreeNode? node, T element)
        {
            if (node == null)
                return null;

            int compareResult = element.CompareTo(node.element);
            
            if (compareResult > 0)
            {
                node.rightChild = Remove(node.rightChild, element);
            }
            else if (compareResult < 0)
            {
                node.leftChild = Remove(node.leftChild, element);
            }
            else
            {
                if (node.leftChild != null && node.rightChild != null)
                {
                    TreeNode? rightmostNode = node.leftChild;
                    while (rightmostNode?.rightChild != null)
                    {
                        rightmostNode = rightmostNode.rightChild;
                    }
                    if (rightmostNode != null)
                    {
                        node.leftChild = Remove(node.leftChild, rightmostNode.element);
                        node.element = rightmostNode.element;
                    }
                }
                else if (node.leftChild != null)
                {
                    return node.leftChild;
                }
                else if (node.rightChild != null)
                {
                    return node.rightChild;
                }
                else
                {
                    return null;
                }
            }

            return node;
        }

        public void Remove(T element)
        {
            root = Remove(root, element);
        }

#if DEBUG
        public void Print()
        {
            if (root == null) return;

            Queue<TreeNode> queue = new();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                TreeNode node = queue.Dequeue();
                
                if (node == null)
                    continue;

                Debug.Log(node.element.ToString() + "\n");

                if (node.leftChild != null)
                {
                    queue.Enqueue(node.leftChild);
                }
                if (node.rightChild != null)
                {
                    queue.Enqueue(node.rightChild);
                }
            }
        }
#endif

        public bool Contains(T element)
        {
            TreeNode? node = root;

            while (node != null)
            {
                int compareResult = element.CompareTo(node.element);

                if (compareResult == 0)
                {
                    return true;
                }
                else if (compareResult > 0)
                {
                    node = node.rightChild;
                }
                else
                {
                    node = node.leftChild;
                }
            }

            return false;
        }
    }
}
