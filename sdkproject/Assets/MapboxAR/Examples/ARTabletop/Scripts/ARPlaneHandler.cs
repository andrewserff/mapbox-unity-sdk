using UnityEngine;
using System;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
namespace UnityARInterface
{
	public class ARPlaneHandler : MonoBehaviour
	{
		public static Action resetARPlane;
		public static Action<ARPlane> returnARPlane;
		private TrackableId _planeId;
		private ARPlane _cachedARPlane;
		private bool _didCacheId = false;

		ARPlaneManager _arPlaneManager;

		void Awake()
		{
			_arPlaneManager = GetComponent<ARPlaneManager>();
			_arPlaneManager.planesChanged += OnPlanesChanged;
		}

		//In ARFoundation 1.5, they got rid of all the seperate events and now
		//just send one PlanesChanged event.
		//See here: https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@1.5/manual/migration-guide.html#events
		void OnPlanesChanged(ARPlanesChangedEventArgs eventArgs)
		{
			Debug.Log("Planes Changed!!");
			if (eventArgs.added.Count > 0 || eventArgs.updated.Count > 0)
			{
				foreach (ARPlane p in eventArgs.added)
				{
					UpdateARPlane(p);
				}
				foreach (ARPlane p in eventArgs.updated)
				{
					UpdateARPlane(p);
				}
			}
		}

		void UpdateARPlane(ARPlane arPlane)
		{

			if (_didCacheId == false)
			{
				_planeId = arPlane.trackableId;
				Debug.Log("Plane Id " + _planeId.ToString());
				_didCacheId = true;
			}

			if (arPlane.trackableId == _planeId)
			{
				_cachedARPlane = arPlane;
				Debug.Log("Cached Plane " + _planeId.ToString());
			}

			if (returnARPlane != null)
			{
				returnARPlane(_cachedARPlane);
			}
		}
	}
}
