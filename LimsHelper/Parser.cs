using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;

namespace LimsHelper
{
    public class Parser
    {
        public Logger Logger { get; set; }

        public Document ParseFile(string path)
        {
            try
            {
                Logger.WriteDebugMessage(string.Format("Parsing file: '{0}'", path));

                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                return _ParseFile(fileStream, path);
            }
            catch (IOException)
            {
                try
                {
                    Logger.WriteFailureMessage("IOException was thrown while parsing file, maybe the file is locked by the Davis Server. Retrying in 100ms!");
                    Thread.Sleep(100);
                    var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                    return _ParseFile(fileStream, path);
                }
                catch (Exception exception)
                {
                    Logger.WriteFailureMessage("Parsing file failed!");
                    Logger.WriteException(exception);
                }
            }
            catch (Exception exception)
            {
                Logger.WriteFailureMessage("Parsing file failed!");
                Logger.WriteException(exception);
            }
            return null;
        }

        private Document _ParseFile(Stream fileStream, string path)
        {
            var document = new Document();
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(fileStream);

            document.File = path;

            foreach (XmlNode childNode in xmlDocument.ChildNodes[1].ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Timestamp":
                        document.Timestamp = _GetTimeStamp(childNode);
                        break;
                    case "Summary":
                        document.Summary = _GetSummary(childNode);
                        break;
                    case "MeasurementData":
                        document.MeasurementData = _GetMeasurementData(childNode);
                        break;
                    case "Comments":
                        document.Comments = _GetCommentList(childNode);
                        break;
                    case "Calibrations":
                        document.Calibrations = _GetCalibrationList(childNode);
                        break;
                    case "Adjustments":
                        document.Adjustments = _GetAdjustmentList(childNode);
                        break;
                }
            }

            fileStream.Close();
            Logger.WriteDebugMessage("Parsed file successfully");

            return document;
        }

