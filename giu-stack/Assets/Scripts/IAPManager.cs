using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eIAP
{
    noAds
}

public class IAPManager : MonoBehaviour
{

    private static IAPManager instance;

    public static IAPManager Instance {
        get
        {
            if (instance == null)
                Instantiate(Resources.Load<IAPManager>("IAPManager"));
            return instance;
        }
    }
    string key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAokn1qotPZOY9RICroQ4E3SDwk5zhE2u9B4dUaYh5sGHmuA/MqLuUs0mFqJM3Cw6tcI6lFQ5h/A+8PvB+k9Tqu8H3j19V5P4XOZkcRAFixLsiHvd1Lr3gG/kRgOW6ySED9IlkPVTJKSIBAama6spR4MFRYFFrne8zgfelkFCZEfDziYsS87Sglsm5gmY6uJYdpBaNQgPVNNmJ6Qvs950d8iaQoK4hpBqmYNm2DcgRvINoEAtAWOHMCRBvE8agP7apAM7m/lYy9SE2chDVRGkv8yaU7DTzNDLo5ebC+CGqTf92EPpmLTg6RLfXGZgb6aQ5WpfRlUKfxxZtGNrx5ZR8fQIDAQAB";
    string[] androidSkus = new string[] { "com.invictus.guistack.noads" };
    string[] iosProductIds = new string[] { "com.invictus.guistack.noads" };


    Dictionary<eIAP, string> IAP_IDS = new Dictionary<eIAP, string>()
    {
        {eIAP.noAds, "com.invictus.guistack.noads" }
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

        }
    }

}
