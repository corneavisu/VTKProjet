#include "ParserTopos.h"

ParserTopos::ParserTopos(std::string filename)
{
    nameFile = filename;
    readToposFile();

}

/***
*\fn void ParserTopos::readToposFile()
*\brief Parse a topo file in a vector of matrice 101*101
*
* Parse the file with 2 delimiter : "::" (separate by typer of value), "," (separate by value in the list)
*/
void ParserTopos::readToposFile()
{
    std::vector<std::vector<std::string> > stringList; // first storage of the file with the delimiter "::"
    std::string name;
    float matriceValeur[SIZEMAX][SIZEMAX]; // matrice 101*101
    int index = 0; // index in the stringList

    UtilsFile::parserCSV(nameFile,&stringList ,delimiterFile );

     while(index < (int)stringList.size())
        if (stringList[index][0] == "Table Value")
        {
            name = stringList[index][1]; /// extract the name of the data
            if (ParserTopos::buildMatrice(&(++index), stringList, matriceValeur)) /// build the data matrice
                dataList.push_back(DataCornee(ParserString::trim(name), matriceValeur)); /// add the data object in a vector
        }
        else
            index++;
}


/***
*\fn void ParserTopos::buildMatrice(int* index, std::vector< std::vector<std::string> > stringList, float matriceValeur[101][101])
*\brief Parse a vector of string vector in a matrice 101*101
*\param index pointer : index from the stringList which inform where we are in this vector
*\param stringList : vector of string vector which stock all line of the file
*\param matriceValeur : float array 2D  : which will stock our data
* Parse the file with 2 delimiter : "," (separate by value in the list)
*/
bool ParserTopos::buildMatrice(int* index, std::vector< std::vector<std::string> > stringList, float matriceValeur[SIZEMAX][SIZEMAX])
{
    int xIndex = 0;
    while(*index < (int)stringList.size() && stringList[*index][0] == "X[mm], Value[mm]")
    {
        std::vector<std::string> vecteurTmp(ParserString::explode(stringList[*index][1], delimiterList));
        vecteurTmp.erase(vecteurTmp.begin());
        UtilsVector::vecteurStringTofloatArray(vecteurTmp, matriceValeur[xIndex]);
        xIndex++;
        (*index)++;
    }
    return xIndex == SIZEMAX;
}

/***
*\fn void ParserTopos::printAllDataName()
*\brief Print all name of data
*
*/
void ParserTopos::printAllDataName()
{
    for (int i = 0 ; i < (int)dataList.size() ; i++)
        std::cout << dataList[i].getName() << std::endl;
}


/***
*fn void ParserTopos::printDataByName(std::string name)
*\brief print of a data from the list
*\param name : string which inform the name of the data set
*/
void ParserTopos::printDataByName(std::string name)
{
    int i(0);
    while (i<(int)dataList.size() && dataList[i].getName() != name)
        i++;

    if (i<(int)dataList.size())
        dataList[i].dataToString();
    else
        std::cout << "Les données n'ont pas était trouvé. Essayer avec un autre nom";
}

/***
*\fn void ParserTopos::getAllDataName(std::vector<std::string> dest)
*\brief return in the vector the list of all of data name
*\param dest : vector of string to stock all the name
*/
void ParserTopos::getAllDataName(std::vector<std::string> dest)
{
    for (int i = 0 ; i < (int)dataList.size() ; i++)
        dest.push_back(dataList[i].getName());
}


/***
*\fn bool ParserTopos::getDataByName(std::string name, float dest[SIZEMAX][SIZEMAX])
*\brief copie of data in a aray float 2d from a name
*\param dest : array 2d pf float and with SIZEMAX as size
*/
bool ParserTopos::getDataByName(std::string name, float dest[SIZEMAX][SIZEMAX])
{
    int i(0);
    while (i<(int)dataList.size() && dataList[i].getName() != name)
        i++;

    if (i<(int)dataList.size())
    {
        dataList[i].getData(dest);
        return true;
    }
    else
        return false;
}

/***
*\fn bool ParserTopos::getDataByName(int index, float dest[SIZEMAX][SIZEMAX])
*\brief copie of data in a aray float 2d from a name
*\param dest : array 2d pf float and with SIZEMAX as size
*\param index : index of the dataList where we extract datas
*\return bool : true if the number in parametre is inferior at the number of data
*/
bool ParserTopos::getDataByName(int number, float dest[SIZEMAX][SIZEMAX])
{

    if (index<(int)dataList.size())
    {
        dataList[number].getData(dest);
        return true;
    }
    else
        return false;
}



ParserTopos::~ParserTopos()
{
    //dtor
}
