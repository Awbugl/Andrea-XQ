#include "pch.h"
#include<cstdio>
#include<string>

#define DLL_EXPORT extern "C" _declspec(dllexport)
#pragma comment(lib,"ws2_32")
using namespace System;
using namespace System::IO;
using namespace AndreaBot::XQBridge;
using namespace System::Reflection;
using namespace System::Runtime::InteropServices;

DLL_EXPORT const char* CALLBACK XQ_Create(const char* frameworkVersion)
{	
	    return (char*)(void*)Marshal::StringToHGlobalAnsi(Main::XQ_Create(gcnew String(frameworkVersion)));
}

DLL_EXPORT int CALLBACK XQ_Event(const char* botQQ, int msgtype, int subType, const char* msgSource, const char* fromQQ, const char* toQQ, const char* msg,
	const char* msgSeq, const char* msgId, const char* rawMessage, const char* timestamp, int refuseReasonBuffer)
{
	String^ botqq = gcnew String(botQQ);
	String^ msgsource = gcnew String(msgSource);
	String^ fromqq = gcnew String(fromQQ);
	String^ toqq = gcnew String(toQQ);
	String^ msgseq = gcnew String(msgSeq);
	String^ msgid = gcnew String(msgId);
	String^ rawmsg = gcnew String(rawMessage);
	String^ timeStamp = gcnew String(timestamp);
	String^ Msg = gcnew String(msg);
	return Main::XQ_Event(botqq, msgtype, subType, msgsource, fromqq, toqq, Msg, msgseq, msgid, rawmsg, timeStamp, refuseReasonBuffer);
}

DLL_EXPORT int CALLBACK XQ_DestoryPlugin()
{
	return Main::XQ_DestroyPlugin();
}

DLL_EXPORT void CALLBACK XQ_AuthId(short id, int addr)
{
	Main::XQ_AuthId(id, addr);
}

#pragma unmanaged
BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
        case DLL_PROCESS_ATTACH:   
            return TRUE;
            
        case DLL_THREAD_ATTACH:
            return TRUE;
            
        case DLL_THREAD_DETACH:
            return TRUE;
            
        case DLL_PROCESS_DETACH:
            XQ_DestoryPlugin();
            return TRUE;
	}
}
