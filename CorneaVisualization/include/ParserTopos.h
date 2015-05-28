#ifndef PARSERTOPOS_H
#define PARSERTOPOS_H
#include "fstream"
#include "vector"
#include "ParserString.h"
#include "UtilsFile.h"
#include "UtilsVector.h"
#include "DataCornee.h"
#include "UtilsTableau.h"


/**
*\file ParserTopos.cpp
*\brief Parse objet
*\author Alexandre.N
*\date 22/05/15
*
*Objet to parse a Topo file
*/

const std::string  delimiterFile("::");
const char delimiterList(',');
class ParserTopos
{

    public:
        ParserTopos(std::string);
        virtual ~ParserTopos();
        bool getDataByName(int index, float dest[SIZEMAX][SIZEMAX]);
        void printAllDataName() ;
        void printDataByName(std::string name);
        void getAllDataName(std::vector<std::string> dest);
        bool getDataByName(std::string name, float dest[SIZEMAX][SIZEMAX]);
        bool testDataVide(int number);


    protected:
    private:
        std::vector<DataCornee> dataList ; /// list of all matrice 101*101 of the file
        std::string nameFile; /// Name of the topos file

        void readToposFile(); /// Parsing of the topos file
        bool buildMatrice(int* index, std::vector<std::vector<std::string> > stringList, float matriceValeur[SIZEMAX][SIZEMAX]);

};

#endif // PARSERTOPOS_H
