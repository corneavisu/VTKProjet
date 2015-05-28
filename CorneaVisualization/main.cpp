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

        ParserTopos newTopos(filename);
        newTopos.printAllDataName();



    }
    else
    {
        std::cout << "ERREUR: Impossible d'ouvrir le fichier en lecture." << std::endl;
    }

    return 0;
}
