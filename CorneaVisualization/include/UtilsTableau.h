#ifndef UTILSTABLEAU_H
#define UTILSTABLEAU_H
#include "iostream"


class UtilsTableau
{
    public:
        UtilsTableau();
        virtual ~UtilsTableau();
        static void printFloat2D(float** data, int taille);
        static void copieFloat2D(float** init, float** dest, int sizeX, int sizeY);
        static void copieFloat2D(float** init, float** dest, int taille);
    protected:
    private:
};

#endif // UTILSTABLEAU_H
