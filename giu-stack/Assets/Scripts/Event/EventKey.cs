using System.Collections;
using System.Collections.Generic;

public class EventKey
{
    //====================================================================                                  
    //                          底层回调事件                    
    //====================================================================
    // 支付成功回调
    public const string PurchaseSuccessCallBack = "PurchaseSuccessCallBack";
    // 支付失败回调
    public const string PurchaseFailedCallBack = "PurchaseFailedCallBack";
    // 登录成功回调
    public const string LoginSuccessCallBack = "LoginSuccessCallBack";
    // 取消登录回调
    public const string LoginCancelCallBack = "LoginCancelCallBack";
    // 广告加载成功
    public const string AdLoadSuccessCallBack = "AdLoadSuccessCallBack";
    // 广告加载失败
    public const string AdLoadFailedCallBack = "AdLoadFailedCallBack";
    // 广告显示成功
    public const string AdShowSuccessCallBack = "AdShowSuccessCallBack";
    // 广告显示失败
    public const string AdShowFailedCallBack = "AdShowFailedCallBack";
    
    // 不知道什么其他回调
    public const string onFunctionCallBack = "onFunctionCallBack";
    //  end
    //====================================================================



    //====================================================================
    //                          业务逻辑自定义事件
    //====================================================================
    // 新手不稳定提示
    public const string OnGreenHandStabilize = "OnGreenHandStabilize";
    // 角色得稳定位置发生改变
    public const string OnCharacterPosChange = "OnCharacterPosChange";
    // 金币发生改变
    public const string OnGoldChange = "OnGoldChange";
    // 使用角色改变
    public const string OnCharacterChange = "OnCharacterChange";
    // 游戏结束
    public const string OnGameOver = "OnGameOver";

}