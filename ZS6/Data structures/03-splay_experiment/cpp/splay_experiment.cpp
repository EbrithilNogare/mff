#include <algorithm>
#include <functional>
#include <string>
#include <utility>
#include <vector>
#include <iostream>
#include <cmath>

#include "splay_operation.h"
#include "random.h"

using namespace std;

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

class BenchmarkingTree : public Tree {
public:
    int num_operations;
    int num_rotations;
    bool do_naive;

    BenchmarkingTree(bool naive=false)
    {
        do_naive = naive;
        reset();
    }

    void reset()
    {
        num_operations = 0;
        num_rotations = 0;
    }

    void rotate(Node *node) override
    {
        num_rotations++;
        Tree::rotate(node);
    }

    void splay(Node *node) override
    {
        num_operations++;
        if (do_naive) {
            while (node->parent)
                rotate(node);
        } else {
            Tree::splay(node);
        }
    }

    // Return the average number of rotations per operation.
    double rot_per_op()
    {
        if (num_operations > 0)
            return (double) num_rotations / num_operations;
        else
            return 0;
    }
};

bool naive;             // Use of naive rotations requested
RandomGen *rng;         // Random generator object

void test_sequential()
{
    for (int n=100; n<=3000; n+=100) {
        BenchmarkingTree tree = BenchmarkingTree(naive);

        for (int x=0; x<n; x++)
            tree.insert(x);

        for (int i=0; i<5; i++)
            for (int x=0; x<n; x++)
                tree.lookup(x);

        cout << n << " " << tree.rot_per_op() << endl;
    }
}

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

void test_random()
{
    for (int e=32; e<=64; e++) {
        int n = (int) pow(2, e/4.);
        BenchmarkingTree tree = BenchmarkingTree(naive);

        vector<int> perm = random_permutation(n);
        for (int x : perm)
            tree.insert(x);

        for (int i=0; i<5*n; i++)
            tree.lookup(rng->next_range(n));

        cout << n << " " << tree.rot_per_op() << endl;
    }
}

/*
 *  An auxiliary function for constructing arithmetic progressions.
 *  The vector seq will be modified to contain an arithmetic progression
 *  of elements in interval [A,B] starting from position s with step inc.
 */
void make_progression(vector<int> &seq, int A, int B, int s, int inc)
{
    for (int i=0; i<seq.size(); i++)
        while (seq[i] >= A && seq[i] <= B && s + inc*(seq[i]-A) != i)
            swap(seq[i], seq[s + inc*(seq[i] - A)]);
}

void test_subset_s(int sub)
{
    for (int e=32; e<=64; e++) {
        int n = (int) pow(2, e/4.);
        if (n < sub)
          continue;

        // We will insert elements in order, which contain several
        // arithmetic progressions interspersed with random elements.
        vector<int> seq = random_permutation(n);
        make_progression(seq, n/4, n/4 + n/20, n/10, 1);
        make_progression(seq, n/2, n/2 + n/20, n/10, -1);
        make_progression(seq, 3*n/4, 3*n/4 + n/20, n/2, -4);
        make_progression(seq, 17*n/20, 17*n/20 + n/20, 2*n/5, 5);

        BenchmarkingTree tree = BenchmarkingTree(naive);
        for (int x : seq)
            tree.insert(x);
        tree.reset();

        for (int i=0; i<10000; i++)
            tree.lookup(seq[rng->next_range(sub)]);

        cout << sub << " " << n << " " << tree.rot_per_op() << endl;
    }
}

void test_subset()
{
    test_subset_s(10);
    test_subset_s(100);
    test_subset_s(1000);
}

vector<pair<string, function<void()>>> tests = {
    { "sequential", test_sequential },
    { "random",     test_random },
    { "subset",     test_subset },
};

int main(int argc, char **argv)
{
    if (argc != 4) {
        cerr << "Usage: " << argv[0] << " <test> <student-id> (std|naive)" << endl;
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

    if (mode == "std")
      naive = false;
    else if (mode == "naive")
      naive = true;
    else
      {
        cerr << "Last argument must be either 'std' or 'naive'" << endl;
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
}
