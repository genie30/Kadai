﻿/****************************************************************************
 *
 * Copyright (c) 2012 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

#if !(!UNITY_EDITOR && UNITY_IOS && ENABLE_MONO)
#define CRIWAREERRORHANDLER_SUPPORT_NATIVE_CALLBACK
#endif

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// \addtogroup CRIWARE_UNITY_COMPONENT
/// @{

/*JP
 * \brief CRIWAREエラーオブジェクト
 * \par 説明:
 * CRIWAREライブラリの初期化を行うためのコンポーネントです。<br>
 */
[AddComponentMenu("CRIWARE/Error Handler")]
public class CriWareErrorHandler : MonoBehaviour{
	/*JP
	 * \brief コンソールデバッグ出力を有効にするかどうか
	 * \par 注意:
	 * Unityデバッグウィンドウだけでなく、コンソールデバッグ出力を有効にするかどうか [deprecated]
	 * PCの場合はデバッグウィンドウに出力されます。
	 */
	public bool enableDebugPrintOnTerminal = false;

	/*JP エラー発生時に強制的にクラッシュさせるかどうか(デバッグ用) */
	public bool enableForceCrashOnError = false;

	/*JP シーンチェンジ時にエラーハンドラを削除するかどうか */
	public bool dontDestroyOnLoad = true;

	/*JP エラーメッセージ */
	public static string errorMessage { get; set; }

	/* オブジェクト作成時の処理 */
	void Awake() {
		/* 初期化カウンタの更新 */
		initializationCount++;
		if (initializationCount != 1) {
			/* 多重初期化は許可しない */
			GameObject.Destroy(this);
			return;
		}
		
		/* エラー処理の初期化 */
		criWareUnity_Initialize();
		criWareUnity_SetForceCrashFlagOnError(enableForceCrashOnError);
		
		/* ターミナルへのログ出力表示切り替え */
		criWareUnity_ControlLogOutput(enableDebugPrintOnTerminal);

#if CRIWAREERRORHANDLER_SUPPORT_NATIVE_CALLBACK
		criWareUnity_SetErrorCallback(ErrorCallbackFromNative);
#endif

		/* シーンチェンジ後もオブジェクトを維持するかどうかの設定 */
		if (dontDestroyOnLoad) {
			DontDestroyOnLoad(transform.gameObject);
		}
	}
	
	/* Execution Order の設定を確実に有効にするために OnEnable をオーバーライド */
	void OnEnable() {
#if CRIWAREERRORHANDLER_SUPPORT_NATIVE_CALLBACK
		criWareUnity_SetErrorCallback(ErrorCallbackFromNative);
#endif
	}

	void OnDisable() {
#if CRIWAREERRORHANDLER_SUPPORT_NATIVE_CALLBACK
		criWareUnity_SetErrorCallback(null);
#endif
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_TVOS)
		if (enableDebugPrintOnTerminal == false){
			/* iOS/Androidの場合、エラーメッセージの出力先が重複してしまうため、*/
			/* ターミナル出力が無効になっている場合のみ、Unity出力を有効にする。*/
			OutputErrorMessage();
		}
#else
		OutputErrorMessage();
#endif
	}
	
	void OnDestroy() {
		/* 初期化カウンタの更新 */
		initializationCount--;
		if (initializationCount != 0) {
			return;
		}

#if CRIWAREERRORHANDLER_SUPPORT_NATIVE_CALLBACK
		criWareUnity_SetErrorCallback(null);
#endif
		
		/* エラー処理の終了処理 */
		criWareUnity_Finalize();
	}
	
	/* エラーメッセージのポーリングと出力 */
	private static void OutputErrorMessage() 
	{
		/* エラーメッセージのポーリング */
		System.IntPtr ptr = criWareUnity_GetFirstError();
		if (ptr == IntPtr.Zero) {
			return;
		}

		/* Unity上でログ出力 */
		string message = Marshal.PtrToStringAnsi(ptr);
		if (message != string.Empty) {
			OutputLog(message);
			criWareUnity_ResetError();
		}
		
		if (CriWareErrorHandler.errorMessage == null) {
			CriWareErrorHandler.errorMessage = message.Substring(0);
		}
	}

	/*JP ログの出力 */
	private static void OutputLog(string errmsg)
	{
		if (errmsg == null) {
			return;
		}
		
		if (errmsg.StartsWith("E")) {
			Debug.LogError("[CRIWARE] Error:" + errmsg);
		} else if (errmsg.StartsWith("W")) {
			Debug.LogWarning("[CRIWARE] Warning:" + errmsg);
		} else {
			Debug.Log("[CRIWARE]" + errmsg);
		}
	}

#if CRIWAREERRORHANDLER_SUPPORT_NATIVE_CALLBACK
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ErrorCallbackFunc(string errmsg);

	[AOT.MonoPInvokeCallback(typeof(ErrorCallbackFunc))]
	private static void ErrorCallbackFromNative(string errmsg)
	{
		OutputLog(errmsg);
	}
#endif

	/*JP 初期化カウンタ */
	private static int initializationCount = 0;

	#region DLL Import
	#if !CRIWARE_ENABLE_HEADLESS_MODE
	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criWareUnity_Initialize();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criWareUnity_Finalize();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern System.IntPtr criWareUnity_GetFirstError();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criWareUnity_ResetError();

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criWareUnity_ControlLogOutput(bool sw);

	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criWareUnity_SetForceCrashFlagOnError(bool sw);

#if CRIWAREERRORHANDLER_SUPPORT_NATIVE_CALLBACK
	[DllImport(CriWare.pluginName, CallingConvention = CriWare.pluginCallingConvention)]
	private static extern void criWareUnity_SetErrorCallback(ErrorCallbackFunc callback);
#endif
	#else
	private static void criWareUnity_Initialize() { }
	private static void criWareUnity_Finalize() { }
	private static System.IntPtr criWareUnity_GetFirstError() { return System.IntPtr.Zero; }
	private static void criWareUnity_ResetError() { }
	private static void criWareUnity_ControlLogOutput(bool sw) { }
	private static void criWareUnity_SetForceCrashFlagOnError(bool sw) { }
	private static void criWareUnity_SetErrorCallback(ErrorCallbackFunc callback) { }
	#endif
	#endregion
} // end of class

/// @}

/* --- end of file --- */
