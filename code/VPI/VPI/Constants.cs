using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPI
{
    public enum Topics 
    {
        MicroRelief,
        DestroyInstrument,
        InstrumentGeometry,
        Radius,
        Equipment,
        Force,
        EksCentr,
        Frequency,
        Shpyndel
    }

    public class Constants
    {
        public Dictionary<Topics, List<string>> Names = new Dictionary<Topics, List<string>>();
        public List<string> InstrumentStali;
        public List<string> PidshipnikStali;
        public List<string> TverdiSplavy;
        public List<string> NatureAlmaz;
        public List<string> SynteticAlmaz;

        public const string instr = "інструментальні сталі";
        public const string pidshipnik = "підшипникові сталі";
        public const string splavy = "тверді сплави";
        public const string natureAlmazy = "природні та штучні алмази";
        public const string sinteticAlazy = "синтетичні алмази";

        public Constants() 
        {
            Names.Add(Topics.MicroRelief, new List<string>() { "ввігнута", "випукла" });
            Names.Add(Topics.DestroyInstrument, new List<string>() { instr, pidshipnik, splavy, natureAlmazy, sinteticAlazy });
            Names.Add(Topics.InstrumentGeometry, new List<string>() { "частина циліндра", "сфера", "конус", "круговий тор" });
            Names.Add(Topics.Radius, new List<string>() { "0,5", "1,0", "1,5", "2,0", "2,5", "3,0", "3,5", "4,0" });
            Names.Add(Topics.Equipment, new List<string>() { "токарний", "фрезерний", "шліфувальний", "полірувальний", "свердлильний" });
            Names.Add(Topics.Force, new List<string>() { "50-100", "150-200", "250-300", "350-400", "450-500", "550-600" });
            Names.Add(Topics.EksCentr, new List<string>() { "0,2", "0,3", "0,4", "0,5", "0,6", "0,7", "0,8", "0,9", "1,0" });
            Names.Add(Topics.Frequency, new List<string>() { "1000", "1100", "1200", "1300", "1400", "1500", "1600", "1700", "1800", "1900", "2000" });
            Names.Add(Topics.Shpyndel, new List<string>() { "25-125", "225-325", "425-525", "625-725", "825-925", "1025-1125", "1225-1325", "1425-1525", "1625-1725", "1825-1925", "2000"});

            InstrumentStali = new List<string>() { "У10", "У10А", "Р18", "Р6АМ5", "Р6М5К5", "Р9М4К8", "Р9К5" };
            PidshipnikStali = new List<string>() { "ШХ15", "ШХ15СГ", "ШХ20СГ", "18ХГТ", "20Х2Н4ВА" };
            TverdiSplavy = new List<string>() { "Т15К6", "ВК6М", "ВК6", "ВК8", "ВК10", "ВК15", "ВК20", "ВК10К", "ВК20К", "ВК10-КС", "ВК15-КС", "ВК20-КС" };
            NatureAlmaz = new List<string>() { "баланс АСБ", "баланс АСПВ", "карбонадо АСПК"};
            SynteticAlmaz = new List<string>() { "синтетичний корунд", "мінералокераміка" };
        }
    }
}
