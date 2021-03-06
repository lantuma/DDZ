using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {


        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            UnityEngine_GameObject_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            UnityEngine_Animator_Binding.Register(app);
            UnityEngine_RuntimeAnimatorController_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_AnimationClip_Binding.Register(app);
            UnityEngine_AnimatorControllerParameter_Binding.Register(app);
            System_Collections_Generic_HashSet_1_String_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_String_Binding.Register(app);
            System_Exception_Binding.Register(app);
            UnityEngine_AnimationClip_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_String_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int64_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int64_ILTypeInstance_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int64_ILTypeInstance_Binding_KeyCollection_Binding_Enumerator_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            System_Linq_Enumerable_Binding.Register(app);
            System_Collections_Generic_List_1_KeyValuePair_2_Int64_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_List_1_KeyValuePair_2_Int64_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Int64_ILTypeInstance_Binding.Register(app);
            System_Int64_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Action_1_String_Binding.Register(app);
            ETModel_Game_Binding.Register(app);
            ETModel_Entity_Binding.Register(app);
            System_Collections_Generic_List_1_Sprite_Binding.Register(app);
            ETModel_SpriteHelper_Binding.Register(app);
            System_Collections_Generic_List_1_Single_Binding.Register(app);
            UnityEngine_UI_Image_Binding.Register(app);
            ETModel_ETVoid_Binding.Register(app);
            System_Action_1_String_Binding.Register(app);
            ETModel_AsyncETVoidMethodBuilder_Binding.Register(app);
            ETModel_TimerComponent_Binding.Register(app);
            ETModel_ETTask_Binding.Register(app);
            ETModel_ETTask_Binding_Awaiter_Binding.Register(app);
            System_Action_2_Int32_Sprite_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Action_Binding.Register(app);
            System_Action_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncVoidMethodBuilder_Binding.Register(app);
            ETModel_AssetBundleHelper_Binding.Register(app);
            ETModel_ResourcesComponent_Binding.Register(app);
            UnityEngine_Vector3_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            System_Threading_Tasks_Task_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            UnityEngine_UI_InputField_Binding.Register(app);
            UnityEngine_Events_UnityEvent_1_String_Binding.Register(app);
            UnityEngine_RectTransform_Binding.Register(app);
            UnityEngine_Vector2_Binding.Register(app);
            UnityEngine_Application_Binding.Register(app);
            UnityEngine_Mathf_Binding.Register(app);
            UnityEngine_TouchScreenKeyboard_Binding.Register(app);
            UnityEngine_Rect_Binding.Register(app);
            UnityEngine_AndroidJavaClass_Binding.Register(app);
            UnityEngine_AndroidJavaObject_Binding.Register(app);
            System_Array_Binding.Register(app);
            UnityEngine_Screen_Binding.Register(app);
            UnityEngine_UI_Button_Binding.Register(app);
            UnityEngine_Events_UnityEventBase_Binding.Register(app);
            ETModel_ActionHelper_Binding.Register(app);
            UnityEngine_EventSystems_EventTrigger_Binding_Entry_Binding.Register(app);
            UnityEngine_Events_UnityEvent_1_BaseEventData_Binding.Register(app);
            UnityEngine_EventSystems_EventTrigger_Binding.Register(app);
            System_Collections_Generic_List_1_UnityEngine_EventSystems_EventTrigger_Binding_Entry_Binding.Register(app);
            System_Collections_Generic_List_1_GameObject_Binding.Register(app);
            ReferenceCollector_Binding.Register(app);
            ETModel_SimpleObjectPoolComponent_Binding.Register(app);
            ETModel_SoundComponent_Binding.Register(app);
            System_Int32_Binding.Register(app);
            UnityEngine_UI_Text_Binding.Register(app);
            ETModel_ObjPool_Binding.Register(app);
            GameObjectExtension_Binding.Register(app);
            ETModel_StringHelper_Binding.Register(app);
            ETModel_NumberHelper_Binding.Register(app);
            UnityEngine_UI_Slider_Binding.Register(app);
            DG_Tweening_ShortcutExtensions_Binding.Register(app);
            DG_Tweening_TweenSettingsExtensions_Binding.Register(app);
            DG_Tweening_TweenExtensions_Binding.Register(app);
            UnityEngine_Events_UnityEvent_Binding.Register(app);
            ETModel_QRCodeComponent_Binding.Register(app);
            System_Collections_Generic_Queue_1_GameObject_Binding.Register(app);
            ETModel_CanvasConfig_Binding.Register(app);
            ETModel_GameObjectHelper_Binding.Register(app);
            System_Collections_Generic_Queue_1_String_Binding.Register(app);
            System_Threading_Tasks_TaskCompletionSource_1_String_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_Binding.Register(app);
            System_Threading_SemaphoreSlim_Binding.Register(app);
            System_Runtime_CompilerServices_ConfiguredTaskAwaitable_Binding.Register(app);
            System_Runtime_CompilerServices_ConfiguredTaskAwaitable_Binding_ConfiguredTaskAwaiter_Binding.Register(app);
            System_Threading_Tasks_Task_1_String_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_String_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_1_Int32_Binding.Register(app);
            UnityEngine_Time_Binding.Register(app);
            ETModel_GlobalConfigComponent_Binding.Register(app);
            ETModel_GlobalProto_Binding.Register(app);
            ETModel_NetworkComponent_Binding.Register(app);
            ETModel_ETTask_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            ETModel_ETTask_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding_Awaiter_Binding.Register(app);
            System_Threading_Tasks_Task_1_Int32_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_Int32_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Action_1_Transform_Binding.Register(app);
            System_Action_1_Transform_Binding.Register(app);
            DG_Tweening_DOTweenModuleUI_Binding.Register(app);
            DG_Tweening_DOTween_Binding.Register(app);
            System_Math_Binding.Register(app);
            ETModel_ETimerComponent_Binding.Register(app);
            ETModel_ETimer_Binding.Register(app);
            UnityEngine_LayerMask_Binding.Register(app);
            System_Threading_Interlocked_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding_ValueCollection_Binding_Enumerator_Binding.Register(app);
            ETModel_Hotfix_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding_Enumerator_Binding.Register(app);
            System_Type_Binding.Register(app);
            System_Reflection_MemberInfo_Binding.Register(app);
            ETModel_UIFactoryAttribute_Binding.Register(app);
            System_Activator_Binding.Register(app);
            System_Collections_Generic_List_1_String_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_1_Dictionary_2_Int32_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_1_List_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Text_RegularExpressions_Regex_Binding.Register(app);
            ETModel_Define_Binding.Register(app);
            UnityEngine_Texture_Binding.Register(app);
            UnityEngine_Sprite_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_List_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Collections_Generic_List_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Collections_Generic_List_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding_Enumerator_Binding.Register(app);
            UnityEngine_PlayerPrefs_Binding.Register(app);
            ETModel_SessionComponent_Binding.Register(app);
            System_Collections_Generic_List_1_Byte_Binding.Register(app);
            System_Random_Binding.Register(app);
            System_Byte_Binding.Register(app);
            System_Text_StringBuilder_Binding.Register(app);
            System_Guid_Binding.Register(app);
            System_BitConverter_Binding.Register(app);
            System_Threading_CancellationTokenSource_Binding.Register(app);
            UnityEngine_UI_Graphic_Binding.Register(app);
            ETModel_ETAsyncTaskMethodBuilder_1_Boolean_Binding.Register(app);
            ETModel_ETAsyncTaskMethodBuilder_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Single_Binding.Register(app);
            ETModel_ETTask_1_Boolean_Binding.Register(app);
            ETModel_ETTask_1_Boolean_Binding_Awaiter_Binding.Register(app);
            System_Console_Binding.Register(app);
            ETModel_ClientComponent_Binding.Register(app);
            ETModel_WebConfigResponse_Binding.Register(app);
            System_Char_Binding.Register(app);
            ETModel_AsyncImageDownloadComponent_Binding.Register(app);
            System_Threading_Tasks_Task_1_Dictionary_2_Int32_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_Dictionary_2_Int32_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Threading_Tasks_Task_1_List_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_List_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_1_Boolean_Binding.Register(app);
            System_Threading_Tasks_Task_1_Boolean_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_Boolean_Binding.Register(app);
            ETModel_BundleHelper_Binding.Register(app);
            ETModel_ETTask_1_VersionConfig_Binding.Register(app);
            ETModel_ETTask_1_VersionConfig_Binding_Awaiter_Binding.Register(app);
            ETModel_VersionConfig_Binding.Register(app);
            System_Collections_Generic_List_1_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_List_1_KeyValuePair_2_String_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            ETModel_ResGroupLoadComponent_Binding.Register(app);
            ETModel_ClickEffectComponent_Binding.Register(app);
            ETModel_CarouseComponent_Binding.Register(app);
            UnityEngine_GUIUtility_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_SubGame_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ResGroupLoadUIComponent_Binding.Register(app);
            ETModel_SubGameFactory_Binding.Register(app);
            ETModel_SubGame_Binding.Register(app);
            ETModel_ResGroupLoadUIComponent_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_SubGame_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Int32_SubGame_Binding.Register(app);
            ETModel_Component_Binding.Register(app);
            UnityEngine_UI_GridLayoutGroup_Binding.Register(app);
            UnityEngine_Color_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            UnityEngine_Quaternion_Binding.Register(app);
            UnityEngine_Random_Binding.Register(app);
            ETModel_OpenInstallComponent_Binding.Register(app);
            ETModel_HttpRequestHelper_Binding.Register(app);
            System_Threading_Tasks_Task_1_GetCustomerUrlResponse_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_GetCustomerUrlResponse_Binding.Register(app);
            ETModel_GetCustomerUrlResponse_Binding.Register(app);
            UnityEngine_Behaviour_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Binding.Register(app);
            Google_Protobuf_ByteString_Binding.Register(app);
            System_Collections_Hashtable_Binding.Register(app);
            System_Collections_IEnumerable_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_List_1_Int32_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Binding_Enumerator_Binding.Register(app);
            UnityEngine_EventSystems_PointerEventData_Binding.Register(app);
            UnityEngine_EventSystems_RaycastResult_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Byte_Int32_Binding.Register(app);
            System_Collections_Generic_IEnumerator_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_Int32_Binding.Register(app);
            UnityEngine_UI_Selectable_Binding.Register(app);
            UnityEngine_UI_HorizontalOrVerticalLayoutGroup_Binding.Register(app);
            System_Boolean_Binding.Register(app);
            ETModel_PbHelper_Binding.Register(app);
            System_Collections_Generic_List_1_Byte_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_GameObject_Binding.Register(app);
            ETModel_RandomHelper_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Boolean_Binding.Register(app);
            System_NotImplementedException_Binding.Register(app);
            ETModel_ByteHelper_Binding.Register(app);
            System_Reflection_PropertyInfo_Binding.Register(app);
            System_Reflection_MethodBase_Binding.Register(app);
            System_Collections_IDictionary_Binding.Register(app);
            LitJson_JsonMapper_Binding.Register(app);
            UnityEngine_Canvas_Binding.Register(app);
            UnityEngine_Camera_Binding.Register(app);
            UnityEngine_RectTransformUtility_Binding.Register(app);
            ETModel_Log_Binding.Register(app);
            ETModel_IdGenerater_Binding.Register(app);
            ETModel_MongoHelper_Binding.Register(app);
            ETModel_LayerNames_Binding.Register(app);
            ETModel_ComponentView_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_ILTypeInstance_Binding_ValueCollection_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_HashSet_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_HashSet_1_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_List_1_Object_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_List_1_ILTypeInstance_Binding.Register(app);
            ETModel_UnOrderMultiMap_2_Type_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Queue_1_Int64_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            ETModel_EventAttribute_Binding.Register(app);
            ETModel_EventProxy_Binding.Register(app);
            ETModel_EventSystem_Binding.Register(app);
            System_Collections_Generic_Queue_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Type_ILTypeInstance_Binding.Register(app);
            UnityEngine_Input_Binding.Register(app);
            ETModel_ConfigAttribute_Binding.Register(app);
            ETModel_AppTypeHelper_Binding.Register(app);
            UnityEngine_TextAsset_Binding.Register(app);
            ETModel_Actor_Test_Binding.Register(app);
            ETModel_SceneChangeComponent_Binding.Register(app);
            ETModel_C2G_EnterMap_Binding.Register(app);
            ETModel_Session_Binding.Register(app);
            ETModel_ETTask_1_IResponse_Binding.Register(app);
            ETModel_ETTask_1_IResponse_Binding_Awaiter_Binding.Register(app);
            ETModel_PlayerComponent_Binding.Register(app);
            ETModel_G2C_EnterMap_Binding.Register(app);
            ETModel_Player_Binding.Register(app);
            ETModel_M2C_CreateUnits_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_UnitInfo_Binding.Register(app);
            System_Collections_Generic_IEnumerator_1_UnitInfo_Binding.Register(app);
            ETModel_UnitInfo_Binding.Register(app);
            ETModel_UnitComponent_Binding.Register(app);
            ETModel_UnitFactory_Binding.Register(app);
            ETModel_Unit_Binding.Register(app);
            ETModel_M2C_PathfindingResult_Binding.Register(app);
            ETModel_AnimatorComponent_Binding.Register(app);
            ETModel_UnitPathComponent_Binding.Register(app);
            ETModel_GizmosDebug_Binding.Register(app);
            System_Collections_Generic_List_1_Vector3_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_Single_Binding.Register(app);
            UnityEngine_Physics_Binding.Register(app);
            UnityEngine_RaycastHit_Binding.Register(app);
            ETModel_Frame_ClickMap_Binding.Register(app);
            Google_Protobuf_ProtoPreconditions_Binding.Register(app);
            Google_Protobuf_CodedOutputStream_Binding.Register(app);
            Google_Protobuf_CodedInputStream_Binding.Register(app);
            Google_Protobuf_MessageParser_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_String_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_Int64_Binding.Register(app);
            Google_Protobuf_FieldCodec_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_UInt16_List_1_ILTypeInstance_Binding.Register(app);
            ETModel_OpcodeTypeComponent_Binding.Register(app);
            ETModel_MessageProxy_Binding.Register(app);
            ETModel_MessageDispatcherComponent_Binding.Register(app);
            ETModel_MessageInfo_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_Queue_1_Object_Binding.Register(app);
            System_Collections_Generic_Queue_1_Object_Binding.Register(app);
            ETModel_DoubleMap_2_UInt16_Type_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_UInt16_Object_Binding.Register(app);
            ETModel_MessageAttribute_Binding.Register(app);
            ETModel_SessionCallbackComponent_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Action_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Action_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            ETModel_IMessagePacker_Binding.Register(app);
            ETModel_OpcodeHelper_Binding.Register(app);
            ETModel_ETTaskCompletionSource_1_Google_Protobuf_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Threading_CancellationToken_Binding.Register(app);
            ETModel_ErrorCode_Binding.Register(app);
            ETModel_RpcException_Binding.Register(app);
            System_Action_1_Object_Binding.Register(app);
            System_Action_2_Object_Object_Binding.Register(app);
            ETModel_TimeHelper_Binding.Register(app);
            System_Action_1_Boolean_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}
