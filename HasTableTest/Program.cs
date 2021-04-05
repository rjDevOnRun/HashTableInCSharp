using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasTableTest
{
    public class TopologySet
    {
        public string OID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public TopologySet(string oid, string name )
        {
            this.OID = oid; this.Name = name;
        }
    }

    public static class TopologyDataSet
    {
        private static Hashtable hashtable = new Hashtable();
        private static List<TopologySet> topologies = new List<TopologySet>();
        private static Dictionary<string, TopologySet> dicTopologies = new Dictionary<string, TopologySet>();

        public static TopologySet GetTopologyFromHashTable(string ID)
        {
            return (TopologySet)hashtable[ID];
        }

        public static TopologySet GetTopologyFromDictionary(string ID)
        {
            return dicTopologies[ID];
        }

        public static TopologySet GetTopologyFromList(string ID)
        {
            return topologies.Where(x => x.OID.Equals(ID)).FirstOrDefault();
        }

        public  static Hashtable GetHashtable()
        {
            return hashtable;
        }

        public static Dictionary<string, TopologySet> GetTopologySetDic()
        {
            return dicTopologies;
        }

        public static void CreateDataSet()
        {
            for (int i = 100000; i < 300000; i++)
            {
                TopologySet topology = new TopologySet($"T-{i}", $"Topo-{i}");
                hashtable.Add(topology.OID, topology);
                dicTopologies.Add(topology.OID, topology);
                topologies.Add(topology);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            TopologyDataSet.CreateDataSet();
            float startTime = 0; float endTime = 0; float deltaTime = 0;
            Random rand = new Random();
            
            int cycles = 10000; int cycle = 0;
            
            
            while(cycle < cycles)
            {
                // Check Times (List)
                int tsID = rand.Next(100000, 249999);
                startTime = stopwatch.ElapsedMilliseconds;
                var tsL = TopologyDataSet.GetTopologyFromList($"T-{tsID}");
                endTime = stopwatch.ElapsedMilliseconds;
                deltaTime = endTime - startTime;
                Console.WriteLine($"List time: {string.Format("{0:0.##}", deltaTime + " ms")}");


                // Check Times (HashTable)
                startTime = stopwatch.ElapsedMilliseconds;
                var tsH = (TopologySet)TopologyDataSet.GetTopologyFromHashTable($"T-{tsID}");
                endTime = stopwatch.ElapsedMilliseconds;
                deltaTime = endTime - startTime;
                Console.WriteLine($"HashTable time: {string.Format("{0:0.##}", deltaTime + " ms")}");

                // Check Times (dictionary)
                startTime = stopwatch.ElapsedMilliseconds;
                var tsD = TopologyDataSet.GetTopologyFromDictionary($"T-{tsID}");
                endTime = stopwatch.ElapsedMilliseconds;
                deltaTime = endTime - startTime;
                Console.WriteLine($"Dictionary time: {string.Format("{0:0.##}", deltaTime + " ms")}");

                cycle++;
            }

            //var ht = TopologyDataSet.GetTopologySetDic();
            
            //foreach (KeyValuePair<string, TopologySet> item in ht)
            //{
            //    Console.WriteLine($"{item.Key}");
            //}

            stopwatch.Stop();
            Console.ReadKey();
        }
    }
}
