using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlane))]
public class UpdateShaderCoordinatesByARPlane : MonoBehaviour
{
	private Quaternion _rotation;
	private Vector3 _localScale, _position;
	private ARPlane _arPlane;

	ARPlaneManager _arPlaneManager;

	private void Awake()
	{
		_arPlane = GetComponent<ARPlane>();
		_arPlaneManager = GetComponent<ARPlaneManager>();
		_arPlaneManager.planesChanged += OnPlanesChanged;
	}
	void Start()
	{
		_arPlane.boundaryChanged += CheckCoordinates;
	}

	void OnPlanesChanged(ARPlanesChangedEventArgs eventArgs)
	{
		if (eventArgs.removed.Count > 0)
		{
			Debug.Log("Planes Removed!!");
			foreach (ARPlane p in eventArgs.removed)
			{
				if (p.trackableId == _arPlane.trackableId)
				{
					ResetShaderValues(_arPlane);
				}
			}
		}
	}

	void CheckCoordinates(ARPlaneBoundaryChangedEventArgs plane)
	{
			
		_position = plane.plane.center;
		//_rotation = Quaternion.Inverse(plane.otation);
		//_localScale = new Vector3(plane.convexBoundary, 10, plane.convexBoundary[1]);

		UpdateShaderValues(_position, _localScale, _rotation);
	}

	void UpdateShaderValues(Vector3 position, Vector3 localScale, Quaternion rotation)
	{

		Shader.SetGlobalVector("_Origin", new Vector4(
			  position.x,
			  position.y,
			  position.z,
			  0f));
		Shader.SetGlobalVector("_BoxRotation", new Vector4(
			   rotation.eulerAngles.x,
			   rotation.eulerAngles.y,
			   rotation.eulerAngles.z,
			   0f));

		Shader.SetGlobalVector("_BoxSize", new Vector4(
			localScale.x,
			localScale.y,
			localScale.z,
			0f));
	}

	private void ResetShaderValues(ARPlane plane)
	{
		var vZero = new Vector3(0, 0, 0);
		var qZero = new Quaternion(0, 0, 0, 0);

		UpdateShaderValues(vZero, vZero, qZero);
	}

	private void OnDisable()
	{
		_arPlane.boundaryChanged -= CheckCoordinates;
	}

}
