public struct CallbackData {

    #region fields
    public CallbackDataType DataType;
    public string DataString;
    public object DataObject;
    public bool DataBoolean;
    #endregion

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

    public static CallbackData Object(object data) {
        CallbackData d = new CallbackData();
        d.DataObject = data;
        d.DataType = CallbackDataType.Object;

        return d;
    }

    public static CallbackData Boolean(bool data) {
        CallbackData d = new CallbackData();
        d.DataBoolean = data;
        d.DataType = CallbackDataType.Boolean;

        return d;
    }
}