#include "File.h"

File::File(std::string filename)
{
    this.filename = filename;
    file(this.filename.c_str());
}

std::ifstream File::openFile(std::string filename)
{
    std::string filename = argv[1];
    std::ifstream fichier(filename.c_str());
    return fichier;
}


File::~File()
{
    //dtor
}
