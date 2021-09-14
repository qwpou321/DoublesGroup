using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Threading.Tasks;

namespace DoublesGroup
{
    public class ScheduleTxt
    {
        string m_fileName;
        public ScheduleTxt(string fileName)
        {
            m_fileName = fileName;
            bool doesExist = File.Exists(m_fileName);
            if (doesExist == false)
            {
                File.CreateText(m_fileName);
            }
        }

        public async Task WriteListToTextFile(List<string> schedule)
        {
            FileStream fileStream = new FileStream(m_fileName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.Flush();
            streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < schedule.Count; i++) await streamWriter.WriteLineAsync(schedule[i]);
            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();
        }

        public async Task<List<string>> ReadTextFileToList()
        {
            FileStream fileStream = new FileStream(m_fileName, FileMode.Open, FileAccess.Read);
            List<string> schedule = new List<string>();
            StreamReader streamReader = new StreamReader(fileStream);
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            string tmp = await streamReader.ReadLineAsync();
            while (tmp != null)
            {
                schedule.Add(tmp);
                tmp = await streamReader.ReadLineAsync();
            }
            streamReader.Close();
            fileStream.Close();
            return schedule;
        }
    }
}
