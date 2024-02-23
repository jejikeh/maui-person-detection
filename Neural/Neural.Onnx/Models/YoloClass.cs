using System.ComponentModel;

namespace Neural.Onnx.Models;

public enum YoloClass
{
    [Description("person")]
    Person,
    [Description("bicycle")]
    Bicycle,
    [Description("car")]
    Car,
    [Description("motorcycle")]
    Motorcycle,
    [Description("airplane")]
    Airplane,
    [Description("bus")]
    Bus,
    [Description("train")]
    Train,
    [Description("truck")]
    Truck,
    [Description("boat")]
    Boat,
    [Description("traffic light")]
    TrafficLight,
    [Description("fire hydrant")]
    FireHydrant,
    [Description("stop sign")]
    StopSign,
    [Description("parking meter")]
    ParkingMeter,
    [Description("bench")]
    Bench,
    [Description("bird")]
    Bird,
    [Description("cat")]
    Cat,
    [Description("dog")]
    Dog,
    [Description("horse")]
    Horse,
    [Description("sheep")]
    Sheep,
    [Description("cow")]
    Cow,
    [Description("elephant")]
    Elephant,
    [Description("bear")]
    Bear,
    [Description("zebra")]
    Zebra,
    [Description("giraffe")]
    Giraffe,
    [Description("backpack")]
    Backpack,
    [Description("umbrella")]
    Umbrella,
    [Description("handbag")]
    Handbag,
    [Description("tie")]
    Tie,
    [Description("suitcase")]
    Suitcase,
    [Description("frisbee")]
    Frisbee,
    [Description("skis")]
    Skis,
    [Description("snowboard")]
    Snowboard,
    [Description("sports ball")]
    SportsBall,
    [Description("kite")]
    Kite,
    [Description("baseball bat")]
    BaseballBat,
    [Description("baseball glove")]
    BaseballGlove,
    [Description("skateboard")]
    Skateboard,
    [Description("surfboard")]
    Surfboard,
    [Description("tennis racket")]
    TennisRacket,
    [Description("bottle")]
    Bottle,
    [Description("wine glass")]
    WineGlass,
    [Description("cup")]
    Cup,
    [Description("fork")]
    Fork,
    [Description("knife")]
    Knife,
    [Description("spoon")]
    Spoon,
    [Description("bowl")]
    Bowl,
    [Description("banana")]
    Banana,
    [Description("apple")]
    Apple,
    [Description("sandwich")]
    Sandwich,
    [Description("orange")]
    Orange,
    [Description("broccoli")]
    Broccoli,
    [Description("carrot")]
    Carrot,
    [Description("hot dog")]
    HotDog,
    [Description("pizza")]
    Pizza,
    [Description("donut")]
    Donut,
    [Description("cake")]
    Cake,
    [Description("chair")]
    Chair,
    [Description("couch")]
    Couch,
    [Description("potted plant")]
    PottedPlant,
    [Description("bed")]
    Bed,
    [Description("dining table")]
    DiningTable,
    [Description("toilet")]
    Toilet,
    [Description("tv")]
    Tv,
    [Description("laptop")]
    Laptop,
    [Description("mouse")]
    Mouse,
    [Description("remote")]
    Remote,
    [Description("keyboard")]
    Keyboard,
    [Description("cell phone")]
    CellPhone,
    [Description("microwave")]
    Microwave,
    [Description("oven")]
    Oven,
    [Description("toaster")]
    Toaster,
    [Description("sink")]
    Sink,
    [Description("refrigerator")]
    Refrigerator,
    [Description("book")]
    Book,
    [Description("clock")]
    Clock,
    [Description("vase")]
    Vase,
    [Description("scissors")]
    Scissors,
    [Description("teddy bear")]
    TeddyBear,
    [Description("hair drier")]
    HairDrier,
    [Description("toothbrush")]
    Toothbrush
}