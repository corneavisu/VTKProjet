#include "test.h"

test::test()
{
    //ctor
}

bool test::testOpenFile(std::ifstream file)
{
    if (file)
        return true;
    else
        return false;
}

test::~test()
{
    //dtor
}
