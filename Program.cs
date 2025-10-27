using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Reflection.Metadata.BlobBuilder;

namespace Redox_Code_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>();
            for (int i = 1; i < 100; i++)
            {
                list.Add(i);
            }

            printEven(list);
            printThreeOrFive(list);

            EventScheduler s = new EventScheduler();
            Event e1 = new Event("test1","Sydney",new DateTime(2000,1,1));
            Event e2 = new Event("test2", "Melbourne", new DateTime(1990,1,1));
            s.ScheduleEvent(e1);
            s.ScheduleEvent(e2);
            s.GetUpcomingEvents();

        }
        /// <summary>
        /// Exercise 1.1
        /// </summary>
        /// <param name="list"></param>
        static void printEven(List<int> list) 
        {
            IEnumerable<int> isEven = from item in list where item % 2 == 0 select item;
            String result = "";
            foreach (int item in isEven)
            {
                result+=item + ", ";
            }
            Console.WriteLine(result.Substring(0, result.Length - 2));

        }
        /// <summary>
        /// Exercise 1.2
        /// </summary>
        /// <param name="list"></param>
        static void printThreeOrFive(List<int> list)
        {
            String result = "";
            foreach (int item in list)
            {
                if (((item % 3 == 0) && (item % 5 != 0)) || ((item % 3 != 0) && (item % 5 == 0))) result += item + ", ";
            }
            Console.WriteLine(result.Substring(0, result.Length - 2));
        }
    }

    class Event 
    {
        public String name {get;set;}
        public String location {get;set;}
        public DateTime dateTime {get;set;}

        public Event(String n, String l, DateTime d) 
        {
            this.name = n;
            this.location = l;
            this.dateTime = d;
        }

    }

    class EventScheduler
    {
        List<Event> list;
        String filePath;
        const String DATE_FORMAT = "yyyy-MM-ddTHH:mm:ss";

        public EventScheduler ()
        {
            filePath = "event.xml";
            this.list = new List<Event>();
            createXml();
            this.list = readFromXml();

        }

        public EventScheduler(String f)
        {
            filePath = f;
            this.list = new List<Event>();
            createXml();
            this.list = readFromXml();

        }

        public void ScheduleEvent(Event newEvent)
        {
            IEnumerable<Event> clashes = from e in list where e.dateTime == newEvent.dateTime select e;
            if (clashes.Any()) return;
            list.Add(newEvent);
            writeToXml(newEvent);
        }

        public void CancelEvent(Event e)
        {
            list.Remove(e);
            removeFromXml(e);
        }

        public List<Event> GetUpcomingEvents()
        {
            this.list = readFromXml();
            foreach (Event item in list)
            {
                Console.WriteLine(item.name + " " + item.location + " " + item.dateTime);
            }

            return this.list;
        }

        List<Event> readFromXml() 
        {
            XDocument document = XDocument.Load(this.filePath);
            List<Event> list = (from e in document.Descendants("Event")
                                select new Event(
                                    e.Element("name").Value,
                                    e.Element("location").Value,
                                    DateTime.Parse(e.Element("dateTime").Value) 
                                )).ToList();
            return list;
        }

        /// <summary>
        /// Default relative path is at path "..\Redox\Machamer\bin\Debug\net5.0\event.xml"
        /// </summary>
        void createXml() 
        {
            if (!System.IO.File.Exists(this.filePath))
            {
                XDocument document = new XDocument(
                    new XElement("List")
                );
                document.Save(this.filePath);
            }
        }

        void writeToXml(Event e) 
        {
            XDocument document = XDocument.Load(this.filePath);
            XElement eventElement = new XElement("Event",
                new XElement("name", e.name),
                new XElement("location", e.location),
                new XElement("dateTime", e.dateTime.ToString(DATE_FORMAT))
            );
            document.Element("List").Add(eventElement);
            document.Save(this.filePath);
        }

        void removeFromXml(Event e)
        {
            XDocument document = XDocument.Load(this.filePath);
            List<XElement> targets = (from t in document.Descendants("Event")
                              where (String)t.Element("name") == e.name
                              where (String)t.Element("location") == e.location
                              where (String)t.Element("dateTime") == e.dateTime.ToString(DATE_FORMAT)
                                      select t).ToList();
            foreach (XElement target in targets) 
            {
                target.Remove();
                document.Save(this.filePath);
            }
        }
    }
}
