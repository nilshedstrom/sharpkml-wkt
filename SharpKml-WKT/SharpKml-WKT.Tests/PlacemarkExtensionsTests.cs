using FluentAssertions;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml_WKT.Base;
using SharpKml_WKT.Dom;
using SharpKml_WKT.Dom.Geometries;
using Xunit;

namespace SharpKml_WKT.Tests
{
    public class PlacemarkExtensionsTests
    {
        private Placemark _placemark;
        private Polygon _polygon;
        private MultipleGeometry _multipleGeometry;
        private Placemark _multiplePlacemark;

        public PlacemarkExtensionsTests()
        {
            _polygon = new Polygon
            {
                OuterBoundary = new OuterBoundary
                {
                    LinearRing = new LinearRing
                    {
                        Coordinates = new CoordinateCollection
                        {
                            new Vector
                            {
                                Longitude = 30,
                                Latitude = 10
                            },
                            new Vector
                            {
                                Longitude = 40.5,
                                Latitude = 40
                            },
                            new Vector
                            {
                                Longitude = 20,
                                Latitude = 40
                            },
                            new Vector
                            {
                                Longitude = 10,
                                Latitude = 20
                            },
                            new Vector
                            {
                                Longitude = 30,
                                Latitude = 10
                            }
                        }
                    }
                }
            };

            _multipleGeometry = new MultipleGeometry();
            _multipleGeometry.AddGeometry(new Polygon
            {
                OuterBoundary = new OuterBoundary
                {
                    LinearRing = new LinearRing
                    {
                        Coordinates = new CoordinateCollection
                        {
                            new Vector
                            {
                                Longitude = 30,
                                Latitude = 20
                            },
                            new Vector
                            {
                                Longitude = 45,
                                Latitude = 40
                            },
                            new Vector
                            {
                                Longitude = 10,
                                Latitude = 40
                            },
                            new Vector
                            {
                                Longitude = 30,
                                Latitude = 20
                            }
                        }
                    }
                }
            });
            _multipleGeometry.AddGeometry(new Polygon
            {
                OuterBoundary = new OuterBoundary
                {
                    LinearRing = new LinearRing
                    {
                        Coordinates = new CoordinateCollection
                        {
                            new Vector
                            {
                                Longitude = 15,
                                Latitude = 5
                            },
                            new Vector
                            {
                                Longitude = 40,
                                Latitude = 10
                            },                            
                            new Vector
                            {
                                Longitude = 10,
                                Latitude = 20
                            },        
                            new Vector
                            {
                                Longitude =5,
                                Latitude = 10
                            },   
                            new Vector
                            {
                                Longitude =15,
                                Latitude = 5
                            }
                        }
                    }
                }
            });
            _placemark = new Placemark
            {
                Geometry = _polygon
            };            
            _multiplePlacemark = new Placemark
            {
                Geometry = _multipleGeometry
            };
        }

        [Theory]
        [InlineData("POLYGON ((30 10,")]
        [InlineData("40.5 40,")]
        [InlineData("20 40,")]
        [InlineData("10 20,")]
        [InlineData("30 10))")]
        [InlineData("POLYGON ((30 10, 40.5 40, 20 40, 10 20, 30 10)")]
        public void AsWKT_should_contain_substring(string subString)
        {
            //Act
            var result = _placemark.AsWKT();

            //Assert
            result.Should().Contain(subString);
        }
        
        [Theory]
        [InlineData(-7.48953, 38.897902, "38.897902 -7.48953")]
        public void Vector_AsCoordinatePair_should_return_correct_result(double latitude, double longitude, string expectedString)
        {
            //Arrange
            var vector = new Vector
            {
                Altitude = 0,
                Latitude = latitude,
                Longitude = longitude
            };

            //Act
            var result = vector.AsCoordinatePair();

            //Assert
            result.Should().Contain(expectedString);
        }

        [Theory]
        [InlineData("(30 10,")]
        [InlineData("40.5 40,")]
        [InlineData("20 40")]
        [InlineData("10 20,")]
        [InlineData("30 10)")]
        public void VectorArrayArray_AsWKT_should_contin_substring(string substring)
        {
            //Act
            var result = _polygon.AsVectorCoordinates().AsWKT();

            //Assert
            result.Should().Contain(substring);
        }
        
        [Theory]
        [InlineData("MULTIPOLYGON (((30 20,")]
        [InlineData("45 40,")]
        [InlineData("10 40,")]
        [InlineData("30 20)),")]
        [InlineData("((15 5,")]
        [InlineData("40 10,")]
        [InlineData("10 20,")]
        [InlineData("5 10,")]
        [InlineData("15 5)))")]
        [InlineData("MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))")]
        public void MultiplePlacemark_AsWKT_should_contain_substring(string substring)
        {
            //Act
            var result = _multiplePlacemark.AsWKT();

            //Assert
            result.Should().Contain(substring);
        }

        [Theory]
        [InlineData("POLYGON ((30 10, 40.5 40, 20 40, 10 20, 30 10))")]
        public void PlaceMark_array_AsWKT_should_return_correct_result_for_single_polygon(string subString)
        {
            //Arrange
            var placemarkArray = new[] {_placemark};

            //Act
            var result = placemarkArray.AsWKT();

            //Assert
            result.Should().Contain(subString);
        } 
        
        [Theory]
        [InlineData("MULTIPOLYGON (((30 10, 40.5 40, 20 40, 10 20, 30 10)),((30 10, 40.5 40, 20 40, 10 20, 30 10)))")]
        public void PlaceMark_array_AsWKT_should_return_correct_result_for_two_polygon(string subString)
        {
            //Arrange
            var placemarkArray = new[] {_placemark, _placemark};

            //Act
            var result = placemarkArray.AsWKT();

            //Assert
            result.Should().Contain(subString);
        }        
        
        [Theory]
        [InlineData("MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))")]
        public void PlaceMark_array_AsWKT_should_return_correct_result_for_single_multiple_polygon(string subString)
        {
            //Arrange
            var placemarkArray = new[] {_multiplePlacemark};

            //Act
            var result = placemarkArray.AsWKT();

            //Assert
            result.Should().Contain(subString);
        }        
        
        [Theory]
        [InlineData("MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)),((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))")]
        public void PlaceMark_array_AsWKT_should_return_correct_result_for_two_multiple_polygon(string subString)
        {
            //Arrange
            var placemarkArray = new[] {_multiplePlacemark, _multiplePlacemark};

            //Act
            var result = placemarkArray.AsWKT();

            //Assert
            result.Should().Contain(subString);
        }        
        
        [Theory]
        [InlineData("MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)),((30 10, 40.5 40, 20 40, 10 20, 30 10)))")]
        public void PlaceMark_array_AsWKT_should_return_correct_result_for_multiple_polygon_and_polygon(string subString)
        {
            //Arrange
            var placemarkArray = new[] {_multiplePlacemark, _placemark};

            //Act
            var result = placemarkArray.AsWKT();

            //Assert
            result.Should().Contain(subString);
        }
    }
}
