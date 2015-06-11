#ifndef AFFICHERCUBE_H
#define AFFICHERCUBE_H


//include the required header files for the vtk classes we are using
#include "vtkActor.h"
#include "vtkCamera.h"
#include "vtkCellArray.h"
#include "vtkFloatArray.h"
#include "vtkPointData.h"
#include "vtkPoints.h"
#include "vtkPolyData.h"
#include "vtkPolyDataMapper.h"
#include "vtkRenderWindow.h"
#include "vtkRenderWindowInteractor.h"
#include "vtkRenderer.h"

class AfficherCube
{
    public:
        AfficherCube();
        virtual ~AfficherCube();
        static void cube1();
    protected:
    private:
};

#endif // AFFICHERCUBE_H
