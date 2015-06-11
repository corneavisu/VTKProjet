#include "AfficherCarre.h"

AfficherCarre::AfficherCarre()
{
       // We'll create the building blocks of polydata including data attributes.
    m_carre = vtkPolyData::New();
    m_points = vtkPoints::New();
    m_polys = vtkCellArray::New();
    m_scalars = vtkFloatArray::New();
}

void AfficherCarre::carre1()
{
    int i;
    float x[4][3] = {{0,0,0}, {1,0,0}, {1,1,0}, {0,1,0}};
    vtkIdType pts[4][3] = {{0,1,2}, {0,3,2}, {1,3,2}, {1,0,3} };



    // Load the point, cell, and data attributes.
    for (i=0; i<4; i++) m_points->InsertPoint(i,x[i]);
    for (i=0; i<4; i++) m_polys->InsertNextCell(3,pts[i]);
    for (i=0; i<4; i++) m_scalars->InsertTuple1(i,i);

    visualization();

}

void AfficherCarre::carre2()
{
    std::cout << "step 1 "<< std::endl;
    std::vector<std::vector< float> > matrice;

    for (int i = 0; i < 4; i++)
    {
        std::vector< float>  matriceTmp;
        for (int i = 0; i < 4; i++)
        {
            matriceTmp.push_back(0);
        }
        matrice.push_back(matriceTmp);
    }
    std::cout << "step 2 "<< std::endl;
    AfficherCarre::maillageTriangleFromVectorMatrice(matrice);
    std::cout << "step 3 "<< std::endl;
    AfficherCarre::visualization();
    std::cout << "step 4 "<< std::endl;
}
/*
void AfficherCarre::carre3()
{
    int i;
    //////////////////////////////////////////////
     std::cout << "step 1 "<< std::endl;
    std::vector<std::vector< float> > matrice;

    for ( i = 0; i < 3; i++)
    {
        std::vector< float>  matriceTmp;
        for (int j = 0; j < 3; j++)
        {
            matriceTmp.push_back(0);
        }
        matrice.push_back(matriceTmp);
    }


    ///////////////////////////////////////////////////
    std::cout << "step 2 "<< std::endl;
    vtkIdType pts[36][3] = {
                        };

    for (float x = 0 ; x< matrice.size(); x++)
    {
        for(float y = 0; y < matrice.size();y++)
        {
            float coord[3] = {x,y, matrice[x][y]};
            m_points->InsertPoint(i,coord);
        }
    }
    for (i=0; i<36; i++) m_polys->InsertNextCell(3,pts[i]);
    for (i=0; i<36; i++) m_scalars->InsertTuple1(i,i);

    ////////////////////////////////////////////////////
    std::cout << "step 3 "<< std::endl;
    AfficherCarre::visualization();
    std::cout << "step 4 "<< std::endl;
}
*/

void AfficherCarre::visualization()
{
        // We now assign the pieces to the vtkPolyData.
    m_carre->SetPoints(m_points);
    m_carre->SetPolys(m_polys);
    m_carre->GetPointData()->SetScalars(m_scalars);

    // Now we'll look at it.
    vtkPolyDataMapper *carreMapper = vtkPolyDataMapper::New();
    carreMapper->SetInputData(m_carre);
    carreMapper->SetScalarRange(0,7);
    vtkActor *carreActor = vtkActor::New();
    carreActor->SetMapper(carreMapper);

    // The usual rendering stuff.
    vtkCamera *camera = vtkCamera::New();
    camera->SetPosition(1,1,1);
    camera->SetFocalPoint(0,0,0);

    vtkRenderer *renderer = vtkRenderer::New();
    vtkRenderWindow *renWin = vtkRenderWindow::New();
    renWin->AddRenderer(renderer);

    vtkRenderWindowInteractor *iren = vtkRenderWindowInteractor::New();
    iren->SetRenderWindow(renWin);

    renderer->AddActor(carreActor);
    renderer->SetActiveCamera(camera);
    renderer->ResetCamera();
    renderer->SetBackground(1,1,1);

    renWin->SetSize(300,300);

    // interact with data
    renWin->Render();
    iren->Start();

    // Clean up

    m_carre->Delete();
    carreMapper->Delete();
    carreActor->Delete();
    camera->Delete();
    renderer->Delete();
    renWin->Delete();
    iren->Delete();
}

void AfficherCarre::maillageTriangleFromVectorMatrice(std::vector<std::vector<float> > matrice )
{
    int i = 0;
    float matSize = (float) matrice.size();
    for (float x = 0; x <  matSize; x++)
    {
        for(float y = 0; y < matSize; y++)
        {

            if (y < matSize-1 && x < matSize -1)
            {
                vtkIdType pts[2][3] = {
                        {i, i+matSize+1, i+1},
                        {i, i+1+matSize, i+matSize}
                                    };
                for (int j = 0; j < 2; j++) m_polys->InsertNextCell(3,pts[j]);
            }
            float z = matrice[x][y];
            float coord[3]= {x, y, z};
            m_scalars->InsertTuple1(i,i);
            m_points->InsertPoint(i++,coord);
        }
    }

}



AfficherCarre::~AfficherCarre()
{
    //dtor
}
