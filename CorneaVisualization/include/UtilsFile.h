#ifndef UTILSFILE_H
#define UTILSFILE_H
#include "ParserString.h"


class UtilsFile
{
    public:
        UtilsFile();
        virtual ~UtilsFile();

         ///\brief parse a file with a delimiter to have a vector of string
        static void parserCSV(std::string namefile, std::vector<std::vector<std::string> >* result,std::string delimiter);

    protected:
    private:
};

#endif // UTILSFILE_H
