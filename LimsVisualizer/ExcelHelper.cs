using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using LimsHelper;
using Excel = Microsoft.Office.Interop.Excel;

namespace LimsVisualizer
{
    class ExcelHelper
    {
        private Excel.Application mApplication;
        private Excel._Workbook mWorkbook;
        private Excel._Worksheet mMeasurementDataWorksheet;
        private Excel._Worksheet mMiscellaneousDataWorksheet;

        public void StartExcel()
        {
            try
            {
                MainForm.LogWriter.WriteDebugMessage("Starting Excel.");
                mApplication = new Excel.Application { Visible = true, UserControl = false };
                MainForm.LogWriter.WriteDebugMessage("Started Excel.");
            }
            catch (Exception exception)
            {
                MainForm.LogWriter.WriteFailureMessage("Starting Excel failed!");
                MainForm.LogWriter.WriteException(exception);
                MainForm.ShowErrorMessage(exception);
            }
        }

        public void AddHeaders(Document document)
        {
            try
            {
                MainForm.LogWriter.WriteDebugMessage(string.Format("Adding Header. Device: '{0}' Line: '{1}' Product: '{2}'",
                                                                        document.Summary.Device.Id,
                                                                        document.Summary.Line.Name,
                                                                        document.Summary.ActiveProduct.Product.Name));

                var channelNames = new string[document.MeasurementData.Channels.Count + 1];
                channelNames[0] = "Timestamp";

                for (var i = 1; i < channelNames.Length; i++)
                {
                    channelNames[i] = document.MeasurementData.Channels[i - 1].Name + " [" + document.MeasurementData.Channels[i - 1].Unit.Name + "]";
                }

                //Add general header for measurement data worksheet
                _WriteDeviceInfo(mMeasurementDataWorksheet, document);

                //Add general header for miscellaneous data worksheet
                _WriteDeviceInfo(mMiscellaneousDataWorksheet, document);

                var miscColumnHeadersRow1 = new[]
                {
                    "System Status",
                    "Device Status",
                    "Line Status",
                    "Timestamp",
                    "Comment",
                    "Calibration",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "Adjustment",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                };

                var miscColumnHeadersRow2 = new[]
                {
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "User Name",
                    "Channel",
                    "Saved Value",
                    "Reference Value",
                    "Deviation",
                    "User Name",
                    "Channel",
                    "Average Deviation",
                    "Original Gain",
                    "New Gain"
                };
                
                _WriteColumnHeaders(mMeasurementDataWorksheet, "A6", channelNames);
                _WriteColumnHeaders(mMiscellaneousDataWorksheet, "A6", miscColumnHeadersRow1);
                _WriteColumnHeaders(mMiscellaneousDataWorksheet, "A7", miscColumnHeadersRow2);

                _MergeCells(mMiscellaneousDataWorksheet, "A6", "A7");
                _MergeCells(mMiscellaneousDataWorksheet, "B6", "B7");
                _MergeCells(mMiscellaneousDataWorksheet, "C6", "C7");
                _MergeCells(mMiscellaneousDataWorksheet, "D6", "D7");
                _MergeCells(mMiscellaneousDataWorksheet, "E6", "E7");
                _MergeCells(mMiscellaneousDataWorksheet, "F6", "J6");
                _MergeCells(mMiscellaneousDataWorksheet, "K6", "O6");

                mMeasurementDataWorksheet.Application.Windows.Item[1].SplitRow = 6;
                mMeasurementDataWorksheet.Application.Windows.Item[1].FreezePanes = true;

                mMiscellaneousDataWorksheet.Application.Windows.Item[2].SplitRow = 7;
                mMiscellaneousDataWorksheet.Application.Windows.Item[2].FreezePanes = true;
                MainForm.LogWriter.WriteDebugMessage("Added Header successfully.");
            }
            catch (Exception exception)
            {
                MainForm.LogWriter.WriteFailureMessage("Adding Header failed!");
                MainForm.LogWriter.WriteException(exception);
                MainForm.ShowErrorMessage(exception);
            }
        }

        private static void _MergeCells(Excel._Worksheet worksheet, string startCell, string endCell)
        {
            var range = worksheet.Range[startCell, endCell];
            //range.Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //range.Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.MergeCells = true;
        }

