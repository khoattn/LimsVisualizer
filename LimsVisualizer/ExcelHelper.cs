using System;
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
        private Excel.Range mRange;

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

                for (int i = 1; i < channelNames.Length; i++)
                {
                    channelNames[i] = document.MeasurementData.Channels[i - 1].Name + " [" + document.MeasurementData.Channels[i - 1].Unit.Name + "]";
                }

                mMeasurementDataWorksheet.Cells[1, 1] = "Device Name:";
                mMeasurementDataWorksheet.Cells[1, 2] = document.Summary.Device.Id;
                mMeasurementDataWorksheet.Cells[2, 1] = "Line Name:";
                mMeasurementDataWorksheet.Cells[2, 2] = document.Summary.Line.Name;
                mMeasurementDataWorksheet.Cells[3, 1] = "Product Type:";
                mMeasurementDataWorksheet.Cells[3, 2] = document.Summary.ActiveProduct.Product.ProductType.Name;
                mMeasurementDataWorksheet.Cells[4, 1] = "Product Name:";
                mMeasurementDataWorksheet.Cells[4, 2] = document.Summary.ActiveProduct.Product.Name;

                mMeasurementDataWorksheet.Range["A1", "B4"].Borders.Weight = Excel.XlBorderWeight.xlMedium;

                mRange = mMeasurementDataWorksheet.Range["A6", "A6"].Resize[Missing.Value, channelNames.Length];
                mRange.Value2 = channelNames;
                mRange.Font.Bold = true;
                mRange.EntireColumn.AutoFit();
                mRange.Borders.Weight = Excel.XlBorderWeight.xlMedium;
                mMeasurementDataWorksheet.Application.ActiveWindow.SplitRow = 6;
                mMeasurementDataWorksheet.Application.ActiveWindow.FreezePanes = true;
                MainForm.LogWriter.WriteDebugMessage("Added Header successfully.");
            }
            catch (Exception exception)
            {
                MainForm.LogWriter.WriteFailureMessage("Adding Header failed!");
                MainForm.LogWriter.WriteException(exception);
                MainForm.ShowErrorMessage(exception);
            }
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
                mRange = mMeasurementDataWorksheet.Range[cell];
                mRange.Value2 = document.MeasurementData.Timestamp.Local.ToShortDateString() + " " +
                                document.MeasurementData.Timestamp.Local.ToLongTimeString() + "." + document.MeasurementData.Timestamp.Local.Millisecond;

                //Add measuring values
                cell = "B" + (mMeasurementDataWorksheet.UsedRange.Rows.Count);
                mRange = mMeasurementDataWorksheet.Range[cell, cell].Resize[Missing.Value, measurementValues.Length];
                mRange.Value2 = measurementValues;

                //Format row
                cell = "A" + (mMeasurementDataWorksheet.UsedRange.Rows.Count);
                mRange = mMeasurementDataWorksheet.Range[cell, cell].Resize[Missing.Value, measurementValues.Length + 1];
                mRange.EntireColumn.AutoFit();
                mRange.Borders.Weight = Excel.XlBorderWeight.xlThin;

                //Display and focus new row
                mApplication.ScreenUpdating = true;

                //Splitview handling?
                mWorkbook.Windows.Item[1].Activate();
                //mMeasurementDataWorksheet.Activate();
                mRange.Activate();

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

                if (mRange != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mRange);

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
    }
}
