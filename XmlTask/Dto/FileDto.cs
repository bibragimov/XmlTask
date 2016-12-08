using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlTask.Dto
{
    [Serializable]
    [XmlRoot("Files")]
    public class FilesDto
    {
        [XmlElement("File")]
        public List<FileDto> Files { get; set; }
    }

    public class FileDto
    {
        [XmlAttribute("FileVersion")]
        public string FileVersion { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("DateTime")]
        public DateTime DateTime { get; set; }
    }
}