
// include these define if you don't use cmake
#include <vtkAutoInit.h>
#define VTK_MODULE_INIT(vtkRenderingOpenGL);

#define vtkRenderingCore_AUTOINIT 4(vtkInteractionStyle,vtkRenderingFreeType,vtkRenderingFreeTypeOpenGL,vtkRenderingOpenGL)
#define vtkRenderingVolume_AUTOINIT 1(vtkRenderingVolumeOpenGL)

#include <iostream>
#include <fstream>
#include <sstream>
#include <cstring>
#include "ParserString.h"
#include "ParserTopos.h"
#include "ColorElevationMap.h"




int main(int argc, char *argv[])
{
    // test si un argument a été mis au niveau de
    std::cout << argc << std::endl;
    if(argc <= 1)
    {
        std::cout << "Usage: " << argv[0] << " File Topos.txt" << std::endl;
        return EXIT_FAILURE;
    }

    std::string filename = argv[1];
  //  std::string filetest = argv[2];

    std::ifstream fichier(filename.c_str());

    // si le fichier existe bien
    if (fichier)
    {
        std::cout << "Etape 1 : "<< std::endl;
        ParserTopos newTopos(filename);
        std::cout << "Etape 2 : "<< std::endl;
        newTopos.printAllDataName();
        std::cout << "Etape 3 : "<< std::endl;
        ColorElevationMap newMap(newTopos.getAnteriorData(),newTopos.getAnteriorBFSData(), 99.9);
        newMap.visualisation();

    }
    else
    {
        std::cout << "ERREUR: Impossible d'ouvrir le fichier en lecture." << std::endl;
    }

    return 0;
}
