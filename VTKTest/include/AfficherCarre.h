#ifndef AFFICHERCARRE_H
#define AFFICHERCARRE_H


#include "vector"
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

class AfficherCarre
{
    public:
        AfficherCarre();
        virtual ~AfficherCarre();
        void carre1();
        void carre2();
      //  void carre3();
        void maillageTriangleFromVectorMatrice(std::vector<std::vector<float> > matrice);
        void visualization();
    protected:
    private:
        vtkPolyData *m_carre;
        vtkPoints *m_points;
        vtkCellArray *m_polys;
        vtkFloatArray *m_scalars;
};

#endif // AFFICHERCARRE_H
