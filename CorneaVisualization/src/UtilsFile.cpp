#include "UtilsFile.h"

UtilsFile::UtilsFile()
{
    //ctor
}

/***
*\fn void ParserString::parserCSV(std::string namefile,std::vector<std::vector<std::string> >* result,  std::string delimiter=",")
*\brief Parse a file in a vecteur of string vecteur
*\param path of a file in string, a vector of string vector, a delimiter for the parse function
*\return nothing
*
*use explode
*/
void UtilsFile::parserCSV(std::string namefile,std::vector<std::vector<std::string> >* result,  std::string delimiter=","){

    std::ifstream file (namefile.c_str());
    std::string line;


    while(std::getline(file, line))
    {
        (*result).push_back(ParserString::explode(line,delimiter ));

    }

}

UtilsFile::~UtilsFile()
{
    //dtor
}
