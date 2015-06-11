#include "VtkUse.h"

VtkUse::VtkUse(std::vector<std::vector<float> > matrice, std::vector<std::vector<float> > matriceScalar, float valeurIgnor)
{

    int etape = 0;

    std::cout << ++etape << std::endl;
    vtkSmartPointer<vtkPolyData> polydata;

    //warp plane according to cv::Mat surface
    int nbreValue = VtkUse::nbreValueNonIgnore(matrice, valeurIgnor);
    std::cout << ++etape << std::endl;
    pcoords = vtkSmartPointer<vtkPoints>::New();
    pcoords->SetNumberOfPoints(nbreValue);
    z_scalars = vtkSmartPointer<vtkFloatArray>::New();
    z_scalars->SetNumberOfTuples(nbreValue);
    std::cout << ++etape << std::endl;
    VtkUse::insertCoord(matrice, matriceScalar, valeurIgnor);
    std::cout << ++etape << std::endl;
    polydata->SetPoints(pcoords);
    std::cout << ++etape << std::endl;
    // Construct the surface and create isosurface.
    vtkSmartPointer<vtkSurfaceReconstructionFilter> surf =vtkSmartPointer<vtkSurfaceReconstructionFilter>::New();
    surf->SetInputData(polydata);

    std::cout << ++etape << std::endl;
    vtkSmartPointer<vtkContourFilter> cf =
    vtkSmartPointer<vtkContourFilter>::New();
    cf->SetInputConnection(surf->GetOutputPort());
    cf->SetValue(0, 0.0);
    std::cout << ++etape << std::endl;
    // Sometimes the contouring algorithm can create a volume whose gradient
    // vector and ordering of polygon (using the right hand rule) are
    // inconsistent. vtkReverseSense cures this problem.
    vtkSmartPointer<vtkReverseSense> reverse =
    vtkSmartPointer<vtkReverseSense>::New();
    reverse->SetInputConnection(cf->GetOutputPort());
    reverse->ReverseCellsOn();
    reverse->ReverseNormalsOn();
    std::cout << ++etape << std::endl;
    vtkSmartPointer<vtkPolyDataMapper> map =
    vtkSmartPointer<vtkPolyDataMapper>::New();
    map->SetInputConnection(reverse->GetOutputPort());
    map->ScalarVisibilityOff();
    std::cout << ++etape << std::endl;
    vtkSmartPointer<vtkActor> surfaceActor =
    vtkSmartPointer<vtkActor>::New();
    surfaceActor->SetMapper(map);
std::cout << ++etape << std::endl;
    // Create the RenderWindow, Renderer and both Actors
    vtkSmartPointer<vtkRenderer> ren =
    vtkSmartPointer<vtkRenderer>::New();
    vtkSmartPointer<vtkRenderWindow> renWin =
    vtkSmartPointer<vtkRenderWindow>::New();
    renWin->AddRenderer(ren);
    vtkSmartPointer<vtkRenderWindowInteractor> iren =
    vtkSmartPointer<vtkRenderWindowInteractor>::New();
    iren->SetRenderWindow(renWin);
    std::cout << ++etape << std::endl;
    // Add the actors to the renderer, set the background and size
    ren->AddActor(surfaceActor);
    ren->SetBackground(.2, .3, .4);

    renWin->Render();
    iren->Start();

}

void VtkUse::insertCoord(std::vector<std::vector<float> > matrice,std::vector<std::vector<float> > matriceScalar ,float valeurIgnor)
{
    int pointNumber = 0;
    for (float i = 0; i < (int) matrice.size() ; i++)
    {
      for (float j = 0; j < (int)matrice.size(); j++)
        if (matrice[i][j] != valeurIgnor)
        {
          float pts[3] = {i,j,matrice[i][j]};
          pcoords->SetPoint(pointNumber, pts);
          z_scalars->SetValue(pointNumber,matrice[i][j] );
        }
    }
}

int VtkUse::nbreValueNonIgnore(std::vector<std::vector<float> > matrice,float valueIgnore)
{
    int pointNumber = 0;
    for (float i = 0; i < (int) matrice.size() ; i++)
    {
      for (float j = 0; j < (int)matrice.size(); j++)
        if (matrice[i][j] != valueIgnore)
        {
          pointNumber++;
        }
    }
    return pointNumber;
}




VtkUse::~VtkUse()
{
    //dtor
}
