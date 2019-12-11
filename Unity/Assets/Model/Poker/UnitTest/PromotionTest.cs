//using System;
//using System.Collections;
//using System.Collections.Generic;
//using ETHotfix;
//using UnityEngine;

//public class PromotionTest : MonoBehaviour
//{
//    [SerializeField] public string BindingPlayerId = "806265";
//    // Start is called before the first frame update
//    void Start()
//    {
//        Debug.Log("绑定测试脚本: 按B键绑定");
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        #region test
//        //        if (Input.GetKeyDown(KeyCode.Z))
//        //        {
//        //            currentPage++;
//        //            Log.Debug($"page:{currentPage}/{pageCount}");
//        //        }
//        //
//        //        if (Input.GetKeyDown(KeyCode.X))
//        //        {
//        //            currentPage--;
//        //            Log.Debug($"page:{currentPage}/{pageCount}");
//        //        }
//        //
//        //        if (Input.GetKeyDown(KeyCode.N))
//        //        {
//        //            memberIndex++;
//        //            Log.Debug($"memberIndex:{memberIndex}/{pageCount}");
//        //        }
//        //        if (Input.GetKeyDown(KeyCode.B))
//        //        {
//        //            memberIndex--;
//        //            Log.Debug($"memberIndex:{memberIndex}/{pageCount}");
//        //        }
//        //
//        //        if (Input.GetKeyDown(KeyCode.Q))
//        //        {
//        //            GetMyPromotion();
//        //        }
//        //        if (Input.GetKeyDown(KeyCode.P))
//        //        {
//        //            GetPlayerPromotion();
//        //        } 
//        #endregion

//        if (Input.GetKeyDown(KeyCode.B))
//            BindingPromotion(BindingPlayerId);
//    }

//    private async void BindingPromotion(string a)
//    {
//        Debug.Log("收到推广绑定回调: " + a);
//        try
//        {
//            var bindingId = -1;
//            if (!string.IsNullOrEmpty(a))
//            {
//                var s = int.TryParse(a, out bindingId);
//                if (!s) bindingId = -1;
//            }
//            var response = (G2C_Promotion_Res)await SessionComponent.Instance.Session.Call(
//                new C2G_Promotion_Req()
//                {
//                    BindingId = bindingId,
//                    UserId = GamePrefs.GetUserId()
//                });

//            if (response.Error != 0)
//            {
//                Debug.LogWarning($"{response.Message}");
//            }
//        }
//        catch (Exception e)
//        {
//            Debug.LogError(e.Message);
//        }
//    }

//    private List<int> memberList = new List<int>();
//    private int currentPage = 1;
//    private int memberIndex = 0;

//    private int pageCount = 0;
//    private async void GetMyPromotion()
//    {
//        try
//        {
//            var response = (G2C_GetPromotion_Res)await SessionComponent.Instance.Session.Call(
//                new C2G_GetPromotion_Req()
//                {
//                    UserId = GamePrefs.GetUserId(),
//                    PageNumber = currentPage,
//                    MemberCount = 4
//                });

//            if (response.Error != 0)
//            {
//                Debug.LogWarning($"{response.Message}");
//                return;
//            }

//            Debug.Log("获取成功");
//            memberList.Clear();
//            foreach (var info in response.MemberList)
//            {
//                memberList.Add(info.PlayerId);
//            }

//            currentPage = response.PageNumber;
//            pageCount = response.PageCount;
//        }
//        catch (Exception e)
//        {
//            Debug.LogError(e.Message);
//        }
//    }

//    private async void GetPlayerPromotion()
//    {
//        var response = (G2C_GetPlayerPromotion_Res)await SessionComponent.Instance.Session.Call(
//            new C2G_GetPlayerPromotion_Req()
//            {
//                PlayerId = memberList[memberIndex],
//                PageNumber = currentPage,
//                MemberCount = 4
//            });

//        if (response.Error != 0)
//        {
//            Debug.LogWarning($"{response.Message}");
//            return;
//        }

//        Debug.Log("获取成功");
//        currentPage = response.PageNumber;
//        pageCount = response.PageCount;
//    }
//}
