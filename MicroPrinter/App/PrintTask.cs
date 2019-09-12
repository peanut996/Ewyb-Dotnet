namespace MicroPrinter.App
{
    using System;

    internal class PrintTask
    {
        private string taskName;
        private DateTime beginTime;
        private DateTime endTime;
        private string fileName;
        private string filePages;
        private string finished;

        public DateTime BeginTime
        {
            get => 
                this.beginTime;
            set => 
                (this.beginTime = value);
        }

        public DateTime EndTime
        {
            get => 
                this.endTime;
            set => 
                (this.endTime = value);
        }

        public string FileName
        {
            get => 
                this.fileName;
            set => 
                (this.fileName = value);
        }

        public string FilePages
        {
            get => 
                this.filePages;
            set => 
                (this.filePages = value);
        }

        public string Finished
        {
            get => 
                this.finished;
            set => 
                (this.finished = value);
        }

        public string TaskName
        {
            get => 
                this.taskName;
            set => 
                (this.taskName = value);
        }
    }
}

