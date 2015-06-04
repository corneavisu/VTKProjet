#ifndef VTKUSE_H
#define VTKUSE_H

#include "UtilsVector.h"
//include the required header files for the vtk classes we are using
#include "vtkActor.h"
#include "vtkCellArray.h"
#include "vtkDoubleArray.h"
#include "vtkFloatArray.h"
#include "vtkIntArray.h"
#include "vtkPointData.h"
#include "vtkPoints.h"
#include "vtkPolyData.h"
#include "vtkPolyDataMapper.h"
#include "vtkRenderWindow.h"
#include "vtkRenderWindowInteractor.h"
#include "vtkRenderer.h"

class VtkUse
{
    public:
        VtkUse( std::vector<std::vector<float> > matrice, float valeurIgnor);
        virtual ~VtkUse();
    protected:
    private:
        vtkFloatArray* pcoords; /// Create a float array which represents the points.

        ///\brief insert coordonnee in pcoords
        void insertCoord(std::vector<std::vector<float> > matrice, int numberOfTuples, float valeurIgnor);

};

#endif // VTKUSE_H
