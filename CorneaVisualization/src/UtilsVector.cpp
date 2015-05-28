#include "UtilsVector.h"

UtilsVector::UtilsVector()
{
    //ctor
}



/***
*\fn void ParserString::vectorToString(std::vector <std::string>* vectorResult)
*\brief Print a vector
*\param a vector of string
*\return nothing
*/
void UtilsVector::stringVectorToString(std::vector <std::string>* vectorResult)
{

    for (int i = 0 ; i < (int)vectorResult->size(); i++)
    {
        std::cout << (*vectorResult)[i] +" "     ;
    }
    std::cout << std::endl;

}


/***
*\fn void ParserString::vectorOfVectorToString(std::vector<std::vector <std::string> >* vectorResult)
*\brief Print a vector of vector
*\param a pointer from a vector of string vector
*\return nothing
*
*use    vectorToString
*/
void UtilsVector::vectorOfStringVectorToString(std::vector<std::vector <std::string> >* vectorResult)
{

    for (int i = 0 ; i < (int)vectorResult->size(); i++)
    {
        UtilsVector::stringVectorToString(&(*vectorResult)[i]);
    }

}


/***
*\fn void ParserString::vecteurStringTofloatArray(std::vector<std::string> vecteur, float[] arr)
*\brief add float in a array from a vector of string
*\param string vector and a float array
*\return nothing
*
*use stringToFloat
*/
void UtilsVector::vecteurStringTofloatArray(std::vector<std::string> vecteur, float arr[101])
{
    for(int i = 0; i< (int)vecteur.size(); i++)
    {

        if (vecteur[i] == "NULL")
            arr[i] = 0.0;
        else
        {
            arr[i]  = ParserString::stringToFloat(vecteur[i]);
        }

    }

}
/***
*\fn void ParserString::vecteurStringTofloatArray(std::vector<std::string> vecteur, float[] arr)
*\brief add float in a vector from a vector of string
*\param string vector and a float vector
*\return nothing
*
*use stringToFloat
*/
void UtilsVector::vecteurStringTofloatVector(std::vector<std::string> vecteurString,std::vector<float>* vecteurFloat){
    for(int i = 0; i< (int)vecteurString.size(); i++)
    {

        if (vecteurString[i] == "NULL")
            (*vecteurFloat).push_back(0.0) ;
        else
        {
            (*vecteurFloat).push_back(ParserString::stringToFloat(vecteurString[i]));
        }
    }
}

/***
*\fn void ParserString::vecteurofVecteurStringTofloatArray(std::vector<std::vector<std::string> > vecteur, float[] arr)
*\brief add float in a array from a vector of string
*\param string vector and a float array
*\return nothing
*
*use stringToFloat
*/
void UtilsVector::vecteurofVecteurStringTofloatArray(std::vector<std::vector<std::string> > vecteur, float arr[101][101])
{
    for(int i = 0; i< (int)vecteur.size(); i++)
        UtilsVector::vecteurStringTofloatArray(vecteur[i], arr[i]);
}



UtilsVector::~UtilsVector()
{
    //dtor
}