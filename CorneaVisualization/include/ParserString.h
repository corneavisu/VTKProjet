#ifndef PARSERSTRING_H
#define PARSERSTRING_H
#include "iostream"
#include "vector"
#include "string"
#include <fstream>
#include "sstream"


class ParserString
{
    public:
        ParserString();
        virtual ~ParserString();
        static std::vector<std::string> explode( std::string delimiter, std::string str);
        static void vectorToString(std::vector <std::string>* vectorResult);
        static void vectorOfVectorToString(std::vector<std::vector <std::string> >* vectorResult);
        static void parserCSV(std::string namefile, std::vector<std::vector<std::string> >* result,std::string delimiter);
        static std::vector<std::string> explode( std::string str, char delimiter );
        static void vecteurStringTofloatArray(std::vector<std::string> vecteur, float[] arr );
        static double stringToFloat(std::string);
    protected:
    private:
};

#endif // PARSERSTRING_H
