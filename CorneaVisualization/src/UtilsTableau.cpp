#include "UtilsTableau.h"

UtilsTableau::UtilsTableau()
{
    //ctor
}

void UtilsTableau::printFloat2D(float** data, int taille)
{
    for(int i = 0; i< taille; i++){
        for(int j = 0; j< taille; j++)
            std::cout << data[i][j];
        std::cout << std::endl;
    }
}

void UtilsTableau::copieFloat2D(float** init, float** dest, int sizeX, int sizeY)
{
    for(int i = 0; i < sizeX; i++){
        for (int j(0); j < sizeY; j++)
            dest[i][j] = init[i][j];
    }
}

void UtilsTableau::copieFloat2D(float** init, float** dest, int taille)
{
    for(int i = 0; i < taille; i++){
        for (int j(0); j < taille; j++)
            dest[i][j] = init[i][j];
    }
}



UtilsTableau::~UtilsTableau()
{
    //dtor
}
