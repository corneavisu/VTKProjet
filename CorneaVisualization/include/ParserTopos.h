#ifndef PARSERTOPOS_H
#define PARSERTOPOS_H
#include "fstream"
#include "vector"
#include "ParserString.h"
#include "File.h"

class ParserTopos
{
    File::File file;
    public:
        ParserTopos();
        virtual ~ParserTopos();

    protected:
    private:
        std::vector<double[101][101]> dataList;
        std::vector<std::string> listTypeValeur;

        void readToposFile();

};

#endif // PARSERTOPOS_H
