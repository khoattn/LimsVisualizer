namespace LimsHelper
{
    public class LimsVisualizerSettings
    {
        public string FilePath { get; set; }
        public int DueTime { get; set; }
    }

    public class LimsSimulatorSettings
    {
        public string SampleFile { get; set; }
        public string DestinationPath { get; set; }
        public int DueTime { get; set; }
    }
}