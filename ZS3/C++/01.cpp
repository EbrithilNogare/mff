#include <iostream>
#include <string>
#include <vector>

using namespace std;


void vypis(const vector <string> & arg) {
    int first = 1;
    for (size_t i = 0; i < arg.size(); i++) {
        if(arg[i].at(0) == '-'){
            int data = stoi(arg[i].substr (2,arg[i].size()));
            cout << arg[i].at(1) << "=[" << data << "]" << "\n";;
        }
        else{
            cout << "something else: " << arg[i] << "\n";
        }
    }
}

int main(int argc, char ** argv) {
    vector <string> arg(argv, argv + argc);

    vypis(arg);

    return 0;
}