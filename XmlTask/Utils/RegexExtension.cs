namespace XmlTask.Utils
{
    public class RegexExtension
    {
        public static readonly string FileNameRegex =
            @"([а-яА-Я] *?){1,100}_((\d *?){1,1}|(\d *?){10,10}|(\d *?){14,20})_([A-Za-zА-Яа-я0-9] *?){1,7}.xml";
    }
}