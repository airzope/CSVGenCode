﻿ /********************************************************************
 **       This head file is generated by program,                   **
 **            Please do not change it directly.                    **
 ********************************************************************/

#ifndef CODE_GEN_BY_CSV_CCONFIG_MGR_H_
#define CODE_GEN_BY_CSV_CCONFIG_MGR_H_

#include "ConfigImpl.h"

class CConfigMgr
{
	static const char* CFG_CSV_DIR;

public:
	static CConfigMgr& Instance()
	{
		static CConfigMgr instance;
		return instance;
	}

	int LoadAll();
	int Reload();
	void PrintAll();

public:
    const CHeroQualityUpTest &GetHeroQualityUpTest() const { return m_stHeroQualityUpTest;}
    const CVipLevelTest &GetVipLevelTest() const { return m_stVipLevelTest;}

private:
    CHeroQualityUpTest m_stHeroQualityUpTest;
    CVipLevelTest m_stVipLevelTest;

private:
	CConfigMgr() {}
	~CConfigMgr() {}
};

#define SO_CFG_RT_IF_NOT_ZERO(func, ret)   \
    if(0 != iRet)   \
    {   \
        return ret; \
    }   \


#endif /* CODE_GEN_BY_CSV_CCONFIG_MGR_H_ */
