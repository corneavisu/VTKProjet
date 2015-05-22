#ifndef FILE_H
#define FILE_H
#include "fstream"

class File
{
    std::ifstream file;
    std::string filename;
    public:
        File();
        virtual ~File();
        std::ifstream openFile(std::string filename);
    protected:
    private:
};

#endif // FILE_H
