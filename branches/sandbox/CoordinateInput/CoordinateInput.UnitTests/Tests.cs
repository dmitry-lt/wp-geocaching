using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CoordinateInput.UnitTests
{
    [TestFixture]
    class Tests
    {
        static void Main(string[] args)
        {
            SecCoordinateViewModelTest();
            MinCoordinateViewModelTest();
        }

        [Test]
        public static void SecCoordinateViewModelTest()
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(0, CoordinateType.Lat);

            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    if (minCoordinateViewModel.SetMinutes(i.ToString()) && minCoordinateViewModel.SetMinutesFraction(j.ToString()))
                    {
                        double coordinate_1 = Math.Round(minCoordinateViewModel.ToCoordinate(), 6);
                        SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(coordinate_1, CoordinateType.Lat);
                        double coordinate_2 = Math.Round(secCoordinateViewModel.ToCoordinate(), 6);

                        Assert.AreEqual(coordinate_1, coordinate_2);
                    }
                }
            }
        }

        [Test]
        public static void MinCoordinateViewModelTest()
        {
            SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(0, CoordinateType.Lat);

            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    for (int k = 0; k < 1000; k++)
                    {
                        if (secCoordinateViewModel.SetMinutes(i.ToString()) && secCoordinateViewModel.SetSeconds(j.ToString()) && secCoordinateViewModel.SetSecondsFraction(k.ToString()))
                        {
                            double coordinate_1 = Math.Round(secCoordinateViewModel.ToCoordinate(), 6);
                            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(coordinate_1, CoordinateType.Lat);
                            double coordinate_2 = Math.Round(minCoordinateViewModel.ToCoordinate(), 6);

                            Assert.AreEqual(coordinate_1, coordinate_2);
                        }
                    }
                }
            }
        }
    }
}