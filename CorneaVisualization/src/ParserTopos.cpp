#include "ParserTopos.h"

ParserTopos::ParserTopos(std::string filename)
{

}

void ParserTopos::readToposFile()
{
    std::string line;
    //        string delimiters = "::";
    std::vector<std::vector<std::string> > stringList;

    std::string foo = "Table Value";
    std::string delimiter = "::";
    float matriceValeur[101][101];
    char delimiter2 = ',';
    int index = 0;
    int xIndex = 0;

    ParserString::parserCSV(filename,&stringList ,delimiter );

     while(index < (int)stringList.size())
     {
        if (stringList[index][0] == "Table Value")
        {
            listTypeValeur.push_back(stringList[index][1]);
            index++;
            while(stringList[index][0] == "X[mm], Value[mm]")
            {
                std::vector<std::string> vecteurTmp(ParserString::explodeTableau(stringList[index][1], delimiter2));
                vecteurTmp.erase(vecteurTmp.begin());
//                ParserString::vectorToString(&vecteurTmp );
                ParserString::vecteurStringTofloatArray(vecteurTmp, matriceValeur[xIndex]);
                xIndex++;
                index++;
            }
            xIndex = 0;
        }
        else
        {
            index++;
        }

     }

}


ParserTopos::~ParserTopos()
{
    //dtor
}