        private static void _WriteDeviceInfo(Excel._Worksheet worksheet, Document document)
        {
            worksheet.Cells[1, 1] = "Device Name:";
            worksheet.Cells[1, 2] = document.Summary.Device.Id;
            worksheet.Cells[2, 1] = "Line Name:";
            worksheet.Cells[2, 2] = document.Summary.Line.Name;
            worksheet.Cells[3, 1] = "Product Type:";
            worksheet.Cells[3, 2] = document.Summary.ActiveProduct.Product.ProductType.Name;
            worksheet.Cells[4, 1] = "Product Name:";
            worksheet.Cells[4, 2] = document.Summary.ActiveProduct.Product.Name;

            worksheet.Range["A1", "B4"].Borders.Weight = Excel.XlBorderWeight.xlMedium;
        }

        private static void _WriteColumnHeaders(Excel._Worksheet worksheet, string startCell, ICollection<string> columnHeaders)
        {
            var range = worksheet.Range[startCell, startCell].Resize[Missing.Value, columnHeaders.Count];
            range.Value2 = columnHeaders;
            range.Font.Bold = true;
            range.EntireColumn.AutoFit();
            range.Borders.Weight = Excel.XlBorderWeight.xlMedium;
        }

        public void AddMeasurementValues(Document document)
        {
            try
            {
                MainForm.LogWriter.WriteDebugMessage(string.Format("Adding Measuring Data. Device: '{0}' Line: '{1}' Product: '{2}'",
                                                                        document.Summary.Device.Id,
                                                                        document.Summary.Line.Name,
                                                                        document.Summary.ActiveProduct.Product.Name));

                mApplication.ScreenUpdating = false;
                var measurementValues = new double[document.MeasurementData.Channels.Count];

                for (var i = 0; i < measurementValues.Length; i++)
                {
                    if (document.MeasurementData.Channels[i].MeasurementValue.Value != null)
                    {
                        measurementValues[i] = Convert.ToDouble(document.MeasurementData.Channels[i].MeasurementValue.Value);
                    }
                }

                //Add timestamp
                var cell = "A" + (mMeasurementDataWorksheet.UsedRange.Rows.Count + 1);

                //Splitview handling?
                mWorkbook.Windows.Item[1].Activate();
                //mMeasurementDataWorksheet.Activate();
                var range = mMeasurementDataWorksheet.Range[cell];
                range.Value2 = _GetFormatedTimeStamp(document.MeasurementData.Timestamp.Local);

                //Add measuring values
                cell = "B" + (mMeasurementDataWorksheet.UsedRange.Rows.Count);
                range = mMeasurementDataWorksheet.Range[cell, cell].Resize[Missing.Value, measurementValues.Length];
                range.Value2 = measurementValues;

                //Format row
                cell = "A" + (mMeasurementDataWorksheet.UsedRange.Rows.Count);
                range = mMeasurementDataWorksheet.Range[cell, cell].Resize[Missing.Value, measurementValues.Length + 1];
                range.EntireColumn.AutoFit();
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;

                //Display and focus new row
                mApplication.ScreenUpdating = true;

                //Splitview handling?
                mWorkbook.Windows.Item[1].Activate();
                //mMeasurementDataWorksheet.Activate();
                //range.Activate();

                MainForm.LogWriter.WriteDebugMessage("Added Measuring Data successfully.");
            }
            catch (Exception exception)
            {
                MainForm.LogWriter.WriteFailureMessage("Adding Measuring Data failed!");
                MainForm.LogWriter.WriteException(exception);
                MainForm.ShowErrorMessage(exception);
            }
        }

        public void Dispose()
        {
            try
            {
                MainForm.LogWriter.WriteDebugMessage("Disposing Excel.");

                if (mMeasurementDataWorksheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mMeasurementDataWorksheet);

                if (mWorkbook != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mWorkbook);

                if (mApplication != null)
                {
                    //Give the user control of Microsoft Excel's lifetime.
                    mApplication.UserControl = true;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mApplication);
                }

                MainForm.LogWriter.WriteDebugMessage("Disposed Excel successfully.");
            }
            catch (Exception exception)
            {
                MainForm.LogWriter.WriteFailureMessage("Disposing failed!");
                MainForm.LogWriter.WriteException(exception);
                MainForm.ShowErrorMessage(exception);
            }
        }

