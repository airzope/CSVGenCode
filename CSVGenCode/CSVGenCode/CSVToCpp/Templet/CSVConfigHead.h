/********************************************************************
**       This head file is generated by program,                   **
**            Please do not change it directly.                    **
********************************************************************/

#ifndef CSV_CONFIG_HEAD_H_H_HHHH
#define CSV_CONFIG_HEAD_H_H_HHHH
#include "CSVConfigStruct.h"
#include <map>
using namespace std;

#Begin_Replace_Tag_Class
class #ClsName {
    public:
    #ClsName() { };
    ~#ClsName() { };

    bool Load(const std::string & szFileName);
    void Clear() { m_mapVipLevelTestTemplate.clear(); }

    public:
    const #StructName* FindContent( #KeyTypeName  #KeyName) const;

    map<#KeyTypeName, #StructName> m_map#FileName;
};
#End_Replace_Tag_Class


#endif /* CSV_CONFIG_HEAD_H_H_HHHH */