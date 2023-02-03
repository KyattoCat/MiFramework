using System.Xml.Linq;

namespace MiFramework.Struct
{
    public class BinarySearchTree<T> where T : IComparable
    {
        private class TreeNode
        {
            public T element;
            public int height;
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

            public void UpdateHeight()
            {
                int leftHeight = leftChild?.height ?? -1;
                int rightHeight = rightChild?.height ?? -1;
                height = Math.Max(leftHeight, rightHeight) + 1;
            }

            public int GetBalanceFactor()
            {
                int leftHeight = leftChild?.height ?? -1;
                int rightHeight = rightChild?.height ?? -1;
                return leftHeight - rightHeight;
            }
        }

        private TreeNode? root;

        private TreeNode? RotateRight(TreeNode root)
        {
            TreeNode? child = root.leftChild;
            if (child == null) return root;

            TreeNode? grandChild = child.rightChild;

            child.rightChild = root;
            root.leftChild = grandChild;

            root.UpdateHeight();
            child.UpdateHeight();
            
            return child;
        }

        private TreeNode? RotateLeft(TreeNode root)
        {
            TreeNode? child = root.rightChild;
            if (child == null) return root;

            TreeNode? grandChild = child.leftChild;

            child.leftChild = root;
            root.rightChild = grandChild;

            root.UpdateHeight();
            child.UpdateHeight();

            return child;
        }

        private TreeNode? Rotate(TreeNode root)
        {
            int rootBalanceFactor = root.GetBalanceFactor();
            // 右侧偏高
            if (rootBalanceFactor < -1)
            {
                if (root.rightChild == null)
                {
                    return root;
                }

                int childBalanceFactor = root.rightChild.GetBalanceFactor();
                
                if (childBalanceFactor <= 0)
                {
                    return RotateLeft(root);
                }
                else
                {
                    root.rightChild = RotateRight(root.rightChild);
                    return RotateLeft(root);
                }
            }
            else if (rootBalanceFactor > 1)
            {
                if (root.leftChild == null)
                {
                    return root;
                }
                
                int childBalanceFactor = root.leftChild.GetBalanceFactor();

                if (childBalanceFactor >= 0)
                {
                    return RotateRight(root);
                }
                else
                {
                    root.leftChild = RotateLeft(root.leftChild);
                    return RotateRight(root);
                }
            }

            return root;
        }

        private TreeNode? Add(TreeNode? root, T element)
        {
            if (root == null) return new TreeNode(element);

            int compareResult = element.CompareTo(root.element);

            if (compareResult > 0)
            {
                root.rightChild = Add(root.rightChild, element);
            }
            else if (compareResult < 0)
            {
                root.leftChild = Add(root.leftChild, element);
            }
            else
            {
                return root;
            }
            root.UpdateHeight();
            root = Rotate(root);
            return root;
        }

        public void Add(T element)
        {
            root = Add(root, element);
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
            
            node.UpdateHeight();
            node = Rotate(node);

            return node;
        }

        public void Remove(T element)
        {
            root = Remove(root, element);
        }

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
    }
}
