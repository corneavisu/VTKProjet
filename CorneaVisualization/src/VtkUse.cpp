#include "VtkUse.h"

VtkUse::VtkUse(std::vector<std::vector<float> > matrice, std::vector<std::vector<float> > matriceScalar, float valeurIgnor)
{

    vtkSmartPointer<vtkPolyData> polydata;

    int nbreValue = VtkUse::insertCoord(matrice, matriceScalar, valeurIgnor);

      //warp plane according to cv::Mat surface

    pcoords = vtkSmartPointer<vtkPoints>::New();
    pcoords->SetNumberOfPoints(nbreValue);
    z_scalars = vtkSmartPointer<vtkFloatArray>::New();
    z_scalars->SetNumberOfTuples(nbreValue);






}

int VtkUse::insertCoord(std::vector<std::vector<float> > matrice,std::vector<std::vector<float> > matriceScalar ,float valeurIgnor)
{
    int pointNumber = 0;
    for (float i = 0; i < (int) matrice.size() ; i++)
    {
      for (float j = 0; j < (int)matrice.size(); j++)
      {
        if (matrice[i][j] != valeurIgnor)
        {
          float pts[3] = {i,j,matrice[i][j]};
          pcoords->SetPoint(pointNumber, pts);
          z_scalars->SetValue(pointNumber,matrice[i][j] );
          pointNumber++;
        }
      }
    }
    return pointNumber;


}




VtkUse::~VtkUse()
{
    //dtor
}
