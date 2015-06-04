#include "VtkUse.h"

VtkUse::VtkUse(std::vector<std::vector<float> > matrice, float valeurIgnor)
{

    int numberOfTuples =(matrice.size()*matrice.size())- UtilsVector::countValueInSquareVector(valeurIgnor, matrice);
    // Initialize the float array which represents the points.
    pcoords = vtkFloatArray::New();
    // Note that by default, an array has 1 component.
    // We have to change it to 3 for points
    pcoords->SetNumberOfComponents(3);
    // We ask pcoords to allocate room for at least 4 tuples
    // and set the number of tuples to x.
    pcoords->SetNumberOfTuples(numberOfTuples);

    VtkUse::insertCoord(matrice, numberOfTuples, valeurIgnor);

      // Create vtkPoints and assign pcoords as the internal data array.
  vtkPoints* points = vtkPoints::New();
  points->SetData(pcoords);

  // Create the cells. In this case, a triangle strip with 2 triangles
  // (which can be represented by 4 points)
  vtkCellArray* strips = vtkCellArray::New();
  strips->InsertNextCell(4);
  strips->InsertCellPoint(0);
  strips->InsertCellPoint(1);
  strips->InsertCellPoint(2);
  strips->InsertCellPoint(3);

  // Create an integer array with 4 tuples. Note that when using
  // InsertNextValue (or InsertNextTuple1 which is equivalent in
  // this situation), the array will expand automatically
  vtkIntArray* temperature = vtkIntArray::New();
  temperature->SetName("Temperature");
  temperature->InsertNextValue(10);
  temperature->InsertNextValue(20);
  temperature->InsertNextValue(30);
  temperature->InsertNextValue(40);

  // Create a double array.
  vtkDoubleArray* vorticity = vtkDoubleArray::New();
  vorticity->SetName("Vorticity");
  vorticity->InsertNextValue(2.7);
  vorticity->InsertNextValue(4.1);
  vorticity->InsertNextValue(5.3);
  vorticity->InsertNextValue(3.4);

  // Create the dataset. In this case, we create a vtkPolyData
  vtkPolyData* polydata = vtkPolyData::New();
  // Assign points and cells
  polydata->SetPoints(points);
  polydata->SetStrips(strips);
  // Assign scalars
  polydata->GetPointData()->SetScalars(temperature);
  // Add the vorticity array. In this example, this field
  // is not used.
  polydata->GetPointData()->AddArray(vorticity);

  // Create the mapper and set the appropriate scalar range
  // (default is (0,1)
  vtkPolyDataMapper* mapper = vtkPolyDataMapper::New();
  mapper->SetInputData(polydata);
  mapper->SetScalarRange(0, 40);

  // Create an actor.
  vtkActor* actor = vtkActor::New();
  actor->SetMapper(mapper);

  // Create the rendering objects.
  vtkRenderer* ren = vtkRenderer::New();
  ren->AddActor(actor);

  vtkRenderWindow* renWin = vtkRenderWindow::New();
  renWin->AddRenderer(ren);
  renWin->SetPosition(1400,0);

  vtkRenderWindowInteractor* iren = vtkRenderWindowInteractor::New();
  iren->SetRenderWindow(renWin);
  iren->Initialize();
  iren->Start();

  pcoords->Delete();
  points->Delete();
  strips->Delete();
  temperature->Delete();
  vorticity->Delete();
  polydata->Delete();
  mapper->Delete();
  actor->Delete();
  ren->Delete();
  renWin->Delete();
  iren->Delete();
}

void VtkUse::insertCoord(std::vector<std::vector<float> > matrice, int numberOfTuples, float valeurIgnor)
{
    float x = 0;
    float y = 0;
    for (int i = 0; i < numberOfTuples; i++)
    {
        if (matrice[x][y] != valeurIgnor)
        {
            float pts[3] = {x,y,matrice[x][y]};
            pcoords->SetTupleValue(i,pts);
            y++;
            if ( y >= (int)matrice.size())
            {
                y = 0;
                x++;
            }
        }
    }

}




VtkUse::~VtkUse()
{
    //dtor
}
