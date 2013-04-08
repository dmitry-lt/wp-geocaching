using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Device.Location;


namespace GeocachingPlus.Model
{
    class KMeans
    {
        public List <GeoCoordinate> Caches;
        public List<GeoCoordinate> Clusters;
        public int NumCaches;
        public int NumClusters;
        public List<int> IndexCluster;
        
        double Dist(GeoCoordinate a, GeoCoordinate b)
        {  
            return Math.Sqrt((a.Latitude - b.Latitude) * (a.Latitude - b.Latitude) 
                           + (a.Longitude - b.Longitude) * (a.Longitude - b.Longitude));
        }

        GeoCoordinate Average(GeoCoordinate a, GeoCoordinate b)
        {
            return new GeoCoordinate((a.Latitude + b.Latitude) / 2, (a.Longitude + b.Longitude) / 2);
        }

        int CompareGeoCoordinate(GeoCoordinate a, GeoCoordinate b)
        {
            int res = 0;
            if (a.Latitude == b.Latitude)
            {
                if (a.Longitude < b.Longitude)
                    res = -1;
                else
                    res = 1;
            }
            else
            {
                if (a.Latitude < b.Latitude)
                    res = -1;
                else 
                    res = 1;
            }
            return res;
        }

        
        
        void InitCoordClusters()
        {
            Clusters = new List<GeoCoordinate>();
            Random rand = new Random();
            List <int> Used = new List<int>();
            for (int i = 0; i < NumCaches; i++)
                Used.Add(0);

            for (int i = 0; i < NumClusters; i++)
            {
                int a = rand.Next(NumCaches);
                while (Used[a] == 1)
                {
                    a++;
                    if (a == NumCaches)
                        a = 0;
                }
                Clusters.Add(Caches[a]);
                Used[a] = 1;
            }
            IndexCluster = new List<int>();
            for (int i = 0; i < NumCaches; i++)
            {
                IndexCluster.Add(0);
            }
     //       Caches.Sort(Less);
        }
        
        int RecountClusters()
        {
            List<GeoCoordinate> AverageClusters = new List<GeoCoordinate>();
            for (int i = 0; i < NumClusters; i++)
            {
                AverageClusters.Add(new GeoCoordinate(-1,-1));
            }
            int ok = 0;
            for (int i = 0; i < NumCaches; i++)
            {
                double MinDist = Dist(Caches[i], Clusters[IndexCluster[i]]);
                for (int j = 0; j < NumClusters; j++)
                {
                    double d = Dist(Caches[i], Clusters[j]);
                    if (d < MinDist)
                    {
                        MinDist = d;
                        IndexCluster[i] = j;
                        ok = 1;
                    }
                }
                int k = IndexCluster[i];
                if (AverageClusters[k].Latitude == -1 && AverageClusters[k].Longitude == -1)
                    AverageClusters[k] = Caches[i];
                else 
                    AverageClusters[k] = Average(AverageClusters[k], Caches[i]);
            }
            for (int i = 0; i < NumClusters; i++)
            {
                Clusters[i] = AverageClusters[i];
            }
            return ok;
        }

        
        public KMeans(List <GeoCoordinate> _Caches, int _NumClusters)
        {
            NumClusters = _NumClusters;
            NumCaches = _Caches.Count;
            if (NumClusters > NumCaches)
                NumClusters = NumCaches;
            Caches = new List<GeoCoordinate>();
            Caches = _Caches.GetRange(0, NumCaches);
            InitCoordClusters();
            while (RecountClusters() == 1) { }
        }

    }
}
