#include <iostream>

// include these define if you don't use cmake
#include <vtkAutoInit.h>
#define VTK_MODULE_INIT(vtkRenderingOpenGL);

#define vtkRenderingCore_AUTOINIT 4(vtkInteractionStyle,vtkRenderingFreeType,vtkRenderingFreeTypeOpenGL,vtkRenderingOpenGL)
#define vtkRenderingVolume_AUTOINIT 1(vtkRenderingVolumeOpenGL)


#include "ColoredElevationMap.h"
#include "AfficherCube.h"
#include "AfficherCarre.h"



using namespace std;

int main()
{
    //ColoredElevationMap::visualisation();
   //AfficherCube::cube1();
   AfficherCarre newCarre;
    newCarre.carre1();
    AfficherCarre newCarre2;
    newCarre2.carre2();
    return 0;
}
