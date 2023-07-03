using Sirenix.OdinInspector;
using UnityEngine;

namespace Codebase.Generator
{
	public class TerrainGenerator : MonoBehaviour
	{
		[BoxGroup("CONFIGURATION"), SerializeField, Tooltip("Коэффициент скалистости")] public float _rockinessFactor = 0f;
		[BoxGroup("CONFIGURATION"), SerializeField, Tooltip("Коэффициент зернистости")] public int _grainFactor = 8;
		[BoxGroup("CONFIGURATION"), SerializeField, Tooltip("Делать ли равнины")] public bool _isFlat = false;
		[BoxGroup("CONFIGURATION"), SerializeField] public Material _material;
		[BoxGroup("CONFIGURATION"), SerializeField] public Terrain _targetTerrain;
		[BoxGroup("SIZE"), SerializeField] private int _width = 2048;
		[BoxGroup("SIZE"), SerializeField] private int _height = 2048;

		private float WH;
		private Color32[] cols;
		private Texture2D texture;

		[Button]
		private void Generate()
		{
			int resolution = _width;
			WH = (float)_width + _height;

			float[,] heights = new float[resolution, resolution];

			texture = new Texture2D(_width, _height);
			cols = new Color32[_width * _height];
			
			DrawPlasma(_width, _height);
			
			texture.SetPixels32(cols);
			texture.Apply();

			_material.SetTexture("_HeightTex", texture);

			for (int i = 0; i < resolution; i++)
			for (int k = 0; k < resolution; k++)
				heights[i, k] = texture.GetPixel(i, k).grayscale * _rockinessFactor;

			TerrainData data = _targetTerrain.terrainData;

			data.size = new Vector3(_width, _width, _height);
			data.heightmapResolution = resolution;
			data.SetHeights(0, 0, heights);
			
			transform.position = new Vector3(-_width / 2, 0, -_height / 2);
		}

		float Displace(float num)
		{
			float max = num / WH * _grainFactor;
			return Random.Range(-0.5f, 0.5f) * max;
		}

		void DrawPlasma(float w, float h)
		{
			float c1, c2, c3, c4;

			c1 = Random.value;
			c2 = Random.value;
			c3 = Random.value;
			c4 = Random.value;

			Divide(0.0f, 0.0f, w, h, c1, c2, c3, c4);
		}

		void Divide(float x, float y, float w, float h, float c1, float c2, float c3, float c4)
		{

			float newWidth = w * 0.5f;
			float newHeight = h * 0.5f;

			if (w < 1.0f && h < 1.0f)
			{
				float c = (c1 + c2 + c3 + c4) * 0.25f;
				cols[(int)x + (int)y * _width] = new Color(c, c, c);
			}
			else
			{
				float middle = (c1 + c2 + c3 + c4) * 0.25f + Displace(newWidth + newHeight);
				float edge1 = (c1 + c2) * 0.5f;
				float edge2 = (c2 + c3) * 0.5f;
				float edge3 = (c3 + c4) * 0.5f;
				float edge4 = (c4 + c1) * 0.5f;

				if (!_isFlat)
				{
					if (middle <= 0)
					{
						middle = 0;
					}
					else if (middle > 1.0f)
					{
						middle = 1.0f;
					}
				}

				Divide(x, y, newWidth, newHeight, c1, edge1, middle, edge4);
				Divide(x + newWidth, y, newWidth, newHeight, edge1, c2, edge2, middle);
				Divide(x + newWidth, y + newHeight, newWidth, newHeight, middle, edge2, c3, edge3);
				Divide(x, y + newHeight, newWidth, newHeight, edge4, middle, edge3, c4);
			}
		}
	}
}
