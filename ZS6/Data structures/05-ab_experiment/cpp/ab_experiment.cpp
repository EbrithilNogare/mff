#include <algorithm>
#include <functional>
#include <string>
#include <utility>
#include <vector>
#include <iostream>
#include <cmath>

#include "ab_tree.h"
#include "random.h"

using namespace std;

void expect_failed(const string& message) {
    cerr << "Test error: " << message << endl;
    exit(1);
}

/*
 *  A modified Splay tree for benchmarking.
 *
 *  We inherit the implementation of operations from the Tree class
 *  and extend it by keeping statistics on the number of splay operations
 *  and the total number of rotations. Also, if naive is turned on,
 *  splay uses only single rotations.
 *
 *  Please make sure that your Tree class defines the rotate() and splay()
 *  methods as virtual.
 */

class BenchmarkingABTree : public ab_tree {
public:
    int num_operations;
    int num_struct_changes;

    BenchmarkingABTree(int a, int b) : ab_tree(a,b)
    {
        reset();
    }

    void reset()
    {
        num_operations = 0;
        num_struct_changes = 0;
    }

    pair<ab_node*, int> split_node(ab_node *node, int size) override
    {
        num_struct_changes++;
        return ab_tree::split_node(node, size);
    }

    void insert(int key) override
    {
        num_operations++;
        ab_tree::insert(key);
    }

    // Return the average number of rotations per operation.
    double struct_changes_per_op()
    {
        if (num_operations > 0)
            return (double) num_struct_changes / num_operations;
        else
            return 0;
    }

    // Delete key from the tree. Does nothing if the key is not in the tree.
    void remove(int key){
        num_operations += 1;

        // Find the key to be deleted
        ab_node *node = root;
        int i;
        bool found = node->find_branch(key, i);
        while(!found){
            node = node->children[i];
            if (!node) return;     // Key is not in the tree
            found = node->find_branch(key, i);
        }

        // If node is not a leaf, we need to swap the key with its successor
        if (node->children[0] != nullptr){        // Only leaves have nullptr as children
            // Successor is leftmost key in the right subtree of key
            ab_node *succ = min(node->children[i+1]);
            swap(node->keys[i], succ->keys[0]);
            node = succ;
        }

        // Now run the main part of the delete
        remove_leaf(key, node);
    }

private:
    // Main part of the remove
    void remove_leaf(int key, ab_node* node)
    {
        EXPECT(node != nullptr, "Trying to delete key from nullptr");
        EXPECT(node->children[0] == nullptr, "Leaf's child must be nullptr");

        while(1){
            // Find the key in the node
            int key_position;
            bool found = node->find_branch(key, key_position);
            EXPECT(found, "Trying to delete key that is not in the node.");

            // Start with the deleting itself
            node->keys.erase(node->keys.cbegin() + key_position);
            node->children.erase(node->children.cbegin() + key_position + 1);

            // No underflow means we are done
            if (node->children.size() >= a) return;

            // Root may underflow, but cannot have just one child (unless tree is empty)
            if (node == root){
                if ((node->children.size() == 1) && (root->children[0] != nullptr)){
                    ab_node *old_root = root;
                    root = root->children[0];
                    root->parent = nullptr;
                    delete_node(old_root);
                }
                return;
            }

            ab_node *brother;
            int separating_key_pos;
            bool tmp;
            tie(brother, separating_key_pos, tmp) = get_brother(node);
            int separating_key = node->parent->keys[separating_key_pos];

            // First check whether we can steal brother's child
            if (brother->children.size() > a){
                steal_child(node);
                return;
            }

            // If the brother is too small, we merge with him and propagate the delete
            node = merge_node(node);
            node = node->parent;
            key = separating_key;
            key_position = separating_key_pos;
        }
    }

    // Return the leftmost node of a subtree rooted at node.
    ab_node* min(ab_node *node)
    {
        EXPECT(node != nullptr, "Trying to search for minimum of nullptr");
        while (node->children[0]) {
            node = node->children[0];
        }
        return node;
    }

    // Return the left brother if it exists, otherwise return right brother.
    // Returns tuple (brother, key_position, is_left_brother), where
    // key_position is a position of the key that separates node and brother in their parent.
    tuple<ab_node*, int, bool> get_brother(ab_node* node)
    {
        ab_node *parent = node->parent;
        EXPECT(parent != nullptr, "Node without parent has no brother");

        // Find node in parent's child list
        int i;
        for(i = 0; i < parent->children.size(); ++i){
            ab_node *c = parent->children[i];
            if (c == node) break;
        }
        EXPECT(i < parent->children.size(), "Node is not inside its parent");

        if (i == 0){
            return make_tuple(parent->children[1], 0, false);
        }
        else{
            return make_tuple(parent->children[i - 1], i - 1, true);
        }
    }

