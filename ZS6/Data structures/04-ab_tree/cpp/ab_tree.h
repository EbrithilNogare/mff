#include <limits>
#include <vector>
#include <tuple>
#include <iostream>

using namespace std;

// If the condition is not true, report an error and halt.
#define EXPECT(condition, message) do { if (!(condition)) expect_failed(message); } while (0)

void expect_failed(const string& message);

/*** One node ***/

class ab_node {
  public:
    // Keys stored in this node and the corresponding children
    // The vectors are large enough to accomodate one extra entry
    // in overflowing nodes.
    vector<ab_node *> children;
    vector<int> keys;
    ab_node *parent;

    // If this node contains the given key, return true and set i to key's position.
    // Otherwise return false and set i to the first key greater than the given one.
    bool find_branch(int key, int &i)
    {
        i = 0;
        while (i < keys.size() && keys[i] <= key) {
            if (keys[i] == key)
                return true;
            i++;
        }
        return false;
    }

    // Insert a new key at posision i and add a new child between keys i and i+1.
    void insert_branch(int i, int key, ab_node *child)
    {
        keys.insert(keys.begin() + i, key);
        children.insert(children.begin() + i + 1, child);
    }

    // An auxiliary function for displaying a sub-tree under this node.
    void show(int indent);
};

/*** Tree ***/

class ab_tree {
  public:
    int a;          // Minimum allowed number of children
    int b;          // Maximum allowed number of children
    ab_node *root;  // Root node (even a tree with no keys has a root)
    int num_nodes;  // We keep track of how many nodes the tree has

    // Create a new node and return a pointer to it.
    ab_node *new_node(ab_node* parent)
    {
        ab_node *n = new ab_node;
        n->keys.reserve(b);
        n->children.reserve(b+1);
        n->parent = parent;
        num_nodes++;
        return n;
    }

    // Delete a given node, assuming that its children have been already unlinked.
    void delete_node(ab_node *n)
    {
        num_nodes--;
        delete n;
    }

    // Constructor: initialize an empty tree with just the root.
    ab_tree(int a, int b)
    {
        EXPECT(a >= 2 && b >= 2*a - 1, "Invalid values of a,b");
        this->a = a;
        this->b = b;
        num_nodes = 0;
        // The root has no keys and one null child pointer.
        root = new_node(nullptr);
        root->children.push_back(nullptr);
    }

    // An auxiliary function for deleting a subtree recursively.
    void delete_tree(ab_node *n)
    {
        for (int i=0; i < n->children.size(); i++)
            if (n->children[i])
                delete_tree(n->children[i]);
        delete_node(n);
    }

    // Destructor: delete all nodes.
    ~ab_tree()
    {
        delete_tree(root);
        EXPECT(num_nodes == 0, "Memory leak detected: some nodes were not deleted");
    }

    // Find a key: returns true if it is present in the tree.
    bool find(int key)
    {
        ab_node *n = root;
        while (n) {
            int i;
            if (n->find_branch(key, i))
                return true;
            n = n->children[i];
        }
        return false;
    }

    // Display the tree on standard output in human-readable form.
    void show();

    // Check that the data structure satisfies all invariants.
    void audit();

    // Split the node into two nodes: move some children of n into 
    // a newly created node such that n contains exactly size children in the end.
    // Return the new node and the key separating n and the new node.
    virtual pair<ab_node*, int> split_node(ab_node* n, int size)
    {
        // FIXME: Implement
    }

    // Insert: add key to the tree (unless it was already present).
    virtual void insert(int key)
    {
        // FIXME: Implement
    }
};
