using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SMILEI.Core.Collections.Generic;
using UnityEngine;

public class DrawGraph : MonoBehaviour
{
    [Serializable]
    public struct DataPoint : IComparable<DataPoint>
    {
        public float TimeStamp;
        public float Value;

        public int CompareTo(DataPoint other)
        {
            return TimeStamp.CompareTo(other.TimeStamp);
        }
    }
    
    public LineRenderer Renderer;
    [Header("Use this in a Canvas with screen space camera.")]
    public bool UseTransformRect;
    public Rect GraphRect = new Rect(-5,-5,10,10);
    public float GraphWidthInSeconds;

    public int BufferSize = 100;

    private float _lastValue;
    private float _earliestShownValue = 0;
    private RectTransform _rectTransform;

    public CircularBuffer<DataPoint> DataPoints;
    //public List<DataPoint> DataPoints { get; set; } //TODO: Implement some kind of circular buffer / fixed size
    private List<Vector3> _drawPoints;
    private Vector3[] _drawArray;
    
    
    
    public void Awake()
    {
        DataPoints = new CircularBuffer<DataPoint>(BufferSize);
        _drawPoints = new List<Vector3>(BufferSize);
        _drawArray = new Vector3[BufferSize + 2];
        _rectTransform = transform as RectTransform;
        if (_rectTransform == null) UseTransformRect = false;
    }


    public void AddDataPoint(float value)
    {
        InsertInOrder(new DataPoint()
        {
            TimeStamp = Time.time,
            Value = value,
        });
    }

    public void AddDataPoint(float value, float time)
    {
        InsertInOrder(new DataPoint()
        {
            TimeStamp = time,
            Value = value,
        });
    }

    void InsertInOrder(DataPoint newPoint)
    {
        /*
        var index = DataPoints.BinarySearch(newPoint);
        if (index < 0)
        {
            DataPoints.Insert(~index, newPoint);
        }*/
        DataPoints.Enqueue(newPoint);
    }
    

    void Update()
    {
        var now = Time.time;
        var earliestTInGraph = now - GraphWidthInSeconds;
        _drawPoints.Clear();
        _drawPoints.Add(FloatsToGraphVector(0, _earliestShownValue));
        bool firstvalueFound = false;
        int dequeueCount = 0;
        for (int i = 0; i < DataPoints.Count; i++)
        {
            var point = DataPoints[i];
            if (point.TimeStamp < earliestTInGraph)
            {
                dequeueCount++; // Out of range
                continue;
            }

            if (!firstvalueFound && i - 1 >= 0)
            {
                firstvalueFound = true;
                var prevPoint = DataPoints[i - 1];
                var normNow = Mathf.InverseLerp(prevPoint.TimeStamp, point.TimeStamp, now);
                _earliestShownValue = Mathf.Lerp(prevPoint.Value, point.Value, normNow);
                _drawPoints[0] = FloatsToGraphVector(0, _earliestShownValue);
            }
            
            var normTime = (point.TimeStamp - earliestTInGraph) / (now - earliestTInGraph);
            var normValue = point.Value;
            _drawPoints.Add(FloatsToGraphVector(normTime, normValue));

            if (i + 1 == DataPoints.Count) _lastValue = point.Value;
        }

        for (int i = 0; i < dequeueCount; i++)
        {
            DataPoints.Dequeue();
        }
        
        var currentTimePoint = FloatsToGraphVector(1, _lastValue);
        _drawPoints.Add(currentTimePoint);
        // Copy to a cached array to prevent gc.
        _drawPoints.CopyTo(_drawArray); 
        Renderer.positionCount = _drawPoints.Count;
        // Positions > positionCount are simply not used.
        Renderer.SetPositions(_drawArray); 
    }
    

    Vector3 FloatsToGraphVector(float normInputX, float normInputY)
    {
        var rect = UseTransformRect ? _rectTransform.rect : GraphRect;
        float x = Mathf.Lerp(rect.xMin, rect.xMax, normInputX);
        float y = Mathf.Lerp(rect.yMin, rect.yMax, normInputY);
        var point = new Vector3(x,y,0);

        if (UseTransformRect)
        {
            point = _rectTransform.TransformPoint(point);
        }

        return point;
    }

}

