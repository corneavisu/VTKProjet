#include "ParserString.h"

ParserString::ParserString()
{
    //ctor
}

/***
*\fn std::vector<std::string> ParserString::explode(  std::string str, std::string delimiter)
*\brief Parse a string into vector
*\param a sentence and a delimiter
*\return vector of string
*/
std::vector<std::string> ParserString::explode(  std::string str, std::string delimiter)
{

    std::vector<std::string> result;
    int lastPos = 0;
    int pos = str.find(delimiter);
    while (pos != (int)std::string::npos) {
        result.push_back(str.substr(lastPos, pos));
        lastPos = pos+delimiter.size();

        pos = str.find(delimiter, lastPos);



    }
    result.push_back(str.substr(lastPos, str.length()-lastPos));
    return result;


}
/***
*\fn std::vector<std::string> ParserString::explode(  std::string str, char delimiter)
*\brief Parse a string into vector
*\param a sentence and a delimiter
*\return vector of string
*/
std::vector<std::string> ParserString::explode(  std::string str, char delimiter)
{
    std::vector<std::string> result;
    std::string token;
    std::stringstream iss;
    iss << str;
    while(std::getline(iss, token, delimiter))
    {
        result.push_back(token);
    }
    return result;
}

std::vector<float> ParserString::StringtoFloatVector(std::string str, char delimiter)
{
    std::vector<float> destArray;
    std::vector <std::string> strVect = ParserString::explode(str, delimiter);
    for (int i =0; i< (int) strVect.size(); i++)
        destArray.push_back(ParserString::stringToFloat(strVect[i])) ;
    return destArray;
}

/***
*\fn double ParserString::stringToFloat(std::string str)
*\brief string to float
*\param a string
*\return a double
*/
float ParserString::stringToFloat(std::string str)
{
    std::istringstream buffer(str);
    float temp;
    buffer >> temp;
    return temp;
}


/***
*\fn std::string ParserString::trim(std::string str)
*\brief erase all space in the start or end of a sentence
*\param string wiht maybe a space in the start or end
*\return string without space in the start or end
*/
std::string ParserString::trim(std::string str)
{
    std::string whitespace = " \t\f\v\n\r";
    int start = str.find_first_not_of(whitespace);
    int fin = str.find_last_not_of(whitespace);
    std::string newstr= str.substr(start, fin);
   return newstr;
}

ParserString::~ParserString()
{
    //dtor
}
