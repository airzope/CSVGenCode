﻿/********************************************************************
**       This head file is generated by program,                   **
**            Please do not change it directly.                    **
********************************************************************/

#ifndef CODE_GEN_BY_CSV_CONFIG_HEAD_H_H_HHHH
#define CODE_GEN_BY_CSV_CONFIG_HEAD_H_H_HHHH

#include "ConfigStruct.h"
#include <map>
using namespace std;

class CCFG_SJScene {
    static const char* FILE_NAME;
public:
    CCFG_SJScene() { };
    ~CCFG_SJScene() { };

    int Load(const char* szDir);
    void Clear() { m_mapContent.clear(); }
    int Print() const;
    const SCFG_SJScene* Find( int  SceneLevel) const;
private:
    map<int, SCFG_SJScene> m_mapContent;
};
class CCFG_VipLevelTest {
    static const char* FILE_NAME;
public:
    CCFG_VipLevelTest() { };
    ~CCFG_VipLevelTest() { };

    int Load(const char* szDir);
    void Clear() { m_mapContent.clear(); }
    int Print() const;
    const SCFG_VipLevelTest* Find( int  id1) const;
private:
    map<int, SCFG_VipLevelTest> m_mapContent;
};
class CCFG_VipLevelTest2 {
    static const char* FILE_NAME;
public:
    CCFG_VipLevelTest2() { };
    ~CCFG_VipLevelTest2() { };

    int Load(const char* szDir);
    void Clear() { m_mapContent.clear(); }
    int Print() const;
    const SCFG_VipLevelTest2* Find( int  id1) const;
private:
    map<int, SCFG_VipLevelTest2> m_mapContent;
};


#endif /* CODE_GEN_BY_CSV_CONFIG_HEAD_H_H_HHHH */