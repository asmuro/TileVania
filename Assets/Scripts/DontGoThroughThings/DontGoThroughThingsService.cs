using UnityEngine;
using System.Collections;

public class DontGoThroughThingsService
{
	#region Constructors

	private DontGoThroughThingsService() { }

    #endregion

    #region Service

	public static IDontGoThroughThingsData GetDontGoThroughThingsService(DontGoThroughThingsData pData)
	{
		return (IDontGoThroughThingsData)new DontGoThroughThings(pData);
	}

    #endregion
}
