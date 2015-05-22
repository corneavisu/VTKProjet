#include "ParserString.h"

ParserString::ParserString()
{
    //ctor
}

std::vector<std::string> ParserString::explode( std::string delimiter, std::string str)
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




void ParserString::vectorToString(std::vector <std::string>* vectorResult)
{

    for (int i = 0 ; i < (int)vectorResult->size(); i++)
    {
        std::cout << (*vectorResult)[i] +" "     ;
    }
    std::cout << std::endl;

}

void ParserString::vectorOfVectorToString(std::vector<std::vector <std::string> >* vectorResult)
{

    for (int i = 0 ; i < (int)vectorResult->size(); i++)
    {
        ParserString::vectorToString(&(*vectorResult)[i])   ;
    }

}

void ParserString::parserCSV(std::string namefile,std::vector<std::vector<std::string> >* result,  std::string delimiter=","){

    std::ifstream file (namefile.c_str());
    std::string line;


    while(std::getline(file, line))
    {
        (*result).push_back(ParserString::explode(delimiter, line));

    }



}

void ParserString::vecteurStringTofloatArray(std::vector<std::string> vecteur, float[] arr)
{
    for(int i = 0, i< vecteur.size(), i++)
    {
        if (vecteur[i] == "NULL")
            (*arr[i]) = 0;
        else
        {
            (*arr)[i]  = ParserString::stringToFloat(vecteur[i]);
        }
    }
}

double ParserString::stringToFloat(std::string str)
{
    istringstream buffer(str);
    double temp;
    buffer >> temp;
    return temp;
}



ParserString::~ParserString()
{
    //dtor
}
