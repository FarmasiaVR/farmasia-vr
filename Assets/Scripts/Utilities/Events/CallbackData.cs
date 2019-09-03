public struct CallbackData {

    public CallbackDataType DataType;
    public string DataString;
    public object DataObject;

    public static CallbackData NoData() {
        CallbackData d = new CallbackData();
        d.DataType = CallbackDataType.NoData;

        return d;
    }
    public static CallbackData String(string data) {
        CallbackData d = new CallbackData();
        d.DataString = data;
        d.DataType = CallbackDataType.String;

        return d;
    }
    public static CallbackData String(object data) {
        CallbackData d = new CallbackData();
        d.DataObject = data;
        d.DataType = CallbackDataType.Object;

        return d;
    }
}