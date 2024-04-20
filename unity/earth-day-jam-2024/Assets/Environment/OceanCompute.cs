using System;
using Unity.Mathematics;
using UnityEngine;

namespace Environment
{
    public struct Voxel
    {
        public Vector3 Position;
    }

    public enum Method
    {
        CPU,
        GPU
    }

    public class OceanCompute : MonoBehaviour
    {
        private Voxel[] _voxelData;
        public ComputeShader computeShader;
        
        [SerializeField, Range(0, 10)] 
        public int skipFrames;
    
        private int _counter;
    
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

        [Range(-20, 20)]
        public float heightOffset;
    
        public Method method = Method.CPU;

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
        
        private void Start()
        {
            Reset();
        }

        private void OnValidate()
        {
            Reset();
        }

        void Reset()
        {
            int size = (int)Math.Pow(density, 2);
            _voxelData = new Voxel[size];
            _matrices = new Matrix4x4[_voxelData.Length];
            for (int x = 0; x < density; x++)
            {
                for (int y = 0; y < density; y++)
                {
                    Voxel voxel = new()
                    {
                        Position = new Vector3(x, 1, y)
                    };
                    int index = x * density + y;
                    _voxelData[index] = voxel;
                    _matrices[index] = Matrix4x4.Scale(new Vector3(scale, scale, scale));
                }
            }
        }
    
        public void CalcWavesGPU()
        {
            const int vectorSize = sizeof(float) * 3;

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
        
            float[] transformPosition = {transform.position.x, transform.position.y, transform.position.z};
            computeShader.SetFloats(TransformPosition, transformPosition);
        
            computeShader.Dispatch(0, density, density,1);
            
            voxelBuffer.GetData(_voxelData);
        
        
            for (int x = 0; x < density; x++)
            {
                for (int y = 0; y < density; y++)
                {
                    int index = x * density + y;
                    Vector3 position = _voxelData[index].Position;
                    Vector4 result = new Vector4(position.x, position.y + heightOffset, position.z, 1);
                    _matrices[index].SetColumn(3, result);
                }
            }
            voxelBuffer.Dispose();
        }

        public void CalcWavesCPU()
        {
            for (int x = 0; x < density; x++)
            {
                for (int y = 0; y < density; y++)
                {
                    int index = x * density + y;
                    float2 transformOffset = new((float)x / density + transform.position.x / 3.75f * Time.fixedDeltaTime,
                                                 (float)y / density + transform.position.z / 3.75f * Time.fixedDeltaTime);
                    float noise = Unity.Mathematics.noise.snoise(transformOffset * scale + Time.fixedTime * Time.fixedDeltaTime * speed);
                    Vector3 position = _voxelData[index].Position;
                    position.y = noise * amplitude;
                    position.x = (x - density / 2f) * ((float)gridSize/density);
                    position.z = (y - density / 10f) * ((float)gridSize/density);
                    Vector4 result = new(position.x, position.y + heightOffset, position.z, 1);
                    _matrices[index].SetColumn(3, result);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_counter >= skipFrames)
            {
                switch (method)
                {
                    case Method.CPU:
                        CalcWavesCPU();
                        break;
                    case Method.GPU:
                        CalcWavesGPU();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _counter = 0;
            }
            Graphics.DrawMeshInstanced(cubeMesh, 0, material, _matrices);
            _counter++;
        }
    }
}