using System;
using System.Collections.Generic;

namespace LimsVisualizer
{
    public class ActiveProduct
    {
        public Product Product;
    }

    public class Adjustment
    {
        public Guid Id { get; set; }
        public Timestamp Timestamp;
        public Product Product;
        public string Username;
        public List<Channel> Channels;
    }

    public class Calibration
    {
        public Guid Id { get; set; }
        public Timestamp Timestamp;
        public Product Product;
        public string Username;
        public List<Channel> Channels;
    }

    public class Channel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Unit Unit;
        public string DataType { get; set; }
        // For Measuringdata Channels
        public MeasuringValue MeasuringValue;
        public Limit Limits;
        // For Calibration Channels
        public double? SavedValue { get; set; }
        public double? Reference { get; set; }
        public double? Deviation { get; set; }
        // For Adjustment Channels
        public double? AverageDeviation { get; set; }
        public double? OriginalGain { get; set; }
        public double? AdjustedGain { get; set; }
    }

    public class Comment
    {
        public Timestamp Timestamp;
        public Product Product;
        public string Text { get; set; }
    }

    public class Device
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int SerialNumber { get; set; }
        public string FirmwareVersion { get; set; }
    }

    public class Document
    {
        public string File { get; set; }
        public Timestamp Timestamp;
        public Summary Summary;
        public MeasuringData MeasuringData;
        public List<Comment> Comments;
        public List<Calibration> Calibrations;
        public List<Adjustment> Adjustments;
    }

    public class Limit
    {
        public bool Active { get; set; }
        public double? LoLo { get; set; }
        public double? Lo { get; set; }
        public double? Target { get; set; }
        public double? Hi { get; set; }
        public double? HiHi { get; set; }
    }

    public class Line
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class MeasuringData
    {
        public Timestamp Timestamp;
        public Product Product;
        public int SystemStatus { get; set; }
        public int DeviceStatus { get; set; }
        public int LineStatus { get; set; }
        public List<StatusMessage> StatusMessages;
        public bool IsFlowStopActive { get; set; }
        public bool IsHoldActive { get; set; }
        public List<Channel> Channels;
    }

    public class MeasuringValue
    {
        public double? Value { get; set; }
        public int Status { get; set; }
    }

    public class Product
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public ProductType ProductType;
    }

    public class ProductType
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class StatusMessage
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Subsystem { get; set; }
    }

    public class Summary
    {
        public Device Device;
        public Line Line;
        public ActiveProduct ActiveProduct;
    }

    public class Timestamp
    {
        public DateTime Local { get; set; }
        public DateTime Utc { get; set; }
    }

    public class Unit
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