        private ActiveProduct _GetActiveProduct(XmlNode xmlNode)
        {
            var activeProduct = new ActiveProduct();

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Product":
                        activeProduct.Product = _GetProduct(childNode);
                        break;

                    default:
                        Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                        break;
                }
            }

            return activeProduct;
        }

        private Adjustment _GetAdjustment(XmlNode xmlNode)
        {
            if (xmlNode.Attributes != null)
            {
                var adjustment = new Adjustment { Id = new Guid(xmlNode.Attributes["Id"].Value) };

                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Timestamp":
                            adjustment.Timestamp = _GetTimeStamp(childNode);
                            break;

                        case "Product":
                            adjustment.Product = _GetProduct(childNode);
                            break;

                        case "UserName":
                            adjustment.Username = childNode.InnerText;
                            break;

                        case "Channels":
                            adjustment.Channels = _GetChannelList(childNode);
                            break;

                        default:
                            Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                            break;
                    }
                }

                return adjustment;
            }

            return null;
        }

        private List<Adjustment> _GetAdjustmentList(XmlNode xmlNode)
        {
            return (from XmlNode childNode in xmlNode.ChildNodes select _GetAdjustment(childNode)).ToList();
        }

        private Calibration _GetCalibration(XmlNode xmlNode)
        {
            if (xmlNode.Attributes != null)
            {
                var calibration = new Calibration {Id = new Guid(xmlNode.Attributes["Id"].Value)};
            
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Timestamp":
                            calibration.Timestamp = _GetTimeStamp(childNode);
                            break;

                        case "Product":
                            calibration.Product = _GetProduct(childNode);
                            break;

                        case "UserName":
                            calibration.Username = childNode.InnerText;
                            break;

                        case "Channels":
                            calibration.Channels = _GetChannelList(childNode);
                            break;

                        default:
                            Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                            break;
                    }
                }

                return calibration;
            }

            return null;
        }

        private List<Calibration> _GetCalibrationList(XmlNode xmlNode)
        {
            return (from XmlNode childNode in xmlNode.ChildNodes select _GetCalibration(childNode)).ToList();
        }

        private Channel _GetChannel(XmlNode xmlNode)
        {
            if (xmlNode.Attributes != null)
            {
                var channel = new Channel
                {
                    MeasurementValue = new MeasurementValue(),
                    Id = xmlNode.Attributes["Id"].Value
                };


                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Name":
                            channel.Name = childNode.InnerText;
                            break;

                        case "Unit":
                            channel.Unit = _GetUnit(childNode);
                            break;

                        case "DataType":
                            channel.DataType = childNode.InnerText;
                            break;

                        case "MeasurementValue":
                            channel.MeasurementValue.Value = _ConvertStringToNullableDouble(childNode.InnerText);
                            
                            if (childNode.Attributes != null)
                            {
                                channel.MeasurementValue.Status = Convert.ToInt16(childNode.Attributes["Status"].Value);
                            }

                            break;

                        case "Limits":
                            channel.Limits = _GetLimits(childNode);
                            break;

                        case "SavedValue":
                            channel.SavedValue = _ConvertStringToNullableDouble(childNode.InnerText);
                            break;

                        case "Reference":
                            channel.Reference = _ConvertStringToNullableDouble(childNode.InnerText);
                            break;

                        case "Deviation":
                            channel.Deviation = _ConvertStringToNullableDouble(childNode.InnerText);
                            break;

                        case "AverageDeviation":
                            channel.AverageDeviation = _ConvertStringToNullableDouble(childNode.InnerText);
                            break;

                        case "OriginalGain":
                            channel.OriginalGain = _ConvertStringToNullableDouble(childNode.InnerText);
                            break;

                        case "AdjustedGain":
                            channel.AdjustedGain = _ConvertStringToNullableDouble(childNode.InnerText);
                            break;

                        default:
                            Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                            break;
                    }
                }

                return channel;
            }

            return null;
        }

        private List<Channel> _GetChannelList(XmlNode xmlNode)
        {
            return (from XmlNode childNode in xmlNode.ChildNodes select _GetChannel(childNode)).ToList();
        }

        private Comment _GetComment(XmlNode xmlNode)
        {
            var comment = new Comment();

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Timestamp":
                        comment.Timestamp = _GetTimeStamp(childNode);
                        break;

                    case "Product":
                        comment.Product = _GetProduct(childNode);
                        break;

                    case "Text":
                        comment.Text = childNode.InnerText;
                        break;

                    default:
                        Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                        break;
                }
            }

            return comment;
        }

        private List<Comment> _GetCommentList(XmlNode xmlNode)
        {
            return (from XmlNode childNode in xmlNode.ChildNodes select _GetComment(childNode)).ToList();
        }

        private Timestamp _GetTimeStamp(XmlNode xmlNode)
        {
            var timestamp = new Timestamp();

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Local":
                        timestamp.Local = _ConvertXmlTimestampToDateTime(childNode.InnerText);
                        break;

                    case "UTC":
                        timestamp.Utc = _ConvertXmlTimestampToDateTime(childNode.InnerText);
                        break;

                    default:
                        Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                        break;
                }
            }

            return timestamp;
        }

        private Device _GetDevice(XmlNode xmlNode)
        {
            if (xmlNode.Attributes != null)
            {
                var device = new Device {Id = xmlNode.Attributes["Id"].Value};

                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Name":
                            device.Name = childNode.InnerText;
                            break;

                        case "Location":
                            device.Location = childNode.InnerText;
                            break;

                        case "SerialNumber":
                            device.SerialNumber = Convert.ToInt32(childNode.InnerText);
                            break;

                        case "FirmwareVersion":
                            device.FirmwareVersion = childNode.InnerText;
                            break;

                        default:
                            Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                            break;
                    }
                }

                return device;
            }

            return null;
        }

        private Limit _GetLimits(XmlNode xmlNode)
        {
            var limit = new Limit {Active = xmlNode.Attributes != null && Convert.ToBoolean(xmlNode.Attributes["Active"].Value)};
            
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "LoLo":
                        limit.LoLo = _ConvertStringToNullableDouble(childNode.InnerText);
                        break;

                    case "Lo":
                        limit.Lo = _ConvertStringToNullableDouble(childNode.InnerText);
                        break;

                    case "Target":
                        limit.Target = _ConvertStringToNullableDouble(childNode.InnerText);
                        break;

                    case "Hi":
                        limit.Hi = _ConvertStringToNullableDouble(childNode.InnerText);
                        break;

                    case "HiHi":
                        limit.HiHi = _ConvertStringToNullableDouble(childNode.InnerText);
                        break;

                    default:
                        Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                        break;
                }
            }

            return limit;
        }

        private Line _GetLine(XmlNode xmlNode)
        {
            if (xmlNode.Attributes != null)
            {
                var line = new Line { Id = new Guid(xmlNode.Attributes["Id"].Value) };

                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Name":
                            line.Name = childNode.InnerText;
                            break;

                        default:
                            Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                            break;
                    }
                }

                return line;
            }

            return null;
        }

        private MeasurementData _GetMeasurementData(XmlNode xmlNode)
        {
            var measurementData = new MeasurementData();

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Timestamp":
                        measurementData.Timestamp = _GetTimeStamp(childNode);
                        break;

                    case "Product":
                        measurementData.Product = _GetProduct(childNode);
                        break;

                    case "SystemStatus":
                        measurementData.SystemStatus = Convert.ToInt16(childNode.InnerText);
                        break;

                    case "DeviceStatus":
                        measurementData.DeviceStatus = Convert.ToInt16(childNode.InnerText);
                        break;

                    case "LineStatus":
                        measurementData.LineStatus = Convert.ToInt16(childNode.InnerText);
                        break;

                    case "StatusMessages":
                        //TODO Check Messagestyle
                        measurementData.StatusMessages = new List<StatusMessage>();
                        break;

                    case "IsFlowStopActive":
                        measurementData.IsFlowStopActive = Convert.ToBoolean(childNode.InnerText);
                        break;

                    case "IsHoldActive":
                        measurementData.IsHoldActive = Convert.ToBoolean(childNode.InnerText);
                        break;

                    case "Channels":
                        measurementData.Channels = _GetChannelList(childNode);
                        break;

                    default:
                        Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                        break;
                }
            }

            return measurementData;
        }

        private Product _GetProduct(XmlNode xmlNode)
        {
            if (xmlNode.Attributes != null)
            {
                var product = new Product { Id = new Guid(xmlNode.Attributes["Id"].Value) };
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Name":
                            product.Name = childNode.InnerText;
                            break;

                        case "Number":
                            product.Number = Convert.ToUInt16(childNode.InnerText);
                            break;

                        case "ProductType":
                            product.ProductType = _GetProductType(childNode);
                            break;

                        default:
                            Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                            break;
                    }
                }

                return product;
            }

            return null;
        }

        private ProductType _GetProductType(XmlNode xmlNode)
        {
            if (xmlNode.Attributes != null)
            {
                var productType = new ProductType { Id = xmlNode.Attributes["Id"].Value };

                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Name":
                            productType.Name = childNode.InnerText;
                            break;

                        default:
                            Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                            break;
                    }
                }

                return productType;
            }

            return null;
        }

        private Summary _GetSummary(XmlNode xmlNode)
        {
            var summary = new Summary();

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "ActiveProduct":
                        summary.ActiveProduct = _GetActiveProduct(childNode);
                        break;

                    case "Device":
                        summary.Device = _GetDevice(childNode);
                        break;

                    case "Line":
                        summary.Line = _GetLine(childNode);
                        break;

                    default:
                        Logger.WriteDebugMessage(string.Format("Unknown XML Node found! Node Name: '{0}'", childNode.Name));
                        break;
                }
            }

            return summary;
        }

        private Unit _GetUnit(XmlNode xmlNode)
        {
            if (xmlNode.Attributes != null)
            {
                var unit = new Unit {Id = xmlNode.Attributes["Id"].Value};
            
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Name":
                            unit.Name = childNode.InnerText;
                            break;
                    }
                }

                return unit;
            }

            return null;
        }
        
        private double? _ConvertStringToNullableDouble(string value)
        {
            if (value == string.Empty)
            {
                return null;
            }

            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        private DateTime _ConvertXmlTimestampToDateTime(string innerText)
        {
            return DateTime.Parse(innerText.Remove(27), CultureInfo.InvariantCulture);
        }
    }
}
