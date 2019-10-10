using NUnit.Framework;

namespace Tests {

    public class TestEnumBitField {

        [Test]
        public void TestEmptyConstructor() {
            EnumBitField<BitEnum> field = new EnumBitField<BitEnum>();
            Assert.AreEqual(0, field.Value);
            Assert.AreEqual(15, field.Max, "Max is not calculated correctly");
        }

        [Test]
        public void TestIntConstructor() {
            EnumBitField<BitEnum> field = new EnumBitField<BitEnum>(3); // 1100
            Assert.IsTrue(field.AreOn(BitEnum.B0, BitEnum.B1), "Integer constructor does not initialize bits correctly");
            Assert.IsTrue(field.AreOff(BitEnum.B2, BitEnum.B3), "Integer constructor does not initialize bits correctly");
        }

        [Test]
        public void TestIntConstructorMaxValue() {
            EnumBitField<BitEnum> field = new EnumBitField<BitEnum>(16); // 10000
            Assert.AreEqual(15, field.Value, "Integer constructor does not clamp with max value");
        }

        [Test]
        public void TestInConstructorMinValue() {
            EnumBitField<BitEnum> field = new EnumBitField<BitEnum>(-1);
            Assert.AreEqual(0, field.Value, "Integer constructor does not clamp with min value");
        }

        [Test]
        public void TestEnumConstructor() {
            EnumBitField<BitEnum> field = new EnumBitField<BitEnum>(BitEnum.B2, BitEnum.B1); // 0110
            Assert.AreEqual(6, field.Value);
            Assert.IsTrue(field.AreOn(BitEnum.B1, BitEnum.B2), "Enum constructor does not initialize bits correctly");
            Assert.IsTrue(field.AreOff(BitEnum.B0, BitEnum.B3), "Enum constructor does not initialize bits correctly");
        }

        [Test]
        public void TestSet() {
            EnumBitField<BitEnum> field = new EnumBitField<BitEnum>(2); // 0100
            field.Set(BitEnum.B0);
            Assert.IsTrue(field.IsOn(BitEnum.B0), "Set function does not enable the correct bits");
            Assert.IsTrue(field.IsOff(BitEnum.B1), "Set function does not disable the correct bits");
        }

        [Test]
        public void TestAll() {
            EnumBitField<BitEnum> field = new EnumBitField<BitEnum>(1); // 1000
            field.All();
            Assert.AreEqual(field.Max, field.Value);
            Assert.IsTrue(field.AreOn(
                BitEnum.B0,
                BitEnum.B1,
                BitEnum.B2,
                BitEnum.B3
            ));
        }

        [Test]
        public void TestOnOff() {
            EnumBitField<BitEnum> field = new EnumBitField<BitEnum>(6); // 0110
            Assert.IsTrue(field.AreOn(BitEnum.B1, BitEnum.B2));
            Assert.IsTrue(field.AreOff(BitEnum.B0, BitEnum.B3));
            Assert.IsTrue(field.IsOn(BitEnum.B1) && field.IsOn(BitEnum.B2));
            Assert.IsTrue(field.IsOff(BitEnum.B0) && field.IsOff(BitEnum.B3));
        }

        [Test]
        public void TestEquality() {
            EnumBitField<BitEnum> field1 = new EnumBitField<BitEnum>(6); // 0110
            EnumBitField<BitEnum> field2 = new EnumBitField<BitEnum>(6); // 0110
            EnumBitField<BitEnum> field3 = new EnumBitField<BitEnum>(7); // 1110
            Assert.AreEqual(field1, field2, "Bit fields with the same value are not evaluated as equal");
            Assert.AreNotEqual(field1, field3, "Bit fields with different values are evaluated as equal");
            Assert.IsTrue(field1 == field2, "Bit fields with the same value are not evaluated as equal");
            Assert.IsTrue(field1 != field3, "Bit fields with different values are evaluated as equal");
        }

        [Test]
        public void TestAdditionOperator() {
            EnumBitField<BitEnum> a = new EnumBitField<BitEnum>(2);
            EnumBitField<BitEnum> b = new EnumBitField<BitEnum>(3);
            Assert.AreEqual(5, (a+b).Value, "Bit field addition does not work correctly");
        }

        [Test]
        public void TestSubtractionOperator() {
            EnumBitField<BitEnum> a = new EnumBitField<BitEnum>(3);
            EnumBitField<BitEnum> b = new EnumBitField<BitEnum>(2);
            Assert.AreEqual(1, (a-b).Value, "Bit field subtraction does not work correctly");
        }
    }

    // Represents a 4-bit value (max value 15)
    public enum BitEnum { B0, B1, B2, B3, }
}