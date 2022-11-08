#include <functional>
#include <cstdlib>
#include <vector>

#include "ab_tree.h"

// Debugging output: showing trees prettily on standard output.

void ab_tree::show()
{
    root->show(0);
    for (int i=0; i<70; i++)
        cout << '=';
    cout << endl;
}

void ab_node::show(int indent)
{
    for (int i = children.size() - 1; i >= 0 ; i--) {
        if (i < keys.size()) {
            for (int j = 0; j < indent; j++)
                cout << "    ";
            cout << keys[i] << endl;
        }
        if (children[i])
            children[i]->show(indent+1);
    }
}

// Invariant checks

void audit_subtree(ab_tree *tree, ab_node *n, ab_node* parent, int key_min, int key_max, int depth, int &leaf_depth)
{
    if (!n) {
        // Check that all leaves are on the same level.
        if (leaf_depth < 0)
            leaf_depth = depth;
        else
            EXPECT(depth == leaf_depth, "Leaves are not on the same level");
        return;
    }
    // Check consistency of parent pointers
    EXPECT(n->parent == parent, "Inconsistent parent pointers");

    // The number of children must be in the allowed range.
    if (depth > 0)
        EXPECT(n->children.size() >= tree->a, "Too few children");
    EXPECT(n->children.size() <= tree->b, "Too many children");

    // We must have one more children than keys.
    EXPECT(n->children.size() == n->keys.size() + 1, "Number of keys does not match number of children");

    // Allow degenerate trees with 0 keys in the root.
    if (n->children.size() == 1)
        return;

    // Check order of keys: they must be increasing and bounded by the keys on the higher levels.
    for (int i = 0; i < n->keys.size(); i++) {
        EXPECT(n->keys[i] >= key_min && n->keys[i] <= key_max, "Wrong key order");
        EXPECT(i == 0 || n->keys[i-1] < n->keys[i], "Wrong key order");
    }

    // Call on children recursively.
    for (int i = 0; i < n->children.size(); i++) {
        int tmin, tmax;
        if (i == 0)
            tmin = key_min;
        else
            tmin = n->keys[i-1] + 1;
        if (i < n->keys.size())
            tmax = n->keys[i] - 1;
        else
            tmax = key_max;
        audit_subtree(tree, n->children[i], n, tmin, tmax, depth+1, leaf_depth);
    }
}

void ab_tree::audit()
{
    EXPECT(root, "Tree has no root");
    int leaf_depth = -1;
    audit_subtree(this, root, nullptr, numeric_limits<int>::min(), numeric_limits<int>::max(), 0, leaf_depth);
}

// A basic test: insert a couple of keys and show how the tree evolves.

void test_basic()
{
    cout << "## Basic test" << endl;

    ab_tree t(2, 3);
    vector<int> keys = { 3, 1, 4, 5, 9, 2, 6, 8, 7, 0 };
    for (int k : keys) {
        t.insert(k);
        t.show();
        t.audit();
        EXPECT(t.find(k), "Inserted key disappeared");
    }

    for (int k : keys)
        EXPECT(t.find(k), "Some keys are missing at the end");
}

// The main test: inserting a lot of keys and checking that they are really there.
// We will insert num_items keys from the set {1,...,range-1}, where range is a prime.

void test_main(int a, int b, int range, int num_items)
{
    // Create a new tree.
    cout << "## Test: a=" << a << " b=" << b << " range=" << range << " num_items=" << num_items << endl;
    ab_tree t(a, b);

    int key = 1;
    int step = (int)(range * 1.618);
    int audit_time = 1;

    // Insert keys.
    for (int i=1; i <= num_items; i++) {
        t.insert(key);
        // Audit the tree occasionally.
        if (i == audit_time || i == num_items) {
            // cout << "== Audit at " << i << endl;
            // t.show();
            t.audit();
            audit_time = (int)(audit_time * 1.33) + 1;
        }
        key = (key + step) % range;
    }

    // Check that the tree contains exactly the items it should contain.
    key = 1;
    for (int i=1; i < range; i++) {
        bool found = t.find(key);
        // cout << "Step #" << i << ": find(" << key << ") = " << found << endl;
        EXPECT(found == (i <= num_items), "Tree contains wrong keys");
        key = (key + step) % range;
    }
}

/*** A list of all tests ***/

vector<pair<string, function<void()>>> tests = {
    { "basic",       [] { test_basic(); } },
    { "small-2,3",   [] { test_main(2, 3, 997, 700); } },
    { "small-2,4",   [] { test_main(2, 4, 997, 700); } },
    { "big-2,3",     [] { test_main(2, 3, 999983, 700000); } },
    { "big-2,4",     [] { test_main(2, 4, 999983, 700000); } },
    { "big-10,20",   [] { test_main(10, 20, 999983, 700000); } },
    { "big-100,200", [] { test_main(100, 200, 999983, 700000); } },
};
