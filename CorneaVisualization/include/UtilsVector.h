#ifndef UTILSVECTOR_H
#define UTILSVECTOR_H
#include "iostream"
#include "ParserString.h"



class UtilsVector
{
    public:
        UtilsVector();
        virtual ~UtilsVector();

        ///\brief print all element from a vector of string
        static void stringVectorToString(std::vector <std::string>* vectorResult);
        ///\brief print all element from a vector of vector of string
        static void vectorOfStringVectorToString(std::vector<std::vector <std::string> >* vectorResult);

        ///\brief  return a float vector from a string vector
        static void vecteurStringTofloatVector(std::vector<std::string> vecteurString,std::vector<float>* vecteurFloat);
        static void vecteurStringTofloatArray(std::vector<std::string> vecteur, float*  arr, float valeurNull );
        static void vecteurofVecteurStringTofloatArray(std::vector<std::vector<std::string> > vecteur, float arr[][101], int valeurNull);

        ///\brief return an integer, number of a value in a matrice
        static int countValueInSquareVector(float value, std::vector<std::vector<float> > matrice);

    protected:
    private:
};

#endif // UTILSVECTOR_H
