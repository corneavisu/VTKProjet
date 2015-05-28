#ifndef PARSERSTRING_H
#define PARSERSTRING_H
#include "iostream"
#include "vector"
#include "string"
#include <fstream>
#include "sstream"

/**
*\file ParserString.cpp
*\brief Parse objet
*\author Alexandre.N
*\date 22/05/15
*
*Objet to parse some string element (vector, string, ...)
*/
class ParserString
{
    public:
        ParserString();
        virtual ~ParserString();

        ///\brief Parse a string into vector
        static std::vector<std::string> explode( std::string delimiter, std::string str);
        static std::vector<std::string> explode( std::string str, char delimiter );

        static std::string trim(std::string str);

        ///\brief return a float from a string
        static float stringToFloat(std::string);

    protected:
    private:
};

#endif // PARSERSTRING_H
