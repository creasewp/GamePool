using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GamePool2Core.Entities
{
    public abstract class Settings : BaseEntity//, ISettings
    {
        private readonly object m_StreamWriterLock = new object();

        public Settings()
        {
            //load from file if exists.
            LoadFromFile();
        }

        private async Task LoadFromFile()
        {
            //UserName = "creasewp";
            lock (m_StreamWriterLock)
            {
                using (Stream m_Stream = CreateStream())
                {
                    using (StreamReader streamReader = new StreamReader(m_Stream, UTF8Encoding.UTF8))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            UserName = streamReader.ReadLine();
                        }
                    }
                }
            }

        }

        public async Task SaveToFile()
        {
            //TODO
            lock (m_StreamWriterLock)
            {
                using (Stream m_Stream = CreateStream())
                {
                    using (StreamWriter streamWriter = new StreamWriter(m_Stream, UTF8Encoding.UTF8, 512, true))
                    {
                        streamWriter.Write(UserName);
                        streamWriter.Write(Environment.NewLine);
                        streamWriter.Write(Password);
                        streamWriter.Flush();
                    }
                }
            }
        }

        private string m_UserName;
        public string UserName { get { return m_UserName; }set { m_UserName = value; RaisePropertyChanged(); } }
        public string Password { get; set; }
        public string PoolId { get; set; }

        protected abstract Stream CreateStream();
    }
}
