#ifndef COLOREDELEVATIONMAP_H
#define COLOREDELEVATIONMAP_H



//include the required header files for the vtk classes we are using
#include <vtkVersion.h>
#include <vtkSmartPointer.h>

#include <vtkActor.h>
#include <vtkDelaunay2D.h>
#include <vtkLookupTable.h>
#include <vtkMath.h>
#include <vtkPointData.h>
#include <vtkPoints.h>
#include <vtkPolyData.h>
#include <vtkPolyDataMapper.h>
#include <vtkProperty.h>
#include <vtkRenderWindow.h>
#include <vtkRenderWindowInteractor.h>
#include <vtkRenderer.h>
#include <vtkVertexGlyphFilter.h>
#include <vtkXMLPolyDataWriter.h>

class ColoredElevationMap
{
    public:
        ColoredElevationMap();
        virtual ~ColoredElevationMap();
        static void visualisation();
    protected:
    private:
};

#endif // COLOREDELEVATIONMAP_H
