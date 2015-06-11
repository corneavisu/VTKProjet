#ifndef VTKUSE_H
#define VTKUSE_H

#include "UtilsVector.h"

// include these define if you don't use cmake
#include <vtkAutoInit.h>
#define VTK_MODULE_INIT(vtkRenderingOpenGL);

#define vtkRenderingCore_AUTOINIT 4(vtkInteractionStyle,vtkRenderingFreeType,vtkRenderingFreeTypeOpenGL,vtkRenderingOpenGL)
#define vtkRenderingVolume_AUTOINIT 1(vtkRenderingVolumeOpenGL)

//include the required header files for the vtk classes we are using
#include <vtkFloatArray.h>
#include <vtkSmartPointer.h>
#include <vtkSurfaceReconstructionFilter.h>
#include <vtkProgrammableSource.h>
#include <vtkContourFilter.h>
#include <vtkReverseSense.h>
#include <vtkPolyDataMapper.h>
#include <vtkProperty.h>
#include <vtkPolyData.h>
#include <vtkCamera.h>
#include <vtkRenderer.h>
#include <vtkRenderWindow.h>
#include <vtkRenderWindowInteractor.h>
#include <vtkSphereSource.h>
#include <vtkXMLPolyDataReader.h>



class VtkUse
{
    public:
        VtkUse( std::vector<std::vector<float> > matrice, std::vector<std::vector<float> > matriceScalar, float valeurIgnor);
        virtual ~VtkUse();
    protected:
    private:
        vtkSmartPointer<vtkPoints> pcoords; /// Create a float array which represents the points.
        vtkSmartPointer<vtkFloatArray> z_scalars;
        ///\brief insert coordonnee in pcoords
        void insertCoord(std::vector<std::vector<float> > matrice, std::vector<std::vector<float> > matriceScalar, float valeurIgnor);
        int nbreValueNonIgnore(std::vector<std::vector<float> > matrice,float valueIgnore);

};

#endif // VTKUSE_H
