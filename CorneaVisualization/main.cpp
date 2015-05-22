#include <iostream>
#include <fstream>
#include <string.h>
#include <stdlib.h>
#include <vector>
#include <list>
#include <sstream>
#include <cstring>
#include "ParserString.h"
#include "ParserTopos.h"


int main(int argc, char *argv[])
{
    // test si un argument a été mis au niveau de
    if(argc != 2)
    {
        std::cout << "Usage: " << argv[0] << " File Topos.txt" << std::endl;
        return EXIT_FAILURE;
    }

    std::string filename = argv[1];

    std::ifstream fichier(filename.c_str());

    // si le fichier existe bien
    if (fichier)
    {
        std::string line;
//        string delimiters = "::";
        std::vector<std::vector<std::string> > stringList;

        std::string foo = "Table Value";
        std::string delimiter = "::";
        double matriceValeur[101][101];
        char delimiter2 = ',';
        int index = 0;

        ParserString::parserCSV(filename,&stringList ,delimiter );

         while(index < (int)stringList.size())
         {
            if (stringList[index][0] == "Table Value")
            {
                index++;
                while(stringList[index][0] == "X[mm], Value[mm]")
                {
                    std::vector<std::string> vecteurTmp(ParserString::explodeTableau(stringList[index][1], delimiter2));
                    vecteurTmp.erase(vecteurTmp.begin());
                    ParserString::vectorToString(&vecteurTmp );
                    index++;
                }
                index = stringList.size();
            }
            else
            {
                index++;
            }

         }


    }
    else
    {
        std::cout << "ERREUR: Impossible d'ouvrir le fichier en lecture." << std::endl;
    }

    return 0;
}
