#include <iostream>

int main(void)
{
    volatile int arr02[4] = {1, 2, 3, 4};
    //volatile int arr06[64];
    //volatile int arr07[128];
    //volatile int arr08[256];

    //volatile int fd = open("testfile.txt", O_RDWR);
    //volatile char *ptr = (char*)mmap(NULL, 4096, PROT_READ | PROT_WRITE, MAP_SHARED, fd, 0);
    //close(fd);
    
    //std::cout<< (void*)ptr << std::endl;
    //cout << ptr[0] << endl;
    std::cout << arr02[0] << std::endl;

    return 0;
}
