/********************************************************************
**       This head file is generated by program,                   **
**            Please do not change it directly.                    **
********************************************************************/

#ifndef CODE_GEN_BY_CSV_CONFIG_HEAD_H_H_HHHH
#define CODE_GEN_BY_CSV_CONFIG_HEAD_H_H_HHHH

#include "ConfigStruct.h"
#include <map>
using namespace std;

#Begin_Replace_Tag_Class
class #ClsName {
    static const char* FILE_NAME;
public:
    #ClsName() { };
    ~#ClsName() { };

    int Load(const char* szDir);
    void Clear() { m_mapContent.clear(); }
    int Print() const;
    const #StructName* Find( #KeyTypeName  #KeyName) const;
private:
    map<#KeyTypeName, #StructName> m_mapContent;
};
#End_Replace_Tag_Class

#endif /* CODE_GEN_BY_CSV_CONFIG_HEAD_H_H_HHHH */