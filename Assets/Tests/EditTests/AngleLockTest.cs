using NUnit.Framework;

namespace Tests {

    // Only no data callbacks are tested. Callback with data code is almost identical though.
    public class AngleLockTest {

        [SetUp]
        public void SetUp() {
        }

        [Test]
        public void TestZeroToNinetyNoClamp() {
            float left = 0;
            float right = 90;
            Assert.AreEqual(left+1, AngleLock.ClampAngleDeg(left+1, left, right), "Angle is clamped to left ");
            Assert.AreEqual(right-1, AngleLock.ClampAngleDeg(right-1, left, right), "Angle is clamped to right");
        }

        [Test]
        public void TestZeroToNinetyClampLeft() {
            float left = 0;
            float right = 90;
            float divider = AngleLock.TrimAngleDeg((left + right) / 2 + 180);
            Assert.AreEqual(left, AngleLock.ClampAngleDeg(left, left, right), "Angle is not clamped to left");
            Assert.AreEqual(left, AngleLock.ClampAngleDeg(left - 1, left, right), "Angle is not clamped to left");
            Assert.AreEqual(left, AngleLock.ClampAngleDeg(divider + 1, left, right), "Angle is not clamped to left");
        }

        [Test]
        public void TestZeroToNinetyClampRight() {
            float left = 0;
            float right = 90;
            float divider = AngleLock.TrimAngleDeg((left + right) / 2 + 180);
            Assert.AreEqual(right, AngleLock.ClampAngleDeg(right, left, right), "Angle is not clamped to right");
            Assert.AreEqual(right, AngleLock.ClampAngleDeg(right + 1, left, right), "Angle is not clamped to right");
            Assert.AreEqual(right, AngleLock.ClampAngleDeg(divider - 1, left, right), "Angle is not clamped to right");
        }

        [Test]
        public void TestMinusFortyFiveToFortyFiveNoClamp() {
            float left = -45;
            float right = 45;
            Assert.AreEqual(AngleLock.TrimAngleDeg(left+1), AngleLock.ClampAngleDeg(left+1, left, right), "Angle is clamped to left ");
            Assert.AreEqual(right-1, AngleLock.ClampAngleDeg(right-1, left, right), "Angle is clamped to right");
        }

        [Test]
        public void TestMinusFortyFiveToFortyFiveClampLeft() {
            float left = -45;
            float right = 45;
            float divider = AngleLock.TrimAngleDeg((left + right) / 2 + 180);
            float expected = AngleLock.TrimAngleDeg(left);
            Assert.AreEqual(expected, AngleLock.ClampAngleDeg(left, left, right), "Angle is not clamped to left");
            Assert.AreEqual(expected, AngleLock.ClampAngleDeg(left - 1, left, right), "Angle is not clamped to left");
            Assert.AreEqual(expected, AngleLock.ClampAngleDeg(divider + 1, left, right), "Angle is not clamped to left");
        }

        [Test]
        public void TestMinusFortyFiveToFortyFiveClampRight() {
            float left = -45;
            float right = 45;
            float divider = AngleLock.TrimAngleDeg((left + right) / 2 + 180);
            Assert.AreEqual(right, AngleLock.ClampAngleDeg(right, left, right), "Angle is not clamped to right");
            Assert.AreEqual(right, AngleLock.ClampAngleDeg(right + 1, left, right), "Angle is not clamped to right");
            Assert.AreEqual(right, AngleLock.ClampAngleDeg(divider - 1, left, right), "Angle is not clamped to right");
        }
    }
}