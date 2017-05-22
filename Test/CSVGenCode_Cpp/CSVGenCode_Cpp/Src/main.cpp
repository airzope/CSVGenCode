//just for window test
#if defined(_WIN32) || defined(_WIN64)

#include <iostream>
#include "CSVParser.h"
#include "ConfigMgr.h"
using namespace std;
int main(int argc, char **argv)
{
	try
	{
#if defined(_DEBUG)
		std::cout << "current version is debug" << endl;
#else
		std::cout << "current version is relese" << endl;
#endif
		auto ret = CConfigMgr::Instance().LoadAll();
		if (ret) {
			cout << CConfigMgr::ErrorFileName << endl;
			cout << "Load Config error" << endl;
		}
		else {
			CConfigMgr::Instance().PrintAll();
		}
	}
	catch (csv::Error &e)
	{
		std::cerr << e.what() << std::endl;
	}
	int i = 1;
	std::cin >> i;
	return 0;
}
#else

#endif