        public void CreateWorkbook()
        {
            var templateFile = Path.Combine(System.Windows.Forms.Application.StartupPath,
                @"Templates\TemplateWorkBook.xlsx");
            MainForm.LogWriter.WriteDebugMessage("Creating new workbook.");
            //mWorkbook = mApplication.Workbooks.Add(Missing.Value);
            mWorkbook = mApplication.Workbooks.Add(templateFile);
            
            mMeasurementDataWorksheet = (Excel.Worksheet)mWorkbook.Sheets.Item[1];
            mMiscellaneousDataWorksheet = (Excel.Worksheet)mWorkbook.Sheets.Item[2];
            //var sheet3 = (Excel.Worksheet)mWorkbook.Sheets.Item[3];
            
            //Rename first and second worksheet, delete third worksheet
            //mMeasurementDataWorksheet.Name = "Measurement Data";
            //mMiscellaneousDataWorksheet.Name = "Miscellaneous Data";
            //sheet3.Delete();

            //Set measurement data worksheet as active worksheet
            mMeasurementDataWorksheet.Activate();

            //Display windows in splitview
            mWorkbook.Windows.Arrange(Excel.XlArrangeStyle.xlArrangeStyleHorizontal);

            MainForm.HeadersWritten = false;
            MainForm.LogWriter.WriteDebugMessage("Created new workbook.");
        }

        public void AddMiscellaneousValues(Document document)
        {
            try
            {
                MainForm.LogWriter.WriteDebugMessage(string.Format("Adding Miscellaneous Data. Device: '{0}' Line: '{1}' Product: '{2}'",
                                                                        document.Summary.Device.Id,
                                                                        document.Summary.Line.Name,
                                                                        document.Summary.ActiveProduct.Product.Name));

                mApplication.ScreenUpdating = false;
                var values = new[]
                {
                    document.MeasurementData.SystemStatus.ToString(CultureInfo.InvariantCulture),
                    document.MeasurementData.DeviceStatus.ToString(CultureInfo.InvariantCulture),
                    document.MeasurementData.LineStatus.ToString(CultureInfo.InvariantCulture),
                    _GetFormatedTimeStamp(document.MeasurementData.Timestamp.Local),
                    document.Comments[0].Text,
                    document.Calibrations[0].Username,
                    document.Calibrations[0].Channels[0].Name,
                    document.Calibrations[0].Channels[0].SavedValue.ToString(),
                    document.Calibrations[0].Channels[0].Reference.ToString(),
                    document.Calibrations[0].Channels[0].Deviation.ToString(),
                    document.Adjustments[0].Username,
                    document.Adjustments[0].Channels[0].Name,
                    document.Adjustments[0].Channels[0].AverageDeviation.ToString(),
                    document.Adjustments[0].Channels[0].OriginalGain.ToString(),
                    document.Adjustments[0].Channels[0].AdjustedGain.ToString()
                };
                
                //Add timestamp
                var cell = "A" + (mMiscellaneousDataWorksheet.UsedRange.Rows.Count + 1);

                //Splitview handling?
                mWorkbook.Windows.Item[2].Activate();
                //mMeasurementDataWorksheet.Activate();

                //range.Value2 = document.MeasurementData.Timestamp.Local.ToShortDateString() + " " +
                //                document.MeasurementData.Timestamp.Local.ToLongTimeString() + "." + document.MeasurementData.Timestamp.Local.Millisecond;

                ////Add measuring values
                //cell = "B" + (mMiscellaneousDataWorksheet.UsedRange.Rows.Count);
                var range = mMiscellaneousDataWorksheet.Range[cell, cell].Resize[Missing.Value, values.Length];
                range.Value2 = values;

                //Format row
                cell = "A" + (mMiscellaneousDataWorksheet.UsedRange.Rows.Count);
                range = mMiscellaneousDataWorksheet.Range[cell, cell].Resize[Missing.Value, values.Length];
                range.EntireColumn.AutoFit();
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;

                //Display and focus new row
                mApplication.ScreenUpdating = true;

                //Splitview handling?
                mWorkbook.Windows.Item[2].Activate();
                //mMeasurementDataWorksheet.Activate();
                //range.Activate();

                MainForm.LogWriter.WriteDebugMessage("Added Miscellaneous Data successfully.");
            }
            catch (Exception exception)
            {
                MainForm.LogWriter.WriteFailureMessage("Adding Measuring Data failed!");
                MainForm.LogWriter.WriteException(exception);
                MainForm.ShowErrorMessage(exception);
            }
        }

        private static string _GetFormatedTimeStamp(DateTime dateTime)
        {
            return string.Format("{0} {1}.{2}", dateTime.ToShortDateString(), dateTime.ToLongTimeString(), dateTime.Millisecond);
        }
    }
}
