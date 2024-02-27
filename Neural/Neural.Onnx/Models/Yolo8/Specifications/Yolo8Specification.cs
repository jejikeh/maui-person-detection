using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo8.Specifications;

public static class Yolo8Specification
{
    public static Size InputSize = new(640, 640);

    public static string[] OnnxOutputNames = ["images"];
    
    public static YoloClass[] Classes { get; set; } = [
        YoloClass.Person,
        YoloClass.Bicycle,
        YoloClass.Car,
        YoloClass.Motorcycle,
        YoloClass.Airplane,
        YoloClass.Bus,
        YoloClass.Train,
        YoloClass.Truck,
        YoloClass.Boat,
        YoloClass.TrafficLight,
        YoloClass.FireHydrant,
        YoloClass.StopSign,
        YoloClass.ParkingMeter,
        YoloClass.Bench,
        YoloClass.Bird,
        YoloClass.Cat,
        YoloClass.Dog,
        YoloClass.Horse,
        YoloClass.Sheep,
        YoloClass.Cow,
        YoloClass.Elephant,
        YoloClass.Bear,
        YoloClass.Zebra,
        YoloClass.Giraffe,
        YoloClass.Backpack,
        YoloClass.Umbrella,
        YoloClass.Handbag,
        YoloClass.Tie,
        YoloClass.Suitcase,
        YoloClass.Frisbee,
        YoloClass.Skis,
        YoloClass.Snowboard,
        YoloClass.SportsBall,
        YoloClass.Kite,
        YoloClass.BaseballBat,
        YoloClass.BaseballGlove,
        YoloClass.Skateboard,
        YoloClass.Surfboard,
        YoloClass.TennisRacket,
        YoloClass.Bottle,
        YoloClass.WineGlass,
        YoloClass.Cup,
        YoloClass.Fork,
        YoloClass.Knife,
        YoloClass.Spoon,
        YoloClass.Bowl,
        YoloClass.Banana,
        YoloClass.Apple,
        YoloClass.Sandwich,
        YoloClass.Orange,
        YoloClass.Broccoli,
        YoloClass.Carrot,
        YoloClass.HotDog,
        YoloClass.Pizza,
        YoloClass.Donut,
        YoloClass.Cake,
        YoloClass.Chair,
        YoloClass.Couch,
        YoloClass.PottedPlant,
        YoloClass.Bed,
        YoloClass.DiningTable,
        YoloClass.Toilet,
        YoloClass.Tv,
        YoloClass.Laptop,
        YoloClass.Mouse,
        YoloClass.Remote,
        YoloClass.Keyboard,
        YoloClass.CellPhone,
        YoloClass.Microwave,
        YoloClass.Oven,
        YoloClass.Toaster,
        YoloClass.Sink,
        YoloClass.Refrigerator,
        YoloClass.Book,
        YoloClass.Clock,
        YoloClass.Vase,
        YoloClass.Scissors,
        YoloClass.TeddyBear,
        YoloClass.HairDrier,
        YoloClass.Toothbrush
    ];
}