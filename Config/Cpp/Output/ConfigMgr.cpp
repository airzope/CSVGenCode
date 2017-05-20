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

int CConfigMgr::LoadAll()
{
	int iRet = 0;
	iRet = m_stHeroQualityUpTest.Load(CFG_CSV_DIR);
	SO_CFG_RT_IF_NOT_ZERO(m_stHeroQualityUpTest.Load, -1);
	iRet = m_stVipLevelTest.Load(CFG_CSV_DIR);
	SO_CFG_RT_IF_NOT_ZERO(m_stVipLevelTest.Load, -1);

	return 0;
}


int CConfigMgr::Reload()
{
	//return LoadAll();
    return 0;
}

void CConfigMgr::PrintAll()
{
	m_stHeroQualityUpTest.Print();
	m_stVipLevelTest.Print();

}


