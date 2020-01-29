using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateSwordTrail : MonoBehaviour
{
	[SerializeField]
	private Transform startPosition, endPosition;

	private Mesh mesh;
	[SerializeField]
	private int saveMeshNum = 10;

	//　頂点リスト
	[SerializeField]
	private List<Vector3> verticesLists = new List<Vector3>();
	//　UVリスト
	[SerializeField]
	private List<Vector2> uvsLists = new List<Vector2>();
	//　剣元の位置リスト
	[SerializeField]
	private List<Vector3> startPoints = new List<Vector3>();
	//　剣先の位置リスト
	[SerializeField]
	private List<Vector3> endPoints = new List<Vector3>();
	//　三角形のリスト
	[SerializeField]
	private List<int> tempTriangles = new List<int>();

	void Start()
	{
		mesh = GetComponent<MeshFilter>().mesh;
	}

	void LateUpdate()
	{
		PositionSet();

		if (startPoints.Count >= saveMeshNum + 1)
		{
			CreateMesh();
		}
	}

	private void OnEnable()
	{ 
		for(var i = 0; i <= saveMeshNum; i++)
		{
			PositionSet();
		}
	}

	void PositionSet()
	{
		if (startPoints.Count >= saveMeshNum + 1)
		{
			startPoints.RemoveAt(0);
			endPoints.RemoveAt(0);
		}
		startPoints.Add(startPosition.position);
		endPoints.Add(endPosition.position);
	}

	void CreateMesh()
	{
		mesh.Clear();

		//　リストのクリア
		verticesLists.Clear();
		uvsLists.Clear();
		tempTriangles.Clear();

		for (int i = 0; i < saveMeshNum; i++)
		{
			verticesLists.AddRange(new Vector3[] {
				startPoints[i], endPoints[i], startPoints[i + 1],
				startPoints[i + 1], endPoints[i], endPoints[i + 1]
			});
		}

		float addParam = 0f;
		for (int i = 0; i < saveMeshNum; i++)
		{
			uvsLists.AddRange(new Vector2[]{
				new Vector2(addParam, 0f), new Vector2(addParam, 1f), new Vector2(addParam + 1f / saveMeshNum, 0f),
				new Vector2(addParam + 1f / saveMeshNum, 0f), new Vector2(addParam, 1f), new Vector2(addParam + 1f / saveMeshNum, 1f)
			});
			addParam += 1f / saveMeshNum;
		}

		for (int i = 0; i < verticesLists.Count; i++)
		{
			tempTriangles.Add(i);
		}

		mesh.vertices = verticesLists.ToArray();
		mesh.uv = uvsLists.ToArray();
		mesh.triangles = tempTriangles.ToArray();

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}
}