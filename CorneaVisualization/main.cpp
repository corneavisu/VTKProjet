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
#include "VtkUse.h"
#include "VTKPlot3D.h"
#include "TestPlotMatrix.h"


int main(int argc, char *argv[])
{
    // test si un argument a été mis au niveau de
    if(argc <= 2)
    {
        std::cout << "Usage: " << argv[0] << " File Topos.txt" << std::endl;
        return EXIT_FAILURE;
    }

    std::string filename = argv[1];
    std::string filetest = argv[2];

    std::ifstream fichier(filename.c_str());

    // si le fichier existe bien
    if (fichier)
    {
        std::cout << "Etape 1 : "<< std::endl;
        ParserTopos newTopos(filename);
        std::cout << "Etape 2 : "<< std::endl;
        newTopos.printAllDataName();
        std::cout << "Etape 3 : "<< std::endl;
        VtkUse newVTK(newTopos.getAnteriorData(), newTopos.getAnteriorBFSData(), 99);
//        std::cout << "Etape 4 : "<< std::endl;
//        newVTK.TestPlotMatrix();
        std::cout << "Etape 5 :" << std::endl;
//        VTKPlot3D newPot3D(filetest);
        std::cout << "Etape 6 : " << std::endl;
//        TestPlotMatrix newTest();
    }
    else
    {
        std::cout << "ERREUR: Impossible d'ouvrir le fichier en lecture." << std::endl;
    }

    return 0;
}
