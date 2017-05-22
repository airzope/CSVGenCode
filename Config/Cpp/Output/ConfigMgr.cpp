﻿/********************************************************************
**       This head file is generated by program,                   **
**            Please do not change it directly.                    **
********************************************************************/

#include "ConfigMgr.h"

#if _WIN32 || _WIN64
const char* CConfigMgr::CFG_CSV_DIR = "../../../Input";
#else
const char* CConfigMgr::CFG_CSV_DIR = "../cfg/csv";
#endif
char* CConfigMgr::ErrorFileName= "";
int CConfigMgr::LoadAll()
{
	int iRet = 0;
	ErrorFileName = "VipLevelTest";
	iRet = m_stVipLevelTest.Load(CFG_CSV_DIR);
	SO_CFG_RT_IF_NOT_ZERO(m_stVipLevelTest.Load, -1);
	ErrorFileName = "VipLevelTest2";
	iRet = m_stVipLevelTest2.Load(CFG_CSV_DIR);
	SO_CFG_RT_IF_NOT_ZERO(m_stVipLevelTest2.Load, -1);

	return 0;
}


int CConfigMgr::Reload()
{
	//return LoadAll();
    return 0;
}

void CConfigMgr::PrintAll()
{
	m_stVipLevelTest.Print();
	m_stVipLevelTest2.Print();

}

