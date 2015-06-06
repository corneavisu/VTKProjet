#ifndef VTKUSE_H
#define VTKUSE_H

#include "UtilsVector.h"

// include these define if you don't use cmake
#include <vtkAutoInit.h>
#define VTK_MODULE_INIT(vtkRenderingOpenGL);

#define vtkRenderingCore_AUTOINIT 4(vtkInteractionStyle,vtkRenderingFreeType,vtkRenderingFreeTypeOpenGL,vtkRenderingOpenGL)
#define vtkRenderingVolume_AUTOINIT 1(vtkRenderingVolumeOpenGL)

//include the required header files for the vtk classes we are using
#include <vtkVersion.h>
#include <vtkRenderer.h>
#include <vtkRenderWindowInteractor.h>
#include <vtkRenderWindow.h>
#include <vtkSmartPointer.h>
#include <vtkFloatArray.h>
#include <vtkPlaneSource.h>
#include <vtkMath.h>
#include <vtkPoints.h>
#include <vtkPointData.h>
#include <vtkPolyData.h>
#include <vtkPolyDataMapper.h>
#include <vtkDataSetMapper.h>
#include <vtkTransformPolyDataFilter.h>
#include <vtkTransform.h>
#include <vtkProperty.h>
#include <vtkStructuredGrid.h>
#include <vtkCubeAxesActor2D.h>
#include <vtkInteractorStyleTrackballCamera.h>
#include <vtkInteractorStyleTrackball.h>
#include <vtkCommand.h>
#include <vtkTimerLog.h>
#include <vtkCallbackCommand.h>
#include <vtkDelaunay2D.h>
#include <vtkXMLPolyDataWriter.h>
#include <vtkLookupTable.h>
#include <vtkElevationFilter.h>
#include <vtkCommand.h>
#include "vtkWarpScalar.h"



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
        int insertCoord(std::vector<std::vector<float> > matrice, std::vector<std::vector<float> > matriceScalar, float valeurIgnor);

};

#endif // VTKUSE_H