    // Transfer one child from node's left brother to the node.
    // If node has no left brother, use right brother instead.
    void steal_child(ab_node* node)
    {
        ab_node *brother;
        int separating_key_pos;
        bool is_left_brother;
        tie(brother, separating_key_pos, is_left_brother) = get_brother(node);
        int separating_key = node->parent->keys[separating_key_pos];

        EXPECT(brother->children.size() > a, "Stealing child causes underflow in brother!");
        EXPECT(node->children.size() < b, "Stealing child causes overflow in the node!");

        // We steal either from front or back
        int steal_position, target_position;
        if (is_left_brother){
            steal_position = brother->children.size()-1;
            target_position = 0;
        }
        else{
            steal_position = 0;
            target_position = node->children.size();
        }
        // Steal the child
        ab_node *stolen_child = brother->children[steal_position];
        if (stolen_child != nullptr){
            stolen_child->parent = node;
        }
        node->children.insert(node->children.cbegin() + target_position, stolen_child);
        brother->children.erase(brother->children.cbegin() + steal_position);

        // List of keys is shorter than list of children
        if (is_left_brother) steal_position -= 1;
        else target_position -= 1;

        // Update keys
        node->keys.insert(node->keys.cbegin() + target_position, separating_key);
        node->parent->keys[separating_key_pos] = brother->keys[steal_position];
        brother->keys.erase(brother->keys.cbegin() + steal_position);
    }

public:
    // Merge node with its left brother and destroy the node. Must not cause overflow!
    // Returns result of the merge.
    // If node has no left brother, use right brother instead.
    ab_node* merge_node(ab_node* node){
        num_struct_changes += 1;

        ab_node *brother;
        int separating_key_pos;
        bool is_left_brother;
        tie(brother, separating_key_pos, is_left_brother) = get_brother(node);
        int separating_key = node->parent->keys[separating_key_pos];

        // We swap brother and node if necessary so that the node is always on the right
        if (!is_left_brother) swap(brother, node);

        for (auto c: node->children)
            brother->children.push_back(c);
        brother->keys.push_back(separating_key);
        for (auto k: node->keys)
            brother->keys.push_back(k);

        EXPECT(brother->children.size() <= b, "Merge caused overflow!");

        // Update parent pointers in non-leaf
        if (brother->children[0] != nullptr){
            for (auto c : brother->children)
                c->parent = brother;
        }
        
        delete_node(node);
        return brother;
    }
};

int a, b;
RandomGen *rng;         // Random generator object

// An auxiliary function for generating a random permutation.
vector<int> random_permutation(int n)
{
    vector<int> perm;
    for (int i=0; i<n; i++)
        perm.push_back(i);
    for (int i=0; i<n-1; i++)
        swap(perm[i], perm[i + rng->next_range(n-i)]);
    return perm;
}

void test_insert()
{
    for (int e=32; e<=64; e++) {
        int n = (int) pow(2, e/4.);
        BenchmarkingABTree tree = BenchmarkingABTree(a,b);

        vector<int> perm = random_permutation(n);
        for (int x : perm)
            tree.insert(x);

        cout << n << " " << tree.struct_changes_per_op() << endl;
    }
}

void test_random()
{
    for (int e=32; e<=64; e++) {
        int n = (int) pow(2, e/4.);
        BenchmarkingABTree tree = BenchmarkingABTree(a,b);

        // We keep track of elements present and not present in the tree
        vector<int> elems;
        vector<int> anti_elems;
        elems.reserve(n);
        anti_elems.reserve(n+1);

        for (int x = 0; x < 2*n; x+=2){
            tree.insert(x);
            elems.push_back(x);
        }

        for (int i = -1; i <2*n + 1; i+=2)
            anti_elems.push_back(i);

        for (int i=0; i<n; i++){
            int r, x;
            // Delete random element
            r = rng->next_range(elems.size());
            x = elems[r];
            tree.remove(x);
            elems.erase(elems.cbegin() + r);
            anti_elems.push_back(x);

            // Insert random "anti-element"
            r = rng->next_range(anti_elems.size());
            x = anti_elems[r];
            tree.insert(x);
            elems.push_back(x);
            anti_elems.erase(anti_elems.cbegin() + r);
        }

        cout << n << " " << tree.struct_changes_per_op() << endl;
    }
}

void test_min()
{
    for (int e=32; e<=64; e++) {
        int n = (int) pow(2, e/4.);
        BenchmarkingABTree tree = BenchmarkingABTree(a,b);

        for (int x = 0; x < n; x++)
            tree.insert(x);

        for (int i=0; i<n; i++){
            tree.remove(0);
            tree.insert(0);
        }

        cout << n << " " << tree.struct_changes_per_op() << endl;
    }
}

vector<pair<string, function<void()>>> tests = {
    { "insert", test_insert },
    { "random", test_random },
    { "min",    test_min },
};

int main(int argc, char **argv)
{
    if (argc != 4) {
        cerr << "Usage: " << argv[0] << " <test> <student-id> (2-3|2-4)" << endl;
        return 1;
    }

    string which_test = argv[1];
    string id_str = argv[2];
    string mode = argv[3];

    try {
        rng = new RandomGen(stoi(id_str));
    } catch (...) {
        cerr << "Invalid student ID" << endl;
        return 1;
    }

    a = 2;
    if (mode == "2-3")
      b = 3;
    else if (mode == "2-4")
      b = 4;
    else
      {
        cerr << "Last argument must be either '2-3' or '2-4'" << endl;
        return 1;
      }

    for (const auto& test : tests) {
        if (test.first == which_test)
          {
            cout.precision(12);
            test.second();
            return 0;
          }
    }
    cerr << "Unknown test " << which_test << endl;
    return 1;
    
   return 0;
}
