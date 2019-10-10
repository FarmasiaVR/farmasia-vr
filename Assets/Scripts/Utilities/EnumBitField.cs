using System;
using System.Collections.Generic;
using System.Linq;

public class EnumBitField<T> where T : Enum {

    #region fields
    public int Value { get; private set; }
    public int Max { get; private set; }
    #endregion

    #region constructors
    public EnumBitField() {
        int bitCount = ((T[])Enum.GetValues(typeof(T))).Length;
        Max = (int)Math.Pow(2, bitCount) - 1;
    }

    public EnumBitField(int initialValue) : this() {
        Value = Math.Max(Math.Min(Max, initialValue), 0);
    }

    public EnumBitField(params T[] bitEnums) : this() {
        Set(bitEnums);
    }
    #endregion

    #region operators
    public static EnumBitField<T> operator +(EnumBitField<T> a, EnumBitField<T> b) {
        return new EnumBitField<T>(a.Value + b.Value);
    }

    public static EnumBitField<T> operator -(EnumBitField<T> a, EnumBitField<T> b) {
        return new EnumBitField<T>(a.Value - b.Value);
    }

    public static bool operator ==(EnumBitField<T> a, EnumBitField<T> b) {
        return a.Value == b.Value;
    }

    public static bool operator !=(EnumBitField<T> a, EnumBitField<T> b) {
        return a.Value != b.Value;
    }

    public static bool operator ==(EnumBitField<T> a, T b) {
        return a.IsOn(b);
    }

    public static bool operator !=(EnumBitField<T> a, T b) {
        return a.IsOff(b);
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) {
            return false;
        }
        if (ReferenceEquals(this, obj)) {
            return true;
        }

        EnumBitField<T> other = (EnumBitField<T>)obj;
        return Value == other.Value;
    }

    public override int GetHashCode() {
        return Value.GetHashCode();
    }
    #endregion

    public void Set(params T[] bitEnums) {
        Value = 0;
        On(bitEnums);
    }

    public void All() {
        Value = Max;
    }

    public void None() {
        Value = 0;
    }

    public void On(params T[] bitEnums) {
        foreach (T t in bitEnums) {
            Value |= ToInt(t);
        }
    }

    public void Off(params T[] bitEnums) {
        foreach (T t in bitEnums) {
            Value &= ~ToInt(t);
        }
    }

    public bool IsOn(T bitEnum) {
        return !IsOff(bitEnum);
    }

    public bool IsOff(T bitEnum) {
        int bit = ToInt(bitEnum);
        return (Value & bit) == 0;
    }

    public bool AreOn(params T[] bitEnums) {
        foreach (T t in bitEnums) {
            if (IsOff(t)) {
                return false;
            }
        }
        return true;
    }

    public bool AreOff(params T[] bitEnums) {
        foreach (T t in bitEnums) {
            if (IsOn(t)) {
                return false;
            }
        }
        return true;
    }

    public List<T> GetOnList() {
        List<T> allValues = Enum.GetValues(typeof(T)).OfType<T>().ToList();
        return allValues.FindAll(t => IsOn(t));
    }

    public List<T> GetOffList() {
        List<T> allValues = Enum.GetValues(typeof(T)).OfType<T>().ToList();
        return allValues.FindAll(t => IsOff(t));
    }

    private int ToInt(T value) {
        return (int)Math.Pow(2, ToBitIndex(value));
    }

    private int ToBitIndex(T value) {
        return (int)(object)value;
    }
}
