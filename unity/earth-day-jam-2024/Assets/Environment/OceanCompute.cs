using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Environment
{
    public struct Voxel
    {
        public Vector3 Position;
        public int IsVisible;
    }

    public class OceanCompute : MonoBehaviour
    {
        private Voxel[] _voxelData;
        public ComputeShader computeShader;
    
        public Mesh cubeMesh;
        
        public Material material;
    
        public int gridSize;
    
        [Range(1, 1000)] 
        public int density;
    
        [Range(0, 20)]
        public float amplitude = 1;

        [Range(0, 20)] 
        public float speed = 1;
    
        [Range(0, 10)] 
        public float scale = 1;
        
        [Space]
        public Transform boat;

        private static readonly int Voxels = Shader.PropertyToID("voxels");
    
        private static readonly int Speed = Shader.PropertyToID("speed");
        private static readonly int Amplitude = Shader.PropertyToID("amplitude");
        private static readonly int Density = Shader.PropertyToID("density");
        private static readonly int CenterOffset = Shader.PropertyToID("center_offset");
        private static readonly int TransformPosition = Shader.PropertyToID("transform_position");
        private static readonly int FixedDeltaTime = Shader.PropertyToID("fixed_delta_time");
        private static readonly int FixedTime = Shader.PropertyToID("fixed_time");
        private static readonly int PerlinScale = Shader.PropertyToID("perlin_scale");
        private static readonly int Size = Shader.PropertyToID("size");
        
        private Matrix4x4[] _matrices;
        private Matrix4x4[] _outMatrices;

        public Transform trash;
        private List<Transform> _props = new();
        private List<float> _propOffsets = new();
        
        private int _groups;

        private float _cubeSize;

        public Camera playerCamera;
        private static readonly int Planes = Shader.PropertyToID("planes");
        private float _xOffset;
        private float _yOffset;

        private float[] _flattenedFrustums = new float[24];

        public Transform cube;


        private void Start()
        {
            Reset();
            float startingHeight = transform.position.y;
            for (int i = 0; i < trash.childCount; i++)
            {
                Transform prop = trash.GetChild(i);
                _props.Add(prop);
                _propOffsets.Add(startingHeight - prop.transform.position.y);
            }
            _xOffset = (float)density / 2;
            _yOffset = (float)density / 10;
        }
        
        void Reset()
        {
            int size = (int)Math.Pow(density, 2);
            _voxelData = new Voxel[size];
            _matrices = new Matrix4x4[_voxelData.Length];
            _cubeSize = (float)gridSize / density;
            Vector3 cubeScale = new(_cubeSize, _cubeSize, _cubeSize);
            for (int x = 0; x < density; x++)
            {
                for (int y = 0; y < density; y++)
                {
                    Voxel voxel = new()
                    {
                        Position = new Vector3(x, 1, y)
                    };
                    int index = UVToIndex(x, y);
                    _voxelData[index] = voxel;
                    _matrices[index] = Matrix4x4.Scale(cubeScale);
                }
            }

            _groups = Mathf.CeilToInt(density / 8f);
        }
        
        float[] FlattenFrustumPlanes(float4[] frustumPlanes)
        {
            for (int i = 0; i < frustumPlanes.Length; i++)
            {
                _flattenedFrustums[i * 4] = frustumPlanes[i].x;
                _flattenedFrustums[i * 4 + 1] = frustumPlanes[i].y;
                _flattenedFrustums[i * 4 + 2] = frustumPlanes[i].z;
                _flattenedFrustums[i * 4 + 3] = frustumPlanes[i].w;
            }
            return _flattenedFrustums;
        }
        
        public void CalcWavesGPU()
        {
            const int vectorSize = sizeof(float) * 3 + sizeof(int);

            ComputeBuffer voxelBuffer = new(_voxelData.Length, vectorSize);
            voxelBuffer.SetData(_voxelData);
        
            computeShader.SetBuffer(0, Voxels, voxelBuffer);
            
            computeShader.SetInt(Density, density);
            computeShader.SetFloat(Amplitude, amplitude);
            computeShader.SetFloat(Speed, speed);
            computeShader.SetFloat(PerlinScale, scale);
        
            computeShader.SetFloat(FixedTime, Time.fixedTime);
            computeShader.SetFloat(FixedDeltaTime, Time.fixedDeltaTime);
            computeShader.SetFloat(Size, (float)gridSize/density);
            computeShader.SetFloat(CenterOffset, density / 2f);

            
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
            float4[] frustums = new float4[6];
            for (int i = 0; i < 6; i++)
            {
                frustums[i] = new float4(planes[i].normal, planes[i].distance);
            }
            computeShader.SetFloats(Planes, FlattenFrustumPlanes(frustums));
        
            float[] transformPosition = {transform.position.x, transform.position.y, transform.position.z};
            computeShader.SetFloats(TransformPosition, transformPosition);
            
            computeShader.Dispatch(0, _groups, _groups, 1);
            voxelBuffer.GetData(_voxelData);

            List<Matrix4x4> finalMatrices = new();
            
            for (int x = 0; x < density; x++)
            {
                for (int y = 0; y < density; y++)
                {
                    int index = UVToIndex(x, y);
                    if (_voxelData[index].IsVisible == 1)
                    {
                        Vector3 position = _voxelData[index].Position;
                        Vector4 result = new(position.x, position.y, position.z, 1);
                        _matrices[index].SetColumn(3, result);
                        finalMatrices.Add(_matrices[index]);
                    }
                }
            }
            voxelBuffer.Dispose();
            _outMatrices = finalMatrices.ToArray();
        }
        
        private int UVToIndex(int u, int v)
        {
            return u * density + v;
        }

        public float SampleHeight(Vector3 pos, float offset=0f)
        {
            int x = Mathf.RoundToInt(pos.x / _cubeSize + _xOffset);
            int y = Mathf.RoundToInt(pos.z / _cubeSize + _yOffset);
            if (Math.Abs(x) >= density || Math.Abs(y) >= density)
            {
                return pos.y;
            }
            return _voxelData[UVToIndex(x, y)].Position.y + offset + _cubeSize/2;
        }
        
        void MoveShip()
        {
            Vector3 boatPos = boat.position;
            boatPos.y = Mathf.Lerp(boatPos.y, SampleHeight(boatPos, 3), .5f);
            boat.position = boatPos;
        }

        void FloatProps()
        {
            int propIndex = 0;
            foreach (Transform prop in _props)
            {
                Vector3 pos = prop.position;
                pos.y = SampleHeight(pos, _propOffsets[propIndex]);
                prop.position = pos;

                propIndex++;
            }
        }
        
        // Update is called once per frame
        void Update()
        {
            Graphics.DrawMeshInstanced(cubeMesh, 0, material, _outMatrices);
            
            Vector3 pos = cube.position;
            pos.y = SampleHeight(pos, 1);
            cube.position = pos;
        }

        private void FixedUpdate()
        {
            CalcWavesGPU();
            MoveShip();
            FloatProps();
        }
    }
}